using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils
{
    public class WMIQueryOption<T>
    {
        public string WMIClassName { get; set; }
        public WMIQueryOption(string className) 
        {
            WMIClassName = className;
        }

        public T CreateAndPopulate(Func<string, Type, object> getValue) 
        {
            var obj = this.CreateInstance();

            // Guard against creation errors
            if (obj == null) throw new Exception($"Could not create instance of type: {typeof(T)}");

            // Get public properties of a type that inherits class
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanWrite);

            // Go through each property and process it
            foreach (var prop in props)
            {
                var value = getValue(prop.Name, prop.PropertyType);

                prop.SetValue(obj, value, null);
            }

            return obj;
        }

        private T CreateInstance()
        {
            return Activator.CreateInstance<T>();
        }
    }

    public class WMIQueryOptionV2<T>
    {
        private CimQuerier _querier = new CimQuerier();
        public string WMIClassName { get; set; }
        
        public WMIQueryOptionV2(string className)
        {
            WMIClassName = className;
        }


        public T[] CreateAndPopulate()
        {
            var queryResults = _querier
                .QueryWMI($"SELECT * FROM {this.WMIClassName}")
                .Select(res =>
                {
                    var cimProps = res.CimInstanceProperties;

                    var constructedOjb = this.CreateAndPopulate((propName, propType) =>
                    {
                        return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
                    });

                    return constructedOjb;
                });

            return queryResults.ToArray();
        }

        public T CreateAndPopulate(Func<string, Type, object> getValue)
        {
            var obj = this.CreateInstance();

            // Guard against creation errors
            if (obj == null) throw new Exception($"Could not create instance of type: {typeof(T)}");

            // Get public properties of a type that inherits class
            var props = typeof(T)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.CanWrite);

            // Go through each property and process it
            foreach (var prop in props)
            {
                var value = getValue(prop.Name, prop.PropertyType);

                prop.SetValue(obj, value, null);
            }

            return obj;
        }

        private T CreateInstance()
        {
            return Activator.CreateInstance<T>();
        }
    }


    // Crazy dreams of a dynamic querier;
    //public class CimMulitQuerier
    //{
    //    private string _wmiNamespace = @"root\cimv2";
    //    private string _baseQuery = "SELECT * FROM";
    //    private CimSession _session;


    //    public CimMulitQuerier()
    //    {
    //        _session = CimSession.Create(null);
    //    }


    //}
}
