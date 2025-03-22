using CollectiveData.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CollectiveData.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCollectiveDataServices(this IServiceCollection services)
        {
            // Register repositories
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<ITagCategoryRepository, TagCategoryRepository>();
            services.AddScoped<IArtworkRepository, ArtworkRepository>();
            services.AddScoped<ICollectionRepository, CollectionRepository>();

            return services;
        }
    }
}
