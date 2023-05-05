namespace DicomWebAPI.Model.DTO
{
    public class RegistrationRequestDto
    {
        public string UserName { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
