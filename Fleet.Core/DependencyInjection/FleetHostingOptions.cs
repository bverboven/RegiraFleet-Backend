using Regira.Entities.DependencyInjection.ServiceCollections.Models;
using Regira.IO.Storage.Abstractions;


namespace Regira.Fleet.Core.DependencyInjection;

public class FleetHostingOptions
{
    public string DatabaseType { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public Func<IServiceProvider, IFileService>? FileServiceFactory { get; set; }
    public void ConfigureStorageService(Func<IServiceProvider, IFileService> configure) => FileServiceFactory = configure;

    /// <summary>
    /// Applied to the same UseEntities options instance the Fleet entities are registered on.
    /// Lets a web host add options this library cannot reach — UseAttachmentUris() lives in
    /// Regira.Entities.Web, which the console hosts do not (and should not) reference.
    /// </summary>
    public Action<EntityServiceCollectionOptions>? EntityOptionsFactory { get; set; }
    public void ConfigureEntities(Action<EntityServiceCollectionOptions> configure) => EntityOptionsFactory = configure;
}
