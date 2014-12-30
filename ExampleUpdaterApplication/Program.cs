using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ExampleUpdaterApplication
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                //As mentioned in Form1, we pass args for a few reasons. The main being so we know where our main app's executable is
                Application.Run(new MainForm(args[0]));
            }
            catch
            {
                //tease the user a bit
                MessageBox.Show("Hey you! Stay out of here!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
