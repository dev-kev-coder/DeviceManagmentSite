using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals
{
    public class Win32_Tpm
    {
        public bool IsEnabled_InitialValue { get; set; }
        public bool Enabled { get; set; }
    }
}
