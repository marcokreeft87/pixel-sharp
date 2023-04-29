var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddHostedService<SplashScreenService>();

builder.Services.AddSingleton<IPixelSharpMatrix, PixelSharpMatrix>();

var app = builder.Build();

app.MapGet("/", () =>  { 
    return "Hello World!";
});

app.MapControllers();

app.Run();