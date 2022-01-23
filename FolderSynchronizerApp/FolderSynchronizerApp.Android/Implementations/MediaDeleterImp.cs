using Android.Content;
using FolderSynchronizerApp.Business.Abstractions;
using Plugin.CurrentActivity;
using System;
using System.IO;

namespace FolderSynchronizerApp.Droid.Implementations
{
    public class MediaDeleterImp : IMediaDeleter
    {
        private IMediaQueryer MediaQueryer { get; set; }

        public MediaDeleterImp(IMediaQueryer mediaQueryer)
        {
            MediaQueryer = mediaQueryer ?? throw new ArgumentNullException(nameof(mediaQueryer));
        }

        public void Delete(string filePath)
        {
            var fileName = Path.GetFileName(filePath);

            // TODO: delete by name and not need queryer?
            var queryResult = MediaQueryer.Query(fileName);
            var androidFileId = queryResult.FileId;

            var selection = GetSelection();
            var selectionArgs = GetSelectionArgs(androidFileId);

            try
            {
                ContentResolver contentResolver = CrossCurrentActivity.Current.AppContext.ContentResolver;
                var count = contentResolver.Delete(Android.Provider.MediaStore.Audio.Media.ExternalContentUri, selection, selectionArgs);

                if (count == 0)
                {
                    throw new FileNotFoundException("Could not find file", filePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to get data back from content resolver. Filename: " + filePath);
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.Flush();
                throw;
            }
        }

        private string GetSelection()
        {
            /**
             * The `selection` is the "WHERE ..." clause of a SQL statement. It's also possible
             * to omit this by passing `null` in its place, and then all rows will be returned.
             * In this case we're using a selection based on the date the image was taken.
             *
             * Note that we've included a `?` in our selection. This stands in for a variable
             * which will be provided by the next variable.
             */
            var result = Android.Provider.IBaseColumns.Id + " = ?";
            return result;
        }

        private string[] GetSelectionArgs(long androidFileId)
        {
            /**
             * The `selectionArgs` is a list of values that will be filled in for each `?`
             * in the `selection`.
             */
            var selectionArgs = new[]
            {
                androidFileId.ToString()
            };
            return selectionArgs;
        }
    }
}