using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;

public interface IRankRepository
{
    List<ApiRank> All();
    ApiRank? Find(int? id);
    ApiRank? FindByRankId(int? id);
    ApiRank? FindByRankName(string? rankName);
    bool Create(ApiRank entity);
    bool Update(ApiRank entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}
public class ApiRankRepository : IRankRepository
{
    private readonly CiderContext _ciderContext;

    public ApiRankRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<ApiRank> All()
    {
        return (_ciderContext.ApiRank ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public ApiRank? Find(int? id)
    {
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.ApiRank.FindAsync(id).Result;
    }
    public ApiRank? FindByRankName(string? rankName)
    {
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return rankName != null
            ? _ciderContext.ApiRank.FirstOrDefault(a => a.RankName == rankName && a.Active == true)
            : null;
    }

    public ApiRank? FindByRankId(int? id)
    {
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return id != null
            ? _ciderContext.ApiRank.FirstOrDefault(a => a.Id == id && a.Active == true)
            : null;
    }


    public bool Create(ApiRank entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiRank.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiRank entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiRank.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        Console.WriteLine("mise a jour : " + result);
        return result > 0;
    }

    public bool Delete(int? id)
    {
        var entity = Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = false;
        entity.Deleted = true;
        entity.DateDeleted = DateTime.Now;
        entity.DeletedBy = "USER";
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiRank.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Status(int? id, bool active)
    {
        var entity = Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = active;
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiRank == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiRank.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

}