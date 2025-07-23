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

        public IEnumerable<CimInstance> QueryWMI(string query, string wmiNameSpaceOverride)
        {
            var targetNameSpace = wmiNameSpaceOverride ?? _wmiNamespace;

            return _defaultCimSession.QueryInstances(targetNameSpace, _queryStructure, query);
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
        public T[] CreateAndPopulate(string wmiNameSpaceOverride = null)
        {
            var cim = _querier.QueryWMI($"SELECT * FROM {this.WMIClassName}", wmiNameSpaceOverride);

            var classTypeName = typeof(T).Name;

            var first100results = new List<CimInstance>();

            // Hack way to capture any errors ocurring in the CIM Class querying
            try
            {
                first100results = cim.Take(100).ToList();
            }
            catch (Exception ex) 
            {
                throw new Exception($"Error: An error occurred querying {this.WMIClassName}. {ex.Message}");
            }

            // Edge case: some queries can return a guhgillion results (over 100,000)
            // We to handle this in a good way.
            // Right now how do we figure out the way to break the program from using a missing feature
            //var resCount = cim.Count();

            // Currently limiting process to only be able to map 100 results
            var mappedResults = first100results.Select(res =>
            {
                // WMI query results
                var cimProps = res.CimInstanceProperties;
                /**
                    * Will perform a series of checks to ensure that the class definition that was passed in 
                    * can be leveraged for fault tolerance during a process.
                    * **/
                var constructedOjb = this.CreateAndPopulate((propName, propType) =>
                {
                    // Check to see class definition properties names correspond to cimProp keys
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
                        throw new Exception($"Error: Value from query was null; {classTypeName} {propName} must be nullabe to accept faults");
                    }

                    if (cimPropValue == null && isObjPropNullable)
                    {
                        WmiNullQueryProperties.Add(propName);

                        return null;
                    }

                    var cimPropValueType = cimPropValue.GetType();

                    if (!DeviceInfoTypeCaster.AreSameOrNullableEquivalent(cimPropValueType, propType))
                    {
                        throw new Exception($"Error: Type mismatch between CIM Type and property type. {classTypeName}.{propName} expected {propType.Name} but got {cimPropValueType.Name}");
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
                
            }

            return obj;
        }

        private T CreateInstance()
        {
            return Activator.CreateInstance<T>();
        }
    }
}
