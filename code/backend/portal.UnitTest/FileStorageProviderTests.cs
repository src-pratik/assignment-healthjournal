using portal.IO.Provider;
using System;
using System.IO;
using System.Text;
using Xunit;

namespace portal.UnitTest
{
    public class FileStorageProviderTests : IDisposable
    {
        private readonly string _testFolderPath;
        private string _filePathToDelete;

        public FileStorageProviderTests()
        {
            // Create a temporary test folder for storing test files
            _testFolderPath = Path.Combine(Path.GetTempPath(), "MyProjectTests");
        }

        [Fact]
        public void FileStorageProvider_UploadFile_ShouldPass()
        {
            var fileName = new Guid("be459970-1cee-4b18-8546-7be4a3ef2bb9");
            var fileContent = "This is a test file.";
            IStorageProvider storageProvider = new FileStorageProvider(_testFolderPath);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
            {
                storageProvider.Upload(fileName, ms);
            }

            var filePath = Path.Combine(_testFolderPath, fileName.ToString());

            _filePathToDelete = filePath;

            Assert.True(File.Exists(filePath));
            var storedFileContent = File.ReadAllText(filePath);
            Assert.Equal(fileContent, storedFileContent);
        }

        [Fact]
        public void FileStorageProvider_DownloadFile_ShouldPass()
        {
            var fileName = new Guid("6dafc832-c440-442e-9d6b-2f7704c15d02");
            var fileContent = "This is a test file. Created For Download";
            IStorageProvider storageProvider = new FileStorageProvider(_testFolderPath);

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(fileContent)))
            {
                storageProvider.Upload(fileName, ms);
            }

            var fileData = storageProvider.Download(fileName);
            string downloadContent;

            using (var ms = new MemoryStream())
            {
                fileData.CopyTo(ms);
                downloadContent = Encoding.UTF8.GetString(ms.ToArray());
            }

            _filePathToDelete = Path.Combine(_testFolderPath, fileName.ToString());

            Assert.Equal(fileContent, downloadContent);
        }

        [Fact]
        public void FileStorageProvider_DownloadFile_ShouldFail()
        {
            var fileName = new Guid("f516e17b-171f-4ae8-946a-86affb9aee99");
            IStorageProvider storageProvider = new FileStorageProvider(_testFolderPath);

            Assert.Throws<FileNotFoundException>(() => storageProvider.Download(fileName));
        }

        public void Dispose()
        {
            try
            {
                if (string.IsNullOrEmpty(_filePathToDelete) == false)
                    File.Delete(_filePathToDelete);
            }
            catch (Exception ex)
            {
            }
        }
    }
}