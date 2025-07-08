using Microsoft.Management.Infrastructure;
using System;
using System.Collections.Generic;
using System.Reflection;

// Below is a link for all definitions for WMI classes
// https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-provider

namespace AgentInstaller.Service.utils.wmiClassOptions
{
    public class WMIOptionQuerier : IDisposable
    {
        private CimQuerier? _querier;

        public WMIOptionQuerier()
        {
            _querier = new CimQuerier();

        }

        public  WMIQueryOption<TWMIClassDefinition> CreateQueryOption<TWMIClassDefinition>(string wmiClassName)
        {
            return new WMIQueryOption<TWMIClassDefinition>(wmiClassName, _querier);

        }

        public void Dispose() 
        {
            _querier = null;
        }
    }


    public class CimQuerier
    {
        private string _wmiNamespace = @"root\cimv2";

        private string _queryStructure = "WQL";

        private CimSession _defaultCimSession = null;

        public CimQuerier()
        {
            _defaultCimSession = CimSession.Create(null);
        }

        public CimQuerier(string wmiNameSpace, string queryStructure)
        {
            _wmiNamespace = wmiNameSpace;
            _queryStructure = queryStructure;
            _defaultCimSession = CimSession.Create(null);
        }

        public IEnumerable<CimInstance> QueryWMI(string query)
        {
            return _defaultCimSession.QueryInstances(_wmiNamespace, _queryStructure, query);
        }
    }


    public class WMIQueryOption<T>
    {
        private CimQuerier _querier;
        public string WMIClassName { get; set; }

        public WMIQueryOption(string className, CimQuerier? querier = null)
        {
            WMIClassName = className;

            // If no querier is provided then we will make one
            if (querier != null)
            {
                _querier = querier;
            }
            else
            {
                _querier = new CimQuerier();
            }
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
}
