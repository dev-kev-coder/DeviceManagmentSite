using System;
using System.Runtime.InteropServices;
using System.Text;

public class Test
{
    public static void CallMe()
    {
        // --- CPU INFORMATION -------------------------------------------------
        SYSTEM_INFO sysInfo = new SYSTEM_INFO();               // struct to receive CPU data
        GetSystemInfo(out sysInfo);                            // kernel32.dll call
        Console.WriteLine($"CPU Architecture : {sysInfo.wProcessorArchitecture}");
        Console.WriteLine($"Logical Processors: {sysInfo.dwNumberOfProcessors}");
        Console.WriteLine();

        // --- MEMORY (RAM) INFORMATION ---------------------------------------
        MEMORYSTATUSEX memStatus = new MEMORYSTATUSEX();       // struct to receive RAM data
        if (GlobalMemoryStatusEx(memStatus))                   // kernel32.dll call
        {
            Console.WriteLine($"Physical RAM Total : {memStatus.ullTotalPhys / (1024 * 1024)} MB");
            Console.WriteLine($"Physical RAM Free  : {memStatus.ullAvailPhys / (1024 * 1024)} MB");
        }
        else
        {
            Console.WriteLine("Unable to query RAM information.");
        }
        Console.WriteLine();

        // --- VOLUME SERIAL NUMBER (C:) --------------------------------------
        StringBuilder volName = new StringBuilder(261);        // buffers for API call
        StringBuilder fsName = new StringBuilder(261);
        uint serialNumber, maxCompLen, sysFlags;

        bool gotSerial = GetVolumeInformation(
            "C:\\",                                            // root path
            volName, volName.Capacity,
            out serialNumber,                                  // volume serial #
            out maxCompLen,                                    // (unused)
            out sysFlags,                                      // (unused)
            fsName, fsName.Capacity);

        if (gotSerial)
        {
            Console.WriteLine($"Volume Serial (C:): 0x{serialNumber:X}");
        }
        else
        {
            Console.WriteLine("Unable to query volume serial number.");
        }
        Console.WriteLine();

        // --- STORAGE SPACE OF (C:) ------------------------------------------
        ulong freeBytesAvail, totalBytes, totalFree;
        bool gotSpace = GetDiskFreeSpaceEx(
            "C:\\",                                            // root path
            out freeBytesAvail,                                // bytes available to caller
            out totalBytes,                                    // total capacity
            out totalFree);                                    // total free bytes

        if (gotSpace)
        {
            Console.WriteLine($"Storage Total (C:): {totalBytes / (1024 * 1024 * 1024)} GB");
            Console.WriteLine($"Storage Free  (C:): {totalFree / (1024 * 1024 * 1024)} GB");
        }
        else
        {
            Console.WriteLine("Unable to query storage information.");
        }
    }

    // ------------------------ P/INVOKE DEFINITIONS --------------------------

    [DllImport("kernel32.dll")]
    private static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern bool GlobalMemoryStatusEx([In, Out] MEMORYSTATUSEX lpBuffer);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetVolumeInformation(
        string lpRootPathName,
        StringBuilder lpVolumeNameBuffer,
        int nVolumeNameSize,
        out uint lpVolumeSerialNumber,
        out uint lpMaximumComponentLength,
        out uint lpFileSystemFlags,
        StringBuilder lpFileSystemNameBuffer,
        int nFileSystemNameSize);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern bool GetDiskFreeSpaceEx(
        string lpDirectoryName,
        out ulong lpFreeBytesAvailable,
        out ulong lpTotalNumberOfBytes,
        out ulong lpTotalNumberOfFreeBytes);

    // ------------------------ DATA STRUCTURES -------------------------------

    private struct SYSTEM_INFO
    {
        public ushort wProcessorArchitecture;  // CPU arch (e.g., 0 = x86, 9 = x64/AMD64)
        public ushort wReserved;               // unused
        public uint dwPageSize;
        public IntPtr lpMinimumApplicationAddress;
        public IntPtr lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public uint dwNumberOfProcessors;      // logical processor count
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort wProcessorLevel;
        public ushort wProcessorRevision;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private class MEMORYSTATUSEX
    {
        public uint dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;     // total physical memory
        public ulong ullAvailPhys;     // free physical memory
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
}
