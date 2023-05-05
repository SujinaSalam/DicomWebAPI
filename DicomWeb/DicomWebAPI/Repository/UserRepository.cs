using DicomWebAPI.Model.DTO;
using DicomWebAPI.Model;
using DicomWebAPI.Repository.IRepository;
using DicomWebAPI.Data;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using YamlDotNet.Core.Tokens;

namespace DicomWebAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _secretkey;
        private readonly IOptions<OktaTokenSettings> _oktaTokenSettings;
        public UserRepository(IOptions<OktaTokenSettings> oktaTokenSettings, ApplicationDbContext context, IMapper mapper,IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _secretkey = configuration.GetValue<string>("ApiSetting:Secret");
            _oktaTokenSettings = oktaTokenSettings;
        }

        public bool IsUniqueUser(string username)
        {
            var localuser = _context.LocalUsers.FirstOrDefault(item=>item.UserName== username);
            if (localuser == null) 
            {
                return true;
            }
            return false;
        }
        public async Task<LocalUser> Register(RegistrationRequestDto registrationRequestDto)
        {
            LocalUser localUser = _mapper.Map<LocalUser>(registrationRequestDto);
            await _context.LocalUsers.AddAsync(localUser);
            await _context.SaveChangesAsync();
            localUser.Password = "";
            return localUser;
        }
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            //if(loginRequestDto == null )
            //{
            //    return null;
            //}

            //var user = await _context.LocalUsers.FirstOrDefaultAsync(item=>item.UserName.ToLower() == loginRequestDto.UserName.ToLower() && item.Password == loginRequestDto.Password);
            //if (user == null)
            //{
            //    var response1 = new LoginResponseDto()
            //    {
            //        Token = "",
            //        LocalUser = null

            //    };
            //    return response1;
            //}

            // If user was found, we need to generate OKta token
            var loginResponse = new LoginResponseDto();
            var client = new HttpClient();
            var clientId = _oktaTokenSettings.Value.ClientID;
            var clientSecret = _oktaTokenSettings.Value.ClientSecret;
            var clientcred = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic",Convert.ToBase64String(clientcred));
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var postMessage = new Dictionary<string, string>();
            postMessage.Add("grant_type", "password");
            postMessage.Add("username", loginRequestDto.UserName);
            postMessage.Add("password", loginRequestDto.Password);
            postMessage.Add("scope", "openid");
            //postMessage.Add("client_id", _oktaTokenSettings.Value.ClientID);
            //postMessage.Add("client_secret", _oktaTokenSettings.Value.ClientSecret);



            /// v1 / token
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, $"{_oktaTokenSettings.Value.Domain}/oauth2/default/v1/token")
            {
                Content = new FormUrlEncodedContent(postMessage)
            };

            var response  = await client.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var jsonserializerstring = new JsonSerializerSettings();
                var json = await response.Content.ReadAsStringAsync();




                loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(json, jsonserializerstring);
                loginResponse.LocalUser = new();
                loginResponse.LocalUser.UserName = loginRequestDto.UserName;
                loginResponse.ExpiresAt = DateTime.UtcNow.AddDays(1);



            }
            else
            {
                var jsonserializerstring1 = new JsonSerializerSettings();
                var json1 = await response.Content.ReadAsStringAsync();
                loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(json1, jsonserializerstring1);
            }

            
            return loginResponse;
        }

        //public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        //{
        //    if (loginRequestDto == null)
        //    {
        //        return null;
        //    }

        //    var user = await _context.LocalUsers.FirstOrDefaultAsync(item => item.UserName.ToLower() == loginRequestDto.UserName.ToLower() && item.Password == loginRequestDto.Password);
        //    if (user == null)
        //    {
        //        var response1 = new LoginResponseDto()
        //        {
        //            Token = "",
        //            LocalUser = null

        //        };
        //        return response1;
        //    }

        //    // If user was found, we need to generate JWT token
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_secretkey);
        //    var tokenDescriptor = new SecurityTokenDescriptor()
        //    {
        //        Subject = new ClaimsIdentity(
        //            new Claim[]
        //            {
        //                new Claim(ClaimTypes.Name,user.Id.ToString()),
        //                new Claim(ClaimTypes.Role,user.Role)
        //            }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    LoginResponseDto response = new LoginResponseDto()
        //    {
        //        Token = tokenHandler.WriteToken(token),
        //        LocalUser = user

        //    };
        //    return response;
        //}
    }



}
