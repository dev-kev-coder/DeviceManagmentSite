using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage
{
    public class Win32_PhysicalMemory
    {
        public ulong Capacity { get; set; }
        public uint Speed { get; set; }
        public string Manufacturer { get; set; }
        public string PartNumber { get; set; }
    };
}
