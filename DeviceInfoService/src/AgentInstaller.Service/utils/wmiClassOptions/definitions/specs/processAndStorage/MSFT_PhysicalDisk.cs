using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.processAndStorage
{
    public class MSFT_PhysicalDisk
    {
        public UInt16 BusType { get; set; }
        public UInt16 HealthStatus { get; set; }
        public UInt16 MediaType { get; set; }
    }
}
