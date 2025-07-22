using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public class Win32_QuickFixEngineering
    {
        public string HotFixID { get; set; }
        public string Description { get; set; }
        public string InstalledOn { get; set; }
        public string InstalledBy { get; set; }
        public string Status { get; set; }
    }
}
