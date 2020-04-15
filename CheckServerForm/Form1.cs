using PilotGaea.Serialize;
using PilotGaea.TMPClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckServerForm
{
    public partial class Form1 : Form
    {
        int second = 0;
        int secondIterval = 60;
        string url = "http://127.0.0.1:8080/docmd?cmd=";
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!CheckIsAlive()) StartServer();

            second++;
            label1.Text = second.ToString();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (!CheckIsAlive()) StartServer();
            
            timer1.Start();
            timer1.Interval = secondIterval * 1000;
        }

        private bool CheckIsAlive()
        {
            CMapDocument cmDoc = new CMapDocument();
            VarStruct inputnum = new VarStruct();
            VarStruct outputnum = new VarStruct();

            bool connect = false;
            string doCmdUrl = url + "isLive";
            connect = cmDoc.DoCommand(doCmdUrl, ref inputnum, ref outputnum);

            return connect;
        }
        private void StartServer()
        {
            //[Server異常]
            //WerFault.exe 以及PGMapServer.exe
            string PrcocessName = "PGMapServer";
            string target = @"C:\Program Files\PilotGaea\TileMap\PGMapServer.exe";
            Process[] MyProcess = Process.GetProcessesByName(PrcocessName);
            if (MyProcess.Length > 0)
            {
                MyProcess[0].Kill(); //關閉執行中的程式
                MyProcess[0].WaitForExit();
            }
            string SystemErrorName = "WerFault";
            Process[] SystemErrorProcess = Process.GetProcessesByName(SystemErrorName);
            if (SystemErrorProcess.Length > 0)
            {
                SystemErrorProcess[0].Kill(); //關閉執行中的程式
                SystemErrorProcess[0].WaitForExit();
            }
            Process.Start(target, "-s 1");

            string str = "重啟伺服器。In TimerCallback: " + DateTime.Now;
            listBox.Items.Add(str);
        }
    }
}
