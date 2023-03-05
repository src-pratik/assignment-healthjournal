using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace portal.Domain.Entities
{
    [Table(nameof(Journal), Schema = Constants.Schema)]
    public class Journal : BaseDomainWithAuditObject<Guid?>
    {
        public string Name { get; set; }
        public virtual ICollection<Edition> Editions { get; set; }
    }
}