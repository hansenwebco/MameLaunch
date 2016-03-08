using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MameLaunch
{
    public partial class Main : Form
    {
        private bool _skipOnce = false;
        public Main()
        {
            InitializeComponent();
            string curDir = Directory.GetCurrentDirectory();
            wb.Url = new Uri(String.Format("file:///{0}/resources/template-main.html", curDir));
            
            wb.PreviewKeyDown += new PreviewKeyDownEventHandler(wb_PreviewKeyDown);
        }

        private void wb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // web browser control has a bug where this is called twice
            if (_skipOnce)
            {
                _skipOnce = false;
                return;
            }

            // exit on ESC key 
            // TODO: CASE This
            if (e.KeyValue == 27)
                Application.Exit();
            else if (e.KeyValue== 38)
            {
                wb.Document.GetElementById("btn-up").InvokeMember("click");
                _skipOnce = true;
            }
             else if (e.KeyValue== 40)
            {
                wb.Document.GetElementById("btn-down").InvokeMember("click");
                 _skipOnce = true;
            }
            else if (e.KeyValue == 13)
            {
                String romName = wb.Document.GetElementById("rom").GetAttribute("value");
                LaunchMame(romName);
                _skipOnce = true;
            }
        }
         public void LaunchMame(string romName)
        {
            string mamePath = System.Configuration.ConfigurationManager.AppSettings["MamePath"].ToString();
            ProcessStartInfo Mame = new ProcessStartInfo(mamePath + "mame.exe");
            Mame.WorkingDirectory = "c:\\temp\\mame";
            Mame.WindowStyle = ProcessWindowStyle.Hidden;
            Mame.Arguments = romName;
            Process.Start(Mame);
        }
    }
}
