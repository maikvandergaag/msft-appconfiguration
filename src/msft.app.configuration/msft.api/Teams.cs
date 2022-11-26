using System.Text.Json.Serialization;

namespace msft.api {
    public class Teams {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? Points { get; set; }
        
        public string? Name { get; set; }
    }
}