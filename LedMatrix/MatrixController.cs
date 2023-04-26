
public class MatrixController
{
    private int _ledRows;
    private int _ledColumns;

    public MatrixController(IConfiguration configuration)
    {
        var pixelDisplaySettings = configuration.GetSection("PixelDisplaySettings").Get<PixelDisplaySettings>();

        if(pixelDisplaySettings == null)
            throw new Exception("PixelDisplaySettings is null");

        _ledRows = pixelDisplaySettings.LedRows;
        _ledColumns = pixelDisplaySettings.LedColumns;
    }

    // https://static.wikia.nocookie.net/pure-good-wiki/images/3/3e/MPSS_Mario.png
    public void DisplayImageWithDownload(string imageUrl)
    {
        var command = $@"wget -O ../rpi-rgb-led-matrix/utils/image.png {imageUrl}";
        var result = "";
        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
        {
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            result += proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();

            proc.WaitForExit();
        }

        DisplayImage("../rpi-rgb-led-matrix/utils/image.png");
    }

    public void DisplayImage(string imageUrl = @"../rpi-rgb-led-matrix/utils/1442802_amni3d_3d-among-us-gifs.gif\?f1601359412")
    {
        var command = $@"sudo ../rpi-rgb-led-matrix/utils/./led-image-viewer --led-rows={_ledRows} --led-cols={_ledColumns} {imageUrl}";
        var result = "";
        using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
        {
            proc.StartInfo.FileName = "/bin/bash";
            proc.StartInfo.Arguments = "-c \" " + command + " \"";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.Start();

            result += proc.StandardOutput.ReadToEnd();
            result += proc.StandardError.ReadToEnd();

            proc.WaitForExit();
        }
    }
}