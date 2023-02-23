using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataProcessing
{
    public class Payer
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("payment")]
        public decimal Payment { get; set; }
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        [JsonPropertyName("account_number")]
        public long Account_number { get; set; }
    }
}
