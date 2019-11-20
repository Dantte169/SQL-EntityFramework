namespace CarDealer.DTO.Export
{
    using Newtonsoft.Json;
    public class ExportPartDTO
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Price")]
        public string Price { get; set; }
    }
}
