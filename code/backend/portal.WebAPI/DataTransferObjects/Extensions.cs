namespace portal.WebAPI.DataTransferObjects
{
    public static class Extensions
    {
        public static DataTransferObjects.JournalDto ToDTO(this Domain.Entities.Journal journal) => new DataTransferObjects.JournalDto
        {
            Id = journal.Id,
            Name = journal.Name
        };

        public static Domain.Entities.Journal FromDTO(this DataTransferObjects.JournalDto dto) => new Domain.Entities.Journal
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}