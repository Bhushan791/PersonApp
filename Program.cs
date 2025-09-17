using System;
using System.Windows.Forms;
using PersonApp.forms; // Make sure this matches your namespace for MainForm

namespace PersonApp
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Initialize application configuration (DPI, fonts, etc.)
            ApplicationConfiguration.Initialize();

            // Run your MainForm as the startup form
            Application.Run(new MainForm());
        }
    }
}
