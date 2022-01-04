using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.AWS.Abstractions;
using FolderSynchronizerApp.Business.AWS.Exceptions;
using System;
using System.IO;
using System.Linq;

namespace FolderSynchronizerApp.Business.AWS.Implementations
{
    public class S3FileDownloaderImp : IS3FileDownloader
    {
        private string BucketName { get; }
        private IS3ClientCreator ClientCreator { get; }

        private IMediaSaver MediaSaver { get; }

        public S3FileDownloaderImp(IConfigManager configManager, IS3ClientCreator clientCreator, IMediaSaver mediaSaver)
        {
            if (configManager == null)
            {
                throw new ArgumentNullException(nameof(configManager));
            }

            ClientCreator = clientCreator ?? throw new ArgumentNullException(nameof(clientCreator));
            MediaSaver = mediaSaver ?? throw new ArgumentNullException(nameof(mediaSaver));

            var configData = configManager.Load();
            BucketName = configData.S3ConfigData.BucketName;
        }

        public void DownloadFile(string fileKey)
        {
            try
            {
                var fileContents = GetFileBytes(fileKey);
                var mimeType = GetMimeType(fileKey);

                MediaSaver.Save(fileKey, fileContents, mimeType);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private string GetFileContents(string fileKey)
        {
            var client = ClientCreator.GetS3Client();

            try
            {
                var response = client.GetObjectAsync(BucketName, fileKey).Result;
                string fileContents;
                using (var responseStream = response.ResponseStream)
                {
                    // TODO: try saving file to phone using different encodings and seeing if any will play/match up with local copy.
                    using (var reader = new StreamReader(responseStream, System.Text.Encoding.Unicode))
                    {
                        fileContents = reader.ReadToEnd();
                    }
                }

                return fileContents;
            } catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Any(e => e.Message == "The specified key does not exist."))
                {
                    throw new FileDoesNotExistInBucketException(fileKey);
                }

                throw;
            }
        }

        private byte[] GetFileBytes(string fileKey)
        {
            var client = ClientCreator.GetS3Client();

            try
            {
                var response = client.GetObjectAsync(BucketName, fileKey).Result;

                string fileContents;
                using (var responseStream = response.ResponseStream)
                {
                    using (var bReader = new BinaryReader(responseStream))
                    {
                        var bytes = bReader.ReadBytes((int)responseStream.Length);
                        return bytes;
                    }
                }
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Any(e => e.Message == "The specified key does not exist."))
                {
                    throw new FileDoesNotExistInBucketException(fileKey);
                }

                throw;
            }
        }

        private string GetMimeType(string fileKey)
        {
            var extension = Path.GetExtension(fileKey);
            var mimeType = MimeTypes.MimeTypeMap.GetMimeType(extension);
            return mimeType;
        }
    }
}