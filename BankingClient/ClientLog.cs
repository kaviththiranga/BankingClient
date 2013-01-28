using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConcurrentBankingServer;
using System.Threading;
using System.IO;

namespace BankingClient
{
    public partial class ClientLog : Form
    {
        public Server.Log logger;
        String logFile = "D:\\temp\\clientLog.txt";

        public ClientLog()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Left = 0;
            Top = 0;
            logger += log;
            logger += logToFile;
        }

        public void log(String logMsg) {

            if (richTextBox1.InvokeRequired)
            {
                Invoke(logger, logMsg);
            }
            else
            {
                richTextBox1.AppendText((DateTime.Now).ToString() + " : " + logMsg + "\n");
            }
        }

        public void logToFile(string logMsg) {
            using (Mutex mutex = new Mutex(false, "Log File Lock"))
            {
                if (!mutex.WaitOne())
                {
                    log("Error Saving to log file");
                }

                TextWriter tw = new StreamWriter(logFile, true);
                tw.WriteLine((DateTime.Now).ToString() + " : " + logMsg);
                tw.Close();

                mutex.ReleaseMutex();
            }
        }

        private void ServerLog_Load(object sender, EventArgs e)
        {

        }

        private void ServerLog_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
