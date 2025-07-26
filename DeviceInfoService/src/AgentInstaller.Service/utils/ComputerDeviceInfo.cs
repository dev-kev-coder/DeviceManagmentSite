using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.ComputerDeviceInfoPayload
{
    //public class ProcessorInfo
    //{

    //}

    //public class RAMInfo
    //{

    //}

    //public class DiskDriveInfo 
    //{
    //    public 

    //}

    //public class GraphicsInfo
    //{
        
    //}

    //public class OperatingSystemInfo
    //{
    //    public DateTime InstallDate { get; set; }
    //    public string LicenseType { get; set; }
    //    public string BuildNumber { get; set; }
    //    public string Architecture { get; set; }
    //}

    public class ComputerDeviceInfo
    {


        //public ProcessorInfo Processor { get; set; }
        //public RAMInfo[] RAM { get; set; }
        //public DiskDriveInfo[] Storage { get; set; }
        //public GraphicsInfo Graphics { get; set; }
        public Win32_OperatingSystem[] OS { get; set; }
        public Win32_Processor[] Processor { get; set; }
        public Win32_PhysicalMemory[] RAM { get; set; }
        public Win32_DiskDrive[] DiskDrives { get; set; }
        public Win32_VideoController[] VideoControllers { get; set; }
    }
}
