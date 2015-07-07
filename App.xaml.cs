using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace VariationGeneration
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        protected override void OnStartup(StartupEventArgs e)
        {
            Process process = Process.GetCurrentProcess();

            foreach (Process p in Process.GetProcessesByName(process.ProcessName))
            {
                if (p.Id != process.Id)
                {
                    Application.Current.Shutdown();
                    SetForegroundWindow(p.MainWindowHandle);
                    return;
                }
            }

            base.OnStartup(e);

        }

    }
}
