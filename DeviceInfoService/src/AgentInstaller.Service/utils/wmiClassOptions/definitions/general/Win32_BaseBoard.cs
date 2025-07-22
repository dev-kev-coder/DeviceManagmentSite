using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.general
{
    // Fall back for Win32_BIOS WMI class
    public class Win32_BaseBoard
    {
        public string SerialNumber { get; set; }
        public string Product { get; set; }
    }
}
