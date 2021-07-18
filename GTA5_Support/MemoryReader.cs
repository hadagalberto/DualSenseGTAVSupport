using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GTA5_Support
{
    public class MemoryReader
    {
        const int PROCESS_WM_READ = 0x0010;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
          long lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        public static bool GameOpened()
        {
            return Process.GetProcessesByName("GTA5").Any();
        }

        public static double GetFireRate()
        {
            Process process = Process.GetProcessesByName("GTA5")[0];
            IntPtr processHandle = OpenProcess(PROCESS_WM_READ, false, process.Id);
            IntPtr startOffset = process.MainModule.BaseAddress;
            var address = startOffset.ToInt64() + 0x1F91D68;


            int bytesRead = 0;
            byte[] buffer = new byte[8]; //'Hello World!' takes 12*2 bytes because of Unicode 

            // 0x0046A3B8 is the address where I found the string, replace it with what you found
            ReadProcessMemory((int)processHandle, address, buffer, buffer.Length, ref bytesRead);

            return BitConverter.ToDouble(buffer, 0);
        }


    }
}
