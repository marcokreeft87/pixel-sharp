using PixelSharp.Constants;

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
}