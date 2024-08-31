namespace CIDERS.Domain.Utils
{
    public static class ErrorHttp
    {
        public static readonly ApiResultError Error = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Error application" };

        public static readonly ApiResultError NotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Object not found" };

        public static readonly ApiResultError DbOpenConnectionFailed = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Database can't open" };

        public static readonly ApiResultError DbQueryRunFailed = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Database query failed" };

        public static readonly ApiResultError DbCreateError = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Failed to create" };

        public static readonly ApiResultError DbErrorObjectEmpty = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Object empty, they is no field" };

        public static readonly ApiResultError DbUpdateError = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Object not found" };

        public static readonly ApiResultError DbFindError = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Object not found" };

        public static readonly ApiResultError DbStatusError = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Status failed" };

        public static readonly ApiResultError DbDeleteError = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Delete failed" };

        public static readonly ApiResultError NotEncoded = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Data not encoded" };

        public static readonly ApiResultError UserNotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "User not found" };

        public static readonly ApiResultError UsernameUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Username already used by another existing account"
        };

        public static readonly ApiResultError EmailUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Email address already used by another existing account"
        };

        public static readonly ApiResultError TokenExpired = new()
        { Status = 0, ErrorCode = "ERROR", Message = "token expired" };

        public static readonly ApiResultError Unauthorized = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Unauthorized" };

        public static readonly ApiResultError BasicCredentialMissed = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Basic credentials missed" };

        public static readonly ApiResultError BadBasicCredential = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Bad Basic credentials" };

        public static readonly ApiResultError BadCredentials = new()
        { Status = 0, ErrorCode = "ERROR", Message = "bad credentials" };

        public static readonly ApiResultError RankNotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Rank not found" };

        public static readonly ApiResultError RankUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Rank already added"
        };
        
        public static readonly ApiResultError DeptNotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Department not found" };

        public static readonly ApiResultError DeptUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Department already added"
        };

        public static readonly ApiResultError LocationNotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Location not found" };

        public static readonly ApiResultError LocationUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Location already added"
        };

        public static readonly ApiResultError EmployeeNotFound = new()
        { Status = 0, ErrorCode = "ERROR", Message = "Employee not found" };

        public static readonly ApiResultError EmployeeUsed = new()
        {
            Status = 0,
            ErrorCode = "ERROR",
            Message = "Employee already added"
        };



    }
}
