using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage
{
    public class Win32_DiskDrive
    {
        public string SerialNumber { get; set; }
        public string Manufacturer { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Status { get; set; }
        public ulong? Size { get; set; }
        public UInt16? Availability { get; set; }
        public string Caption { get; set; }
        public string MediaType { get; set; }
    };
}
