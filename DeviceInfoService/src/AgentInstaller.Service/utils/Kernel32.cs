using System;
using System.Runtime.InteropServices;

namespace AgentInstaller.Service.utils
{
    internal static class Kernel32
    {
        [DllImport("kernel32.dll")]
        internal static extern void GetSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [DllImport("kernel32.dll")]
        internal static extern void GetNativeSystemInfo(out SYSTEM_INFO lpSystemInfo);

        [StructLayout(LayoutKind.Sequential)]
        internal struct SYSTEM_INFO
        {
            public ProcessorArchitecture wProcessorArchitecture;
            public ushort wReserved;
            public uint dwPageSize;
            public IntPtr lpMinimumApplicationAddress;
            public IntPtr lpMaximumApplicationAddress;
            public IntPtr dwActiveProcessorMask;
            public uint dwNumberOfProcessors;
            public uint dwProcessorType;
            public uint dwAllocationGranularity;
            public ushort wProcessorLevel;
            public ushort wProcessorRevision;
        }

        internal enum ProcessorArchitecture : ushort
        {
            x86 = 0,
            Arm = 5,
            IA64 = 6,
            x64 = 9,
            Arm64 = 12,
            Unknown = 0xFFFF
        }
    }

    // --- sample use ---
    //Kernel32.GetNativeSystemInfo(out var info);
    //Console.WriteLine($"{info.dwNumberOfProcessors} logical procs, page size {info.dwPageSize} bytes, arch {info.wProcessorArchitecture}");
}