using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConcurrentBankingServer.Model;

namespace BankingClient
{
    public partial class MyAccounts : Form
    {
        public MainWindow main;

        public MyAccounts(MainWindow main)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            this.main = main;
        }

        private void MyAccounts_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = main.currentCard.getAccounts();
        }

        private void MyAccounts_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.Show();
            main.currentCard = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String accNo = (String) comboBox1.SelectedValue;

            DebitCard card = main.currentCard;
            String balance = main.server.AccountService.getBalance(card.CardNumber, card.Pin, accNo).ToString();
            label2.Text = "Balance of Account : " + accNo + " is\n Rs."+balance;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Transfer transfer = new Transfer(main);
            transfer.StartPosition = FormStartPosition.CenterScreen;
            transfer.Show();
        }
    }
}
