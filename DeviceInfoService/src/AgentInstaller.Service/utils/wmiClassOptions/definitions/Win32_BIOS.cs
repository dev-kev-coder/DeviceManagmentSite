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

        /**
        * Constraints: SerialNumber must be SN from Vendor.
        * Edge cases: SerialNumber
        *   1.  value = "System Serial Number"; BIOS might not have SN from Vendor.
        * **/
        public string SerialNumber { get; set; }
        public string IdentificationCode { get; set; }
        public string Name { get; set; }
        public string SoftwareElementID { get; set; }

        public string Version { get; set; }

        public string Status { get; set; }

        public UInt16[] BiosCharacteristics { get; set; }
    }
}
