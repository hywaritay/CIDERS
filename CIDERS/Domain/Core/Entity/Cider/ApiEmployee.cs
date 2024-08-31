using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiEmployee:BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? ForceNumber { get; set; }
    public string? PinNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public DateTime? DOB { get; set; }
    public string? Mobile { get; set; }
    public string? Email { get; set; }
    public DateTime? EntryDate { get; set; }
    public DateTime? DepartureDate { get; set; }
    public string? imgProfile { get; set; }
    public string? DepartureReason { get; set; }
    public ApiDepartment? FkDept { get; set; }
    public ApiRank? FkRank { get; set; }
    public ApiLocation? FkLocation { get; set; }

}
