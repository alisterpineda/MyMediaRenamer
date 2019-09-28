using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.CommandLineUtils;
using MyMediaRenamer.Core;

namespace MyMediaRenamer.Cli
{
    internal static class Program
    {
        private static readonly Renamer Renamer = new Renamer();
        private static readonly List<MediaFile> MediaFiles = new List<MediaFile>();
        private static int _returnCode = 0;

        internal static int Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "MyMediaRenamer";
            app.Description = "Bulk renamer of photo and video files.";

            app.HelpOption("-h|--help");

            var defaultStringOption = app.Option("--default-string", "The default string used when a tag fails to produce a valid string", CommandOptionType.SingleValue);
            var dryRunOption = app.Option("-d|--dry-run", "Do a dry-run where the program does not actually rename any files", CommandOptionType.NoValue);
            var recursiveOption = app.Option("-r|--recursive", "Recursively access files under given directories", CommandOptionType.NoValue);
            var skipNullOption = app.Option("--skip-null", "Skip renaming a file if a tag fails to produce a valid string", CommandOptionType.NoValue);

            var patternArgument = app.Argument("File Name Pattern", "Pattern used to determine the new file name of each file", false);
            var filesArgument = app.Argument("Media Files", "File(s) to rename", true);

            app.OnExecute(() =>
            {
                if (string.IsNullOrEmpty(patternArgument.Value) || !(filesArgument.Values.Count > 0))
                {
                    app.ShowHint();
                    _returnCode = 1;
                }

                if (defaultStringOption.HasValue())
                    Renamer.NullTagString = defaultStringOption.Value();
                if (skipNullOption.HasValue())
                    Renamer.SkipOnNullTag = true;
                if (dryRunOption.HasValue())
                    Renamer.DryRun = true;

                try
                {
                    var tags = PatternParser.Parse(patternArgument.Value);
                    ProcessFilePaths(filesArgument.Values.ToArray(), recursiveOption.HasValue());
                    Renamer.Execute(MediaFiles, tags);
                }
                catch (PatternInvalidException e)
                {
                    WriteError("Pattern provided by the user is invalid.", e, false);
                    _returnCode = 1;
                }
                catch (Exception e)
                {
                    WriteError("Program has encountered an unexpected error!\n", e, true);
                    _returnCode = 1;
                }

                return _returnCode;
            });

            return app.Execute(args);
        }

        private static void ProcessFilePaths(string[] filePaths, bool recursive)
        {
            foreach (var filePath in filePaths)
            {
                if (Directory.Exists(filePath))
                {
                    ProcessFilePaths(Directory.GetFileSystemEntries(filePath), recursive);
                }
                else if (File.Exists(filePath))
                {
                    MediaFiles.Add(GetMediaFileFromFilePath(filePath));
                }
            }
        }
        private static MediaFile GetMediaFileFromFilePath(string filePath)
        {
            var mediaFile = new MediaFile(filePath);
            mediaFile.Renamed += MediaFile_Renamed;
            mediaFile.ErrorReported += MediaFile_ErrorReported;
            return mediaFile;
        }

        private static void WriteError(string header, Exception exception, bool showStackTrace)
        {
            Console.Error.WriteLine(header + '\n');
            Console.Error.WriteLine($"Message:\n\n{exception.Message}\n");
            if (showStackTrace)
                Console.Error.WriteLine($"Stack Trace:\n\n{exception.StackTrace}\n");
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
            _returnCode = 1;
        }
    }
}
