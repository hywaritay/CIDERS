using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIDERS.Domain.Controllers.Secure.Admin;

//[Authorize(Roles = Consts.Vars.Roles.Admin)]
[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.Location.Base)]
public class LocationController : ControllerBase
{
    private readonly ILocationRespository _locationRepository;
    private readonly ILogger<LocationController> _logger;

    public LocationController(ILocationRespository locationRepository, ILogger<LocationController> logger)
    {
        _locationRepository = locationRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Location.All)]
    public async Task<ApiResult> Locations()
    {
        try
        {
            var locations = _locationRepository.All();
            var locationsResult = new List<ApiLocation>();
            foreach (var location in locations)
            {
                locationsResult.Add(location);
            }
            return await Task.FromResult(ApiResponse.Success(locationsResult));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Location.Find)]
    [HttpGet]
    public async Task<ApiResult> LocationRank(int id)
    {
        try
        {
            var result = _locationRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Location.Create)]
    public async Task<ApiResult> CreateLocation(LocationRequest data)
    {
        try
        {
            if (_locationRepository.FindByLocationName(data.District) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.LocationUsed));

            var location = new ApiLocation
            {
                District = data.District,
            };

            var result = _locationRepository.Create(location);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var locationSaved = _locationRepository.FindByLocationName(data.District);
            if (locationSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Location.Update)]
    [HttpPost]
    public async Task<ApiResult> UpdateLocation(ApiLocation data)
    {
        try
        {
            var location = _locationRepository.FindByLocationId(data.Id);
            if (location == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.LocationNotFound));

            location.District = data.District;

            var result = _locationRepository.Update(location);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DbUpdateError));

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }


    [Route(Routes.ApiRoute.Secure.Admin.Location.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeleteLocation(int id)
    {
        try
        {
            var result = _locationRepository.Delete(id);
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

    [Route(Routes.ApiRoute.Secure.Admin.Location.State)]
    [HttpGet]
    public async Task<ApiResult> StateRank(int id, bool active)
    {
        try
        {
            var result = _locationRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}

