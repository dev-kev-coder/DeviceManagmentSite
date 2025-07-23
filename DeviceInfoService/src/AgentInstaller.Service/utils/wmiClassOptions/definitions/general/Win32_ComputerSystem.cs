
namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.general
{
    public class Win32_ComputerSystem
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public UInt64 TotalPhysicalMemory { get; set; }
        public string Domain { get; set; }
        public string UserName { get; set; }
    }
}
