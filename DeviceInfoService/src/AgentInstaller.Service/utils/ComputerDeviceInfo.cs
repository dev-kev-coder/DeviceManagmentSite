using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.ComputerDeviceInfoPayload
{
    public class ProcessorInfo
    {

    }

    public class RAMInfo
    {

    }

    public class StorageInfo 
    {

    }

    public class GraphicsInfo
    {
        
    }

    public class OperatingSystemInfo
    {
        public DateTime InstallDate { get; set; }
        public string LicenseType { get; set; }
        public string BuildNumber { get; set; }
        public string Architecture { get; set; }
    }

    public class ComputerDeviceInfo
    {
        public ProcessorInfo Processor { get; set; }
        public RAMInfo[] RAM { get; set; }
        public StorageInfo[] Storage { get; set; }
        public GraphicsInfo Graphics { get; set; }
        public OperatingSystemInfo OS { get; set; }
    }
}
