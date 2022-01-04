using Android.Content;
using Android.Provider;
using FolderSynchronizerApp.Business.Abstractions;
using Plugin.CurrentActivity;
using System;
using System.IO;

namespace FolderSynchronizerApp.Droid.Implementations
{
    public class MediaSaverImp : IMediaSaver
    {
        public void Save(string filePath, byte[] fileContents, string mimeType)
        {
            var fileName = Path.GetFileName(filePath);
            var fileNameWithoutExt = Path.ChangeExtension(fileName, null);

            var fileSize = fileContents.Length;

            var values = new ContentValues();
            var contentResolver = CrossCurrentActivity.Current.AppContext.ContentResolver;

            values.Put(MediaStore.IMediaColumns.Title, fileName);
            values.Put(MediaStore.IMediaColumns.MimeType, mimeType);
            values.Put(MediaStore.IMediaColumns.Size, fileSize);
            values.Put(MediaStore.Audio.Media.InterfaceConsts.DisplayName, fileNameWithoutExt);

            Android.Net.Uri newUri;
            try
            {
                // TODO: support folders?
                var folderUri = MediaStore.Audio.Media.ExternalContentUri;
                newUri = contentResolver.Insert(folderUri, values);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to get data back from content resolver. Filename: " + filePath);
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.Flush();

                throw;
            }

            try
            {
                var saveStream = contentResolver.OpenOutputStream(newUri);

                using var writer = new BinaryWriter(saveStream);
                writer.Write(fileContents);
                writer.Close();
                writer.Dispose();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed file write: " + filePath);
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.Flush();

                throw;
            }
        }
    }
}