using FolderSynchronizerApp.Business.AWS.Enums;
using System;

namespace FolderSynchronizerApp.Business.AWS.Data
{
    public class S3MessageData
    {
        public string BucketName { get; set; }

        public DateTime? Timestamp { get; set; }

        public S3Action Action { get; set; }

        public string Key { get; set; }
    }
}