using Microsoft.AspNetCore.Mvc.Testing;
using SPTarkov.Server;
using SPTarkov.Server.Core.Constants;

namespace UnitTests.Tests.Integration;

[TestClass]
public class TestConfiguration
{
    private static BasicWebApplicationFactory<Program> _factory;
    protected static HttpClient _client;

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext _)
    {
        _factory = new BasicWebApplicationFactory<Program>();
        _client = _factory.CreateClient();

        _client.DefaultRequestHeaders.Add("requestcompressed","0");
        _client.DefaultRequestHeaders.Add("responsecompressed","0");
    }

    [AssemblyCleanup]
    public static void AssemblyCleanup(TestContext _)
    {
        _factory.Dispose();
    }
}

public class BasicWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment(EnvironmentNames.Testing);
    }
}
