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
using ConcurrentBankingServer.Model;

namespace BankingClient
{
    public partial class ConcurrencyDemo : Form
    {
        MainWindow main;

        public ConcurrencyDemo(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }


        public void executeMultipleTrans()
        {

            Thread t1 = new Thread(new ThreadStart(

                        () => { main.server.AccountService.executeTransaction("DC001", "1989", "AC00001", new Transaction("credit", 1000));
                       
                                }

                        ));

            Thread t2 = new Thread(new ThreadStart(

                        () => { main.server.AccountService.executeTransaction("DC001", "1989", "AC00001", new Transaction("credit", 1000));
                       
                                }

                        ));


            Thread t3 = new Thread(new ThreadStart(

                        () => { main.server.AccountService.executeTransaction("DC001", "1989", "AC00001", new Transaction("debit", 1000));
                                
                                }

                        ));

            Thread t4 = new Thread(new ThreadStart(

                        () => {
                                main.server.AccountService.executeTransaction("DC001", "1989", "AC00001", new Transaction("debit", 500));
                               
                        
                                }

                        ));

            t1.Name = " Thread 1";
            t2.Name = " Thread 2";
            t3.Name = " Thread 3";
            t4.Name = " Thread 4";
            t1.Start();
            t2.Start();

            t3.Start();
            t4.Start();

        }
        private void ConcurrencyDemo_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            executeMultipleTrans();
        }
    }
}
