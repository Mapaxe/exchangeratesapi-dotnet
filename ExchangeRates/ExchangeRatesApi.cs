using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ExchangeRates.Models;
using ExchangeRates.Utils;

namespace ExchangeRates
{
    public class ExchangeRatesApi : ExchangeRatesHttpClient
    {
        #region Properties
        private readonly bool _debug;
        #endregion
        #region Constructor

        public ExchangeRatesApi(HttpClient client, bool debug = false) : base( client, debug)
        {
            _debug = debug;
        }

        #endregion

        #region Public

        // ReSharper disable once MemberCanBeMadeStatic.Global
        public List<string> GetAvaibleCurrency()
        {
            var fields = typeof(Currency).GetFields(BindingFlags.Static | BindingFlags.Public);
            return fields.Select(prop => prop.Name).ToList();
        }

        public Task<LatestResponse> GetLatestAsync(string baseCurrency = null, IEnumerable<string> symbols = null)
        {
            var url = "latest";

            if (baseCurrency != null)
            {
                url = AddBaseCurrencyParameter(url, baseCurrency);
            }

            if (symbols != null)
            {
                url = AddSymbolsParameter(url, symbols);
            }
            return GetAsync<LatestResponse>(url);
        }

        public Task<LatestResponse> GetByDateAsync(DateTime date, string baseCurrency = null, IEnumerable<string> symbols = null)
        {
            if(date < DateTime.Parse("1999-01-04")) throw new ExchangeRatesException("There is no data for dates older then 1999-01-04");

            var url = date.ToString("yyyy-MM-dd");
            if(_debug) Console.WriteLine("Date: " + url);

            if (baseCurrency != null)
            {
                url = AddBaseCurrencyParameter(url, baseCurrency);
            }

            if (symbols != null)
            {
                url = AddSymbolsParameter(url, symbols);
            }

            return GetAsync<LatestResponse>(url);
        }

        public Task<HistoryResponse> GetByRangeDateAsync(DateTime startAt, DateTime endAt, string baseCurrency = null, IEnumerable<string> symbols = null)
        {
            if(startAt < DateTime.Parse("1999-01-04")) throw new ExchangeRatesException("There is no data for dates older then 1999-01-04");
            if(endAt < startAt) throw new ExchangeRatesException("The end date cannot be greater than the start date");

            var url = "history";
            url = AddRangeDateParameter(url, startAt);
            url = AddRangeDateParameter(url, endAt, false);

            if (baseCurrency != null)
            {
                url = AddBaseCurrencyParameter(url, baseCurrency);
            }

            if (symbols != null)
            {
                url = AddSymbolsParameter(url, symbols);
            }

            return GetAsync<HistoryResponse>(url);
        }

        #endregion

        #region Private

        private string AddBaseCurrencyParameter(string url, string baseCurrency)
        {
            if(_debug) Console.WriteLine("BaseCurrency: " + baseCurrency);

            //Use default base currency (EUR)
            if (baseCurrency.Equals(Currency.EUR)) return url;

            //Check if all symbols are valid currency
            if (!ValidateCurrency(baseCurrency)) throw new ExchangeRatesException($"The currency {baseCurrency} is unsupported, please check GetAvaibleCurrency() Method");
            return ParameterBuilder.ApplyParameterToUrl(url, "base", baseCurrency);
        }

        private string AddSymbolsParameter(string url, IEnumerable<string> symbolsParam)
        {
            //Return all Exchange available currency
            if (symbolsParam == null) return url;

            //Get Unique symbols list
            var symbols = symbolsParam.Distinct().ToList();
            if(_debug) Console.WriteLine("Symbols: " + string.Join(',', symbols));

            //Prevent Not Empty List
            if (!symbols.Any()) throw new ExchangeRatesException("Symbol list is empty, use null instead");

            //Check if all symbols are valid currency
            foreach (var symbol in symbols.Where(symbol => !ValidateCurrency(symbol)))
            {
                throw new ExchangeRatesException($"The currency {symbol} is unsupported, please check GetAvaibleCurrency() Method");
            }

            //Check if use Default Base Currency and Ignore EUR Symbol because EUR is Default Base Currency
            if (Regex.Match(url, "(latest)?([0-9]{4}-[0-9]{2}-[0-9]{2})?$", RegexOptions.IgnoreCase).Success)
                symbols.Remove("EUR");
            return ParameterBuilder.ApplyParameterToUrl(url, "symbols", string.Join(',', symbols));
        }

        private string AddRangeDateParameter(string url, DateTime date, bool start = true)
        {
            var dateStr = date.ToString("yyyy-MM-dd");
            if(_debug) Console.WriteLine($"Date {(start ? "Start At" : "End At")}: " + dateStr);
            return ParameterBuilder.ApplyParameterToUrl(url, (start ? "start_at" : "end_at"), dateStr);
        }

        private static bool ValidateCurrency(string currency)
        {
            return currency != null && typeof(Currency).GetField(currency, BindingFlags.Static | BindingFlags.Public) != null;
        }

        #endregion
    }
}