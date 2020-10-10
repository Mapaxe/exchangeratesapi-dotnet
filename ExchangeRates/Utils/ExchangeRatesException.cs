using System;

namespace ExchangeRates.Utils
{
    public class ExchangeRatesException : Exception
    {
        public ExchangeRatesException(string message) : base(message)
        {}
    }
}