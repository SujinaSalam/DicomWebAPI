namespace DicomWebAPI.Model
{
    public class LocalUser
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }
}
