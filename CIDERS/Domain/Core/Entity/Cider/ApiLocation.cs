using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiLocation : BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? District { get; set; }
}
