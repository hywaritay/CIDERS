using CIDERS.Domain.Core.Dto.Response;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CIDERS.Domain.Utils;

public class PaginateResponse<T> : DataPaginateResponse<T> // List<T>
{
    private PaginateResponse(IEnumerable<T> items, int count, int pageIndex, int pageSize)
    {
        PageIndex = pageIndex;
        Pages = (int)Math.Ceiling(count / (double)pageSize);

        Data.AddRange(items);
    }

    public bool HasPreviousPage => PageIndex > 1;
    public bool HasNextPage => PageIndex < Pages;

    public static async Task<PaginateResponse<T>> CreateAsync(IQueryable<T> source, int? pageIndex, int? pageSize)
    {
        var pIndex = (pageIndex is > 0 ? pageIndex : 1) ?? 1;
        var pSize = (pageSize is > 0 ? pageSize : 50) ?? 50;
        var count = await source.CountAsync();
        var items = await source.Skip((pIndex - 1) * pSize).Take(pSize).ToListAsync();
        return new PaginateResponse<T>(items, count, pIndex, pSize);
    }
}