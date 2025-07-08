using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils
{
    public class DeviceInfoTypeCaster
    {
        /// <summary>
        /// Extracts out a boxed objects value.
        /// Will extract out the type of the object and use it map to the appropriate type cast.
        /// 
        /// A type override is provided incase the boxed object is unstable.
        /// Useful when you want a second source of truth for your type casting
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static dynamic UnboxToType(object value, Type? type = null)
        {
            var targetType = type ?? value.GetType();


            if (targetType.Equals(typeof(string[])))
            {
                return (string[])value;
            }
            if (targetType.Equals(typeof(string))) 
            {
                return (string)value; 
            }
            if (targetType.Equals(typeof(UInt16[])))
            {
                return (UInt16[])value;
            }
            if (targetType.Equals(typeof(UInt16)))
            {
                return (UInt16)value;
            }

            throw new Exception("Exhausted all list of types. Missing cast for " + targetType.FullName);
        }
    }
}
