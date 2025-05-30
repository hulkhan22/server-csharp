using System.Text.Json;
using SPTarkov.Server.Core.Models.Eft.Profile;

namespace UnitTests.Tests.Integration.Launcher;

[TestClass]
public class LauncherServerTests : TestConfiguration
{
    [TestMethod]
    public async Task Launcher_Connect_Returns_Ok_And_Proper_Json_Object()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/launcher/server/connect");

        // Assert
        Assert.AreEqual(System.Net.HttpStatusCode.OK, response.StatusCode);
        var responseString = await response.Content.ReadAsStringAsync();
        Assert.IsNotNull(responseString);
        Assert.IsNotEmpty(responseString);
        ConnectResponse responseObject = null;

        try
        {
            responseObject = JsonSerializer.Deserialize<ConnectResponse>(responseString);
        }
        catch (Exception e)
        {
            Assert.Fail("Failed to deserialize response: " + e.Message);
        }

        Assert.IsNotNull(responseObject!.Editions);
        Assert.IsNotEmpty(responseObject.Editions);
        Assert.IsNotNull(responseObject.ProfileDescriptions);
        Assert.IsNotEmpty(responseObject.ProfileDescriptions);
        Assert.IsNotNull(responseObject.BackendUrl);

        try
        {
            _ = new Uri(responseObject.BackendUrl);
        }
        catch (Exception e)
        {
            Assert.Fail("Failed to parse backend url: " + e.Message);
        }
    }
}
