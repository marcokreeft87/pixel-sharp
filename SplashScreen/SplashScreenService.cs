public class SplashScreenService : IHostedService
{
    private int _ledRows;
    private int _ledColumns;

    public SplashScreenService(IConfiguration configuration)
    {
        (_ledRows, _ledColumns) = MatrixHelper.GetDimensionsFromConfiguration(configuration);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        MatrixHelper.ShowLogo(_ledRows, _ledColumns);

        Thread.Sleep(2000);

        // TODO - Show text "Waiting for commands"
        
        await Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}