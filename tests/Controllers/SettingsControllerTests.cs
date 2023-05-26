using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PixelSharp.Controllers;
using PixelSharp.Models;

namespace tests.Controllers;

[TestFixture]
public class SettingsControllerTests
{
    private IConfiguration _configuration;

    [SetUp]
    public void Setup()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(TestContext.CurrentContext.TestDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
    }

    [Test]
    public void GivenAppsettings_WhenGetSettings_SettingsShouldBeReturned()
    {
        // Arrange
        var controller = new SettingsController(_configuration);

        // Act
        var result = controller.GetSettings();

        // Assert
        result.Should().BeOfType<PixelDisplaySettings>();
        result.LedColumns.Should().Be(64);
        result.LedRows.Should().Be(64);
    }

    [Test]
    public void GivenJobjectWithChanges_WhenUpdateSettings_SettingsShouldBeUpdated()
    {
        // Arrange
        var controller = new SettingsController(_configuration);
        var existing = controller.GetSettings();

        existing.HardwareMapping = "adafruit";

        // Act
        var result = controller.UpdateSettings(existing);

        // Assert
        result.Should().BeOfType<OkResult>();

        var assertSettings = controller.GetSettings();
        assertSettings.HardwareMapping.Should().Be("adafruit");
    }
}
