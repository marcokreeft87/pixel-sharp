// using Microsoft.AspNetCore.Mvc;

// public class PythonController : Controller
// {   
//     private int _ledRows;
//     private int _ledColumns;

//     public PythonController(IConfiguration configuration)
//     {
//         (_ledRows, _ledColumns) = MatrixHelper.GetDimensionsFromConfiguration(configuration);
//     }

//     // https://static.wikia.nocookie.net/pure-good-wiki/images/3/3e/MPSS_Mario.png
    
//     [HttpGet("python/download")]
//     public void DisplayImageWithDownload(string imageUrl)
//     {
//         var command = $@"wget -O ../rpi-rgb-led-matrix/utils/image.png {imageUrl}";
//         var result = "";
//         using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
//         {
//             proc.StartInfo.FileName = "/bin/bash";
//             proc.StartInfo.Arguments = "-c \" " + command + " \"";
//             proc.StartInfo.UseShellExecute = false;
//             proc.StartInfo.RedirectStandardOutput = true;
//             proc.StartInfo.RedirectStandardError = true;
//             proc.Start();

//             result += proc.StandardOutput.ReadToEnd();
//             result += proc.StandardError.ReadToEnd();

//             proc.WaitForExit();
//         }

//         DisplayImage("../rpi-rgb-led-matrix/utils/image.png");
//     }

//     [HttpGet("python/image")]
//     public void DisplayImage(string imageUrl = @"../rpi-rgb-led-matrix/utils/1442802_amni3d_3d-among-us-gifs.gif\?f1601359412")
//     {
//         var command = $@"../rpi-rgb-led-matrix/utils/./led-image-viewer --led-rows={_ledRows} --led-cols={_ledColumns} {imageUrl}";
//         var result = "";
//         using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
//         {
//             proc.StartInfo.FileName = "/bin/bash";
//             proc.StartInfo.Arguments = "-c \" " + command + " \"";
//             proc.StartInfo.UseShellExecute = false;
//             proc.StartInfo.RedirectStandardOutput = true;
//             proc.StartInfo.RedirectStandardError = true;
//             proc.Start();

//             result += proc.StandardOutput.ReadToEnd();
//             result += proc.StandardError.ReadToEnd();

//             proc.WaitForExit();
//         }
//     }    
// }