using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;

public interface IAuditLogRepository
{
    List<AuditLog> All();
    AuditLog? Find(int? id);
    bool Create(AuditLog auditLog);
    bool Update(AuditLog auditLog);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}

public class AuditLogRepository : IAuditLogRepository
{
    private readonly CiderContext _ciderContext;

    public AuditLogRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<AuditLog> All()
    {
        return (_ciderContext.AuditLog ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public AuditLog? Find(int? id)
    {
        if (_ciderContext.AuditLog == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.AuditLog.FindAsync(id).Result;
    }

    public bool Create(AuditLog entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.AuditLog == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.AuditLog.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(AuditLog entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.AuditLog == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.AuditLog.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
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
        if (_ciderContext.AuditLog == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.AuditLog.Update(entity);
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
        if (_ciderContext.AuditLog == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.AuditLog.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }
}