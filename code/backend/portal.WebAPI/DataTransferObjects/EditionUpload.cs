using Microsoft.AspNetCore.Http;

namespace portal.WebAPI.DataTransferObjects
{
    public class EditionUpload
    {
        public Guid? Id { get; set; }
        public Guid JournalId { get; set; }

        public IFormFile File { get; set; }
    }
}