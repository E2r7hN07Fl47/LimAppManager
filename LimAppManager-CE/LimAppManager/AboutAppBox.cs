﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace LimAppManager
{
    public partial class AboutAppBox : Form
    {
        private string AppName;

        public AboutAppBox(string CurrentAppName)
        {
            InitializeComponent();
            AppName = CurrentAppName;

            this.Text = String.Format("About {0}", AppProduct);
            this.labelProductName.Text = AppProduct;
            this.labelVersion.Text = String.Format("Version {0}", AppVersion);
            this.labelCompanyName.Text = String.Format("Author: {0}", AppCompany);
            this.labelInstallDate.Text = String.Format("Install date: {0}", AppInstallDate);
            this.textBoxInstallPath.Text = String.Format("Install path: {0}", AppInstallPath);
        }

        public string AppTitle
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
        }

        public string AppVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        public string AppBuildDate
        {
            get
            {
                string filePath = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;
                byte[] b = new byte[2048];
                System.IO.Stream s = null;

                try
                {
                    s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    s.Read(b, 0, 2048);
                }
                finally
                {
                    if (s != null)
                    {
                        s.Close();
                    }
                }

                int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
                int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds(secondsSince1970);
                dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
                return dt.ToString("dd.MM.yy");
            }
        }

        public string AppInstallPath
        {
            get
            {
                return SystemHelper.GetInstallDir(AppName);
            }
        }

        public string AppProduct
        {
            get
            {
                string AppProductName = SystemHelper.GetInstallDir(AppName).Split('\\')[SystemHelper.GetInstallDir(AppName).Split('\\').Length - 1];

                if (String.IsNullOrEmpty(AppProductName)) return "\\";
                else return AppProductName;
            }
        }

        public string AppCopyright
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
            }
        }

        public string AppCompany
        {
            get
            {
                return AppName.Replace(this.labelProductName.Text, String.Empty);
            }
        }

        public string AppInstallDate
        {
            get
            {
                return Directory.GetLastWriteTime(AppInstallPath).ToString("dd.MM.yy");
            }
        }

        private void OkMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}