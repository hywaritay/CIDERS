using CIDERS.Domain.Core.Db;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace CIDERS.Domain.Core.Repository.Cider;


public interface IDepartmentRepository
{
    List<ApiDepartment> All();
    ApiDepartment? Find(int? id);
    ApiDepartment? FindByDeptId(int? id);
    ApiDepartment? FindBydeptName(string? deptName);
    bool Create(ApiDepartment entity);
    bool Update(ApiDepartment entity);
    bool Delete(int? id);
    bool Status(int? id, bool active);
}
public class ApiDepartmentRepository : IDepartmentRepository
{
    private readonly CiderContext _ciderContext;

    public ApiDepartmentRepository(CiderContext ciderContext)
    {
        _ciderContext = ciderContext;
    }

    public List<ApiDepartment> All()
    {
        return (_ciderContext.ApiDepartment ?? throw new Except(ErrorHttp.DbQueryRunFailed))
            .Where(a => a.Deleted == false || a.Deleted == null).ToListAsync().Result;
    }

    public ApiDepartment? Find(int? id)
    {
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return _ciderContext.ApiDepartment.FindAsync(id).Result;
    }
    public ApiDepartment? FindBydeptName(string? deptName)
    {
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return deptName != null
            ? _ciderContext.ApiDepartment.FirstOrDefault(a => a.DeptName == deptName && a.Active == true)
            : null;
    }

    public ApiDepartment? FindByDeptId(int? id)
    {
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbQueryRunFailed);
        return id != null
            ? _ciderContext.ApiDepartment.FirstOrDefault(a => a.Id == id && a.Active == true)
            : null;
    }


    public bool Create(ApiDepartment entity)
    {
        entity.Active = true;
        entity.Deleted = false;
        entity.DateCreated = DateTime.Now;
        entity.CreatedBy = "USER";
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiDepartment.Add(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

    public bool Update(ApiDepartment entity)
    {
        entity.DateUpdated = DateTime.Now;
        entity.UpdatedBy = "USER";
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiDepartment.Update(entity);
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
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiDepartment.Update(entity);
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
        if (_ciderContext.ApiDepartment == null) throw new Except(ErrorHttp.DbCreateError);
        _ciderContext.ApiDepartment.Update(entity);
        var result = _ciderContext.SaveChangesAsync().Result;
        return result > 0;
    }

}