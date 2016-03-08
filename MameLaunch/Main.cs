﻿using Newtonsoft.Json;
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
        //private List<Game> _games;
        private bool _skipOnce = false;
        public Main()
        {
            InitializeComponent();

            //LoadGames();

            string curDir = Directory.GetCurrentDirectory();
            this.wb.Url = new Uri(String.Format("file:///{0}/resources/template-main.html", curDir));
            
            wb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(wb_DocumentCompleted);
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
        private void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

            HtmlElement ele = wb.Document.GetElementById("launch");
            if (ele != null)
            {
                ele.Click -= htmlDoc_Click;
                ele.Click += new HtmlElementEventHandler(htmlDoc_Click);
            }
        }
        private void htmlDoc_Click(object sender, HtmlElementEventArgs e)
        {
            HtmlElement elm = (HtmlElement)sender;

          
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
        private void LoadGames()
        {
           // string curDir = Directory.GetCurrentDirectory();

            //string json = File.ReadAllText(curDir + "\\resources\\json\\games.json");
           // _games = JsonConvert.DeserializeObject<List<Game>>(json);

        }

    }
}
