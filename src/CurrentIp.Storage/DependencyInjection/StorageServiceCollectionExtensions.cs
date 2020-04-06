using Microsoft.Extensions.DependencyInjection;

namespace CurrentIp.Storage.DependencyInjection {
  public static class StorageServiceCollectionExtensions {
    public static IServiceCollection AddPageStorage(this IServiceCollection serviceCollection) {
      serviceCollection.AddTransient<IRecordsRepository, RecordsRepository>();
      serviceCollection.AddTransient<IMachineHistoryStorageProvider, MachineHistoryStorageProvider>();
      return serviceCollection;
    }
  }
}