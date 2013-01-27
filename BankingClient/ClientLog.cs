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
    public partial class ClientLog : Form
    {
        public Server.Log logger;
        
        public ClientLog()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.Manual;
            Left = 0;
            Top = 0;
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
