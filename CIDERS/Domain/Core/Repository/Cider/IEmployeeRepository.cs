using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;



public interface IEmployeeRepository
{
    List<ApiEmployee> All();
    ApiEmployee? Find(int? id);
    ApiEmployee? FindByEmpId(int? id);
    ApiEmployee? FindByEmpName(string? deptName);
    bool Create(ApiEmployee entity);
    bool Update(ApiEmployee entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}
public class ApiEmployeeRepository : IEmployeeRepository
{
    private readonly CiderContext _ciderContext;

    public ApiEmployeeRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<ApiEmployee> All()
    {
        return (_ciderContext.ApiEmployee ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public ApiEmployee? Find(int? id)
    {
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.ApiEmployee.FindAsync(id).Result;
    }
    public ApiEmployee? FindByEmpName(string? pinNumber)
    {
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return pinNumber != null
            ? _ciderContext.ApiEmployee.FirstOrDefault(a => a.PinNumber == pinNumber && a.Active == true)
            : null;
    }

    public ApiEmployee? FindByEmpId(int? id)
    {
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return id != null
            ? _ciderContext.ApiEmployee.FirstOrDefault(a => a.Id == id && a.Active == true)
            : null;
    }


    public bool Create(ApiEmployee entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiEmployee.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiEmployee entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiEmployee.Update(entity);
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
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiEmployee.Update(entity);
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
        if (_ciderContext.ApiEmployee == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiEmployee.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

}