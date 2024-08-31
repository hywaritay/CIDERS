using CIDERS.Domain.Core.Dto.Request;
using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using CIDERS.Domain.Utils;
using Microsoft.AspNetCore.Mvc;
using static CIDERS.Domain.Utils.Routes.ApiRoute.Secure.Admin;

namespace CIDERS.Domain.Controllers.Secure.HR;


//[Authorize(Roles = Consts.Vars.Roles.Admin)]
[ApiController]
[Route(Routes.ApiRoute.Secure.Admin.Employee.Base)]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ILogger<EmployeeController> _logger;
    private readonly Functions _functions;

    public EmployeeController(IEmployeeRepository employeeRepository, ILogger<EmployeeController> logger, Functions functions)
    {
        _employeeRepository = employeeRepository;
        _logger = logger;
        _functions = functions;
    }

    [HttpGet]
    [Route(Routes.ApiRoute.Secure.Admin.Employee.All)]
    public async Task<ApiResult> Employyes()
    {
        try
        {
            var ranks = _employeeRepository.All();
            var ranksResult = new List<ApiEmployee>();
            foreach (var empPinNumber in ranks)
            {
                ranksResult.Add(empPinNumber);
            }
            return await Task.FromResult(ApiResponse.Success(ranksResult));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Employee.Find)]
    [HttpGet]
    public async Task<ApiResult> FindEmployee(int id)
    {
        try
        {
            var result = _employeeRepository.Find(id);
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [HttpPost]
    [Route(Routes.ApiRoute.Secure.Admin.Employee.Create)]
    public async Task<ApiResult> CreateEmployee(EmployeeRequest data)
    {

        try
        {
            var rank= _functions.GetRank(data.FkRank);
            var dept = _functions.GetDept(data.FkDept);
            var loc = _functions.GetLocation(data.FkLoc);

            if (rank == null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.RankNotFound));
            if (dept == null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.DeptNotFound));
            if (loc == null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.LocationNotFound));

            if (_employeeRepository.FindByEmpName(data.PinNumber) != null && _employeeRepository.FindByEmpName(data.ForceNumber) != null)
                return await Task.FromResult(ApiResponse.Error(ErrorHttp.EmployeeUsed));

            var empData = new ApiEmployee
            {
                FkRank = rank,
                FkDept = dept,
                FkLocation = loc,
                ForceNumber = data.ForceNumber,
                PinNumber = data.PinNumber,
                FirstName = data.FirstName, 
                LastName = data.LastName,
                MiddleName = data.MiddleName,
                DOB = data.DOB,
                Mobile = data.Mobile,
                Email = data.Email,
                EntryDate = data.EntryDate,
                DepartureDate = data.DepartureDate,
                DepartureReason = data.DepartureReason,
                imgProfile = data.imgProfile

            };

            var result = _employeeRepository.Create(empData);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            var empSaved = _employeeRepository.FindByEmpName(data.PinNumber);
            if (empSaved == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error));
            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Employee.Update)]
    [HttpPost]
    public async Task<ApiResult> UpdateEmployee(ApiEmployee data)
    {
        try
        {
            var emp = _employeeRepository.FindByEmpId(data.Id);
            if (emp == null) return await Task.FromResult(ApiResponse.Error(ErrorHttp.EmployeeNotFound));

            emp.ForceNumber = data.ForceNumber;
            emp.PinNumber = data.PinNumber;
            emp.FirstName = data.FirstName;
            emp.LastName = data.LastName;
            emp.MiddleName = data.MiddleName;
            emp.DOB = data.DOB;
            emp.Mobile = data.Mobile;
            emp.Email = data.Email;
            emp.EntryDate = data.EntryDate;
            emp.DepartureDate = data.DepartureDate;
            emp.DepartureReason = data.DepartureReason;
            emp.FkRank = data.FkRank;
            emp.FkDept = data.FkDept;
            emp.FkLocation = data.FkLocation;
            emp.imgProfile = data.imgProfile;

            var result = _employeeRepository.Update(emp);
            if (!result) return await Task.FromResult(ApiResponse.Error(ErrorHttp.DbUpdateError));

            return await Task.FromResult(ApiResponse.Success(result));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }

    [Route(Routes.ApiRoute.Secure.Admin.Employee.Delete)]
    [HttpDelete]
    public async Task<ApiResult> DeleteEmployee(int id)
    {
        try
        {
            var result = _employeeRepository.Delete(id);
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

    [Route(Routes.ApiRoute.Secure.Admin.Employee.State)]
    [HttpGet]
    public async Task<ApiResult> StateEmployee(int id, bool active)
    {
        try
        {
            var result = _employeeRepository.Status(id, active);
            return await Task.FromResult(result ? ApiResponse.Success(result) : ApiResponse.Error(ErrorHttp.NotFound));
        }
        catch (Exception e)
        {
            _logger.LogError("error");
            return await Task.FromResult(ApiResponse.Error(ErrorHttp.Error, e));
        }
    }
}

