using DSProject;
using DualSenseSupport;
using System;
using System.Threading;
using System.Windows.Input;

namespace GTA5_Support
{
    class Program
    {

        static DsTrigger.Modes weaponMode = DsTrigger.Modes.Pulse_AB;
        static DsTrigger.Modes offMode = DsTrigger.Modes.Off;

        static bool active = true;
        static bool isRunning = true;

        static void Main(string[] args)
        {
            Thread thread = new Thread(KeyboardThread);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            Devices.Init();
            Console.Clear();
            if (Devices.GetDeviceCount() == 0)
            {
                Console.WriteLine("No dualsense controller connected.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            if (!MemoryReader.GameOpened())
            {
                Console.WriteLine("The game is not opened.");
                Console.ReadKey();
                Environment.Exit(1);
            }

            Console.WriteLine("Program started, try some weapons");

            var selectedDevice = 0;

            while (true)
            {
                if (active)
                {
                    var fireRate = MemoryReader.GetFireRate();

                    double forceDouble = (fireRate * 16) / 72;
                    int force = (int)Math.Round(fireRate < 50 ? forceDouble / 3 : forceDouble, 0);
                    Devices.GetDevice(selectedDevice).SetTriggerRight(new DsTrigger
                    {
                        Force1 = 90,
                        Force2 = 10,
                        Force3 = 0,
                        Force4 = 0,
                        Force5 = 0,
                        Force6 = 255,
                        Force7 = force,
                        Mode = weaponMode
                    });
                }
                else
                {
                    Devices.GetDevice(selectedDevice).SetTriggerRight(new DsTrigger
                    {
                        Force1 = 0,
                        Force2 = 0,
                        Force3 = 0,
                        Force4 = 0,
                        Force5 = 0,
                        Force6 = 0,
                        Force7 = 0,
                        Mode = offMode
                    });
                }
                Thread.Sleep(100);
            }

        }

        static void KeyboardThread()
        {
            while (isRunning)
            {
                Thread.Sleep(100);
                if((Keyboard.GetKeyStates(Key.F7) & KeyStates.Down) > 0)
                {
                    active = !active;
                }
            }
        }
    }
}
