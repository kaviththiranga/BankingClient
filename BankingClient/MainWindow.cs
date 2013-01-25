using System;
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
        public ServerLog logWindow;
        public Server server;
        public DebitCard currentCard;


        public MainWindow()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            logWindow = new ServerLog();
            server = new Server(logWindow.logger);
            logWindow.Show();

        }

        public void executeMultipleTrans() {

            /*Thread t1 = new Thread(new ThreadStart(

                        () => { server.AccountService.executeTransaction("AC00002", "1002", new Transaction("credit", 555)); }

                        ));

            Thread t2 = new Thread(new ThreadStart(

                        () => { server.AccountService.executeTransaction("AC00002", "1002", new Transaction("credit", 1000)); }

                        ));


            Thread t3 = new Thread(new ThreadStart(

                        () => { server.AccountService.executeTransaction("AC00002", "1002", new Transaction("debit", 1000)); }

                        ));

            Thread t4 = new Thread(new ThreadStart(

                        () => {
                                t1.Join();
                                t2.Join();
                                t3.Join();
                                server.AccountService.executeTransaction("AC00002", "1002", new Transaction("debit", 445));
                                logWindow.logger(server.AccountService.getBalance("AC00002", "1002").ToString());
                        
                                }

                        ));

            t1.Name = " Thread 1";
            t2.Name = " Thread 2";
            t3.Name = " Thread 3";
            t4.Name = " Thread 4";
            t1.Start();
            logWindow.logger(server.AccountService.getBalance("AC00002", "1002").ToString());
            t2.Start();

            t3.Start();
            t4.Start();*/
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bool authenticated = server.AccountService.authenticateTransaction(cardNo.Text, pinNo.Text);

            if (authenticated)
            {
                currentCard = new DebitCard(cardNo.Text, pinNo.Text);
                currentCard.setAccounts(server.AccountService.getAccountsByCard(cardNo.Text));
                MyAccounts myAccounts = new MyAccounts(this);
                this.Hide();
                myAccounts.Show();
            }
            else
            {
                currentCard = null;
                MessageBox.Show("Invalid Pin or card.");
            }

        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            server.terminate();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
