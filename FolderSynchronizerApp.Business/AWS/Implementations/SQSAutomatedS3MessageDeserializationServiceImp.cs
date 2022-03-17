using Amazon.SQS.Model;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using FolderSynchronizerApp.Business.AWS.Data;
using FolderSynchronizerApp.Business.AWS.Enums;
using FolderSynchronizerApp.Business.AWS.Exceptions;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class SQSAutomatedS3MessageDeserializationServiceImp : ISQSAutomatedS3MessageDeserializationService
    {
        private IJsonSerializationService SerializationService { get; set; }

        public SQSAutomatedS3MessageDeserializationServiceImp(IJsonSerializationService jsonSerializationService)
        {
            SerializationService = jsonSerializationService ?? throw new ArgumentNullException(nameof(jsonSerializationService));
        }

        public S3MessageData Deserialize(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            try
            {
                var deserializedData = SerializationService.Deserialize<SerializedSQSAutomatedMessageData>(message.Body);
                var result = ConvertDeserializedDataToS3Data(deserializedData);

                return result;
            } catch (Newtonsoft.Json.JsonReaderException ex)
            {
                throw new NotAutomatedMessageException(message, ex);
            }
        }

        private S3MessageData ConvertDeserializedDataToS3Data(SerializedSQSAutomatedMessageData deserializedData)
        {
            if (deserializedData.Records.Count() != 1)
            {
                throw new UnsupportedSQSEventRecordCountException(deserializedData.Records.Count());
            }

            var record = deserializedData.Records.Single();
            var result = new S3MessageData
            {
                BucketName = record.S3.Bucket.Name,
                Timestamp = DateTime.Parse(record.EventTime),
                Key = SanitizeKey(record.S3.Object.Key),
                Action = ConvertEventNameToS3Action(record.EventName)
            };

            return result;
        }

        private string SanitizeKey(string key)
        {
            foreach(var pair in ReplacementPairs)
            {
                key = key.Replace(pair.Item1, pair.Item2);
            }
            
            return key;
        }

        private Tuple<string, string>[] ReplacementPairs => new[]
        {
            Tuple.Create("+", " "),
            Tuple.Create("%26", "&"),
            Tuple.Create("%28", "("),
            Tuple.Create("%29", ")")
        };

        public const string UploadEventName = "ObjectCreated:Put";
        public const string DeleteEventName = "ObjectRemoved:Delete";

        private S3Action ConvertEventNameToS3Action(string eventName)
        {
            switch (eventName)
            {
                case UploadEventName:
                    return S3Action.Upload;
                case DeleteEventName:
                    return S3Action.Deletion;
                default:
                    throw new NotImplementedException($"Unknown event name: {eventName}.");
            }
        }
    }
}