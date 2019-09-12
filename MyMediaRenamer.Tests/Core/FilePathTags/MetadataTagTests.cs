using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class MetadataTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] { IosPhoto, new MetadataTag(), null },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifOrientation}, "Top, left side (Horizontal / normal)" },
            new object[] { IosPhoto, new MetadataTag{Name = MetadataName.ExifOrientation, UseRaw = true}, "1" },
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
