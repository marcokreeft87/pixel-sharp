var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
builder.Services.AddControllers();
builder.Services.AddHostedService<SplashScreenService>();

var app = builder.Build();

app.MapGet("/", () =>  { 
    return "Hello World!";
});

// app.MapGet("/render", async (string imageUrl) =>  { 
//     return await new MatrixController(app.Configuration).RenderImage(imageUrl);
// });

// app.MapGet("/image", (string imageUrl) =>  { 
//     new PythonController(app.Configuration).DisplayImageWithDownload(imageUrl);
//     return $"Image {imageUrl} downloaded and displayed";
// });

app.MapControllers();

app.Run();