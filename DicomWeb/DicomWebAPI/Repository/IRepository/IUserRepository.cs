using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;

namespace DicomWebAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
