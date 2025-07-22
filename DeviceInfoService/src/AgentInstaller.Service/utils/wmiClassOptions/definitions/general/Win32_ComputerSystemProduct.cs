using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.general
{
    public class Win32_ComputerSystemProduct
    {
        public string UUID { get; set; }
        public string IdentifyingNumber { get; set; }
        public string Name { get; set; }
    }
}
