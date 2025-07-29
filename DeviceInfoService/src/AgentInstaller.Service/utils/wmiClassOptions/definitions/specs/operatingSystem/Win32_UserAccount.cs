using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public class Win32_UserAccount
    {
        public UInt32? AccountType { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public string FullName { get; set; }
        public string Status { get; set; }
    }
}
