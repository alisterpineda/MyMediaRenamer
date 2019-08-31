using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MyMediaRenamer.Core.FilePathTags;

namespace MyMediaRenamer.Core
{
    public class MediaRenamer
    {
        public async void Execute(IList<MediaFile> mediaFiles, IList<BaseFilePathTag> filePathTags)
        {
            try
            {
                await Task.Run(() =>
                {
                    foreach (var mediaFile in mediaFiles)
                    {
                        string newFilePath = String.Empty;

                        foreach (var filePathTag in filePathTags)
                        {
                            newFilePath += filePathTag.GetString(this, mediaFile);
                        }

                        mediaFile.Rename(Path.Combine(mediaFile.FileDirectory, newFilePath));
                    }
                });
            }
            catch (Exception) {}
        }
    }
}
