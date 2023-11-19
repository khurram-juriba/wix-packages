using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Scenarios.Helpers
{
    static class SymbolicLinkHelper
    {
        const int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;
        const int SYMBOLIC_LINK_FLAG_ALLOW_UNPRIVILEGED_CREATE = 0x2; // only works in Creator's Update / 14972 when Dev Mode is enabled or in MSIX

        const uint FILE_READ_EA = 0x0008;
        const uint FILE_FLAG_BACKUP_SEMANTICS = 0x2000000;

        static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        [DllImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int CreateSymbolicLinkW(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);


        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetFinalPathNameByHandle(IntPtr hFile, [MarshalAs(UnmanagedType.LPTStr)] StringBuilder lpszFilePath, uint cchFilePath, uint dwFlags);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CreateFile(
            [MarshalAs(UnmanagedType.LPTStr)] string filename,
            [MarshalAs(UnmanagedType.U4)] uint access,
            [MarshalAs(UnmanagedType.U4)] FileShare share,
            IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
                [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
            [MarshalAs(UnmanagedType.U4)] uint flagsAndAttributes,
            IntPtr templateFile);

        public static bool CreateSymbolicLink(Action<string> log, string symbolicLink, string target, bool isDirectory)
        {
            int flags = 0;
            //if in Creator's Update; we can set the UNPRIVILEGED flag; but its of no use generally due to Dev Mode requirement
            if (isDirectory) flags |= SYMBOLIC_LINK_FLAG_DIRECTORY;
            var result = CreateSymbolicLinkW(symbolicLink, target, flags);
            if (result == 1)
                return true;
            else
            {
                int w32Error = Marshal.GetLastWin32Error(); //this will always be some positive value
                log($"Failed to create {symbolicLink} symbolic link for {target}; Win32 error: {w32Error}");
                return false;
            }
        }

        public static string ResolveSymbolicLink(string path)
        {
            var h = CreateFile(path,
                FILE_READ_EA,
                FileShare.ReadWrite | FileShare.Delete,
                IntPtr.Zero,
                FileMode.Open,
                FILE_FLAG_BACKUP_SEMANTICS,
                IntPtr.Zero);
            if (h == INVALID_HANDLE_VALUE) throw new Win32Exception();

            try
            {
                var sb = new StringBuilder(1024);
                var res = GetFinalPathNameByHandle(h, sb, 1024, 0);
                if (res == 0)
                    throw new Win32Exception();

                var finalPath = sb.ToString();
                if (finalPath.StartsWith(@"\\?\"))
                    return finalPath.Substring(4);
                else
                    return finalPath;
            }
            finally
            {
                CloseHandle(h);
            }
        }
    }
}
