using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class CitySummary
    {
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("services")]
        public Service[] Services { get; set; }
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }
}
