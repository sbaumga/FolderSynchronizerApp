namespace FolderSynchronizerApp.Business.AWS.Data
{
    public class AWSConfigData
    {
        public AWSKeyConfigData KeyConfigData { get; set; }

        public AWSSQSConfigData SQSConfigData { get; set; }

        public AWSS3ConfigData S3ConfigData { get; set; }
    }
}