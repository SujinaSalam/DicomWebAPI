namespace DicomWebAPI.Model
{
    public class OktaTokenSettings
    {
        public string? ClientID { get; set; }
        public string ClientSecret { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;
        public string AuthorizationServerId { get; set; } = string.Empty;
        public string Audience { get; set;} = string.Empty;
    }
}
