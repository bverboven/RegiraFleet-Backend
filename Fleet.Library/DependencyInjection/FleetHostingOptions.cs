using Regira.IO.Storage.Abstractions;


namespace Regira.Fleet.DependencyInjection;

public class FleetHostingOptions
{
    public string? ConnectionString { get; set; }
    internal Func<IServiceProvider, IFileService>? FileServiceFactory { get; private set; }
    public void ConfigureStorageService(Func<IServiceProvider, IFileService> configure) => FileServiceFactory = configure;
}
