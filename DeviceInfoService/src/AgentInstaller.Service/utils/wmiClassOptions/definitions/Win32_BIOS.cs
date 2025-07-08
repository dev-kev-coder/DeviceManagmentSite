using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions
{
    class Win32_BIOS
    {
        public string[] BIOSVersion { get; set; }
        public string Manufacturer { get; set; }
        public string SerialNumber { get; set; }
        public string Version { get; set; }
        public string Status { get; set; }
        public UInt16[] BiosCharacteristics { get; set; }
    }
}
