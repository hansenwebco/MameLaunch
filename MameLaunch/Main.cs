using SlimDX.DirectInput;
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
        // SlimDX Tutorial: https://www.youtube.com/watch?v=rtnLGfAj7W0

        private bool _skipOnce = false;
        private bool _stickMoved = false;
        DirectInput input = new DirectInput();
        SlimDX.DirectInput.Joystick stick;
        Joystick[] sticks;
        int yValue = 0;
        int xValue = 0;
        int zValue = 0;

        public Main()
        {

            InitializeComponent();

            Cursor.Hide();

            string curDir = Directory.GetCurrentDirectory();
            string templatePath = System.Configuration.ConfigurationManager.AppSettings["SkinRootPage"].ToString();
            wb.Url = new Uri(String.Format("file:///{0}/resources/" + templatePath, curDir));

            wb.PreviewKeyDown += new PreviewKeyDownEventHandler(wb_PreviewKeyDown);

            GetSticks();
            sticks = GetSticks();
            if (sticks.Count() > 0)
                timerMain.Enabled = true;
            else
                MessageBox.Show("Warning, no joysticks detected.");
        }

        private void wb_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            // web browser control has a bug where this is called twice
            if (_skipOnce)
            {
                _skipOnce = false;
                return;
            }

            switch (e.KeyValue)
            {
                case 27:  // esc
                    Application.Exit();
                    break;
                case 38: // up arrow
                    wb.Document.GetElementById("btn-up").InvokeMember("click");
                    _skipOnce = true;
                    break;
                case 40: // down arrow
                    wb.Document.GetElementById("btn-down").InvokeMember("click");
                    _skipOnce = true;
                    break;
                case 13: // enter key
                    wb.Document.InvokeScript("PlayLaunch");
                    LaunchMame();
                    _skipOnce = true;
                    break;
                default:
                    break;
            }

        }
        public void LaunchMame()
        {
            string romName = wb.Document.GetElementById("rom").GetAttribute("value");
            string mamePath = System.Configuration.ConfigurationManager.AppSettings["MamePath"].ToString();
            ProcessStartInfo Mame = new ProcessStartInfo(mamePath + "mame.exe");
            Mame.WorkingDirectory = mamePath;
            Mame.WindowStyle = ProcessWindowStyle.Hidden;
            Mame.Arguments = romName;
            Process.Start(Mame);
        }

        public Joystick[] GetSticks()
        {
            List<Joystick> sticks = new List<Joystick>();
            foreach (DeviceInstance device in input.GetDevices(DeviceClass.GameController, DeviceEnumerationFlags.AttachedOnly))
            {
                try
                {
                    stick = new SlimDX.DirectInput.Joystick(input, device.InstanceGuid);
                    stick.Acquire();

                    foreach (DeviceObjectInstance deviceObject in stick.GetObjects())
                    {
                        if ((deviceObject.ObjectType & ObjectDeviceType.Axis) != 0)
                        {
                            stick.GetObjectPropertiesById((int)deviceObject.ObjectType).SetRange(-100, 100);
                        }
                    }
                    sticks.Add(stick);
                }
                catch (DirectInputException)
                {
                    // TODO : Swallow?
                }
            }
            return sticks.ToArray();
        }

        void stickHandle(Joystick stick, int id)
        {
            JoystickState state = new JoystickState();
            state = stick.GetCurrentState();

            yValue = state.Y;
            xValue = state.X;
            zValue = state.Z;

            bool[] buttons = state.GetButtons();

            if (id == 0)
            {
                if (yValue > -20 && yValue < 20)
                    _stickMoved = false;

                if (yValue < -40 && wb.Focused && !_stickMoved)
                {
                    wb.Document.GetElementById("btn-up").InvokeMember("click");
                    _stickMoved = true;
                }

                if (yValue > 40 && wb.Focused && !_stickMoved)
                {
                    wb.Document.GetElementById("btn-down").InvokeMember("click");
                    _stickMoved = true;
                }

                if (buttons[0])
                {
                    LaunchMame();
                }

                if (buttons[1])
                {
                    // ... handle other buttons if needed
                }
            }

        }

        private void timerMain_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < sticks.Length; i++)
            {
                stickHandle(sticks[i], i);
            }
        }
    }
}
