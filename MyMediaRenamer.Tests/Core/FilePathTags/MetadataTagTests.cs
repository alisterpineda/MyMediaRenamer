using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class MetadataTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            //-- Functionality tests --//
            new object[] { TextFile, new MetadataTag(), null },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifOrientation}, "Top, left side (Horizontal / normal)" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifOrientation, UseRaw = true}, "1" },

            //-- Mapping Tests --//
            // Exif IFD0
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifMake}, "Apple" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifModel}, "iPhone 7 Plus" },
            // Exif SubIFD
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifExposureTime}, "1/349 sec" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifFNumber}, "f/2.8" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifExposureProgram}, "Program normal" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifIso}, "20" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifVersion}, "2.21" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifDateTimeOriginal}, "2017:08:26 16:00:19" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifDateTimeDigitized}, "2017:08:26 16:00:19" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifComponentsConfiguration}, "YCbCr" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifShutterSpeed}, "1/349 sec" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifBrightness}, "9.225" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifExposureBias}, "0 EV" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifMeteringMode}, "Spot" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifFlash}, "Flash did not fire, auto" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifFocalLength}, "6.6 mm" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSubjectLocation}, null }, // TODO MetadataExtractor :: TagSubjectLocation vs TagSubjectLocationTiffEp???            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSubSecTimeOriginal}, "349" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSubSecTimeDigitized}, "349" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifColorSpace}, "Undefined" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifImageWidth}, "4032 pixels" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifImageHeight}, "3024 pixels" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSensingMethod}, "One-chip color area sensor" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSceneType}, "Directly photographed image" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifExposureMode}, "Auto exposure" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifWhiteBalanceMode}, "Auto white balance" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifFocalLengthIn35}, "57 mm" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifSceneCaptureType}, "Standard" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifLensSpecification}, "3.99-6.6mm f/1.8-2.8" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifLensMake}, "Apple" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifLensModel}, "iPhone 7 Plus back dual camera 6.6mm f/2.8" },

            // Exif GPS
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsLatitudeRef}, "N" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsLatitude}, "12° 0' 16.6\"" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsLongitudeRef}, "E" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsLongitude}, "120° 12' 17.24\"" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsAltitudeRef}, "Sea level" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.GpsAltitude}, "200.23 metres" },
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, MetadataTag testTag, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));

            string actualString = testTag.GetString(TestRenamer, testMediaFile);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
