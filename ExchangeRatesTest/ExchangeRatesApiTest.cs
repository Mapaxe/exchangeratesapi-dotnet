using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ExchangeRates;
using ExchangeRates.Utils;
using NUnit.Framework;

namespace ExchangeRatesTest
{
    public class ExchangeRatesApiTest
    {
        private ExchangeRatesApi _exchangeRatesApi;
        [OneTimeSetUp]
        public void Setup()
        {
            _exchangeRatesApi = new ExchangeRatesApi(new HttpClient(), true);
        }

        [Test]
        public void TestAvaibleCurrency()
        {
            Assert.AreEqual(33, _exchangeRatesApi.GetAvaibleCurrency().Count, "Currencies");
        }

        #region Latest

        [Test]
        public async Task Test_001_GetTheLatestExchange()
        {
            var result = await _exchangeRatesApi.GetLatestAsync();

            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(32, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public void Test_002_GetTheLatestExchangeCurrencyUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetLatestAsync("UNK");
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }



        [Test]
        public void Test_003_GetTheLatestExchangeSymbolsUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetLatestAsync(symbols: new List<string>(){"MXN", "UNK"});
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }

        [Test]
        public void Test_004_GetTheLatestExchangeSymbolsEmpty()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetLatestAsync(symbols: new List<string>());
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "Symbol list is empty, use null instead"));
        }

        [Test]
        public async Task Test_005_GetTheLatestExchangeWithBase()
        {
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var result = await _exchangeRatesApi.GetLatestAsync(randomCurrency);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(33, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_006_GetTheLatestExchangeWithSymbolsEUR()
        {
            var result = await _exchangeRatesApi.GetLatestAsync(symbols: new List<string>(){"MXN", "EUR"});
            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(1, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_007_GetTheLatestExchangeWithSymbols()
        {
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var expectedRates = randomSymbols.Distinct().Count(e => e != "EUR") != 0
                ? randomSymbols.Distinct().Count(e => e != "EUR")
                : 32;
            var result = await _exchangeRatesApi.GetLatestAsync(symbols: randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(expectedRates, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_008_GetTheLatestExchangeWithBaseAndSymbols()
        {
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var expectedRates = randomSymbols.Distinct().Count(e => e != "EUR") != 0
                ? randomSymbols.Distinct().Count(e => e != "EUR")
                : 32;
            var result = await _exchangeRatesApi.GetLatestAsync(randomCurrency, randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(DateTime.Now.ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(expectedRates, result.Rates.Count, "Rates");
            });
        }

        #endregion

        #region ByDate

        [Test]
        public async Task Test_009_GetExchangeByDate()
        {
            var date = RandomDay();
            var result = await _exchangeRatesApi.GetByDateAsync(date);
            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(AjustWeekEndDay(date).ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(32, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public void Test_010_GetExchangeByDateBadDate()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetByDateAsync(DateTime.Parse("1999-01-01"));
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "There is no data for dates older then 1999-01-04"));
        }

        [Test]
        public void Test_011_GetExchangeByDateCurrencyUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var date = RandomDay();
                    _exchangeRatesApi.GetByDateAsync(date,"UNK");
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }



        [Test]
        public void Test_012_GetExchangeByDateSymbolsUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var date = RandomDay();
                    _exchangeRatesApi.GetByDateAsync(date, symbols: new List<string>(){"MXN", "UNK"});
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }

        [Test]
        public void Test_013_GetExchangeByDateSymbolsEmpty()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var date = RandomDay();
                    _exchangeRatesApi.GetByDateAsync(date, symbols: new List<string>());
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "Symbol list is empty, use null instead"));
        }

        [Test]
        public async Task Test_014_GetExchangeByDateWithBase()
        {
            var date = RandomDay();
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var result = await _exchangeRatesApi.GetByDateAsync(date, randomCurrency);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(AjustWeekEndDay(date).ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(33, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_015_GetExchangeByDateWithSymbolsEUR()
        {
            var date = RandomDay();
            var result = await _exchangeRatesApi.GetByDateAsync(date, symbols: new List<string>(){"MXN", "EUR"});
            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(AjustWeekEndDay(date).ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(1, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_016_GetExchangeByDateWithSymbols()
        {
            var date = RandomDay();
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var expectedRates = randomSymbols.Distinct().Count(e => e != "EUR") != 0
                ? randomSymbols.Distinct().Count(e => e != "EUR")
                : 32;
            var result = await _exchangeRatesApi.GetByDateAsync(date, symbols: randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(AjustWeekEndDay(date).ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(expectedRates, result.Rates.Count, "Rates");
            });
        }

        [Test]
        public async Task Test_017_GetExchangeByDateWithBaseAndSymbols()
        {

            var date = RandomDay();
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var expectedRates = randomSymbols.Distinct().Count(e => e != "EUR") != 0
                ? randomSymbols.Distinct().Count(e => e != "EUR")
                : 32;
            var result = await _exchangeRatesApi.GetByDateAsync(date, randomCurrency, randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(AjustWeekEndDay(date).ToString("yyyy-MM-dd"), result.Date.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(expectedRates, result.Rates.Count, "Rates");
            });
        }

        #endregion

        #region ByRangeDate

        [Test]
        public async Task Test_018_GetExchangeByRangeDate()
        {
            var start = RandomDay();
            var end = RandomDay(start);
            var result = await _exchangeRatesApi.GetByRangeDateAsync(start, end);
            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(start.ToString("yyyy-MM-dd"), result.StartAt.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(end.ToString("yyyy-MM-dd"), result.EndAt.ToString("yyyy-MM-dd"), "Date");
            });
        }

        [Test]
        public void Test_019_GetExchangeByRangeDateBadRangeDate()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetByRangeDateAsync(DateTime.Parse("1999-01-07"), DateTime.Parse("1999-01-05"));
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The end date cannot be greater than the start date"));
        }

        [Test]
        public void Test_019_GetExchangeByRangeDateBadDate()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    _exchangeRatesApi.GetByRangeDateAsync(DateTime.Parse("1999-01-01"), DateTime.Parse("1999-01-01"));
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "There is no data for dates older then 1999-01-04"));
        }

        [Test]
        public void Test_021_GetExchangeByRangeDateCurrencyUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var start = RandomDay();
                    var end = RandomDay(start);
                    _exchangeRatesApi.GetByRangeDateAsync(start, end,"UNK");
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }

        [Test]
        public void Test_022_GetExchangeByRangeDateSymbolsUnknown()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var start = RandomDay();
                    var end = RandomDay(start);
                    _exchangeRatesApi.GetByRangeDateAsync(start, end, symbols: new List<string>(){"MXN", "UNK"});
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "The currency UNK is unsupported, please check GetAvaibleCurrency() Method" ));
        }

        [Test]
        public void Test_023_GetExchangeByRangeDateSymbolsEmpty()
        {
            var ex = Assert.Throws<ExchangeRatesException>(
                delegate
                {
                    var start = RandomDay();
                    var end = RandomDay(start);
                    _exchangeRatesApi.GetByRangeDateAsync(start, end, symbols: new List<string>());
                }
            );
            Assert.That( ex.Message, Is.EqualTo( "Symbol list is empty, use null instead"));
        }

        [Test]
        public async Task Test_024_GetExchangeByRangeDateWithBase()
        {
            var start = RandomDay();
            var end = RandomDay(start);
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var result = await _exchangeRatesApi.GetByRangeDateAsync(start, end, randomCurrency);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(start.ToString("yyyy-MM-dd"), result.StartAt.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(end.ToString("yyyy-MM-dd"), result.EndAt.ToString("yyyy-MM-dd"), "Date");
            });
        }

        [Test]
        public async Task Test_025_GetExchangeByRangeDateWithSymbolsEUR()
        {
            var start = RandomDay();
            var end = RandomDay(start);
            var result = await _exchangeRatesApi.GetByRangeDateAsync(start, end, symbols: new List<string>(){"MXN", "EUR"});
            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(start.ToString("yyyy-MM-dd"), result.StartAt.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(end.ToString("yyyy-MM-dd"), result.EndAt.ToString("yyyy-MM-dd"), "Date");
            });
        }

        [Test]
        public async Task Test_026_GetExchangeByRangeDateWithSymbols()
        {
            var start = RandomDay();
            var end = RandomDay(start);
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var result = await _exchangeRatesApi.GetByRangeDateAsync(start, end, symbols: randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual("EUR", result.Base, "Base");
                Assert.AreEqual(start.ToString("yyyy-MM-dd"), result.StartAt.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(end.ToString("yyyy-MM-dd"), result.EndAt.ToString("yyyy-MM-dd"), "Date");
            });
        }

        [Test]
        public async Task Test_027_GetExchangeByRangeDateWithBaseAndSymbols()
        {

            var start = RandomDay();
            var end = RandomDay(start);
            var randomCurrency = RandomCurrency(_exchangeRatesApi.GetAvaibleCurrency());
            var randomSymbols = RandomSymbol(_exchangeRatesApi.GetAvaibleCurrency()).ToList();
            var result = await _exchangeRatesApi.GetByRangeDateAsync(start, end, randomCurrency, randomSymbols);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(randomCurrency, result.Base, "Base");
                Assert.AreEqual(start.ToString("yyyy-MM-dd"), result.StartAt.ToString("yyyy-MM-dd"), "Date");
                Assert.AreEqual(end.ToString("yyyy-MM-dd"), result.EndAt.ToString("yyyy-MM-dd"), "Date");
            });
        }

        #endregion

        #region TestUtils

        public static DateTime RandomDay(DateTime? dateTime = null)
        {
            var start = dateTime ?? new DateTime(2020, 1, 4);
            var range = (DateTime.Today - start).Days;
            return start.AddDays(new Random().Next(range));
        }

        public static DateTime AjustWeekEndDay(DateTime date)
        {
            Console.WriteLine("DayOfWeek: " + date.DayOfWeek);
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(-1);
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(-2);
            }
            Console.WriteLine("DateAjust: " + date.ToString("yyyy-MM-dd"));
            return date;
        }
        public static string RandomCurrency(IEnumerable<string> source)
        {
            var enumerable = source.ToList();
            return enumerable.Skip(new Random().Next(enumerable.Count)).First();
        }

        public static IEnumerable<string> RandomSymbol(IEnumerable<string> source)
        {
            var result = new List<string>();
            var enumerable = source.ToList();
            for (var i = 0; i < new Random().Next(enumerable.Count); i++)
            {
                result.Add(RandomCurrency(enumerable));
            }
            return result;
        }

        #endregion
    }
}