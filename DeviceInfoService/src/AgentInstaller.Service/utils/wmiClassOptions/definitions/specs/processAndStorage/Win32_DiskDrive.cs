using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage
{
    public class Win32_DiskDrive
    {
        public string SerialNumber;
        public string Manufacturer;
        public string Name;
        public string Model;
        public string Status;
        public ulong Size;
        public ushort Availability;
    };
}
