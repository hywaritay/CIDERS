using System.ComponentModel.DataAnnotations;

namespace CIDERS.Domain.Core.Entity.Cider;

public class ApiRank: BaseEntity
{
    [Key] public int? Id { get; set; }
    public string? RankName { get; set; }
}
