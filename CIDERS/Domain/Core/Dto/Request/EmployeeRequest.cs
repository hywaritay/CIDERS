using CIDERS.Domain.Core.Entity.Cider;

namespace CIDERS.Domain.Core.Dto.Request
{
    public class EmployeeRequest
    {
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
        public string? DepartureReason { get; set; }
        public string? imgProfile { get; set; }
        public int? FkRank { get; set; }
        public int? FkDept { get; set; }
        public int? FkLoc { get; set; }
    }
}
