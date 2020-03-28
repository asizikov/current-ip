using CurrentIp.Storage.Local;
using Microsoft.Extensions.DependencyInjection;

namespace CurrentIp.Storage.DependencyInjection
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddInMemoryStorage(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IRecordsRepository, InMemoryRecordsRepository>();
            return serviceCollection;
        }
    }
}