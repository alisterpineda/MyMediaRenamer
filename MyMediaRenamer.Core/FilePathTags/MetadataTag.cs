using System;
using System.Collections.Generic;
using System.Linq;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace MyMediaRenamer.Core.FilePathTags
{
    public enum MetadataName
    {
        // Exif IFD0
        ExifImageDescription,
        ExifMake,
        ExifModel,
        ExifOrientation,

        // Exif SubIFD
        ExifExposureTime,
        ExifFNumber,
        ExifExposureProgram,
        ExifIso,
        ExifVersion,
        ExifDateTimeOriginal,
        ExifDateTimeDigitized,
        ExifComponentsConfiguration,
        ExifShutterSpeed,
        ExifAperture,
        ExifBrightness,
        ExifExposureBias,
        ExifMeteringMode,
        ExifFlash,
        ExifFocalLength,
        ExifSubjectLocation,
        ExifSubSecTimeOriginal,
        ExifSubSecTimeDigitized,
        ExifColorSpace,
        ExifImageWidth,
        ExifImageHeight,
        ExifSensingMethod,
        ExifSceneType,
        ExifExposureMode,
        ExifWhiteBalanceMode,
        ExifFocalLengthIn35,
        ExifSceneCaptureType,
        ExifLensSpecification,
        ExifLensMake,
        ExifLensModel,

        // Exif GPS
        GpsLatitudeRef,
        GpsLatitude,
        GpsLongitudeRef,
        GpsLongitude,
        GpsAltitudeRef,
        GpsAltitude
    }

    public class MetadataTag : BaseTag
    {
        #region Members
        private static readonly Dictionary<MetadataName, (Type, int)> _enumToTagTypeMap = new Dictionary<MetadataName, (Type,int)>
        {
            // Exif IFD0
            { MetadataName.ExifImageDescription, (typeof(ExifIfd0Directory), ExifDirectoryBase.TagImageDescription) },
            { MetadataName.ExifMake, (typeof(ExifIfd0Directory), ExifDirectoryBase.TagMake) },
            { MetadataName.ExifModel, (typeof(ExifIfd0Directory), ExifDirectoryBase.TagModel) },
            { MetadataName.ExifOrientation, (typeof(ExifIfd0Directory), ExifDirectoryBase.TagOrientation) },
            // Exif SubIFD
            { MetadataName.ExifExposureTime, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExposureTime) },
            { MetadataName.ExifFNumber, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagFNumber) },
            { MetadataName.ExifExposureProgram, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExposureProgram) },
            { MetadataName.ExifIso, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagIsoEquivalent) },
            { MetadataName.ExifVersion, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExifVersion) },
            { MetadataName.ExifDateTimeOriginal, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagDateTimeOriginal) },
            { MetadataName.ExifDateTimeDigitized, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagDateTimeDigitized) },
            { MetadataName.ExifComponentsConfiguration, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagComponentsConfiguration) },
            { MetadataName.ExifShutterSpeed, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagShutterSpeed) },
            { MetadataName.ExifAperture, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagAperture) },
            { MetadataName.ExifBrightness, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagBrightnessValue) },
            { MetadataName.ExifExposureBias, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExposureBias) },
            { MetadataName.ExifMeteringMode, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagMeteringMode) },
            { MetadataName.ExifFlash, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagFlash) },
            { MetadataName.ExifFocalLength, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagFocalLength) },
            { MetadataName.ExifSubjectLocation, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSubjectLocation) },
            { MetadataName.ExifSubSecTimeOriginal, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSubsecondTimeOriginal) },
            { MetadataName.ExifSubSecTimeDigitized, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSubsecondTimeDigitized) },
            { MetadataName.ExifColorSpace, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagColorSpace) },
            { MetadataName.ExifImageWidth, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExifImageWidth) },
            { MetadataName.ExifImageHeight, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExifImageHeight) },
            { MetadataName.ExifSensingMethod, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSensingMethod) },
            { MetadataName.ExifSceneType, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSceneType) },
            { MetadataName.ExifExposureMode, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagExposureMode) },
            { MetadataName.ExifWhiteBalanceMode, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagWhiteBalanceMode) },
            { MetadataName.ExifFocalLengthIn35, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.Tag35MMFilmEquivFocalLength) },
            { MetadataName.ExifSceneCaptureType, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagSceneCaptureType) },
            { MetadataName.ExifLensSpecification, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagLensSpecification) },
            { MetadataName.ExifLensMake, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagLensMake) },
            { MetadataName.ExifLensModel, (typeof(ExifSubIfdDirectory), ExifDirectoryBase.TagLensModel) },
            // Exif GPS
            { MetadataName.GpsLatitudeRef, (typeof(GpsDirectory),GpsDirectory.TagLatitudeRef) },
            { MetadataName.GpsLatitude, (typeof(GpsDirectory), GpsDirectory.TagLatitude) },
            { MetadataName.GpsLongitudeRef, (typeof(GpsDirectory), GpsDirectory.TagLongitudeRef) },
            { MetadataName.GpsLongitude, (typeof(GpsDirectory), GpsDirectory.TagLongitude) },
            { MetadataName.GpsAltitudeRef, (typeof(GpsDirectory), GpsDirectory.TagAltitudeRef) },
            { MetadataName.GpsAltitude, (typeof(GpsDirectory), GpsDirectory.TagAltitude) }
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

        public override string ToString()
        {
            return $"[MetadataTag :: Name={Name}{GetBasePartialToString()}]";
        }

        protected override string GenerateString(Renamer renamer, MediaFile mediaFile)
        {
            (Type directoryType, int tagType) = _enumToTagTypeMap[Name];
            var metadataDirectory = mediaFile.MetadataDirectories.FirstOrDefault(x => x.GetType() == directoryType);

            if (UseRaw)
            {
                return metadataDirectory?.GetString(tagType);
            }

            return metadataDirectory?.GetDescription(tagType);
        }

        #endregion
    }
}
