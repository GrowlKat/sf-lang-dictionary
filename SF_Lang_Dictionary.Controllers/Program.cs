using SF_Lang_Dictionary;
using System.Configuration;
using System.Text;

Console.OutputEncoding = Encoding.Unicode;

var builder = WebApplication.CreateBuilder(args);
var context = new SfLangContext();

builder.Services.AddDbContext<SfLangContext>(options =>
{
    var context = new SfLangContext();
});

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