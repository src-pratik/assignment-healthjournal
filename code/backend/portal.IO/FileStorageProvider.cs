using System;
using System.IO;

namespace portal.IO
{
    public class FileStorageProvider : IStorageProvider
    {
        private string _rootFolder;

        public FileStorageProvider(string rootFolder)
        {
            _rootFolder = rootFolder;
        }

        Stream IStorageProvider.Download(Guid guid)
        {
            string path = Path.Combine(_rootFolder, guid.ToString());
            return File.Open(path, FileMode.Open);
        }

        void IStorageProvider.Upload(Guid guid, Stream data)
        {
            string path = Path.Combine(_rootFolder, guid.ToString());
            using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
            {
                data.CopyTo(outputFileStream);
            }
        }
    }
}