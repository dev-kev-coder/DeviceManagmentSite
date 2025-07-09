using AgentInstaller.Service.utils.wmiClassOptions;
using AgentInstaller.Service.utils.wmiClassOptions.definitions;
using Microsoft.Management.Infrastructure;
using System.Reflection;

namespace AgentInstaller.Service.utils
{
    internal class DeviceInfo
    {
        public static T CreateAndPopulateV2<T>(Func< string, Type, object> getValue) where T : class
        {
            // Attempt to create instance of type T
            var obj = Activator.CreateInstance(typeof(T)) as T;

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


        public static void GetDeviceInfoWMI()
        {
            // Tool set at the bottom is cool but it's kinda manual having to build the class each time.
            var wmiOptionQuerier = new WMIOptionQuerier();
            
            var option1 = wmiOptionQuerier
                .CreateQueryOption<Win32_BIOS>("Win32_BIOS")
                .CreateAndPopulate();

            var option2 = wmiOptionQuerier
                .CreateQueryOption<Win32_DiskDrive>("Win32_DiskDrive")
                .CreateAndPopulate();

            var option3 = wmiOptionQuerier
                .CreateQueryOption<Win32_ComputerSystemProduct>("Win32_ComputerSystemProduct")
                .CreateAndPopulate();

            var option4 = wmiOptionQuerier
                .CreateQueryOption<Win32_Directory>("Win32_Directory")
                .CreateAndPopulate();


            // Cool experiment!!! (uses T4 templates)
            // Basically use the query to read the properties for target WMI Classnames
            // We can extract out the property name and type our managed process is expected to get
            // Ideally these are queries that would not actually happen during the run time. Should only be run on build if the files weren't already made (idk how to do this)
           /* <#@ template language="C#" hostspecific="true" debug="false" #>
              <#@ output extension=".cs" #>
              <#@ assembly name="Microsoft.Management.Infrastructure" #>
              <#@ import namespace="Microsoft.Management.Infrastructure" #>
              <#@ import namespace="System.Linq" #>
              <#
                  // 1️⃣ Open a CIM session against the local machine
                  var session = CimSession.Create(null);

                          // 2️⃣ Grab the class definition (not an instance) so we can inspect its properties
                          var cimClass = session.GetClass(namespaceName: null, className: "Win32_BIOS");

                          // 3️⃣ Build a schema: property name → C# type name
                          var schema = cimClass.CimClassProperties
                              .ToDictionary(
                                  prop => prop.Name,
                                  prop => MapCimTypeToCSharp(prop.CimType)
                              );
              #>
              namespace Generated
                  {
                      public class Win32_BIOS
                      {
              <#
                  // 4️⃣ Emit one property per entry in the schema dictionary
                  foreach (var kv in schema)
                  {
              #>
                      public <#= kv.Value #> <#= kv.Key #> { get; set; }
              <#
                  }
              #>
                  }
              }

              <#+   // helper method in the template to map CIM types to C# types
                  string MapCimTypeToCSharp (CimType cimType) => cimType switch
                  {
                      CimType.Boolean => "bool",
                      CimType.SInt32 => "int",
                      CimType.SInt16 => "short",
                      CimType.SInt64 => "long",
                      CimType.Real32 => "float",
                      CimType.Real64 => "double",
                      CimType.String => "string",
                      CimType.DateTime => "DateTime",
                      CimType.UInt32 => "uint",
                      CimType.UInt16 => "ushort",
                      CimType.UInt64 => "ulong",
                      CimType.Reference => "object",     // or a nested class
                      CimType.Char16 => "char[]",
                      CimType.Object => "object",
                      CimType.ReferenceArray => "object[]",
                      CimType.StringArray => "string[]",
                      CimType.SInt32Array => "int[]",
                      CimType.Real64Array => "double[]",
                      // …add more as you need…
                      _ => "object"
                  };
              #>
            */
        }
    }
}
