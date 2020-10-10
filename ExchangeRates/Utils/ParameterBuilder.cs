using System.Web;

namespace ExchangeRates.Utils
{
    internal static class ParameterBuilder
    {
        public static string ApplyParameterToUrl(string url, string argument, string value)
        {
            var token = "&";

            if (!url.Contains("?"))
                token = "?";

            return $"{url}{token}{argument}={HttpUtility.UrlEncode(value)}";
        }
    }
}