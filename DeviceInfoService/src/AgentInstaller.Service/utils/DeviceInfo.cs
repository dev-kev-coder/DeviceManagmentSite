using AgentInstaller.Service.utils.wmiClassOptions;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.general;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.network;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage;

namespace AgentInstaller.Service.utils
{
    internal class DeviceInfo
    {
        public static void GetDeviceInfoWMI(ILogger<Worker> log)
        {
            // Tool set at the bottom is cool but it's kinda manual having to build the class each time.
            // TODO might be a good idea to figure out how to implement the builder pattern with this.
            var optionQuerier = new WMIOptionQuerier();

            try
            {

                GetGeneralDeviceInfo(optionQuerier);

                GetNetworkInfo(optionQuerier);

                GetOSInfo(optionQuerier);

                GetPeripheralInfo(optionQuerier);

                GetProcessAndStorageInfo(optionQuerier);
            }
            catch (Exception ex) 
            {
                // this will log ex.Message *and* ex.StackTrace
                log.LogError(ex, "Error in {Method}", nameof(GetDeviceInfoWMI));
                // if you really want a Trace‑level log:
                log.LogTrace(ex, "Trace for exception in {Method}", nameof(GetDeviceInfoWMI));
                // rethrow
            }
        }

        private static void GetGeneralDeviceInfo(WMIOptionQuerier querier)
        {
            var bios = querier
                .CreateQueryOption<Win32_BIOS>("Win32_BIOS")
                .CreateAndPopulate();

            var compSystemProduct = querier
                .CreateQueryOption<Win32_ComputerSystemProduct>("Win32_ComputerSystemProduct")
                .CreateAndPopulate();

            var computerSystem = querier
                .CreateQueryOption<Win32_ComputerSystem>("Win32_ComputerSystem")
                .CreateAndPopulate();
        }

        private static void GetNetworkInfo(WMIOptionQuerier querier) 
        {
            var adatper = querier
                .CreateQueryOption<Win32_NetworkAdapter>("Win32_ComputerSystem")
                .CreateAndPopulate();

            var adpaterConfig = querier
                .CreateQueryOption<Win32_NetworkAdapterConfiguration>("Win32_ComputerSystem")
                .CreateAndPopulate();
        }

        private static void GetOSInfo(WMIOptionQuerier querier)
        {
            var os = querier
                .CreateQueryOption<Win32_OperatingSystem>("Win32_OperatingSystem")
                .CreateAndPopulate();

            var quickFix= querier
                .CreateQueryOption<Win32_QuickFixEngineering>("Win32_QuickFixEngineering")
                .CreateAndPopulate();
        }

        private static void GetProcessAndStorageInfo(WMIOptionQuerier querier)
        {
            var drives = querier
                .CreateQueryOption<Win32_DiskDrive>("Win32_DiskDrive")
                .CreateAndPopulate();

            var ram = querier
                .CreateQueryOption<Win32_PhysicalMemory>("Win32_PhysicalMemory")
                .CreateAndPopulate();

            var processors = querier
                .CreateQueryOption<Win32_Processor>("Win32_Processor")
                .CreateAndPopulate();
        }

        private static void GetPeripheralInfo(WMIOptionQuerier querier)
        {
            var monitors = querier
                .CreateQueryOption<Win32_DesktopMonitor>("Win32_DesktopMonitor")
                .CreateAndPopulate();

            var tpm = querier
                .CreateQueryOption<Win32_Tpm>("Win32_Tpm")
                .CreateAndPopulate();

            var gpu = querier
                .CreateQueryOption<Win32_VideoController>("Win32_VideoController")
                .CreateAndPopulate();
        }

        #region Crazy Ideas to be lazier
        #region T4 templates to create classes
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
        #endregion T4 templates to create classes
        #region Source generators to create classes
        // EVEN CRAZIER IDEA!!!: Use source generators
        /*
            using System;
            using System.Linq;
            using System.Text;
            using Microsoft.CodeAnalysis;
            using Microsoft.CodeAnalysis.Text;
            using Microsoft.Management.Infrastructure;

            [Generator]
            public class CimClassGenerator : ISourceGenerator
            {
                public void Initialize(GeneratorInitializationContext context)
                {
                    // no-op; we’re not using syntax notifications here
                }

                public void Execute(GeneratorExecutionContext context)
                {
                    // 1️⃣ Open a CIM session (localhost)
                    using var session = CimSession.Create(null);

                    // 2️⃣ Query the class definition
                    //    You could make this configurable via AdditionalFiles or an attribute
                    var cimClass = session.GetClass(namespaceName: null, className: "Win32_BIOS");

                    // 3️⃣ Build a map of property → C# type
                    var props = cimClass.CimClassProperties
                        .Select(p => (Name: p.Name, CsType: MapCimType(p.CimType)))
                        .ToArray();

                    // 4️⃣ Emit source
                    var sb = new StringBuilder();
                    sb.AppendLine("// <auto-generated/>");
                    sb.AppendLine("namespace Generated;");
                    sb.AppendLine("public class Win32_BIOS");
                    sb.AppendLine("{");
                    foreach (var (Name, CsType) in props)
                    {
                        sb.AppendLine($"    public {CsType} {Name} {{ get; set; }}");
                    }
                    sb.AppendLine("}");

                    // 5️⃣ Add it into the compilation
                    context.AddSource("Win32_BIOS.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
                }

                private static string MapCimType(CimType cim)
                    => cim switch
                    {
                        CimType.Boolean      => "bool",
                        CimType.String       => "string",
                        CimType.SInt32       => "int",
                        CimType.SInt16       => "short",
                        CimType.SInt64       => "long",
                        CimType.Real32       => "float",
                        CimType.Real64       => "double",
                        CimType.DateTime     => "DateTime",
                        CimType.UInt32       => "uint",
                        CimType.UInt16       => "ushort",
                        CimType.UInt64       => "ulong",
                        CimType.StringArray  => "string[]",
                        CimType.SInt32Array  => "int[]",
                        CimType.Real64Array  => "double[]",
                        // … add more as needed …
                        _                    => "object",
                    };
            }

         */
        #endregion Source generators to create classes
        #endregion Crazy Ideas to be lazier
    }
}
