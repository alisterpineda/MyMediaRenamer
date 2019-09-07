using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class NameTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] {AndroidPhoto, new NameTag(), @"android_photo" },
            new object[] {AndroidPhoto, new NameTag{Type=NameTagType.Extension}, @"jpg" },
            new object[] {AndroidPhoto, new NameTag{Type=NameTagType.Full}, @"android_photo.jpg" }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, NameTag testTag, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));

            string actualString = testTag.GetString(TestRenamer, testMediaFile);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
