namespace CIDERS.Domain.Core.Dto.Request;

public class RegisterRequest
{
    public string? ApiKey { get; set; }
    public string? ApiSecret { get; set; }
    public string? Firstname { get; set; }
    public string? Lastname { get; set; }
    public string? Email { get; set; }
    public string? Channel { get; set; }
    public int? ApiTokenMinute { get; set; }
}
