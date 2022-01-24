namespace App2.Models.Json
{
    public class Info
    {
        public string originalCode { get; set; }
        public string destinationCode { get; set; }
        public double originalRate { get; set; }
        public double destinationRate { get; set; }
        public double result { get; set; }
        public DateTime? conversionDate { get; set; }
    }
}
