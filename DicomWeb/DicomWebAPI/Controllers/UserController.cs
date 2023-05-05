using DicomWebAPI.Model;
using DicomWebAPI.Model.DTO;
using DicomWebAPI.Repository;
using DicomWebAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DicomWebAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly APIResponse _apiResponse;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _apiResponse = new();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            LoginResponseDto loginresponse = await _userRepository.Login(loginRequestDto);
            if (loginresponse.LocalUser == null || string.IsNullOrEmpty(loginresponse.Token))
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage =  "invalid username or password" ;
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = loginresponse;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(loginresponse);
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequestDto RequestDto)
        {
            var IsUniqueUser = _userRepository.IsUniqueUser(RequestDto.UserName);
            if(!IsUniqueUser) 
            {
                _apiResponse.IsSuccess = false;
                _apiResponse.ErrorMessage = "User already exist";
                _apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return BadRequest(_apiResponse);
            }

            var localuser = await _userRepository.Register(RequestDto);
            _apiResponse.IsSuccess = true;
            _apiResponse.Result = localuser;
            _apiResponse.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(localuser);
        }
    }

}
