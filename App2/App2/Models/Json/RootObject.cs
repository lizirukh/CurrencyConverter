using Newtonsoft.Json;

namespace App2.Models.Json
{
    public class RootObject
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("currencies")]
        public Country [] Currencies { get; set; }
    }
}
