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

var builder = WebApplication.CreateBuilder(args);

// Initialize Azure Key Vault
var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri") ?? throw new("Vault URI not found"));
var tokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeAzurePowerShellCredential = true });
SecretManager secretManager = new();

// Initialize JWT
string signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value ?? throw new("Signature Key not found");
string issuer = secretManager.Client.GetSecret("issuer").Value.Value ?? throw new("Issuer not found");
string audience = secretManager.Client.GetSecret("audience").Value.Value ?? throw new("Issuer not found");
List<string> origins = new() { issuer, audience };

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
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signatureKey))
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