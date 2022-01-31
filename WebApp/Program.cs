using System.Reflection;
using Microsoft.Extensions.Options;
using Serilog;
using DadataRequestLibrary;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.Configure<DadataConfiguration>(
    builder.Configuration.GetSection(DadataConfiguration.Configuration));
//var dadataConfig = builder.Configuration.GetSection(DadataConfiguration.Configuration).Get<DadataConfiguration>();

//builder.Services.AddSingleton<DadataConfiguration>();
builder.Services.AddTransient<DadataLibrary>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();

public class TokenContainer
{
    private readonly DadataConfiguration _options;

    public TokenContainer(IOptions<DadataConfiguration> options)
    {
        _options = options.Value;
    }
    public string GetToken()
    {
        return _options.Token;
    }
}
