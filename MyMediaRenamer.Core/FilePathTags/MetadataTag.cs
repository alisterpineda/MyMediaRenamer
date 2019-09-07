﻿using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace MyMediaRenamer.Core.FilePathTags
{
    public enum MetadataName
    {
        ExifMake,
        ExifModel,
        ExifOrientation
    }

    public class MetadataTag : BaseTag
    {
        #region Members
        private static readonly Dictionary<MetadataName, int> _enumToTagTypeMap = new Dictionary<MetadataName, int>
        {
            { MetadataName.ExifMake, ExifDirectoryBase.TagMake },
            { MetadataName.ExifModel, ExifDirectoryBase.TagModel },
            { MetadataName.ExifOrientation, ExifDirectoryBase.TagOrientation }
        };
        #endregion

        #region Constructors
        public MetadataTag(string tagOptionsString = null) : base(tagOptionsString) { }
        #endregion

        #region Properties

        public MetadataName Name { get; set; }

        public bool UseRaw { get; set; }

        #endregion

        #region Methods

        public override bool Equals(object obj)
        {
            if (!(obj is MetadataTag other))
                return false;

            return base.Equals(obj) &&
                   UseRaw.Equals(other.UseRaw);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = base.GetHashCode();
                hash = hash * 13 + Name.GetHashCode();
                hash = hash * 13 + UseRaw.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"MetadataTag :: {GetHashCode().ToString("x8")}";
        }

        protected override string GenerateString(MediaRenamer mediaRenamer, MediaFile mediaFile)
        {
            int tagType = _enumToTagTypeMap[Name];

            if (UseRaw)
            {
                return mediaFile.MetadataDirectories.Where(x => x.ContainsTag(tagType))
                    ?.FirstOrDefault()?.GetString(tagType);
            }

            return mediaFile.MetadataDirectories.Where(x => x.ContainsTag(tagType))
                ?.FirstOrDefault()?.GetDescription(tagType);
        }

        #endregion
    }
}