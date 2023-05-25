using PixelSharp.Constants;
using PixelSharp.Settings;

namespace PixelSharp.Helpers;

public static class ConfigurationHelper
{
    public static PixelDisplaySettings GetSettingsFromConfiguration(IConfiguration configuration)
    {
        var pixelDisplaySettings = configuration.GetSection(ConfigrationConstants.PixelDisplaySettings).Get<PixelDisplaySettings>();

        if (pixelDisplaySettings == null)
            throw new Exception("PixelDisplaySettings is null");

        return pixelDisplaySettings;
    }

    public static ApplicationSettings GetDevelopmentSettings(IConfiguration configuration)
    {
        var developmentSettings = configuration.GetSection(ConfigrationConstants.DevelopmentSettings).Get<ApplicationSettings>();

        if (developmentSettings == null)
            throw new Exception("DevelopmentSettings is null");

        return developmentSettings;
    }
}