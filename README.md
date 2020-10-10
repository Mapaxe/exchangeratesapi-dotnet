# ExchangeRatesAPI.NetCore - Currency Exchange Rates API Net Core


<!--
[![Build history](https://buildstats.info/github/chart/Mapaxe/exchangeratesapi-dotnet)](https://buildstats.info/github/chart/Mapaxe/exchangeratesapi-dotnet)
 -->

![.NET Core Build](https://github.com/Mapaxe/exchangeratesapi-dotnet/workflows/.NET%20Core/badge.svg?branch=master)
[![NuGet Badge](https://buildstats.info/nuget/ExchangeRatesAPI.NetCore?vWidth=100&dWidth=100)](https://www.nuget.org/packages/ExchangeRatesAPI.NetCore)
[![Software License](https://img.shields.io/badge/license-MIT-brightgreen.svg?style=flat-square)](LICENSE.md)


Unofficial **NetCore** wrapper for [ExchangeRatesAPI](https://exchangeratesapi.io/), which provides exchange rate lookups of the [Central European Bank](https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html). eratesapi.io/), which provides exchange rate lookups of the [Central European Bank](https://www.ecb.europa.eu/stats/policy_and_exchange_rates/euro_reference_exchange_rates/html/index.en.html).

### Important ExchangeRatesAPI Recommends
>Please cache results whenever possible this will allow us to keep the service without any rate limits or api key requirements.
>The API comes with no warranty but we do our best effort to keep the service working relibly.

## Table of Contents:

1. [Compatibility](#1-compatibility)
2. [Dependencies](#2-dependencies)
3. [Quick Start](#3-quick-start)
4. [Usage](#4-usage)
5. [Supported Currencies](#5-supported-currencies)
6. [Issues](#6-issues)

---

## 1. Compatibility
* NetCore 3.1

## 2. Dependencies
* [Newtonsoft.Json](http://james.newtonking.com/json)

## 3. Quick Start
#### Installation #####

It is recommended that you use [NuGet](http://docs.nuget.org) for install this library. Or you can fork the code and build it.

#### Configuration by Instantiation #####

Create ExchangeRatesApi object like this:

```cs
System.Net.Http.HttpClient httpClient = ...
ExchangeRatesApi exchangeRatesApi = new ExchangeRatesApi(httpClient);
```
#### Configuration by Injection Dependency #####

Startup.cs
```cs
...
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.AddHttpClient<ExchangeRatesApi, ExchangeRatesApi>();
    ...
}
...
```

AntoherClass.cs
```cs
private readonly ExchangeRatesApi _exchangeRatesApi;

public AntoherClass(ExchangeRatesApi exchangeRatesApi)
{
    _exchangeRatesApi = exchangeRatesApi;
}
```

## 4. Usage

Since the CurrencyExchangeAPI does not require API keys or authentication in order to access and interrogate its API, getting started with this library is easy. The following examples show how to achieve various functions using the library.

#### Latest #####

**Get the latest foreign exchange reference rates.**
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetLatestAsync();
```

**Rates are quoted against the Euro by default. Quote against a different currency by setting the base parameter in your request.**
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetLatestAsync("USD");
```

**Request specific exchange rates by setting the symbols parameter.**
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetLatestAsync(symbols: new List<string>(){"USD", "GBP"});
```
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetLatestAsync("MXN", new List<string>(){"USD", "GBP"});
```

#### Latest By Date ####
**Get historical rates for any day since 1999-01-04**
```cs
LatestResponse latestResponse = await _exchangeRatesApi.GetByDateAsync(DateTime.Parse("2020-01-01"));
```

**Rates are quoted against the Euro by default. Quote against a different currency by setting the base parameter in your request and specific date.**
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetByDateAsync(DateTime.Parse("2020-01-01"), "USD");
```

**Request specific exchange rates by setting the symbols parameter.**
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetByDateAsync(DateTime.Parse("2020-01-01"), symbols: new List<string>(){"USD", "GBP"});
```
```cs
LatestResponse latestResponse = await _exchangeRatesApi
    .GetByDateAsync(DateTime.Parse("2020-01-01"), "MXN", new List<string>(){"USD", "GBP"});
```

#### Latest By Range ####
**Get historical rates for a time period.**
```cs
HistoryResponse latestResponse = await _exchangeRatesApi
    .GetByRangeDateAsync(DateTime.Parse("2020-01-01"), DateTime.Parse("2020-02-01"));
```

**Quote the historical rates against a different currency.**
```cs
HistoryResponse latestResponse = await _exchangeRatesApi
    .GetByRangeDateAsync(DateTime.Parse("2020-01-01"), DateTime.Parse("2020-02-01"), "USD");
```

**Limit results to specific exchange rates to save bandwidth with the symbols parameter.**
```cs
HistoryResponse latestResponse = await _exchangeRatesApi
    .GetByRangeDateAsync(DateTime.Parse("2020-01-01"), DateTime.Parse("2020-02-01"), symbols: new List<string>(){"USD", "GBP"});
```
```cs
HistoryResponse latestResponse = await _exchangeRatesApi
    .GetByRangeDateAsync(DateTime.Parse("2020-01-01"), DateTime.Parse("2020-02-01"), "MXN", new List<string>(){"USD", "GBP"});
```

## 5. Supported Currencies

The library supports any currency currently available on the European Central Bank's web service, which at the time of the latest release are as follows:

![](https://www.ecb.europa.eu/shared/img/flags/AUD.gif) Australian Dollar (AUD)<br />
![](https://www.ecb.europa.eu/shared/img/flags/BRL.gif) Brazilian Real (BRL)<br />
![](https://www.ecb.europa.eu/shared/img/flags/GBP.gif) British Pound Sterline (GBP)<br />
![](https://www.ecb.europa.eu/shared/img/flags/BGN.gif) Bulgarian Lev (BGN)<br />
![](https://www.ecb.europa.eu/shared/img/flags/CAD.gif) Canadian Dollar (CAD)<br />
![](https://www.ecb.europa.eu/shared/img/flags/CNY.gif) Chinese Yuan Renminbi (CNY)<br />
![](https://www.ecb.europa.eu/shared/img/flags/HRK.gif) Croatian Kuna (HRK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/CZK.gif) Czech Koruna (CZK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/DKK.gif) Danish Krone (DKK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/EUR.gif) Euro (EUR)<br />
![](https://www.ecb.europa.eu/shared/img/flags/HKD.gif) Hong Kong Dollar (HKD)<br />
![](https://www.ecb.europa.eu/shared/img/flags/HUF.gif) Hungarian Forint (HUF)<br />
![](https://www.ecb.europa.eu/shared/img/flags/ISK.gif) Icelandic Krona (ISK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/IDR.gif) Indonesian Rupiah (IDR)<br />
![](https://www.ecb.europa.eu/shared/img/flags/INR.gif) Indian Rupee (INR)<br />
![](https://www.ecb.europa.eu/shared/img/flags/ILS.gif) Israeli Shekel (ILS)<br />
![](https://www.ecb.europa.eu/shared/img/flags/JPY.gif) Japanese Yen (JPY)<br />
![](https://www.ecb.europa.eu/shared/img/flags/MYR.gif) Malaysian Ringgit (MYR)<br />
![](https://www.ecb.europa.eu/shared/img/flags/MXN.gif) Mexican Peso (MXN)<br />
![](https://www.ecb.europa.eu/shared/img/flags/NZD.gif) New Zealand Dollar (NZD)<br />
![](https://www.ecb.europa.eu/shared/img/flags/NOK.gif) Norwegian Krone (NOK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/PHP.gif) Philippine Peso (PHP)<br />
![](https://www.ecb.europa.eu/shared/img/flags/PLN.gif) Polish Zloty (PLN)<br />
![](https://www.ecb.europa.eu/shared/img/flags/RON.gif) Romanian Leu (RON)<br />
![](https://www.ecb.europa.eu/shared/img/flags/RUB.gif) Russian Rouble (RUB)<br />
![](https://www.ecb.europa.eu/shared/img/flags/SGD.gif) Singapore Dollar (SGD)<br />
![](https://www.ecb.europa.eu/shared/img/flags/ZAR.gif) South African Rand (ZAR)<br />
![](https://www.ecb.europa.eu/shared/img/flags/KRW.gif) South Korean Won (KRW)<br />
![](https://www.ecb.europa.eu/shared/img/flags/SEK.gif) Swedish Krona (SEK)<br />
![](https://www.ecb.europa.eu/shared/img/flags/CHF.gif) Swiss Franc (CHF)<br />
![](https://www.ecb.europa.eu/shared/img/flags/THB.gif) Thai Baht (THB)<br />
![](https://www.ecb.europa.eu/shared/img/flags/TRY.gif) Turkish Lira (TRY)<br />
![](https://www.ecb.europa.eu/shared/img/flags/USD.gif) US Dollar (USD)<br />

## 6. Issues:

If you have spotted any bugs, or would like to request additional features from the library, please file an issue via the Issue Tracker on the project's Github page: [https://github.com/Mapaxe/exchangeratesapi-dotnet/issues](https://github.com/Mapaxe/exchangeratesapi-dotnet/issues).