using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals
{
    public class Win32_VideoController
    {
        public string Name { get; set; }
        public string AdapterRAM { get; set; }
        public string DriverVersion { get; set; }
    }
}
