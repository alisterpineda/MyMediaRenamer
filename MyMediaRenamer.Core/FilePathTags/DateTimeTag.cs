﻿using System;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.QuickTime;
using MetadataExtractor.Util;

namespace MyMediaRenamer.Core.FilePathTags
{
    public sealed class DateTimeTag : BaseTag
    {
        #region Members
        #endregion

        #region Constructors

        public DateTimeTag(string tagOptionsString = null) : base(tagOptionsString)
        {

        }

        #endregion

        #region Properties

        public string Format { get; set; } = "u";

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            DateTimeTag other = obj as DateTimeTag;
            if (other == null)
                return false;
            return Format.Equals(other.Format);
        }

        public override string ToString()
        {
            return $"[DateTimeTag :: Format='{Format}'{GetBasePartialToString()}]";
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            DateTime? dateTime = null;
            if (mediaFile.FileType == FileType.Jpeg)
                dateTime = GetDateTimeFromPhoto(mediaFile);
            else if (mediaFile.FileType == FileType.QuickTime || mediaFile.FileType == FileType.Mp4)
                dateTime = GetDateTimeFromVideo(mediaFile);

            return dateTime?.ToString(Format);
        }

        private DateTime? GetDateTimeFromPhoto(MediaFile mediaFile)
        {
            DateTime dateTime = DateTime.MinValue;
            int subsec = 0;

            // Try to get date/time from metadata
            bool isDateTimeAvailable = mediaFile.MetadataDirectories?.OfType<ExifSubIfdDirectory>().FirstOrDefault()
                                           ?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out dateTime) ?? false;

            if (!isDateTimeAvailable)
                return null;

            // Try to get subsecond data
            bool isSubsecAvailable = mediaFile.MetadataDirectories?.OfType<ExifSubIfdDirectory>().FirstOrDefault()
                                         ?.TryGetInt32(ExifDirectoryBase.TagSubsecondTimeOriginal, out subsec) ?? false;
            if (isSubsecAvailable)
                dateTime = dateTime.AddMilliseconds(subsec);

            return dateTime;
        }

        private DateTime? GetDateTimeFromVideo(MediaFile mediaFile)
        {
            DateTime dateTime = DateTime.MinValue;

            // Try to get date/time from metadata
            bool isDateTimeAvailable = mediaFile.MetadataDirectories?.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault()
                                           ?.TryGetDateTime(QuickTimeMovieHeaderDirectory.TagCreated, out dateTime) ?? false;
            if (!isDateTimeAvailable)
                return null;

            return dateTime;
        }

        #endregion
    }
}
