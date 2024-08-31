using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiDepartment : BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? DeptName { get; set; }

   
}
