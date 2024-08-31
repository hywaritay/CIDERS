namespace CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

public interface IApiUserPermissionRepository
{
    List<ApiUserPermission> All();
    List<ApiUserPermission> FindByUserId(int? userId);
    ApiUserPermission? Find(int? id);
    bool Create(ApiUserPermission entity);
    bool Update(ApiUserPermission entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}

public class ApiUserPermissionRepository : IApiUserPermissionRepository
{
    private readonly CiderContext _apiinternalContext;

    public ApiUserPermissionRepository(CiderContext apiinternalContext)
    {
        _apiinternalContext = apiinternalContext;
    }

    public List<ApiUserPermission> All()
    {
        return (_apiinternalContext.ApiUserPermission ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public List<ApiUserPermission> FindByUserId(int? userId)
    {
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _apiinternalContext.ApiUserPermission
            .Where(a => a.FkUser != null && a.FkUser.Id == userId && a.Deleted != true).ToListAsync().Result;
    }

    public ApiUserPermission? Find(int? id)
    {
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _apiinternalContext.ApiUserPermission.FindAsync(id).Result;
    }

    public bool Create(ApiUserPermission entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _apiinternalContext.ApiUserPermission.Add(entity);
        var result = _apiinternalContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiUserPermission entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _apiinternalContext.ApiUserPermission.Update(entity);
        var result = _apiinternalContext.SaveChangesAsync().Result;
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
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _apiinternalContext.ApiUserPermission.Update(entity);
        var result = _apiinternalContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Status(int? id, bool active)
    {
        var entity = Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = active;
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_apiinternalContext.ApiUserPermission == null) throw new Except(ErrorHttp.DbCreateError);
        _apiinternalContext.ApiUserPermission.Update(entity);
        var result = _apiinternalContext.SaveChangesAsync().Result;
        return result > 0;
    }
}
