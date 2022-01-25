using System.Reflection;
using Microsoft.Extensions.Options;
using WebApplication1;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.Configure<PositionOptions>(
    builder.Configuration.GetSection(PositionOptions.Position));

builder.Services.AddTransient<TokenContainer>();

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
    private readonly PositionOptions _options;

    public TokenContainer(IOptions<PositionOptions> options)
    {
        _options = options.Value;
    }
    public string GetToken()
    {
        return _options.Token;
    }
}
