using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SF_Lang_Dictionary.Models;
using Azure.Identity;
using SF_Lang_Dictionary.Controllers.Auth;

// A conlang needs unicode to print special characters of it's writing system latinization and IPA pronunciation characters
Console.OutputEncoding = Encoding.Unicode;

IConfiguration config = new ConfigurationBuilder().AddUserSecrets("2f781bee-b429-4f52-a84d-3f36352c147c").Build();
var builder = WebApplication.CreateBuilder(args);
var keyVaultEndpoint = new Uri(config.GetValue<string>("VaultUri") ?? throw new("Vault URI not found"));
var tokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeAzurePowerShellCredential = true });
SecretManager secretManager = new();
string signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value;
List<string> origins = new()
{
    config.GetValue<string>("issuer") ?? throw new("Origins not found"),
    config.GetValue<string>("audience") ?? throw new("Origins not found")
};

IdentityModelEventSource.ShowPII = true;

// Add Azure Key Vault to the configuration pipeline
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, tokenCredential);

// Adds the DbContext in API services
builder.Services.AddDbContext<SfLangContext>(options =>
{
    var context = new SfLangContext();
});

// Adds CORS to API services
builder.Services.AddCors(o =>
{
    o.AddDefaultPolicy(p =>
    {
        p.WithOrigins(origins.ToArray())
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Adds authentication to API services
builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = config.GetValue<string>("issuer"),
            ValidAudience = config.GetValue<string>("audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey ?? throw new("Signature Key not found")))
        };
    });

// Adds NewtonsoftJson to API services, making JSON easier to work with
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "SF Lang API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();