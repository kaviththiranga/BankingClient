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
    public partial class Transfer : Form
    {
        MainWindow main;
        public Transfer(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void Transfer_Load(object sender, EventArgs e)
        {
            List<String> list = new List<string>(main.currentCard.getAccounts());
            comboBox1.DataSource = list;
            comboBox1.SelectedIndex = 0;
            list.RemoveAt(comboBox1.SelectedIndex);
            comboBox2.DataSource = list;
            

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<String> list = new List<string>(main.currentCard.getAccounts());
            list.Remove((String)comboBox1.SelectedItem);
            comboBox2.DataSource = list;
            comboBox2.SelectedIndex = 0;
 
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double amount;
            try
            {
                DebitCard card = main.currentCard;

                amount = Double.Parse(textBox1.Text);
                String ac1 = (String) comboBox1.SelectedItem;
                String ac2 = (String) comboBox2.SelectedItem;

                Transaction tr1 = new Transaction("debit", amount);
                Transaction tr2 = new Transaction("credit", amount);

                main.server.AccountService.executeTransaction(card.CardNumber, card.Pin, ac1, tr1);
                main.server.AccountService.executeTransaction(card.CardNumber, card.Pin, ac2, tr2);
                MessageBox.Show("Money transfer is successful.");
                this.Dispose();
            }
            catch (Exception ex) {
                label4.Text = "Oops! Error Happened.";
            }
        }
    }
}
