using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core
{
    public class RenamerTests : BaseTestFixture
    {
        private static readonly MediaFile testMediaFile = new MediaFile(@"C:\image.jpg")
        {
            FileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {@"C:\image.jpg", new MockFileData("") }
            })
        };

        private static object[] GetNewFilePathCases =
        {
            new object[]
            {
                new Renamer(),
                new List<BaseTag>
                {
                    new TextTag{Text="abc"}
                },
                @"C:\abc.jpg"
            },
            new object[]
            {
                new Renamer{PreserveExtension = false},
                new List<BaseTag>
                {
                    new TextTag{Text="abc"}
                },
                @"C:\abc"
            }
        };

        [TestCaseSource(nameof(GetNewFilePathCases))]
        public void GetNewFilePathTest(Renamer testRenamer, IEnumerable<BaseTag> testTags, string expectedPath)
        {
            string actualPath = testRenamer.GetNewFilePath(testMediaFile, testTags);

            Assert.AreEqual(expectedPath, actualPath);
        }
    }
}
