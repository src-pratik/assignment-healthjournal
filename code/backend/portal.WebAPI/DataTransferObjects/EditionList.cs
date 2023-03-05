namespace portal.WebAPI.DataTransferObjects
{
    public class EditionList
    {
        public Guid Id { get; set; }
        public string JournalName { get; set; }
        public Guid JournalId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}