using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.CommandLineUtils;
using MyMediaRenamer.Core;

namespace MyMediaRenamer.Cli
{
    internal static class Program
    {
        private static readonly Renamer Renamer = new Renamer();
        private static int returnCode = 0;

        internal static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "MyMediaRenamer";
            app.Description = "Bulk renamer of photo and video files.";

            app.HelpOption("-h|--help");

            var defaultStringOption = app.Option("--default-string", "The default string used when a tag fails to produce a valid string", CommandOptionType.SingleValue);
            var skipNullOption = app.Option("--skip-null", "Skip renaming a file if a tag fails to produce a valid string.", CommandOptionType.NoValue);
            var testOption = app.Option("-t|--test", "Do a dry-run where the program does not actually rename any files.", CommandOptionType.NoValue);
            

            var patternArgument = app.Argument("File Name Pattern", "Pattern used to determine the new file name of each file.", false);
            var filesArgument = app.Argument("Media Files", "File(s) to rename.", true);

            app.OnExecute(() =>
            {
                if (string.IsNullOrEmpty(patternArgument.Value) || !(filesArgument.Values.Count > 0))
                {
                    app.ShowHint();
                    returnCode = 1;
                }

                if (defaultStringOption.HasValue())
                    Renamer.NullTagString = defaultStringOption.Value();
                if (skipNullOption.HasValue())
                    Renamer.SkipOnNullTag = true;
                if (testOption.HasValue())
                    Renamer.TestMode = true;

                try
                {
                    var tags = PatternParser.Parse(patternArgument.Value);
                    var mediaFiles = GetMediaFilesFromStrings(filesArgument.Values);
                    Renamer.Execute(mediaFiles, tags);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    returnCode = 1;
                }

                return returnCode;
            });

            return app.Execute(args);
        }

        internal static IList<MediaFile> GetMediaFilesFromStrings(IList<string> filePaths)
        {
            List<MediaFile> mediaFiles = new List<MediaFile>();

            foreach (var filePath in filePaths)
            {
                var mediaFile = new MediaFile(filePath);
                mediaFile.Renamed += MediaFile_Renamed;
                mediaFile.ErrorReported += MediaFile_ErrorReported;
                mediaFiles.Add(mediaFile);
            }

            return mediaFiles;
        }

        private static void MediaFile_Renamed(object sender, MediaFileRenamedArgs e)
        {
            var output = $"'{e.OldFilePath}' -> '{e.NewFilePath}'";

            if (e.TestMode)
                output = $"[TEST] {output}";

            Console.WriteLine(output);
        }

        private static void MediaFile_ErrorReported(object sender, MediaFileErrorReportedArgs e)
        {
            MediaFile mediaFile = sender as MediaFile;

            Console.WriteLine($"Error occured while trying to rename '{mediaFile.FilePath}'. Reason: {e.ErrorMessage}");
            returnCode = 1;
        }
    }
}
