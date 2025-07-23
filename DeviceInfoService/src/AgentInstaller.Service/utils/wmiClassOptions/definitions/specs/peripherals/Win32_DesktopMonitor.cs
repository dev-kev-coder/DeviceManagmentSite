using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.peripherals
{
    public class Win32_DesktopMonitor
    {
        public string Description { get; set; }
        public string DeviceID { get; set; }
        public string MonitorManufacturer { get; set; }
        public string MonitorType { get; set; }
        public uint? ScreenHeight { get; set; }
        public uint? ScreenWidth { get; set; }
        public ushort? StatusInfo { get; set; }
    }
}
