using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRates.Models
{
    public class LatestResponse
    {
        [JsonProperty(PropertyName = "rates", NullValueHandling=NullValueHandling.Ignore)]
        public Dictionary<string, decimal> Rates { get; set; }
        [JsonProperty(PropertyName = "base", NullValueHandling=NullValueHandling.Ignore)]
        public string Base { get; set; }
        [JsonProperty(PropertyName = "date", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime Date { get; set; }
    }
}