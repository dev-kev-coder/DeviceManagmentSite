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
        /// 
        /// Will check underlying types in order to do fault tolerant type comparisons.
        /// 
        /// Properties on Classes that have a Nullable<T> should still be able to accept the type T as a value when being compared.
        /// 
        /// </summary>
        /// <param name="typeA"></param>
        /// <param name="typeB"></param>
        /// <returns></returns>
        public static bool AreSameOrNullableEquivalent(Type typeA, Type typeB)
        {
            var ntypeA = Nullable.GetUnderlyingType(typeA) ?? typeA;

            var ntypeB = Nullable.GetUnderlyingType(typeB) ?? typeB;

            return ntypeA == ntypeB;
        }

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
            if (value == null)
            {
                throw new Exception("Util is not designed to handle null gracefully as it is part of the DeviceInfo library.");
            }

            var targetType = type ?? value.GetType();

            if (targetType.Equals(typeof(bool[])))
            {
                return (bool[])value;
            }
            if (targetType.Equals(typeof(bool)))
            {
                return (bool)value;
            }
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
            if (targetType.Equals(typeof(UInt32)))
            {
                return (UInt32)value;
            }
            if (targetType.Equals(typeof(UInt32[])))
            {
                return (UInt32[])value;
            }
            if (targetType.Equals(typeof(UInt64)))
            {
                return (UInt64)value;
            }
            if (targetType.Equals(typeof(UInt64[])))
            {
                return (UInt64)value;
            }
            if (targetType.Equals(typeof(UInt128)))
            {
                return (UInt128)value;
            }
            if (targetType.Equals(typeof(UInt128[])))
            {
                return (UInt128)value;
            }
            if (targetType.Equals(typeof(DateTime)))
            {
                return (DateTime)value;
            }

            throw new Exception("Exhausted all list of types. Missing cast for " + targetType.FullName);
        }
    }
}
