using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core
{
    public class RenamerTests : BaseTestFixture
    {
        private static object[] GetNewFilePathCases =
        {
            new object[]
            {
                new Renamer(),
                new List<BaseTag>
                {
                    new TextTag{Text="abc"}
                },
                @"abc.jpg"
            },
            new object[]
            {
                new Renamer{PreserveExtension = false},
                new List<BaseTag>
                {
                    new TextTag{Text="abc"}
                },
                @"abc"
            },
            new object[]
            {
                new Renamer(),
                new List<BaseTag>
                {
                    new MetadataTag()
                },
                @"null.jpg"
            },
            new object[]
            {
                new Renamer{SkipOnNullTag = true},
                new List<BaseTag>
                {
                    new MetadataTag()
                },
                null
            },
        };

        [TestCaseSource(nameof(GetNewFilePathCases))]
        public void GetNewFilePathTest(Renamer testRenamer, IEnumerable<BaseTag> testTags, string expectedFileName)
        {
            MediaFile testMediaFile = new MediaFile(GetTestDataFilePath(IosPhoto));
            bool result = testRenamer.TryGetNewFilePath(testMediaFile, testTags, out string actualPath);

            if (expectedFileName != null)
                Assert.AreEqual(GetTestDataFilePath(expectedFileName), actualPath);
            else
                Assert.IsFalse(result);
        }
    }
}
