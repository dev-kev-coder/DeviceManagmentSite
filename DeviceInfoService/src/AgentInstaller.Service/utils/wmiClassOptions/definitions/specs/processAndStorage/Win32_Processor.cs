using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.ProcessAndStorage
{
    public class Win32_Processor
    {
        public string Name { get; set; }
        public uint NumberOfCores { get; set; }
        public uint NumberOfLogicalProcessors { get; set; }
        public uint MaxClockSpeed { get; set; }
        public ushort AddressWidth { get; set; }
    };
}
