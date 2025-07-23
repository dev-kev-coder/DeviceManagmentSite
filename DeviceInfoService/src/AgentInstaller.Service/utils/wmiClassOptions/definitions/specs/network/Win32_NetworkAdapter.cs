
namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.network
{
    public class Win32_NetworkAdapter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string MACAddress { get; set; }
        public string Status { get; set; } 
        public string AdapterType {  get; set; }
        public string PermanentAddress { get; set; }
        public string[] NetworkAddresses { get; set; }
    }
}
