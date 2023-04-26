var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var app = builder.Build();

app.MapGet("/", () =>  { 
    //new MatrixController(app.Configuration).DisplayImage();
    return "Hello World!";
    // string command = @"sudo ../rpi-rgb-led-matrix/utils/./led-image-viewer --led-rows=64 --led-cols=64 ../rpi-rgb-led-matrix/utils/1442802_amni3d_3d-among-us-gifs.gif\?f1601359412";
    // string result = "";
    // using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
    // {
    //     proc.StartInfo.FileName = "/bin/bash";
    //     proc.StartInfo.Arguments = "-c \" " + command + " \"";
    //     proc.StartInfo.UseShellExecute = false;
    //     proc.StartInfo.RedirectStandardOutput = true;
    //     proc.StartInfo.RedirectStandardError = true;
    //     proc.Start();

    //     result += proc.StandardOutput.ReadToEnd();
    //     result += proc.StandardError.ReadToEnd();

    //     proc.WaitForExit();
    // }
    // return result;
});

app.MapGet("/image", (string imageUrl) =>  { 
    new MatrixController(app.Configuration).DisplayImageWithDownload(imageUrl);
    return $"Image {imageUrl} downloaded and displayed";
});

app.Run();