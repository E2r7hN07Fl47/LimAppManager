﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace LimAppManager
{
    public partial class MainForm : Form
    {
        Parameters.DebugInfo SysInfo;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {   
            Parameters.OSVersion = (Parameters.OSVersions)Environment.OSVersion.Version.Major;

            SystemHelper.GetDebugInfo(out SysInfo);

            try
            {
                IOHelper.ReadSettings();
            }
            catch
            {
                MessageBox.Show("Config file not found or corrupted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                Parameters.IconSize = 100;
                Parameters.TempPath = @"\Temp\AppManager";
                Parameters.ServersPath = IOHelper.GetCurrentDirectory() + @"\Servers.list";
            }

            try
            {
                Parameters.ServersList = GetServersList(Parameters.ServersPath);
            }
            catch
            {
                MessageBox.Show("No servers found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                Parameters.ServersList = new Dictionary<string, Uri>();
            }

            ImageLogoList_SetSize();

            if (!String.IsNullOrEmpty(Parameters.Server))
            {
                Uri ServerUri = Parameters.ServersList[Parameters.Server];

                GetAppsList(ServerUri, out Parameters.AppsList);
            }
            else
            {
                MessageBox.Show("Server not set", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void GetAppsList(Uri ServerUri, out Dictionary<string, Uri> AppsList)
        {
            Mutex ListingMutex = new Mutex();
            ThreadStart ListingStarter = delegate { ListingWorker(ServerUri, ListingMutex); };
            Thread ListingThread = new Thread(ListingStarter);

            AppsList = new Dictionary<string, Uri>();
            Parameters.EndResponseEvent = new AutoResetEvent(false);

            
            ListingThread.Start();
        }

        private void ListingWorker(Uri ServerUri, Mutex ListingMutex)
        {
            NetHelper Net = new NetHelper();
            Cursor.Current = Cursors.WaitCursor;

            switch (Parameters.OSVersion)
            {
                case Parameters.OSVersions.WM2003:
                    Net.GetAvailableApps(ServerUri, "WinMobile_2003");
                    break;
                case Parameters.OSVersions.WM5:
                    Net.GetAvailableApps(ServerUri, "WinMobile_5");
                    break;
                case Parameters.OSVersions.WM6:
                    Net.GetAvailableApps(ServerUri, "WinMobile_6");
                    break;
                case Parameters.OSVersions.CE4:
                    Net.GetAvailableApps(ServerUri, "WinCE_4");
                    break;
                case Parameters.OSVersions.CE5:
                    Net.GetAvailableApps(ServerUri, "WinCE_5");
                    break;
                case Parameters.OSVersions.CE6:
                    Net.GetAvailableApps(ServerUri, "WinCE_6");
                    break;
                default:
                    Net.GetAvailableApps(ServerUri, "WinMobile_2003");
                    break;
            }

            Parameters.EndResponseEvent.WaitOne();

            if (!String.IsNullOrEmpty(Parameters.ResponseMessage))
            {

                string[] Lines = Parameters.ResponseMessage.Split('\n');

                foreach (string line in Lines)
                {
                    string[] AppLine = line.Split('=');

                    Parameters.AppsList.Add(AppLine[0], new Uri(AppLine[1].Split('\r')[0]));
                    AppsListBox.Invoke((Action)(() => { AppsListBox.Items.Add(new ListViewItem(AppLine[0])); }));
                }
                
                Cursor.Current = Cursors.Default;
            }
            else
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Couldn't connect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void ImageLogoList_SetSize()
        {
            int NewSize = (int)(Parameters.IconSize * System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width / 1000);

            AppsLogoList.ImageSize = new Size(NewSize, NewSize);
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox About = new AboutBox();

            About.Show();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Up))
            {
                // Rocker Up
                // Up
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Down))
            {
                // Rocker Down
                // Down
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Left))
            {
                // Left
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Right))
            {
                // Right
            }
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                // Enter
            }
        }

        private void DonateMenuItem_Click(object sender, EventArgs e)
        {
            DonateBox Donate = new DonateBox();

            Donate.Show();
        }

        private void QuitMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            ParamsForm Params = new ParamsForm();

            Params.ShowDialog();

            ImageLogoList_SetSize();

            try
            {
                IOHelper.WriteSettings();
            }
            catch
            {
                MessageBox.Show("Config file not found or corrupted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
            }
        }

        private void UpdateMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox NewAboutBox = new AboutBox();
            string CurrentVersion = NewAboutBox.AssemblyVersion;
            string Version;

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                Version = NetHelper.CheckUpdates();
                Cursor.Current = Cursors.Default;
            }
            catch
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Failed to check for updates", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                return;
            }

            if (CurrentVersion != Version)
            {
                DialogResult Result = MessageBox.Show("Version: " + Version, "Update?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                if (Result == DialogResult.Yes)
                {
                    Version = Version.Remove(Version.LastIndexOf('.'), 2);

                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        NetHelper.GetUpdates(Version);
                        Cursor.Current = Cursors.Default;
                    }
                    catch
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("Failed to download update", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
                    }

                    //SystemHelper.CabInstall(ParamsHelper.DownloadPath + "\\Update.bat", ParamsHelper.InstallPath + "\\LimFTPClient", true);
                }
            }
            else
            {
                MessageBox.Show("Nothing to update", "Info");
            }
        }

        private void MainForm_Closing(object sender, CancelEventArgs e)
        {
        }

        private Dictionary<string, Uri> GetServersList(string FileName)
        {   
            string Text = IOHelper.ReadTextFile(IOHelper.GetCurrentDirectory() + FileName);
            string[] Lines = Text.Split('\n');
            Dictionary<string, Uri> ServersList = new Dictionary<string, Uri>();

            foreach (string line in Lines)
            {
                string[] ServerLine = line.Split('=');

                ServersList.Add(ServerLine[0], new Uri(ServerLine[1].Split('\r')[0]));
            }

            return ServersList;
        }

        private void InstalledMenuItem_Click(object sender, EventArgs e)
        {
            InstalledForm Installed = new InstalledForm();

            Installed.Show();
        }

        private void AppsListBox_ItemActivate(object sender, EventArgs e)
        {
            AppForm App = new AppForm(AppsListBox.FocusedItem.Text);

            Cursor.Current = Cursors.WaitCursor;

            App.ShowDialog();
        }

        private void SearchBox_GotFocus(object sender, EventArgs e)
        {
            if (SearchBox.Text == "Search...") SearchBox.Text = "";

            InputPanel.Enabled = true;
        }

        private void SearchBox_LostFocus(object sender, EventArgs e)
        {
            if (SearchBox.Text == "") SearchBox.Text = "Search...";

            InputPanel.Enabled = false;
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            if (SearchBox.Text != "Search...")
            {   /*
                List<string> SearchedList = Parameters.AppsList.Keys.Cast().ToList<string>().Where(x => x.ToLower().Contains(SearchBox.Text.ToLower())).ToList();
                AppsListBox.DataSource = null;
                AppsListBox.Items.Clear();

                foreach (string app in SearchedList)
                {
                    AppsListBox.Items.Add(app);
                }
                */
            }
        }

        private void RefreshMenuItem_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(Parameters.Server))
            {
                Uri ServerUri = Parameters.ServersList[Parameters.Server];

                GetAppsList(ServerUri, out Parameters.AppsList);
            }
            else
            {
                MessageBox.Show("Server not set", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
            }
        }

        private void SendBugMenuItem_Click(object sender, EventArgs e)
        {
            SendBugForm SendBug = new SendBugForm(SysInfo);

            SendBug.Show();
        }
    }
}