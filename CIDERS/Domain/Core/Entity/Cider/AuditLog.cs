using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class AuditLog : BaseEntity
{
    [Key] public int Id { get; set; }
    public string? Form { get; set; }
    public string? Description { get; set; }
}
