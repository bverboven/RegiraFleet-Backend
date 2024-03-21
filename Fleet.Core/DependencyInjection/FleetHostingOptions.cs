using Regira.IO.Storage.Abstractions;


namespace Regira.Fleet.Core.DependencyInjection;

public class FleetHostingOptions
{
    public string? ConnectionString { get; set; }
    public Func<IServiceProvider, IFileService>? FileServiceFactory { get; set; }
    public void ConfigureStorageService(Func<IServiceProvider, IFileService> configure) => FileServiceFactory = configure;
}
