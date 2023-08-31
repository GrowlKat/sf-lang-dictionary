using SF_Lang_Dictionary;
using System.Text;

Console.OutputEncoding = Encoding.Unicode; // A conlang needs unicode to print special characters of it's writing system and IPA pronunciation characters

var builder = WebApplication.CreateBuilder(args);

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
        p.WithOrigins("http://localhost:3000", "http://localhost:5215", "https://localhost:7162")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.MapControllers();
app.Run();