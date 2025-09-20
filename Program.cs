using System;
using System.Windows.Forms;
using PersonApp.forms; // Make sure this matches your namespace for MainForm

namespace PersonApp
{
    static class Program
    {
       
[STAThread]
static void Main()
{
    try
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
    catch (Exception ex)
    {
        MessageBox.Show(ex.ToString(), "Startup Error");
    }
}

    }
}


