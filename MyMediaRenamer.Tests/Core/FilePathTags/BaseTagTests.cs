using System;
using System.Collections.Generic;
using System.Text;
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
            new object[] { "", testString, testString },
            new object[] { "maxlength=3", testString, testString.Substring(0,3) }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string tagOptionsString, string sourceString, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(IosPhoto));
            TextTag testTag = new TextTag(tagOptionsString){Text=sourceString};

            string actualString = testTag.GetString(TestMediaRenamer, testMediaFile);

            Assert.AreEqual(expectedString, actualString);
        }
    }
}
