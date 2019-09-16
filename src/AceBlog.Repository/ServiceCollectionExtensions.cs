using Ace;
using Ace.Data;
using Ace.Domain;
using Chloe;
using Chloe.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace AceBlog.Repository
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            AddDatabase(services, config);
            AddRepositories(services);

            return services;
        }

        public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
        {
            string connString = config["db:ConnString"];
            string dbType = config["db:DbType"];

            var defMaps = MappingHelper.FindMapsFromAssembly(Assembly.GetExecutingAssembly());
            DbConfiguration.UseTypeBuilders(defMaps);

            IDbContextFactory dbContextFactory = new DefaultDbContextFactory(dbType, connString);
            services.AddSingleton<IDbContextFactory>(dbContextFactory);
            services.AddScoped<IDbContext>(a =>
            {
                return a.GetService<IDbContextFactory>().CreateContext();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterRepositories(assembly);

            return services;
        }
    }
}
