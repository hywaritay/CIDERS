namespace CIDERS.Domain.Core.Entity.Cider;

public class BaseEntity
{
    public bool? Active { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateDeleted { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
    public string? DeletedBy { get; set; }
    public string? DeletedReason { get; set; }
}
