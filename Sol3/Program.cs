using System.Reflection;
using FacilityContextLib;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sol3.Profiles;
using Sol3;
using UserContextLib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Sol3.Binder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidation();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    //добавляет кнопочку в сваггер
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Authtorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>(); 
});
KeysConfiguration keyConfig = new();
builder.Configuration.GetSection(KeysConfiguration.Configuration).Bind(keyConfig);
//var keyConfig = builder.Configuration.GetSection(KeysConfiguration.Configuration).Get<KeysConfiguration>(); альтернативное получение экземпляра

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyConfig.Key1)),
            ValidateIssuer  = false,
            ValidateAudience = false,
        };
    });


builder.Services.AddSingleton<KeysConfiguration>(o => keyConfig);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IValidator<TankDTO>, TankDTOValidator>();
builder.Services.AddTransient<IValidator<UnitDTO>, UnitDTOValidator>();
builder.Services.AddDbContext<FacilityContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Credentials")));
builder.Services.AddDbContext<UserContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("Credentials2")));
builder.Services.AddTransient<Sol3.FacilityRepo>();
builder.Services.AddTransient<Sol3.UserDBRepo>();
builder.Services.AddHostedService<VolumeUpdateHostedService>();
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});
builder.Services.AddMvc(
        config => config.ModelBinderProviders.Insert(0, new CustomBinderProvider())
    );

var app = builder.Build();

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
