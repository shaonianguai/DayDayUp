using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzUXRibbon.Internal
{
    internal static class TypeHelper
    {
        public static bool InheritsFrom(this Type type, string typeName)
        {
            var currentType = type;

            do
            {
                if (currentType.Name == typeName)
                {
                    return true;
                }

                currentType = currentType.BaseType;
            }
            while (currentType != null
                   && currentType != typeof(object));

            return false;
        }
    }
}
