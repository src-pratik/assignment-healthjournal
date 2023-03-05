using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Domain.Entities
{
    [Table(nameof(Subscription), Schema = Constants.Schema)]
    public class Subscription : BaseDomainWithAuditObject<Guid?>
    {
        public string UserId { get; set; }
        public Guid JournalId { get; set; }
        public virtual Journal Journal { get; set; }
    }
}