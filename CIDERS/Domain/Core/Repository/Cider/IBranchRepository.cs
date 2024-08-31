using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;

public interface IBranchRepository
{
    List<Branch> All();
    Branch? Find(int? id);
    Branch? FindByCode(string? code);
    bool Create(Branch entity);
    bool Update(Branch entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}

public class BranchRepository : IBranchRepository
{
    private readonly CiderContext _ciderContext;

    public BranchRepository(CiderContext gtcollectionContext)
    {
        _ciderContext = gtcollectionContext;
    }

    public List<Branch> All()
    {
        return (_ciderContext.Branch ?? throw new Except(ErrorHttp.DbQueryRunFailed)).Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public Branch? Find(int? id)
    {
        if (_ciderContext.Branch == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.Branch.FindAsync(id).Result;
    }

    public Branch? FindByCode(string? code)
    {
        if (_ciderContext.Branch == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.Branch.FirstOrDefault(a => a.Code == code);
    }

    public bool Create(Branch entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        if (_ciderContext.Branch == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.Branch.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result; return result > 0;
    }

    public bool Update(Branch entity)
    {
        entity.DateUpdated = DateTime.Now;
        _ciderContext.Entry(Find(entity.Id) ?? throw new Except(ErrorHttp.DbUpdateError)).CurrentValues.SetValues(entity);
        var result = _ciderContext.SaveChangesAsync().Result; return result > 0;
    }

    public bool Delete(int? id)
    {
        var entity = this.Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = false;
        entity.Deleted = true;
        entity.DateDeleted = DateTime.Now;
        _ciderContext.Entry(Find(entity.Id) ?? throw new Except(ErrorHttp.DbUpdateError)).CurrentValues.SetValues(entity);
        var result = _ciderContext.SaveChangesAsync().Result; return result > 0;
    }

    public bool Status(int? id, bool active)
    {
        var entity = this.Find(id);
        if (id == null || entity == null) throw new Except(ErrorHttp.NotFound);
        entity.Active = active;
        entity.DateUpdated = DateTime.Now;
        _ciderContext.Entry(Find(entity.Id) ?? throw new Except(ErrorHttp.DbUpdateError)).CurrentValues.SetValues(entity);
        var result = _ciderContext.SaveChangesAsync().Result; return result > 0;
    }
}
