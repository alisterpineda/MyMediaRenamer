using System.IO;
using MyMediaRenamer.Core;
using NUnit.Framework;

namespace MyMediaRenamer.Tests
{
    public abstract class BaseTestFixture
    {
        protected static readonly MediaRenamer TestMediaRenamer = new MediaRenamer();
        protected static string TestDataFolderName = "Test Data";

        #region Android

        protected static string AndroidPhoto = "android_photo.jpg";

        protected static string AndroidVideoMp4 = "android_video.mp4";

        #endregion

        #region iOS

        protected static string IosPhoto = "ios_photo.jpg";

        protected static string IosVideoMp4 = "ios_video.mp4";
        protected static string IosVideoMov = "ios_video.mov";

        #endregion

        #region Properties

        protected string TestDataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "Test Data");

        protected string TempDataDirectory => Path.Combine(TestContext.CurrentContext.TestDirectory, "Temp Data");

        #endregion

        #region Methods

        [SetUp]
        public void SetUp()
        {
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

        protected string GetTestDataFilePath(string fileName)
        {
            return Path.Combine(TestDataDirectory, fileName);
        }

        #endregion
    }
}