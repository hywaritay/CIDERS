using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiUser : BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? ApiKey { get; set; }
    public string? ApiSecret { get; set; }
    public int? ApiTokenMinute { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? AccountFeeBank { get; set; }
    public string? AccountFeeCompany { get; set; }
    public string? Channel { get; set; }
    public bool? TransferAnyToAny { get; set; } = false;
    public DateTime? Lastconnect { get; set; }
    public List<ApiUserPermission>? UserPermissions { get; set; }
}
