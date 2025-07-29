using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public  class Win32_Group
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public DateTime? InstallDate { get; set; }
        public string Status { get; set; }
        public bool? LocalAccount { get; set; }
        public string SID { get; set; }
        public string Domain { get; set; }
        public string Name { get; set; }
    }
}
