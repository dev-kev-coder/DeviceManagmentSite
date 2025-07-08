using Microsoft.Management.Infrastructure;

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

        /**
         * Method below will be the testing point to see if WMI would be suitable tool to query information
         * on IT related devices. Paticularly windows laptops or desktops
         * 
         * Will need to check to ensure that the service is running on the targetted machine.
         * 
         * Need to add check for build time in order to ensure this can be bundled in Windows
         */
        public static void GetDeviceInfoWMI()
        {
            var wmiNamespace = @"root\cimv2";
            var diskDriveQuery = "SELECT * FROM Win32_BIOS";
            var wmiQuerier = new CimQuerier();
            var results = wmiQuerier
                .QueryWMI(diskDriveQuery)
                .Select(res =>
                {
                    var win32BiosInfo = new Win32_BIOS();
                    var cimProps = res.CimInstanceProperties;

                    win32BiosInfo.Manufacturer = TypeCaster
                    .Cast(cimProps[nameof(win32BiosInfo.Manufacturer)].Value, win32BiosInfo.Manufacturer);
                    win32BiosInfo.Status = TypeCaster
                    .Cast<string>(cimProps[nameof(win32BiosInfo.Status)].Value);
                    win32BiosInfo.SerialNumber = TypeCaster
                    .Cast<string>(cimProps[nameof(win32BiosInfo.SerialNumber)].Value);
                    win32BiosInfo.BiosCharacteristics = TypeCaster
                    .Cast<ushort[]>(cimProps[nameof(win32BiosInfo.BiosCharacteristics)].Value);
                    win32BiosInfo.Version = TypeCaster
                    .Cast<string>(cimProps[nameof(win32BiosInfo.Version)].Value);
                    win32BiosInfo.BIOSVersion = TypeCaster
                    .Cast<string[]>(cimProps[nameof(win32BiosInfo.BIOSVersion)].Value);

                    return win32BiosInfo;
                }).ToArray();

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

    public static class TypeCaster
    {
        /// <summary>
        /// Tries to cast <paramref name="value"/> to <typeparamref name="T"/>.
        /// — Returns the cast value on success, or <c>null</c> on failure.
        /// Works for value types (primitives, structs) and reference types.
        /// </summary>
        public static T? CastOrNull<T>(object? value)  // no constraint needed
        {
            return value is T t ? t : default;   // default == null for both T? cases
        }

        /// <summary>
        /// Un-safe cast operation.
        /// Will cause an Exception to be thrown when a cast is unsuccessful.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Cast<T>(object? value) => (T)value;

        /// <summary>
        /// Un-safe cast operation.
        /// Will cause an Exception to be thrown when a cast is unsuccessful.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Cast<T>(object? value, T refValue) => (T)value;
    }
}
