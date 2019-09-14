using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class HashTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] { IosPhoto, new HashTag(), "74ad3ec7f12ea444e0a945822f40e908" },
            new object[] { IosPhoto, new HashTag { Algorithm = Algorithm.SHA1 }, "47eb7822bf9d44cb2803602c49a384445a23bf23" },
            new object[] { IosPhoto, new HashTag { Algorithm = Algorithm.SHA256 }, "aff08d673acd18c6ca354a336aa1bdda35a681f1fe46d5572fc70787aed50c7f" },
            new object[] { IosPhoto, new HashTag { Algorithm = Algorithm.SHA384 }, "f79f752fc16923723d3c3380cb657510df687d9a3d54121c7bc93273fea73670e8dd6ce386db66ea914b81ed499e27e2" },
            new object[] { IosPhoto, new HashTag { Algorithm = Algorithm.SHA512 }, "c0533f574eaa52e1163038d2bce3fb6ac5db39b5668c6eb62c64e6bb40d96b6caed4b50aeae34cec89a6534decf3396731786e2b40c09cfa67a9cab39b8b5acc" },
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, HashTag testTag, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));

            string actualString = testTag.GetString(TestRenamer, testMediaFile);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}