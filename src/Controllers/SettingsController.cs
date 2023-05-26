using Microsoft.AspNetCore.Mvc;
using PixelSharp.Constants;
using PixelSharp.Helpers;
using PixelSharp.Models;

namespace PixelSharp.Controllers;

public class SettingsController :  Controller
{
    private readonly IConfiguration _configuration;

    public SettingsController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpGet("settings")]
    public PixelDisplaySettings GetSettings() => ConfigurationHelper.GetSettingsFromConfiguration(_configuration);
    
    [HttpPost("settings")]
    public IActionResult UpdateSettings([FromBody] PixelDisplaySettings newSettings)
    {
        var section = _configuration.GetSection(ConfigrationConstants.PixelDisplaySettings);
        section["LedRows"] = newSettings.LedRows.ToString();
        section["LedColumns"] = newSettings.LedColumns.ToString();
        section["HardwareMapping"] = newSettings.HardwareMapping;

        return Ok();
    }
}
