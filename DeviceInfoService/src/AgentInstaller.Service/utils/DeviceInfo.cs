using Microsoft.Management.Infrastructure;
using System.Reflection;

namespace AgentInstaller.Service.utils
{
    internal class DeviceInfo
    {
        public DeviceInfo() 
        {
        }
        class Win32_BIOS
        {
            public string[] BIOSVersion { get; set; }
            public string Manufacturer { get; set; }
            public string SerialNumber { get; set; }
            public  string Version { get; set; }
            public string Status { get; set; }
            public UInt16[] BiosCharacteristics { get; set; }
        }

        // https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-provider
        class Win32_DiskDrive
        {
            public Win32_DiskDrive() 
            {
            }

            //public UInt16 Availability;
            public string Availability;
            public UInt32 BytesPerSector;
            public UInt16[] Capabilities;
            public string[] CapabilityDescriptions;
            public string Caption;
            public string CompressionMethod;
            public UInt32 ConfigManagerErrorCode;
            public bool ConfigManagerUserConfig;
            public string CreationClassName;
            public UInt64 DefaultBlockSize;
            public string Description;
            public string DeviceID;
            public bool ErrorCleared;
            public string ErrorDescription;
            public string ErrorMethodology;
            public string FirmwareRevision;
            public UInt32 Index;
            public DateTime InstallDate;
            public string InterfaceType;
            public UInt32 LastErrorCode;
            public string Manufacturer;
            public UInt64 MaxBlockSize;
            public UInt64 MaxMediaSize;
            public bool MediaLoaded;
            public string MediaType;
            public UInt64 MinBlockSize;
            public string Model;
            public string Name;
            public bool NeedsCleaning;
            public UInt32 NumberOfMediaSupported;
            public UInt32 Partitions;
            public string PNPDeviceID;
            public UInt16[] PowerManagementCapabilities;
            public bool PowerManagementSupported;
            public UInt32 SCSIBus;
            public UInt16 SCSILogicalUnit;
            public UInt16 SCSIPort;
            public UInt16 SCSITargetId;
            public UInt32 SectorsPerTrack;
            public string SerialNumber;
            public UInt32 Signature;
            public UInt64 Size;
            public string Status;
            public UInt16 StatusInfo;
            public string SystemCreationClassName;
            public string SystemName;
            public UInt64 TotalCylinders;
            public UInt32 TotalHeads;
            public UInt64 TotalSectors;
            public UInt64 TotalTracks;
            public UInt32 TracksPerCylinder;
        };

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

            //var option1 = new WMIQueryOption<Win32_BIOS>("Win32_BIOS");
            //var option2 = new WMIQueryOption<Win32_DiskDrive>("Win32_DiskDrive");

            var option1 = new WMIQueryOptionV2<Win32_BIOS>("Win32_BIOS")
                .CreateAndPopulate();

            var option2 = new WMIQueryOptionV2<Win32_DiskDrive>("Win32_DiskDrive")
                .CreateAndPopulate();


            //var wmiNamespace = @"root\cimv2";
            //var diskDriveQuery = "SELECT * FROM Win32_BIOS";
            //var wmiQuerier = new CimQuerier();
            //var results = wmiQuerier
            //    .QueryWMI(diskDriveQuery)
            //    .Select(res =>
            //    {
            //        var cimProps = res.CimInstanceProperties;

            //        //var constructionator = new MagicAutoConstructinator<Win32_BIOSV2>();

            //        //var test = constructionator.CreateAndPopulate((propName, propType) =>
            //        //{
            //        //    return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
            //        //});

            //        //return test;

            //        var win32BiosInfo = CreateAndPopulateV2<Win32_BIOS>((propName, propType) =>
            //        {
            //            //return TypeCaster.Cast(cimProps[propName].Value, propType);
            //            return DeviceInfoTypeCaster.UnboxToType(cimProps[propName].Value);
            //        });

            //        return win32BiosInfo;
            //    }).ToArray();

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
}
