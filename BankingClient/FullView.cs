﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConcurrentBankingServer.Model;
using System.Threading;
using ConcurrentBankingServer.Service;

namespace BankingClient
{
    public partial class FullView : Form
    {
        MainWindow main;

        public delegate void Notify(string note);

        public Notify notify;

        public AccoutService.UpdateProgress progresser;

        public FullView(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            StartPosition = FormStartPosition.CenterScreen;
            notify = note;
            progresser = updateProgress;
        }

        private void FullView_Load(object sender, EventArgs e)
        {
            List<String> list = new List<string>(main.currentCard.getAccounts());
            comboBox1.DataSource = list;
            comboBox1.SelectedIndex = 0;
            comboBox5.DataSource = CDList.getCdTitleList();
            comboBox4.DataSource = CDList.getCdTitleList();
            list.RemoveAt(comboBox1.SelectedIndex);
            comboBox2.DataSource = list;
            comboBox3.DataSource = main.currentCard.getAccounts();
            comboBox6.DataSource = main.currentCard.getAccounts();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            String accNo = (String)comboBox3.SelectedValue;
            DebitCard card = main.currentCard;
            String balance = main.server.AccountService.getBalance(card.CardNumber, card.Pin, accNo).ToString("N");
            label6.Text = "Balance is Rs. " + balance;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<String> list = new List<string>(main.currentCard.getAccounts());
            list.Remove((String)comboBox1.SelectedItem);
            comboBox2.DataSource = list;
            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double amount = Double.Parse(textBox1.Text);
            String ac1 = (String)comboBox1.SelectedItem;
            String ac2 = (String)comboBox2.SelectedItem;
            Thread thread = new Thread(() => transferMoney(amount, ac1, ac2));
            thread.Start();
        }

        private void transferMoney(double amount, String ac1, String ac2)
        {
            DebitCard card = main.currentCard;
            Transaction tr1 = new Transaction("debit", amount);
            Transaction tr2 = new Transaction("credit", amount);

            main.server.AccountService.executeTransaction(card.CardNumber, card.Pin, ac1, tr1);
            if (tr1.Success)
            {
                main.server.AccountService.executeTransaction(card.CardNumber, card.Pin, ac2, tr2);
                main.logWindow.logger("Money transfer from " + ac1 + " to " + ac2 + " sucessfull.");

                //(new Form()).
                //MessageBox.Show("Money transfer from " + ac1 + " to " + ac2 + " sucessfull.");
                notify("Money transfer from " + ac1 + " to " + ac2 + " sucessfull.");
            }
            else
            {
                main.logWindow.logger("Money transfer from " + ac1 + " to " + ac2 + " unsucessfull.");
                //MessageBox.Show("Money transfer from " + ac1 + " to " + ac2 + " unsucessfull.\nMay be there's not sufficeint funds in " + ac1);
                notify("Money transfer from " + ac1 + " to " + ac2 + " unsucessfull.");
            }
        }

        private void FullView_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.Show();
            this.Dispose();
        }

        public void updateProgress(object sender,
                                  ProgressChangedEventArgs e)
        {
           // progressBar1.Value = e.ProgressPercentage;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ConcurrencyDemo demo = new ConcurrencyDemo(main);
            demo.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            main.Dispose();
            this.Dispose();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Dispose();
            main.currentCard = null;
            main.Show();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            CDItem cd = CDList.getCdByTitleAndID((String)comboBox4.SelectedItem);

            label21.Text = "Album : "+ cd.Title;
            label23.Text = "by " + cd.Artist;
            label22.Text = cd.Description;
         
            label24.Text = "Rs. " +cd.Price.ToString("N")+"/-";
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            CDItem cd = CDList.getCdByTitleAndID((String)comboBox4.SelectedItem);
            label18.Text = "Price: Rs." + cd.Price.ToString("N") + "/-";
            label21.Text = "Album : " + cd.Title;
            label23.Text = "by " + cd.Artist;
            label22.Text = cd.Description;

            label24.Text = "Rs. " + cd.Price.ToString("N") + "/-";
        }

        private void button4_Click(object sender, EventArgs e)
        {

            ConcurrencyDemo demo = new ConcurrencyDemo(main);
            demo.Show();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        public void note(String logMsg)
        {

            if (label25.InvokeRequired)
            {
                Invoke(notify, logMsg);
            }
            else
            {
                label25.Text = (DateTime.Now).ToString() + " : " + logMsg ;
                Refresh();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            CDItem cd = CDList.getCdByTitleAndID((String)comboBox4.SelectedItem);
            
            DebitCard card = main.currentCard;

            Transaction tr = main.server.AccountService.executeTransaction(card.CardNumber, card.Pin,
                                    (String)comboBox6.SelectedItem,
                                     new Transaction("debit",cd.Price));

            if(tr.Success){
                notify("Transaction Successfully. Please collect the cd from the tray.");
            }else{
                notify("Transaction failed. Please try again.");
            }

        }
    }
}
