using PixelSharp.Helpers;
using PixelSharp.Settings;

namespace PixelSharp.SplashScreen;

public class SplashScreenService : IHostedService
{    
    private IPixelSharpMatrix _matrix;
    public ApplicationSettings ApplicationSettings { get; }

    public SplashScreenService(IConfiguration configuration, IPixelSharpMatrix matrix)
    {
        ApplicationSettings = ConfigurationHelper.GetDevelopmentSettings(configuration);
        _matrix = matrix;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (!ApplicationSettings.UseMatrix)
            return;

        _matrix.DrawLogo();

        Thread.Sleep(2000);

        //_matrix.ScrollText("Awaiting your command...", cancellationToken);
        
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}