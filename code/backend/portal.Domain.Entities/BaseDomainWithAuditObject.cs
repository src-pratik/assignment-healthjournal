using System;

namespace portal.Domain.Entities
{
    public abstract class BaseDomainWithAuditObject<T> : BaseDomainObject<T>
    {
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public RecordStatus Status { get; set; } = RecordStatus.Active;
    }
}