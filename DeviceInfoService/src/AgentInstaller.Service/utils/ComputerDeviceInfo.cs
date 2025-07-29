using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals;
using AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage;

namespace AgentInstaller.Service.utils.ComputerDeviceInfoPayload
{
    

    public class ComputerDeviceInfo
    {

        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Brand { get; set; }
        public string Workgroup { get; set; }
        public string Domain { get; set; }
        public string ComputerName { get; set; }
        
        public Win32_OperatingSystem[] OS { get; set; }
        public Win32_Processor[] Processor { get; set; }
        public Win32_PhysicalMemory[] RAM { get; set; }
        public Win32_DiskDrive[] DiskDrives { get; set; }
        public Win32_VideoController[] VideoControllers { get; set; }
    }
}
