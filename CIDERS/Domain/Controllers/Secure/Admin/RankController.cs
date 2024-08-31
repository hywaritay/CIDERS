using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CIDERS.Domain.Utils.Routes.ApiRoute.Secure.Admin;

namespace CIDERS.Domain.Controllers.Secure.Admin;

//[Authorize(Roles = Consts.Vars.Roles.Admin)]
[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.Rank.Base)]
public class RankController : ControllerBase
{
    private readonly IRankRepository _rankRepository;
    private readonly ILogger<RankController> _logger;

    public RankController(IRankRepository rankRepository, ILogger<RankController> logger)
    {
        _rankRepository = rankRepository;
        _logger = logger;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Rank.All)]
    public async Task<ApiResult> Ranks()
    {
        try
        {
            var ranks = _rankRepository.All();
            var ranksResult = new List<ApiRank>();
            foreach (var rank in ranks)
            {
                ranksResult.Add(rank);
            }
            return await Task.FromResult(ApiResponse.Success(ranksResult));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Rank.Find)]
    [HttpGet]
    public async Task<ApiResult> FindRank(int id)
    {
        try
        {
            var result = _rankRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Rank.Create)]
    public async Task<ApiResult> CreateRank(RankRequest data)
    {
        try
        {
            if (_rankRepository.FindByRankName(data.RankName) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.RankUsed));

            var rank = new ApiRank
            {
                RankName = data.RankName,
            };

            var result = _rankRepository.Create(rank);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var rankSaved = _rankRepository.FindByRankName(data.RankName);
            if (rankSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Rank.Update)]
    [HttpPost]
    public async Task<ApiResult> UpdateRank(ApiRank data)
    {
        try
        {
            var rank = _rankRepository.FindByRankId(data.Id);
            if (rank == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.RankNotFound));

            rank.RankName = data.RankName;

            var result = _rankRepository.Update(rank);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DbUpdateError));

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }


    [Route(Routes.ApiRoute.Secure.Admin.Rank.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeleteRank(int id)
    {
        try
        {
            var result = _rankRepository.Delete(id);
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

    [Route(Routes.ApiRoute.Secure.Admin.Rank.State)]
    [HttpGet]
    public async Task<ApiResult> StateRank(int id, bool active)
    {
        try
        {
            var result = _rankRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}

