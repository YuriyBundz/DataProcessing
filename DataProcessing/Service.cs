using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class Service
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("payers")]
        public Payer[] Payers { get; set; }
        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }
}
