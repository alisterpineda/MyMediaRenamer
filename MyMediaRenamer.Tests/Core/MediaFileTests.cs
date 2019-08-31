using System.Collections.Generic;
using System.IO.Abstractions.TestingHelpers;
using MyMediaRenamer.Core;
using NUnit.Framework;

namespace MyMediaRenamer.Tests.Core
{
    class MediaFileTests
    {
        public static object[] RenamingConflictCases =
        {
            new object[] {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {@"C:\image.jpg", new MockFileData("") }
                }),
                @"C:\image.jpg",
                @"C:\image (1).jpg"
            },
            new object[] {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {@"C:\image.jpg", new MockFileData("") },
                    {@"C:\image (1).jpg", new MockFileData("") }
                }),
                @"C:\image.jpg",
                @"C:\image (2).jpg"
            },
            new object[] {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {@"C:\image.jpg", new MockFileData("") },
                    {@"C:\image (1).jpg", new MockFileData("") },
                    {@"C:\image (2).jpg", new MockFileData("") },
                    {@"C:\image (3).jpg", new MockFileData("") },
                    {@"C:\image (4).jpg", new MockFileData("") },
                    {@"C:\image (5).jpg", new MockFileData("") },
                    {@"C:\image (6).jpg", new MockFileData("") },
                    {@"C:\image (7).jpg", new MockFileData("") },
                    {@"C:\image (8).jpg", new MockFileData("") },
                    {@"C:\image (9).jpg", new MockFileData("") }
                }),
                @"C:\image.jpg",
                @"C:\image (10).jpg"
            },
            new object[] {
                new MockFileSystem(new Dictionary<string, MockFileData>
                {
                    {@"C:\some\directory\path\image.jpg", new MockFileData("") }
                }),
                @"C:\some\directory\path\image.jpg",
                @"C:\some\directory\path\image (1).jpg"
            }
        };

        [TestCaseSource(nameof(RenamingConflictCases))]
        public void RenamingConflictTest(MockFileSystem mockFileSystem, string srcPath, string expNewPath)
        {
            MediaFile mediaFile = new MediaFile(srcPath){FileSystem = mockFileSystem};
            mediaFile.Rename(srcPath);

            Assert.IsTrue(mockFileSystem.File.Exists(expNewPath));
        }
    }
}
