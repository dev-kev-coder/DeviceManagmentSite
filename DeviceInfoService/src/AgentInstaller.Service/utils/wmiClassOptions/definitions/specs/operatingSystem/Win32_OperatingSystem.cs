using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public class Win32_OperatingSystem
    {
        public string Caption { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public string BuildType { get; set; }
        public DateTime InstallDate { get; set; }
        public string OSArchitecture { get; set; }
        public string SerialNumber { get; set; }
        public UInt32 NumberOfUsers { get; set; }
    }
}
