using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;


public interface IApiUserRepository
{
    List<ApiUser> All();
    ApiUser? Find(int? id);
    ApiUser? FindByApiKey(string? apiKey);
    ApiUser? FindByEmail(string? email);
    bool Create(ApiUser entity);
    bool Update(ApiUser entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}

public class ApiUserRepository : IApiUserRepository
{
    private readonly CiderContext _ciderContext;

    public ApiUserRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<ApiUser> All()
    {
        return (_ciderContext.ApiUser ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public ApiUser? Find(int? id)
    {
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.ApiUser.FindAsync(id).Result;
    }

    public ApiUser? FindByApiKey(string? apiKey)
    {
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return apiKey != null
            ? _ciderContext.ApiUser.FirstOrDefault(a => a.ApiKey == apiKey && a.Active == true)
            : null;
    }

    public ApiUser? FindByEmail(string? email)
    {
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return email != null
            ? _ciderContext.ApiUser.FirstOrDefault(a => a.Email == email && a.Active == true)
            : null;
    }

    public bool Create(ApiUser entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiUser.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiUser entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiUser.Update(entity);
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
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiUser.Update(entity);
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
        if (_ciderContext.ApiUser == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiUser.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }
}
