using System;

namespace StockManagement.Core.Entities
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; } = false;
    }
}
