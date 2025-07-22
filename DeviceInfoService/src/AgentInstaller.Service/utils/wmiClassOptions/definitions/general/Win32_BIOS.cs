using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.general
{
    class Win32_BIOS
    {
        /**
        * Constraints: SerialNumber must be SN from Vendor.
        * Edge cases: SerialNumber
        *   1.  value = "System Serial Number"; BIOS might not have SN from Vendor.
        * **/
        public string SerialNumber { get; set; }
        public string[] BIOSVersion { get; set; }
    }
}
