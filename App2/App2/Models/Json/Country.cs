using Newtonsoft.Json;

namespace App2.Models.Json
{
    public class Country
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("rateFormated")]
        public decimal RateFormated { get; set; }

        [JsonProperty("diffFormated")]
        public decimal DiffFormated { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("diff")]
        public decimal Diff { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("validFromDate")]
        public DateTime ValidFromDate { get; set; }


    }
}
