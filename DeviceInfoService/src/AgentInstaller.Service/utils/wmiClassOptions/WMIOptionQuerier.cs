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

        /// <summary>
        /// WMI Querier/Mapper utility
        /// Provide class definition that maps to the WMI return types on the MS Doc page.
        /// 
        /// Leverages relfection to properties from WMI resultes.
        /// Properties must be declared a public for util to have access
        /// </summary>
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
        public List<string> WmiNullQueryProperties {  get; } 
        public List <string> WMIQueryPropertiesNotFound { get; }

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

            WmiNullQueryProperties = new List<string>();
            WMIQueryPropertiesNotFound = new List<string>();
        }

        private bool IsNullableType(Type type)
        {
            if (!type.IsValueType) return true;

            return Nullable.GetUnderlyingType(type) != null;
        }

        /// <summary>
        /// <para>
        /// Takes your Class Definition and Queries WMI using the Property names via reflection.
        /// Only processes public properites on class definition.
        /// </para>
        /// 
        /// <para>
        /// Class Definitions that have nullable property types are treated as fault tolerant.
        /// Anytime the process encounters issue casting/converting boxed values from WMI
        /// it will return back null only if the property was defined as nullable
        /// </para>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public T[] CreateAndPopulate()
        {
            var cim = _querier.QueryWMI($"SELECT * FROM {this.WMIClassName}");

            // Edge case: some queries can return a guhgillion results (over 100,000)
            // We to handle this in a good way.
            // Right now how do we figure out the way to break the program from using a missing feature
            //var resCount = cim.Count();

            // Currently limiting process to only be able to map 100 results
            var mappedResults = cim.Take(100).Select(res =>
            {
                // WMI query results
                var cimProps = res.CimInstanceProperties;

                var constructedOjb = this.CreateAndPopulate((propName, propType) =>
                {
                    if (cimProps[propName] == null)
                    {
                        WMIQueryPropertiesNotFound.Add(propName);
                    }

                    var isObjPropNullable = IsNullableType(propType);

                    var cimPropValue = cimProps[propName] == null 
                    ? null 
                    : cimProps[propName].Value;

                    if (cimPropValue == null && !isObjPropNullable)
                    {
                        throw new Exception($"Value from query was null and {propName} is not a nullable type");
                    }

                    if (cimPropValue == null && isObjPropNullable)
                    {
                        WmiNullQueryProperties.Add(propName);

                        return null;
                    }

                    var unboxedVal = DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);

                    return unboxedVal;
                });

                return constructedOjb;
            });

             return mappedResults.ToArray();
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

            foreach (var prop in props)
            {
                // Go through each property and process it
                var value = getValue(prop.Name, prop.PropertyType);
                prop.SetValue(obj, value, null);

                //try
                //{
                //    // Go through each property and process it
                //    var value = getValue(prop.Name, prop.PropertyType);
                //    prop.SetValue(obj, value, null);

                //} catch (Exception ex) 
                //{
                //    // Empty throw preserves call stack details
                //    throw new Exception($"Error: {ex.Message}\n Failed to set {prop.Name} to {prop.PropertyType.FullName}");
                //}
            }

            return obj;
        }

        private T CreateInstance()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
