var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

//app.MapGet("/test/", () => "World!");
app.Run(async (context) => await context.Response.WriteAsync("world"));
app.Run();
