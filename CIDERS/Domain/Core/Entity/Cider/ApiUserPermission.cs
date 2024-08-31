using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiUserPermission : BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? Name { get; set; }
    public ApiUser? FkUser { get; set; }
    public ApiPermission? FkPermission { get; set; }
}
