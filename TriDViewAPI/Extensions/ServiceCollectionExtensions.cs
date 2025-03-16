namespace TriDViewAPI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var appServices = typeof(Program).Assembly.GetTypes()
                    .Where(t => t.Name.EndsWith("Service") && !t.IsInterface && !t.IsAbstract);
                    //.ToList();

            foreach (var service in appServices)
            {
                var serviceInterface = service.GetInterfaces().FirstOrDefault(i => i.Name == $"I{service.Name}");
                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, service);
                }
            }
            return services;
        }
        public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
        {
            var appRepositories = typeof(Program).Assembly.GetTypes()
                .Where(t => t.Name.EndsWith("Repository") && !t.IsInterface && !t.IsAbstract);

            foreach (var repository in appRepositories)
            {
                var repositoryInterface = repository.GetInterfaces().FirstOrDefault(i => i.Name == $"I{repository.Name}");
                if (repositoryInterface != null)
                {
                    services.AddScoped(repositoryInterface, repository);
                }
            }
            return services;
        }


    }
}
