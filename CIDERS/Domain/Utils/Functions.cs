using CIDERS.Domain.Core.Entity.Cider;
using CIDERS.Domain.Core.Repository.Cider;
using System.Security.Cryptography;

namespace CIDERS.Domain.Utils;


public class Functions
{
    private readonly IApiUserRepository _apiUserRepository;
    private readonly IRankRepository _rankRepository;
    private readonly IDepartmentRepository _deptRepository;
    private readonly ILocationRespository _locationRepository;

    public Functions(IApiUserRepository apiUserRepository, IRankRepository rankRepository, IDepartmentRepository deptRepository, ILocationRespository locationRepository)
    {
        _apiUserRepository = apiUserRepository;
        _rankRepository = rankRepository;
        _deptRepository = deptRepository;
        _locationRepository = locationRepository;
    }

    public static string TitleCaseConvert(string title)
    {
        try
        {
            var wordStart = 0;
            var result = new char[title.Length];
            var ri = 0;
            for (var i = 0; i < title.Length; ++i)
                if (title[i] == '_')
                    wordStart = i + 1;
                else if (i == wordStart) result[ri++] = char.ToUpper(title[i]);
                else result[ri++] = char.ToLower(title[i]);

            return new string(result, 0, ri);
        }
        catch (Exception)
        {
            return title;
        }
    }

    public static string TitleCaseRevert(string title)
    {
        try
        {
            var result = new char[10000];
            var ri = 0;
            foreach (var t in title)
            {
                if (char.IsUpper(t) && ri != 0)
                {
                    result[ri] = '_';
                    ri++;
                    result[ri] = char.ToLower(t);
                }
                else
                {
                    result[ri] = char.ToLower(t);
                }

                ri++;
            }

            return new string(result, 0, ri);
        }
        catch (Exception)
        {
            return title;
        }
    }

    public static string GenerateUid(bool? braces = true)
    {
        return Guid.NewGuid().ToString(braces == true ? "D" : "N");
    }

    public static string Generate(int? length = 10)
    {

        const string charset = "09182736455463728190ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        if (length != null && length > charset.Length) length = charset.Length;
        var outputChars = new char[length ?? 10];
        using var rng = RandomNumberGenerator.Create();
        const int minIndex = 0;
        var maxIndexExclusive = charset.Length;
        var diff = maxIndexExclusive - minIndex;
        var upperBound = uint.MaxValue / diff * diff;
        var randomBuffer = new byte[sizeof(int)];

        for (var i = 0; i < outputChars.Length; i++)
        {
            uint randomUInt;
            do
            {
                rng.GetBytes(randomBuffer);
                randomUInt = BitConverter.ToUInt32(randomBuffer, 0);
            } while (randomUInt >= upperBound);

            var charIndex = (int)(randomUInt % diff);

            outputChars[i] = charset[charIndex];
        }

        return new string(outputChars);
    }

    public ApiUser? GetUser(string? username)
    {
        return _apiUserRepository.FindByApiKey(username);
    }
    public ApiRank? GetRank(int? id)
    {
        return _rankRepository.FindByRankId(id);
    }
    public ApiDepartment? GetDept(int? id)
    {
        return _deptRepository.FindByDeptId(id);
    }
    public ApiLocation? GetLocation(int? id)
    {
        return _locationRepository.FindByLocationId(id);
    }

    public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
    {
        for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            yield return day;
    }

    public static bool IsBase64(string base64)
    {
        var buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out var bytesParsed);
    }

   


}
