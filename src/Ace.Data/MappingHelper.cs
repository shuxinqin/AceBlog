using Chloe.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ace.Data
{
    public class MappingHelper
    {
        public static IEntityTypeBuilder[] FindMapsFromAssembly(Assembly assembly)
        {
            Type typeOf_IMap = typeof(IEntityTypeBuilder);

            List<Type> mapTypes = assembly.GetTypes().Where(a => a.IsClass && !a.IsAbstract && typeOf_IMap.IsAssignableFrom(a)).ToList();
            var maps = mapTypes.Select(a => (IEntityTypeBuilder)Activator.CreateInstance(a));

            return maps.ToArray();
        }
    }
}
