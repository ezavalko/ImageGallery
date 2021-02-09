using Newtonsoft.Json;

namespace ImageGallery.Core.Models.ImageApi
{
    public class UnathorizedResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
