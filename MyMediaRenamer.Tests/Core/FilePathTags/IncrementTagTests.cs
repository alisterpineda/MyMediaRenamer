using System;
using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using System.Text;
using MyMediaRenamer.Core;
using MyMediaRenamer.Core.FilePathTags;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core.FilePathTags
{
    public class IncrementTagTests : BaseTestFixture
    {
        private static readonly string[,] _mockFilePaths =
        {
            { @"C:\file00.txt", @"C:\file01.txt", @"C:\file02.txt", @"C:\file03.txt", @"C:\file04.txt" },
            { @"C:\dir1\file10.txt", @"C:\dir1\file11.txt", @"C:\dir1\file12.txt", @"C:\dir1\file13.txt", @"C:\dir1\file14.txt" },
            { @"C:\dir2\file20.txt", @"C:\dir2\file21.txt", @"C:\dir2\file22.txt", @"C:\dir2\file23.txt", @"C:\dir2\file24.txt" }
        };

        private static readonly MockFileSystem mockFileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
        {
            { _mockFilePaths[0,0], new MockFileData("") },
            { _mockFilePaths[0,1], new MockFileData("") },
            { _mockFilePaths[0,2], new MockFileData("") },
            { _mockFilePaths[0,3], new MockFileData("") },
            { _mockFilePaths[0,4], new MockFileData("") },
            { _mockFilePaths[1,0], new MockFileData("") },
            { _mockFilePaths[1,1], new MockFileData("") },
            { _mockFilePaths[1,2], new MockFileData("") },
            { _mockFilePaths[1,3], new MockFileData("") },
            { _mockFilePaths[1,4], new MockFileData("") },
            { _mockFilePaths[2,0], new MockFileData("") },
            { _mockFilePaths[2,1], new MockFileData("") },
            { _mockFilePaths[2,2], new MockFileData("") },
            { _mockFilePaths[2,3], new MockFileData("") },
            { _mockFilePaths[2,4], new MockFileData("") },
        });

        private static object[] GetStringCases =
        {
            new object[]
            {
                new IncrementTag(),
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[0,0],_mockFilePaths[0,0]},
                new List<string>{"0", "1", "2"}
            },
            new object[]
            {
                new IncrementTag{Start = 99},
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[0,0],_mockFilePaths[0,0]},
                new List<string>{"99", "100", "101"}
            },
            new object[]
            {
                new IncrementTag{Step = 5},
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[0,0],_mockFilePaths[0,0]},
                new List<string>{"0", "5", "10"}
            },
            new object[]
            {
                new IncrementTag{Start = 1337, Step = 42},
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[0,0],_mockFilePaths[0,0]},
                new List<string>{"1337", "1379", "1421"}
            },
            new object[]
            {
                new IncrementTag{Format = "E4"},
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[0,0],_mockFilePaths[0,0]},
                new List<string>{ "0.0000E+000", "1.0000E+000", "2.0000E+000" }
            },
            new object[]
            {
                new IncrementTag{Reference = IncrementReference.Directory},
                new List<string>{_mockFilePaths[0,0],_mockFilePaths[1,0],_mockFilePaths[1,1],_mockFilePaths[1,2],_mockFilePaths[2,0],_mockFilePaths[2,1]},
                new List<string>{"0", "0", "1", "2", "0", "1"}
            }
        };

        [TestCaseSource(nameof(GetStringCases))]
        public void GetStringTest(IncrementTag testTag, IList<string> srcPaths, IList<string> expectedStrings)
        {
            Assert.AreEqual(srcPaths.Count, expectedStrings.Count);

            List<string> actualStrings = new List<string>();
            for (int i = 0; i < srcPaths.Count; i++)
            {
                MediaFile testMediaFile = new MediaFile(srcPaths[i]) { FileSystem = mockFileSystem };
                string actualString = testTag.GetString(TestRenamer, testMediaFile);

                Assert.AreEqual(expectedStrings[i], actualString);
            }
            

           

            
        }
    }
}
