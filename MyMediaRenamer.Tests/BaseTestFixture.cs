using System.IO;
using MyMediaRenamer.Core;
using NUnit.Framework;

namespace MyMediaRenamer.Tests
{
    public abstract class BaseTestFixture
    {
        protected const string TestDataFolderName = "Test Data";

        #region Android

        protected const string AndroidPhoto = "android_photo.jpg";
        protected const string AndroidVideoMp4 = "android_video.mp4";

        #endregion

        #region iOS

        protected const string IosPhoto = "ios_photo.jpg";
        protected const string IosVideoMp4 = "ios_video.mp4";
        protected const string IosVideoMov = "ios_video.mov";

        #endregion

        protected const string TextFile = "text.txt";

        protected static Renamer TestRenamer;

        #region Properties

        protected static string TestDataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "Test Data");

        protected static string TempDataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "Temp Data");

        #endregion

        #region Methods

        [SetUp]
        public void SetUp()
        {
            TestRenamer = new Renamer();

            if (!Directory.Exists(TempDataDirectory))
                Directory.CreateDirectory(TempDataDirectory);
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(TempDataDirectory))
            {
                foreach (FileInfo file in new DirectoryInfo(TempDataDirectory).GetFiles())
                {
                    file.Delete();
                }
            }

        }

        protected static string GetTestDataFilePath(string fileName)
        {
            return Path.Combine(TestDataDirectory, fileName);
        }

        #endregion
    }
}