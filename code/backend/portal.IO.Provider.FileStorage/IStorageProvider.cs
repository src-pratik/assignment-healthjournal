using System;
using System.IO;

namespace portal.IO.Provider
{
    public interface IStorageProvider
    {
        public void Upload(Guid guid, Stream data);

        public Stream Download(Guid guid);
    }
}