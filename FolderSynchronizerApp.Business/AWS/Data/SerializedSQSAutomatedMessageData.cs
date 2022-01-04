using System.Collections.Generic;

namespace FolderSynchronizerApp.Business.AWS.Data
{
    public class SerializedSQSAutomatedMessageData
    {
        public IList<SerializedSQSAutomatedMessageRecordData> Records { get; set; }
    }

    public class SerializedSQSAutomatedMessageRecordData
    {
        public string EventTime { get; set; }

        public string EventName { get; set; }

        public SerializedSQSAutomatedMessageS3Data S3 { get; set; }
    }

    public class SerializedSQSAutomatedMessageS3Data
    {
        public SerializedSQSAutomatedMessageS3BucketData Bucket { get; set; }

        public SerializedSQSAutomatedMessageS3ObjectData Object { get; set; }
    }

    public class SerializedSQSAutomatedMessageS3BucketData
    {
        public string Name { get; set; }
    }

    public class SerializedSQSAutomatedMessageS3ObjectData
    {
        public string Key { get; set; }
    }
}