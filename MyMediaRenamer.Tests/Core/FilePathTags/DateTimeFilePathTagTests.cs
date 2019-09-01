using System;
using System.Collections.Generic;
using System.Text;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class DateTimeFilePathTagTests : BaseTestFixture
    {
        private static object[] GetStringCases =
        {
            new object[] {AndroidPhoto, "", @"2016-06-17 13:07:00Z" },
            new object[] {IosPhoto, "", @"2017-08-26 16:00:19Z" },
            new object[] {IosVideoMp4, "", @"2014-05-24 07:17:35Z"},
            new object[] {IosVideoMov, "", @"2018-06-15 20:11:45Z"},
            new object[] {IosPhoto, "format=yyyyMMdd_HHmmss_fff", @"20170826_160019_349" }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(string fileName, string tokenOptions, string expectedString)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(fileName));
            DateTimeFilePathTag testDateTimeToken = new DateTimeFilePathTag(tokenOptions);

            string actualString = testDateTimeToken.GetString(TestMediaRenamer, testMediaFile);
            Assert.AreEqual(expectedString, actualString);
        }
    }
}
