using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions
{
    public class Win32_ComputerSystemProduct
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string IdentifyingNumber { get; set; }
        public string Name { get; set; }
        public string SKUNumber { get; set; }
        public string Vendor { get; set; }
        public string Version { get; set; }
        public string UUID { get; set; }
    }
}
