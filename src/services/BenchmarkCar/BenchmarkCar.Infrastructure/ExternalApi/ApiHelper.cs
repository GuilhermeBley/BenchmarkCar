using System.Globalization;

namespace BenchmarkCar.Infrastructure.ExternalApi;

internal static class ApiHelper
{
    public static CultureInfo CarApiCultureInfo
        = CultureInfo.GetCultureInfo("en-US");
}
