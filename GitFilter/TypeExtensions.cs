using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GitFilter
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetAllInterfaces(this Type type)
        {
            return type.InternalGetAllInterfaces().Distinct();
        }

        private static IEnumerable<Type> InternalGetAllInterfaces(this Type type)
        {
            foreach (Type internalGetAllInterface in type.BaseType.InternalGetAllInterfaces())
                yield return internalGetAllInterface;
            foreach (Type @interface in type.GetInterfaces())
                yield return @interface;
        }
    }
}
