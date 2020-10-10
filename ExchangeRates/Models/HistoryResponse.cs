using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace ExchangeRates.Models
{
    public class HistoryResponse
    {
        [JsonProperty(PropertyName = "rates", NullValueHandling=NullValueHandling.Ignore)]
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
        [JsonProperty(PropertyName = "base", NullValueHandling=NullValueHandling.Ignore)]
        public string Base { get; set; }
        [JsonProperty(PropertyName = "start_at", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime StartAt { get; set; }
        [JsonProperty(PropertyName = "end_at", NullValueHandling=NullValueHandling.Ignore)]
        public DateTime EndAt { get; set; }
    }
}