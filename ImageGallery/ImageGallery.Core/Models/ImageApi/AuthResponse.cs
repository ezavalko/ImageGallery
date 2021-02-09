using Newtonsoft.Json;

namespace ImageGallery.Core.Models.ImageApi
{
    public class AuthResponse
    {
        [JsonProperty("auth")]
        public string Auth { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
