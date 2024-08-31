using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;

public interface ILocationRespository
{
    List<ApiLocation> All();
    ApiLocation? Find(int? id);
    ApiLocation? FindByLocationId(int? id);
    ApiLocation? FindByLocationName(string? LocationName);
    bool Create(ApiLocation entity);
    bool Update(ApiLocation entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}
public class ApiLocationRepository : ILocationRespository
{
    private readonly CiderContext _ciderContext;

    public ApiLocationRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<ApiLocation> All()
    {
        return (_ciderContext.ApiLocation ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public ApiLocation? Find(int? id)
    {
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.ApiLocation.FindAsync(id).Result;
    }
    public ApiLocation? FindByLocationName(string? LocationName)
    {
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return LocationName != null
            ? _ciderContext.ApiLocation.FirstOrDefault(a => a.District == LocationName && a.Active == true)
            : null;
    }

    public ApiLocation? FindByLocationId(int? id)
    {
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return id != null
            ? _ciderContext.ApiLocation.FirstOrDefault(a => a.Id == id && a.Active == true)
            : null;
    }


    public bool Create(ApiLocation entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiLocation.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiLocation entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiLocation.Update(entity);
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
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiLocation.Update(entity);
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
        if (_ciderContext.ApiLocation == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiLocation.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

}
