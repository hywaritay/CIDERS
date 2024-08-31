using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CIDERS.Domain.Controllers.Secure.Admin;

[Authorize(Roles = Consts.Vars.Roles.Admin)]
[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.User.Base)]
public class AdminUserController : ControllerBase
{
    private readonly IApiPermissionRepository _apiPermissionRepository;
    private readonly IApiUserPermissionRepository _apiUserPermissionRepository;
    private readonly IApiUserRepository _apiUserRepository;
    private readonly ILogger<AdminUserController> _logger;

    public AdminUserController(IApiUserRepository apiUserRepository, ILogger<AdminUserController> logger,
        IApiPermissionRepository apiPermissionRepository, IApiUserPermissionRepository apiUserPermissionRepository)
    {
        _apiUserRepository = apiUserRepository;
        _logger = logger;
        _apiPermissionRepository = apiPermissionRepository;
        _apiUserPermissionRepository = apiUserPermissionRepository;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.User.All)]
    public async Task<ApiResult> Users()
    {
        try
        {
            var users = _apiUserRepository.All();
            var usersResult = new List<ApiUser>();
            foreach (var user in users)
            {
                user.UserPermissions = _apiUserPermissionRepository.FindByUserId(user.Id);
                usersResult.Add(user);
            }

            return await Task.FromResult(ApiResponse.Success(usersResult));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.User.Find)]
    [HttpGet]
    public async Task<ApiResult> FindUser(int id)
    {
        try
        {
            var result = _apiUserRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.User.Create)]
    public async Task<ApiResult> CreateUser(RegisterRequest data)
    {
        try
        {
            if (_apiUserRepository.FindByApiKey(data.ApiKey) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.UsernameUsed));
            if (_apiUserRepository.FindByEmail(data.Email) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.EmailUsed));

            var user = new ApiUser
            {
                ApiKey = data.ApiKey,
                Firstname = data.Firstname,
                Lastname = data.Lastname,
                Email = data.Email,
                ApiSecret = BCrypt.Net.BCrypt.HashPassword(data.ApiSecret),
                ApiTokenMinute = data.ApiTokenMinute
            };

            var result = _apiUserRepository.Create(user);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var userSaved = _apiUserRepository.FindByApiKey(data.ApiKey);
            if (userSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.User.Update)]
    [HttpPost]
    public async Task<ApiResult> UpdateUser(ApiUser data)
    {
        try
        {
            var userPermissions = data.UserPermissions ?? null;
            data.UserPermissions = null;
            var user = _apiUserRepository.FindByApiKey(data.ApiKey);
            if (user == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.UserNotFound));

            user.ApiKey = data.ApiKey;
            user.ApiKey = data.ApiKey;
            user.Firstname = data.Firstname;
            user.Lastname = data.Lastname;
            user.Email = data.Email;
            user.Channel = data.Channel;

            var result = _apiUserRepository.Update(user);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DbUpdateError));
            user = _apiUserRepository.FindByApiKey(data.ApiKey);
            if (user == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.UserNotFound));

            var userPermissionActive = _apiUserPermissionRepository.FindByUserId(user.Id);
            foreach (var userPermissionTmp in userPermissionActive)
                _apiUserPermissionRepository.Delete(userPermissionTmp.Id);

            if (userPermissions == null) return await Task.FromResult(ApiResponse.Success(result));
            foreach (var userPermission in userPermissions.Select(userPermissionTmp => new ApiUserPermission
            {
                FkUser = user,
                FkPermission = _apiPermissionRepository.Find(userPermissionTmp.FkPermission?.Id)
            }))
                _apiUserPermissionRepository.Create(userPermission);

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.User.UpdateSecret)]
    public async Task<ApiResult> UpdateSecret(UpdateCredential data)
    {
        try
        {
            var user = _apiUserRepository.FindByApiKey(data.Username);
            if (user == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.UserNotFound));

            user.ApiSecret = BCrypt.Net.BCrypt.HashPassword(data.Password);
            var result = _apiUserRepository.Update(user);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.User.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeleteUser(int id)
    {
        try
        {
            var result = _apiUserRepository.Delete(id);
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

    [Route(Routes.ApiRoute.Secure.Admin.User.State)]
    [HttpGet]
    public async Task<ApiResult> StateUser(int id, bool active)
    {
        try
        {
            var result = _apiUserRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}
