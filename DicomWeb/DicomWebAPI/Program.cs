using DicomWebAPI;
using DicomWebAPI.Data;
using DicomWebAPI.Repository;
using DicomWebAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using DicomWebAPI.Model;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultSqlConnection"));
});
var key = builder.Configuration.GetValue<string>("ApiSetting:Secret");
// JWT
//builder.Services.AddAuthentication(
//    option => {
//        option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//        option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//    }).AddJwtBearer(
//    x =>
//    {
//        x.RequireHttpsMetadata = false;
//        x.SaveToken = true;
//        x.TokenValidationParameters = new TokenValidationParameters()
//        {
//            ValidateIssuerSigningKey = true,
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
//            ValidateIssuer = false,
//            ValidateAudience = false
//        };
//    }
//    );
//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//}).AddOpenIdConnect(options =>
//{
//    options.ClientId = builder.Configuration["Okta:ClientID"];
//    options.ClientSecret = builder.Configuration["Okta:ClientSecret"];
//    options.Authority = builder.Configuration["Okta:Issuer"];
//    options.CallbackPath = "/authorization-code/callback";
//    options.ResponseType = "code";
//    options.SaveTokens = true;
//    options.Scope.Add("openid");
//    options.Scope.Add("profile");
//    options.TokenValidationParameters.ValidateIssuer = false;
//    options.TokenValidationParameters.NameClaimType = "name";
//}).AddCookie();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultSignOutScheme = OktaDefaults.ApiAuthenticationScheme;
    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
}).AddOktaWebApi(new OktaWebApiOptions
{
    OktaDomain = builder.Configuration["Okta:Issuer"],
    Audience = builder.Configuration["Okta:Audience"],
    AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"],
});

builder.Services.AddAuthorization(); // newly added line
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// JWT
//builder.Services.AddSwaggerGen(options => 
//{
//    options.AddSecurityDefinition("Bearer",
//                                    new OpenApiSecurityScheme { Description = "JWT authetication using Bearer",
//                                    Name ="Authorization",
//                                    In = ParameterLocation.Header,
//                                    Scheme = "Bearer"});
//    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                            {
//                                Type = ReferenceType.SecurityScheme,
//                                Id= "Bearer"
//                            },
//                Scheme = "oauth2",
//                Name="Bearer",
//                In=ParameterLocation.Header
//            },
//            new List<string>()
//        }
//    });
//});

builder.Services.AddSwaggerGen(options =>
{
    // Add Okta OAuth support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Okta authorization",
        Name= "Authorization",
        In = ParameterLocation.Header,
        Type= SecuritySchemeType.ApiKey
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Name = "Bearer",
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<String>()
        }
    });
    });
builder.Services.Configure<OktaTokenSettings>(options =>
{
    options.ClientID = builder.Configuration["Okta:ClientID"];
    options.ClientSecret = builder.Configuration["Okta:ClientSecret"];
    options.Domain = builder.Configuration["Okta:Issuer"];
    options.AuthorizationServerId = builder.Configuration["Okta:AuthorizationServerId"];
    options.Audience = builder.Configuration["Okta:Audience"];
});
builder.Services.AddScoped<IPatientRepository,PatientRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStudyRepository, StudyRepository>();
builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();
builder.Services.AddAutoMapper(typeof(MappingConfig));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
