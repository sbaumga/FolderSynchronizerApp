using Android.Database;
using FolderSynchronizerApp.Business.Abstractions;
using FolderSynchronizerApp.Business.Data;
using Plugin.CurrentActivity;
using System;
using System.IO;
using System.Linq;

namespace FolderSynchronizerApp.Droid.Implementations
{
    public class MediaQueryerImp : IMediaQueryer
    {
        public AndroidFileQueryResultData Query(string filename)
        {
            var projection = GetProjection();

            var selection = GetSelection();

            var selectionArgs = GetSelectionArgs(filename);

            ICursor cursor;

            ////https://github.com/android/storage-samples/blob/main/MediaStore/app/src/main/java/com/android/samples/mediastore/MainActivityViewModel.kt

            try
            {
                var contentResolver = CrossCurrentActivity.Current.AppContext.ContentResolver;
                cursor = contentResolver.Query(BaseUri, projection, selection, selectionArgs.ToArray(), null);

                if (cursor == null)
                {
                    // If this does happen, then there's a problem somewhere
                    throw new FileNotFoundException("Could not find the file", filename);
                }
                else if (cursor != null && cursor.Count > 0)
                {
                    var idColumn = cursor.GetColumnIndexOrThrow(IdName);
                    var dispNameColumn = cursor.GetColumnIndexOrThrow(DisplayNameName);
                    var addedColumn = cursor.GetColumnIndexOrThrow(DateAddedName);
                    var titleColumn = cursor.GetColumnIndexOrThrow(TitleName);
                    var relativePathColumn = cursor.GetColumnIndexOrThrow(RelativePathName);

                    cursor.MoveToFirst();

                    System.Diagnostics.Debug.WriteLine("Cursor found " + cursor.Count + " rows");

                    do
                    {
                        var id = cursor.GetLong(idColumn);
                        var displayName = cursor.GetString(dispNameColumn);
                        var addedTimeInSeconds = cursor.GetLong(addedColumn);
                        var title = cursor.GetString(titleColumn);
                        var relativePath = cursor.GetString(relativePathColumn);

                        if (displayName.Equals(filename))
                        {
                            var fileUri = GetFileUri(id);

                            // TODO: See what this is...
                            bool isUriDocument = Android.Provider.DocumentsContract.IsDocumentUri(CrossCurrentActivity.Current.AppContext, fileUri);
                            if (isUriDocument)
                            {
                                Android.OS.Bundle metadata = Android.Provider.DocumentsContract.GetDocumentMetadata(contentResolver, fileUri);
                            }

                            var result = new AndroidFileQueryResultData
                            {
                                AndroidUri = fileUri.ToString(),
                                FileId = id
                            };
                            return result;
                        }
                    }
                    while (cursor.MoveToNext());

                    cursor.Close();
                    cursor.Dispose();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to get data back from content resolver. Filename: " + filename);
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                System.Diagnostics.Debug.Flush();
                throw;
            }

            throw new FileNotFoundException("Could not find the file", filename);
        }

        private Android.Net.Uri BaseUri => Android.Provider.MediaStore.Audio.Media.ExternalContentUri;

        private string[] GetProjection()
        {
            /**
             * A key concept when working with Android [ContentProvider]s is something called
             * "projections". A projection is the list of columns to request from the provider,
             * and can be thought of (quite accurately) as the "SELECT ..." clause of a SQL
             * statement.
             *
             * It's not _required_ to provide a projection. In this case, one could pass `null`
             * in place of `projection` in the call to [ContentResolver.query], but requesting
             * more data than is required has a performance impact.
             *
             * For this sample, we only use a few columns of data, and so we'll request just a
             * subset of columns.
             */

            var projection = new[]
            {
                IdName,
                DisplayNameName,
                DateAddedName,
                TitleName,
                RelativePathName,
            };

            return projection;
        }

        private string IdName => Android.Provider.IBaseColumns.Id;
        private string DisplayNameName => Android.Provider.MediaStore.IMediaColumns.DisplayName;
        private string DateAddedName => Android.Provider.MediaStore.IMediaColumns.DateAdded;
        private string TitleName => Android.Provider.MediaStore.IMediaColumns.Title;
        private string RelativePathName => Android.Provider.MediaStore.IMediaColumns.RelativePath;

        private string GetSelection()
        {
            /**
             * The `selection` is the "WHERE ..." clause of a SQL statement. It's also possible
             * to omit this by passing `null` in its place, and then all rows will be returned.
             * In this case we're using a selection based on the date the image was taken.
             *
             * Note that we've included a `?` in our selection. This stands in for a variable
             * which will be provided by the next variable. Note: I modified this to not use
             * the parameter, but just hard code the value - which is the text mime type.
             */
            // TODO: use a different property?
            var selection = Android.Provider.MediaStore.IMediaColumns.DisplayName + " = ?";
            return selection;
        }

        private string[] GetSelectionArgs(string fileName)
        {
            /**
             * The `selectionArgs` is a list of values that will be filled in for each `?`
             * in the `selection`.
             */
            var selectionArgs = new[]
            {
                fileName
            };
            return selectionArgs;
        }

        private Android.Net.Uri GetFileUri(long fileId)
        {
            var uri = BaseUri.BuildUpon().AppendPath(fileId.ToString()).Build();
            return uri;
        }
    }
}