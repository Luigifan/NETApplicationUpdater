using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ExampleUpdaterApplication
{
    public partial class MainForm : Form
    {
        static string exeLocation = "DIRECT URL TO UPDATED APPLICATION URL HERE";
        string appDataLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + Path.DirectorySeparatorChar + "PLACEHOLDERAPPLICATIONNAME";
        string programLocation;

        /// <summary>
        /// The main constructor of the form
        /// </summary>
        /// <param name="programLocation">We pass it the program location for a few reasons
        /// 1. So the program knows where its at and we don't have to play with current directory voodoo magic
        /// 2. To make sure the user isn't trying to run the updater outside of our main application's scope</param>
        public MainForm(string _programLocation)
        {
            InitializeComponent();
            if(!File.Exists(programLocation)) //and this is just an extra layer of security, you could however many more you wanted to such as comparing MD5 sums and blah blah blah
            {
                MessageBox.Show("Passed an invalid argument!", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-2);
            }
            else
            {
                programLocation = _programLocation;
            }
        }
        //
        private void MainForm_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            Thread t = new Thread(TryKillProcess);
            t.Start();
        }
        //
        private void TryKillProcess()
        {
            statusLabel.Text = "Checking if a process is still running..";
            try
            {
                Process[] proc = Process.GetProcessesByName("PLACEHOLDERAPPLICATIONNAME");
                statusLabel.Text = "Killing processes..";
                foreach (var i in proc)
                {
                    i.Kill();
                }
            }
            catch
            { Console.WriteLine("SpriteBlender not running"); }
            DownloadUpdate();
        }
        private void DownloadUpdate()
        {
            statusLabel.Text = "Download update..";
            try
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(exeLocation, appDataLocation + Path.DirectorySeparatorChar + "PLACEHOLDERAPPLICATIONANME_Latest.exe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occurred while trying to update!\n\nStack: {0}", ex.Message),
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Environment.Exit(-2); //-2 means an error occurred while trying to download
            }
            Replace();
        }
        private void Replace()
        {
            statusLabel.Text = "Replacing original...";
            File.Delete(programLocation);
            try
            {
                File.Move(appDataLocation + Path.DirectorySeparatorChar + "PLACEHOLDERAPPLICATIONNAME_Latest.exe", programLocation);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("An error occurred while trying to update!\n\nStack: {0}", ex.InnerException),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(-3); //-3 means an error occurred while trying to replace
            }
            Complete();
        }
        private void Complete()
        {
            statusLabel.Text = "Updated!";
            Thread.Sleep(1000);
            DialogResult dr = MessageBox.Show("PLACEHOLDEERAPPLICATIONNAME finished updating successfully! Would you like to launch now?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            switch (dr)
            {
                case (DialogResult.Yes):
                    Process.Start(programLocation);
                    break;
                case (DialogResult.No):
                    //do nothing
                    break;
            }
            Environment.Exit(1); //1 means success!
        }
        //
    }
}
