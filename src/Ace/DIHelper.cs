using Ace.Application;
using Ace.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ace
{
    public static class DIHelper
    {
        /// <summary>
        /// 从指定程序集中找出所有的仓储实现，注册到 DI 容器中
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterRepositories(this IServiceCollection services, Assembly assembly)
        {
            Type typeOf_IRepository = typeof(IRepository);
            var repositoryTypes = assembly.GetTypes().Where(a => a.IsClass && !a.IsAbstract && typeOf_IRepository.IsAssignableFrom(a));

            foreach (var repositoryType in repositoryTypes)
            {
                var implementedInterfaces = repositoryType.GetInterfaces().Where(a => a != typeof(IDisposable) && a != typeOf_IRepository);
                foreach (Type implementedInterface in implementedInterfaces)
                {
                    services.AddScoped(implementedInterface, repositoryType);
                }
            }

            return services;
        }

        /// <summary>
        /// 从指定程序集中找出所有的应用层服务实现，注册到 DI 容器中
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterBizServices(this IServiceCollection services, Assembly assembly)
        {
            Type typeOf_IService = typeof(IService);
            var serviceTypes = assembly.GetTypes().Where(a => a.IsClass && !a.IsAbstract && typeOf_IService.IsAssignableFrom(a));

            foreach (var serviceType in serviceTypes)
            {
                var implementedInterfaces = serviceType.GetInterfaces().Where(a => a != typeof(IDisposable) && a != typeOf_IService);
                foreach (Type implementedInterface in implementedInterfaces)
                {
                    services.AddScoped(implementedInterface, serviceType);
                }
            }

            return services;
        }
    }

}
