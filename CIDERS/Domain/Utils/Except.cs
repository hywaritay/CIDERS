namespace CIDERS.Domain.Utils;

public class Except : Exception
{
    public int? Status { get; }
    public override string? Message { get; }
    public int? StatusCode { get; }
    public string? ErrorCode { get; }
    private ApiResultError Apiresulterror { get; }
    public string? Exception { get; }

    public Except(ApiResultError errorHttp, string? exception = null)
    {
        Status = errorHttp.Status;
        Message = (errorHttp.Message ?? ErrorHttp.Error.Message) ?? string.Empty;
        ErrorCode = (errorHttp.ErrorCode ?? ErrorHttp.Error.ErrorCode) ?? string.Empty;
        StatusCode = errorHttp.HttpCode;
        Apiresulterror = errorHttp;
        Exception = exception;
    }

    public ApiResultError GetError()
    {
        return Apiresulterror;
    }
}
