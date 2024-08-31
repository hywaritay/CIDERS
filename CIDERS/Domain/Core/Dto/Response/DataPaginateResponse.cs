namespace CIDERS.Domain.Core.Dto.Response
{
    public class DataPaginateResponse<T>
    {
        public List<T>? Data { get; set; } = new();
        public int? Pages { get; set; } = null;
        public int? PageIndex { get; set; } = null;
    }
}
