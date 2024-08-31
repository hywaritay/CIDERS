using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIDERS.Domain.Controllers.Secure.Admin;


//[Authorize(Roles = Consts.Vars.Roles.Admin)]
[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.Permission.Base)]
public class AdminPermissionController : ControllerBase
{
    private readonly IApiPermissionRepository _apiPermissionRepository;
    private readonly ILogger<AdminPermissionController> _logger;

    public AdminPermissionController(IApiPermissionRepository apiPermissionRepository, ILogger<AdminPermissionController> logger)
    {
        _apiPermissionRepository = apiPermissionRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Permission.All)]
    public async Task<ApiResult> Permissions()
    {
        try
        {
            var users = _apiPermissionRepository.All();
            return await Task.FromResult(ApiResponse.Success(users));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Permission.Find)]
    public async Task<ApiResult> FindPermission(int id)
    {
        try
        {
            var result = _apiPermissionRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Permission.Create)]
    public async Task<ApiResult> CreatePermission(ApiPermission data)
    {
        try
        {
            var result = _apiPermissionRepository.Create(data);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.Error));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Permission.Update)]
    public async Task<ApiResult> UpdatePermission(ApiPermission data)
    {
        try
        {
            var result = _apiPermissionRepository.Update(data);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Permission.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeletePermission(int id)
    {
        try
        {
            var result = _apiPermissionRepository.Delete(id);
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

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Permission.State)]
    public async Task<ApiResult> StatePermission(int id, bool active)
    {
        try
        {
            var result = _apiPermissionRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}
