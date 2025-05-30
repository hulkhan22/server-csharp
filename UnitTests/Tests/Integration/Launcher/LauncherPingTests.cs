namespace UnitTests.Tests.Integration.Launcher;

[TestClass]
public class LauncherPingTests : TestConfiguration
{
    [TestMethod]
    public async Task Launcher_Ping_Returns_Ok_And_Pong()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/launcher/ping");

        // Assert
        Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.OK);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(responseString);
        Assert.IsNotEmpty(responseString);
        Assert.AreEqual("\"pong!\"", responseString);
    }
}


