using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using SF_Lang_Dictionary.Models;
using Azure.Identity;
using SF_Lang_Dictionary.Controllers.Auth;

var builder = WebApplication.CreateBuilder(args);
IWebHostEnvironment env = builder.Environment;

// A conlang needs unicode to print special characters of it's writing system latinization and IPA pronunciation characters
Console.OutputEncoding = env.IsProduction() ? Encoding.UTF8 : Encoding.Unicode;
Console.WriteLine($"Environment: {env.EnvironmentName}");

// Initialize JWT
string signatureKey;
string issuer;
string audience;
List<string> origins;

// If the environment is production, get the secrets from Azure Key Vault
if (env.IsProduction())
{
    // Initialize Azure Key Vault
    var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri") ?? throw new("Vault URI not found"));
    var tokenCredential = new DefaultAzureCredential(new DefaultAzureCredentialOptions { ExcludeAzurePowerShellCredential = true });
    SecretManager secretManager = new();

    signatureKey = secretManager.Client.GetSecret("signatureKey").Value.Value ?? throw new("Signature Key not found");
    issuer = secretManager.Client.GetSecret("issuer").Value.Value ?? throw new("Issuer not found");
    audience = secretManager.Client.GetSecret("audience").Value.Value ?? throw new("Issuer not found");
    origins = [issuer, audience];

    // Add Azure Key Vault to the configuration pipeline
    builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, tokenCredential);
}
else
{
    IConfiguration configuration = builder.Configuration;
    signatureKey = configuration.GetValue<string>("signatureKey") ?? throw new("Signature Key not found");
    issuer = configuration.GetValue<string>("issuer") ?? throw new("Issuer not found");
    audience = configuration.GetValue<string>("audience") ?? throw new("Audience not found");
    origins = [issuer, audience];
}

IdentityModelEventSource.ShowPII = true;


// Adds the DbContext in API services
builder.Services.AddDbContext<SfLangContext>(options =>
{
    var context = new SfLangContext();
});

if (env.IsProduction())
{
    // Adds CORS to API services
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("AllowSpecificOrigin", p =>
        {
            p.WithOrigins(origins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
    });
}
else
{
    // Adds CORS to API services
    builder.Services.AddCors(o =>
    {
        o.AddPolicy("DefaultPolicy", p =>
        {
            p.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
        });
    });
}


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
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors(env.IsProduction() ? "AllowSpecificOrigin" : "DefaultPolicy");
//app.UseCors("AllowSpecificOrigin");

app.Use(async (context, next) =>
{
    var origin = context.Request.Headers["Origin"];
    var method = context.Request.Method;

    // Log the origin and method for each request
    Console.WriteLine($"Request Origin: {origin}, HTTP Method: {method}");
    await next.Invoke();
});

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke();
    }
    catch (Exception ex)
    {
        // Log any CORS-related errors
        if (ex.Message.Contains("CORS"))
        {
            Console.WriteLine("CORS Exception: " + ex.Message);
        }

        throw;
    }
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();