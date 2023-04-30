public class SplashScreenService : IHostedService
{    private IPixelSharpMatrix _matrix;

    public SplashScreenService(IPixelSharpMatrix matrix)
    {
        _matrix = matrix;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
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