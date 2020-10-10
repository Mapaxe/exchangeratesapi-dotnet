using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRates.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExchangeRates
{
    public class ExchangeRatesHttpClient
    {
        #region Properties
        private const string ApiEndpoint = "https://api.exchangeratesapi.io/";
        private const string UserAgent = "ExchangeRates .NET Core v1";
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly bool _debug;
        private readonly HttpClient _client;
        #endregion

        #region Constructor

        protected ExchangeRatesHttpClient(HttpClient client, bool debug = false)
        {
            _debug = debug;
            client.BaseAddress = new Uri(ApiEndpoint);
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            _client = client;

            _jsonSerializerSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
        }
        #endregion

        #region Private

        private protected async Task<T> GetAsync<T>(string endpoint)
        {
            var json = await DoRequestAsync(endpoint);
            return JsonConvert.DeserializeObject<T>(json, _jsonSerializerSettings);
        }
        private protected async Task<string> DoRequestAsync(string url)
        {
            var endpoint = ApiEndpoint + url;

            if(_debug) Console.WriteLine("Request to: " + endpoint);
            var httpResponse = await _client.GetAsync($"{endpoint}");
            if (httpResponse.IsSuccessStatusCode) return await httpResponse.Content.ReadAsStringAsync();
            var statusCode = (int) httpResponse.StatusCode;
            if (statusCode >= 400 && statusCode < 500)
                throw new ExchangeRatesException(GetExchangeRatesError(await httpResponse.Content.ReadAsStringAsync()).Error);
            throw new Exception("Unknow Error");
        }

        internal ExchangeRatesError GetExchangeRatesError(string raw)
        {
            if(_debug) Console.WriteLine("Error Raw to: " + raw);
            var jObj = JObject.Parse(raw);

            var serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CustomDeserializationContractResolver()
            };

            var badParams = jObj.ToObject<ExchangeRatesError>(serializer);
            return badParams;
        }
        #endregion
    }
}