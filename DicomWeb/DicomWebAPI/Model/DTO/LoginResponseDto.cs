using Newtonsoft.Json;

namespace DicomWebAPI.Model.DTO
{
    public class LoginResponseDto
    {
        public LocalUser? LocalUser { get; set; }

        [JsonProperty(PropertyName = "access_token")]
        public string Token { get; set; }

        public int ExpiresIn{ get;set;}

        public DateTime ExpiresAt { get;set;}

        public string Scope { get; set; }

        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }
    }
}
