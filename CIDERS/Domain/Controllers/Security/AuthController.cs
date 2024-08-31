using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Provider.jwt;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace CIDERS.Domain.Controllers.Security;

[ApiController]
[Route(Routes.ApiRoute.Security.Auth.Base)]
public class AuthController : ControllerBase
{
    private readonly IApiPermissionRepository _apiPermissionRepository;
    private readonly IApiUserPermissionRepository _apiUserPermissionRepository;
    private readonly IApiUserRepository _apiUserRepository;
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger, IOptions<JwtOptions> options,
        IApiUserRepository apiUserRepository, IApiUserPermissionRepository apiUserPermissionRepository,
        IApiPermissionRepository apiPermissionRepository)
    {
        _logger = logger;
        _apiUserRepository = apiUserRepository;
        _apiUserPermissionRepository = apiUserPermissionRepository;
        _apiPermissionRepository = apiPermissionRepository;
        _jwtOptions = options.Value;
    }

    [Route(Routes.ApiRoute.Security.Auth.Login)]
    [HttpPost]
    public async Task<ApiResult> Login(AuthRequest authRequest)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(authRequest.Username) ||
                string.IsNullOrWhiteSpace(authRequest.Password) ||
                !Functions.IsBase64(authRequest.Username) ||
                !Functions.IsBase64(authRequest.Password))
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.NotEncoded));

            // Decode username and password from Base64
            authRequest.Username = Encoding.UTF8.GetString(Convert.FromBase64String(authRequest.Username.Trim()));
            authRequest.Password = Encoding.UTF8.GetString(Convert.FromBase64String(authRequest.Password.Trim()));

            if (authRequest.Username == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.BadCredentials));

            var user = _apiUserRepository.FindByApiKey(authRequest.Username);

            if (user == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.BadCredentials));
   
            if (!BCrypt.Net.BCrypt.Verify(authRequest.Password, user.ApiSecret))
               return await Task.FromResult(ApiResponse.Error(ErrorHttp.BadCredentials));
            

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString("N")),
                new(JwtRegisteredClaimNames.UniqueName, authRequest.Username),
                new(JwtRegisteredClaimNames.Name, authRequest.Username),
                new(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new(JwtRegisteredClaimNames.NameId, authRequest.Username),
                new(JwtRegisteredClaimNames.GivenName, authRequest.Username),
                new(ClaimTypes.Name, authRequest.Username),
                new(ClaimTypes.NameIdentifier, authRequest.Username)
            };
          
            if (_jwtOptions.SecretKey != null)
            {
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
                    SecurityAlgorithms.HmacSha256Signature);

                var token = new JwtSecurityToken(
                    _jwtOptions.Issuer,
                    _jwtOptions.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(user.ApiTokenMinute ?? 1),
                    signingCredentials: signingCredentials);

                user.Lastconnect = DateTime.Now;
                _apiUserRepository.Update(user);

                user.ApiSecret = null;

                return await Task.FromResult(ApiResponse.Success(new JwtResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Username = user.ApiKey
                }));
            }
            else
            {
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred during login");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error,e));
        }
       

    }


    [Route(Routes.ApiRoute.Security.Auth.Register)]
    [HttpPost]
    public async Task<ApiResult> Register(RegisterRequest data)
    {
        try
        {
            data.ApiKey = "Sal@2024";
            data.Email = "yayah.waritay@njala.edu.sl";
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
                Channel = data.Channel,
                ApiSecret = BCrypt.Net.BCrypt.HashPassword(data.ApiSecret),
                ApiTokenMinute = data.ApiTokenMinute
            };

            var result = _apiUserRepository.Create(user);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var userSaved = _apiUserRepository.FindByApiKey(data.ApiKey);
            if (userSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));

            var userPermissions = new List<string>
            {
                Consts.Vars.Roles.Admin,
            };
            foreach (var userPermission in userPermissions.Select(userPermissionTmp => new ApiUserPermission
            {
                FkUser = user,
                FkPermission = _apiPermissionRepository.FindByName(userPermissionTmp)
            }).Where(userPermission => userPermission.FkPermission != null))
                _apiUserPermissionRepository.Create(userPermission);

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}
