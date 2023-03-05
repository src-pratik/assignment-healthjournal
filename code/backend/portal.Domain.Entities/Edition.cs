using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Domain.Entities
{
    [Table(nameof(Edition), Schema = Constants.Schema)]
    public class Edition : BaseDomainWithAuditObject<Guid?>
    {
        public Guid JournalId { get; set; }
        public Journal Journal { get; set; }
    }
}