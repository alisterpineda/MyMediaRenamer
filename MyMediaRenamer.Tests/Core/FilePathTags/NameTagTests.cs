using System;
using System.Collections.Generic;
using System.Text;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class NameTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] {AndroidPhoto, "", @"android_photo" },
            new object[] {AndroidPhoto, "type=name", @"android_photo" },
            new object[] {AndroidPhoto, "type=extension", @"jpg" },
            new object[] {AndroidPhoto, "type=full", @"android_photo.jpg" }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, string tokenOptions, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));
            NameTag testTag = new NameTag(tokenOptions);

            string actualString = testTag.GetString(TestMediaRenamer, testMediaFile);
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
