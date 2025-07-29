namespace AgentInstaller.Service.utils.wmiClassOptions.definitions.specs.operatingSystem
{
    public  class Win32_Account
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string Domain { get; set; }
        public DateTime? InstallDate { get; set; }
        public bool? LocalAccount { get; set; }
        public string Name { get; set; }
        public string SID { get; set; }
        //public  UInt8 SIDType { get; set; }
        public string Status { get; set; }
    }
}
