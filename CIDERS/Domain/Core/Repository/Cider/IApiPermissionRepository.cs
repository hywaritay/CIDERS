using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace CIDERS.Domain.Core.Repository.Cider;



public interface IApiPermissionRepository
{
    List<ApiPermission> All();
    ApiPermission? Find(int? id);
    ApiPermission? FindByName(string? name);
    bool Create(ApiPermission entity);
    bool Update(ApiPermission entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}

public class ApiPermissionRepository : IApiPermissionRepository
{
    private readonly CiderContext _CiderContext;

    public ApiPermissionRepository(CiderContext CiderContext)
    {
        _CiderContext = CiderContext;
    }

    public List<ApiPermission> All()
    {
        return (_CiderContext.ApiPermission ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted != true).ToListAsync().Result;
    }

    public ApiPermission? Find(int? id)
    {
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _CiderContext.ApiPermission.FindAsync(id).Result;
    }

    public ApiPermission? FindByName(string? name)
    {
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return name != null
            ? _CiderContext.ApiPermission
                .FirstOrDefault(a => a.Name == name && a.Deleted != true)
            : null;
    }

    public bool Create(ApiPermission entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _CiderContext.ApiPermission.Add(entity);
        var result = _CiderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiPermission entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _CiderContext.ApiPermission.Update(entity);
        var result = _CiderContext.SaveChangesAsync().Result;
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
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _CiderContext.ApiPermission.Update(entity);
        var result = _CiderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Status(int? id, bool active)
    {
        var entity = Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = active;
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_CiderContext.ApiPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _CiderContext.ApiPermission.Update(entity);
        var result = _CiderContext.SaveChangesAsync().Result;
        return result > 0;
    }
}
