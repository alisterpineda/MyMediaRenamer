using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class DateTimeTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] {TextFile, new DateTimeTag(), null},
            new object[] {AndroidPhoto, new DateTimeTag(), @"2016-06-17 13:07:00Z" },
            new object[] {IosPhoto, new DateTimeTag(), @"2017-08-26 16:00:19Z" },
            new object[] {IosVideoMp4, new DateTimeTag(), @"2014-05-24 07:17:35Z"},
            new object[] {IosVideoMov, new DateTimeTag(), @"2018-06-15 20:11:45Z"},
            new object[] {IosPhoto, new DateTimeTag{Format = "yyyyMMdd_HHmmss_fff" }, @"20170826_160019_349" }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, DateTimeTag testTag, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));

            string actualString = testTag.GetString(TestRenamer, testMediaFile);
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
