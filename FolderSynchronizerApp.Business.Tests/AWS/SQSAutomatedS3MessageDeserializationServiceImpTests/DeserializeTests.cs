using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using FolderSynchronizerApp.Business.AWS.Exceptions;
using FolderSynchronizerApp.Business.AWS.Implementations;
using Moq;
using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;

namespace FolderSynchronizerApp.Business.Tests.AWS.SQSAutomatedS3MessageDeserializationServiceImpTests
{
    [TestFixture]
    public class DeserializeTests
    {
        private Mock<IJsonSerializationService> MockJsonSerializationService { get; set; }
        private SQSAutomatedS3MessageDeserializationServiceImp Service { get; set; }

        [SetUp]
        public void SetUp()
        {
            MockJsonSerializationService = new Mock<IJsonSerializationService>(MockBehavior.Strict);

            Service = new SQSAutomatedS3MessageDeserializationServiceImp(MockJsonSerializationService.Object);
        }

        [Test]
        public void NullMessageTest()
        {
            Should.Throw<ArgumentNullException>(() => Service.Deserialize(null));
        }

        [Test]
        public void EmptyMessageTest()
        {
            var message = CreateMessage(string.Empty);

            MockJsonSerializationService.Setup(s => s.Deserialize<SerializedSQSAutomatedMessageData>(message.Body)).Throws(new Newtonsoft.Json.JsonReaderException());

            Should.Throw<NotAutomatedMessageException>(() => Service.Deserialize(message));
        }

        private Message CreateMessage(string content)
        {
            return new Message
            {
                Body = content
            };
        }

        [Test]
        public void MultipleRecordsTest()
        {
            var message = CreateMessage("Test");
            var messageData = new SerializedSQSAutomatedMessageData
            {
                Records = new List<SerializedSQSAutomatedMessageRecordData>
                {
                    new SerializedSQSAutomatedMessageRecordData(),
                    new SerializedSQSAutomatedMessageRecordData(),
                }
            };

            MockJsonSerializationService.Setup(s => s.Deserialize<SerializedSQSAutomatedMessageData>(message.Body)).Returns(messageData);

            Should.Throw<UnsupportedSQSEventRecordCountException>(() => Service.Deserialize(message));
        }

        [Test]
        public void InvalidDateEventTimeTest()
        {
            var message = CreateMessage("Test");
            var messageData = CreateMessageData(SQSAutomatedS3MessageDeserializationServiceImp.UploadEventName, "Not a Date", "Test", "Test");

            MockJsonSerializationService.Setup(s => s.Deserialize<SerializedSQSAutomatedMessageData>(message.Body)).Returns(messageData);

            Should.Throw<FormatException>(() => Service.Deserialize(message));
        }

        [Test]
        public void SpaceConversionTest()
        {
            var message = CreateMessage("Test+Message");
            var messageData = CreateUploadMessageDataWithObjectKeyFromMessage(message);

            MockJsonSerializationService.Setup(s => s.Deserialize<SerializedSQSAutomatedMessageData>(message.Body)).Returns(messageData);

            var result = Service.Deserialize(message);
            result.Key.ShouldBe("Test Message");
        }

        private SerializedSQSAutomatedMessageData CreateUploadMessageDataWithObjectKeyFromMessage(Message message)
        {
            var data = CreateMessageData(SQSAutomatedS3MessageDeserializationServiceImp.UploadEventName, DateTime.Now.ToString(), "Test Bucket", message.Body);
            return data;
        }

        private SerializedSQSAutomatedMessageData CreateMessageData(string eventName, string eventTime, string bucketName, string objectKey)
        {
            var data = new SerializedSQSAutomatedMessageData
            {
                Records = new List<SerializedSQSAutomatedMessageRecordData>
                {
                    CreateRecordData(eventName, eventTime, bucketName, objectKey)
                }
            };
            return data;
        }

        private SerializedSQSAutomatedMessageRecordData CreateRecordData(string eventName, string eventTime, string bucketName, string objectKey)
        {
            var data = new SerializedSQSAutomatedMessageRecordData
            {
                EventName = eventName,
                EventTime = eventTime,
                S3 = CreateS3Data(bucketName, objectKey)
            };
            return data;
        }

        private SerializedSQSAutomatedMessageS3Data CreateS3Data(string bucketName, string objectKey)
        {
            var data = new SerializedSQSAutomatedMessageS3Data
            {
                Bucket = CreateBucketData(bucketName),
                Object = CreateObjectData(objectKey)
            };
            return data;
        }

        private SerializedSQSAutomatedMessageS3BucketData CreateBucketData(string bucketName)
        {
            var data = new SerializedSQSAutomatedMessageS3BucketData
            {
                Name = bucketName,
            };
            return data;
        }

        private SerializedSQSAutomatedMessageS3ObjectData CreateObjectData(string key)
        {
            var data = new SerializedSQSAutomatedMessageS3ObjectData
            {
                Key = key
            };
            return data;
        }

        [Test]
        public void BracketAndAmpsandReplacementTest()
        {
            var message = CreateMessage("Test%26%28Message%29");
            var messageData = CreateUploadMessageDataWithObjectKeyFromMessage(message);

            MockJsonSerializationService.Setup(s => s.Deserialize<SerializedSQSAutomatedMessageData>(message.Body)).Returns(messageData);

            var result = Service.Deserialize(message);
            result.Key.ShouldBe("Test&(Message)");
        }
    }
}