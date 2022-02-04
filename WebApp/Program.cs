using System.Reflection;
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

//var dadataConfig = builder.Configuration.GetSection(DadataConfiguration.Configuration).Get<DadataConfiguration>(); оставил для напоминания

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