using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class BaseTagTests : BaseTestFixture
    {
        private static string testString = " abcdefghijklmNOPQRSTUVWXYZ ";

        private static object[] GetStringCases =
        {
            new object[] { new TextTag(), testString },
            new object[] { new TextTag{MaxLength = 3}, testString.Substring(0,3) },
            new object[] { new TextTag{ForceCase = ForceCase.Lower}, testString.ToLower() },
            new object[] { new TextTag{ForceCase = ForceCase.Upper}, testString.ToUpper() }
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
