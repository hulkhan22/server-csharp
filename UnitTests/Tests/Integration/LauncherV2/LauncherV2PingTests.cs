using System.Text.Json;
using SPTarkov.Server.Core.Models.Spt.Launcher;

namespace UnitTests.Tests.Integration.LauncherV2;

[TestClass]
public class LauncherV2PingTests : TestConfiguration
{
    [TestMethod]
    public async Task Launcher_Ping_Returns_Ok_And_Pong()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/launcher/v2/ping");

        // Assert
        Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(responseString);
        Assert.IsNotEmpty(responseString);

        LauncherV2PingResponse responseObject = null;

        try
        {
            responseObject = JsonSerializer.Deserialize<LauncherV2PingResponse>(responseString);
        }
        catch (Exception e)
        {
            Assert.Fail("Failed to deserialize response: " + e.Message);
        }

        Assert.IsNotNull(responseObject);
        Assert.AreEqual("Pong!", responseObject.Response);
    }
}


