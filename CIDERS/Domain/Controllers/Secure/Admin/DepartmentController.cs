using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
namespace CIDERS.Domain.Controllers.Secure.Admin;



[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.Dept.Base)]
public class DepartmentController : ControllerBase
{
    private readonly IDepartmentRepository _deptRepository;
    private readonly ILogger<DepartmentController> _logger;

    public DepartmentController(IDepartmentRepository deptRepository, ILogger<DepartmentController> logger)
    {
        _deptRepository = deptRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Dept.All)]
    public async Task<ApiResult> Depts()
    {
        try
        {
            var depts = _deptRepository.All();
            var ranksResult = new List<ApiDepartment>();
            foreach (var dept in depts)
            {
                ranksResult.Add(dept);
            }
            return await Task.FromResult(ApiResponse.Success(ranksResult));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Dept.Find)]
    [HttpGet]
    public async Task<ApiResult> FindRank(int id)
    {
        try
        {
            var result = _deptRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Dept.Create)]
    public async Task<ApiResult> CreateDept(DeptRequest data)
    {
        try
        {
            if (_deptRepository.FindBydeptName(data.DeptName) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.DeptUsed));

            var dept = new ApiDepartment
            {
                DeptName = data.DeptName,
            };

            var result = _deptRepository.Create(dept);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var rankSaved = _deptRepository.FindBydeptName(data.DeptName);
            if (rankSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Dept.Update)]
    [HttpPost]
    public async Task<ApiResult> UpdateRank(ApiDepartment data)
    {
        try
        {
            var dept = _deptRepository.FindByDeptId(data.Id);
            if (dept == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DeptNotFound));

            dept.DeptName = data.DeptName;

            var result = _deptRepository.Update(dept);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DbUpdateError));

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }


    [Route(Routes.ApiRoute.Secure.Admin.Dept.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeleteRank(int id)
    {
        try
        {
            var result = _deptRepository.Delete(id);
            return await Task.FromResult(result
                ? ApiResponse.Success(new object())
                : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Dept.State)]
    [HttpGet]
    public async Task<ApiResult> StateRank(int id, bool active)
    {
        try
        {
            var result = _deptRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}