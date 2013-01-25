using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConcurrentBankingServer;

namespace BankingClient
{
    public partial class ServerLog : Form
    {
        public Server.Log logger;
        
        public ServerLog()
        {
            InitializeComponent();
            logger = log;
        }

        public void log(String logMsg) {

            if (richTextBox1.InvokeRequired)
            {
                Invoke(new Action(
                        delegate()
                        {
                            richTextBox1.AppendText((DateTime.Now).ToString() + " : " + logMsg + "\n");
                        }));
            }
            else
            {
                richTextBox1.AppendText((DateTime.Now).ToString() + " : " + logMsg + "\n");
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
