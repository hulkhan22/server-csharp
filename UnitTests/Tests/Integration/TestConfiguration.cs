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
        // builder.ConfigureServices(services =>
        // {
        //     var loggerFactoryDescriptor = services.Where(
        //         d => d.ServiceType ==
        //              typeof(ILoggerFactory)).ToList();
        //
        //     foreach (var descriptor in loggerFactoryDescriptor)
        //     {
        //         services.Remove(descriptor);
        //     }
        //
        //     var loggerProviderDescriptor = services.Single(
        //         d => d.ServiceType ==
        //              typeof(SptLoggerProvider));
        //     services.Remove(loggerProviderDescriptor);
        //
        //
        //     var mockedLoggerFactory = Substitute.For<ILoggerFactory>();
        //     var mockedLogger = Substitute.For<ILogger>();
        //     mockedLoggerFactory.CreateLogger(Arg.Any<string>())
        //         .Returns(mockedLogger);
        //     services.AddSingleton<ILoggerFactory>(mockedLoggerFactory);
        //
        //     var sptLoggerDescriptors = services.Where(d => d.ServiceType.IsGenericType && d.ServiceType.GetGenericTypeDefinition() ==
        //         typeof(ISptLogger<>)).ToList();
        //     foreach (var descriptor in sptLoggerDescriptors)
        //     {
        //         services.Remove(descriptor);
        //         var type = (typeof(MockLogger<>).MakeGenericType(descriptor.ServiceType.GenericTypeArguments[0]));
        //         var abstractType = (typeof(ISptLogger<>).MakeGenericType(descriptor.ServiceType.GenericTypeArguments[0]));
        //         services.AddSingleton(abstractType, type);
        //     }
        // });

        builder.UseEnvironment(EnvironmentNames.Testing);
    }
}
