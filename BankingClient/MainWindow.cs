﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ConcurrentBankingServer;
using ConcurrentBankingServer.Model;
using System.Threading;

namespace BankingClient
{
    public partial class MainWindow : Form
    {
        public ClientLog logWindow;
        public Server server;
        public DebitCard currentCard;


        public MainWindow()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            logWindow = new ClientLog();
            logWindow.Show();
            server = new Server();
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool authenticated = server.AccountService.authenticateTransaction(cardNo.Text, pinNo.Text);

            if (authenticated)
            {
                currentCard = new DebitCard(cardNo.Text, pinNo.Text);
                currentCard.setAccounts(server.AccountService.getAccountsByCard(cardNo.Text));
                FullView myAccounts = new FullView(this);
                this.Hide();
                cardNo.Text = "";
                pinNo.Text = "";
                logWindow.logger("Authentication Successfull for Debit Card : "+currentCard.CardNumber);
                myAccounts.Show();
            }
            else
            {
                currentCard = null;
                pinNo.Text = "";
                logWindow.logger("Authentication failed for Debit Card : " + cardNo.Text + "Invalid Pin or card.");
                MessageBox.Show("Invalid Pin or card.");
            }

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            cardNo.Text = "";
            pinNo.Text = "";
            currentCard = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.terminate();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.terminate();
        }

        private void pinNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                button1_Click(sender, new EventArgs());
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            Dispose();
            Application.Exit();
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
           
            currentCard = null;
        }
    }
}
