namespace portal.IO
{
    public interface IStorageProvider
    {
        public void Upload(Guid guid, Stream data);

        public Stream Download(Guid guid);
    }
}