namespace portal.WebAPI.DataTransferObjects
{
    public static class Extensions
    {
        public static DataTransferObjects.Journal ToDTO(this Domain.Entities.Journal journal) => new DataTransferObjects.Journal
        {
            Id = journal.Id,
            Name = journal.Name
        };

        public static Domain.Entities.Journal FromDTO(this DataTransferObjects.Journal dto) => new Domain.Entities.Journal
        {
            Id = dto.Id,
            Name = dto.Name
        };
    }
}