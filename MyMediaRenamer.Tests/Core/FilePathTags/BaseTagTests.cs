using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class BaseTagTests : BaseTestFixture
    {
        private static string testString = "abcdefghijklmnopqrstuvwxyz";

        private static object[] GetStringCases =
        {
            new object[] { new TextTag(), testString },
            new object[] { new TextTag{MaxLength = 3}, testString.Substring(0,3) }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(TextTag testTag, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(IosPhoto));
            testTag.Text = testString;

            string actualString = testTag.GetString(TestRenamer, testMediaFile);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
