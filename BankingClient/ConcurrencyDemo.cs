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
using ConcurrentBankingServer.Service;

namespace BankingClient
{
    public partial class ConcurrencyDemo : Form
    {
        MainWindow main;

        public AccoutService.UpdateProgress progresser;

        public ConcurrencyDemo(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            StartPosition = FormStartPosition.CenterScreen;
            progresser = updateProgress;
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

        private void button2_Click(object sender, EventArgs e)
        {
            BackgroundWorker _bw = new BackgroundWorker
            {
                WorkerReportsProgress = false,
                WorkerSupportsCancellation = true
            };

            _bw.DoWork += main.server.AccountService.executeTransaction2;

           // _bw.ProgressChanged += updateProgress;
            _bw.RunWorkerCompleted += bw_RunWorkerCompleted;

            BackgroundWorkerArg request = new BackgroundWorkerArg();

            request.AccountNumber = "AC0001";
            request.CardNumber = main.currentCard.CardNumber;
            request.Pin = main.currentCard.Pin;
            request.Transaction = new Transaction("debit", 25);
            _bw.RunWorkerAsync(request);

            /*Console.WriteLine("Press Enter in the next 5 seconds to cancel");
            Console.ReadLine();
            if (_bw.IsBusy) _bw.CancelAsync();
            Console.ReadLine();*/
        }

        private void bw_RunWorkerCompleted(object sender,
                                     RunWorkerCompletedEventArgs e)
        {
           if (e.Cancelled)
                main.logWindow.logger("You canceled!");
           else if (e.Error != null) { }
                //main.logWindow.logger("Worker exception: "+((ExeTransacBackgResult)e.Result).Msg);
            //else
                //main.logWindow.logger("Complete: " + ((ExeTransacBackgResult)e.Result).Msg);      // from DoWork*/
        }

        public void updateProgress(object sender,
                                 ProgressChangedEventArgs e)
        {
            if (progressBar1.InvokeRequired)
            {
                Invoke(new Action(
                        delegate()
                        {
                            /*int progress = e.ProgressPercentage;
                            int maxProgress = progressBar1.Maximum;*/
                            progressBar1.Value = e.ProgressPercentage;//Math.Min(progress, maxProgress);
                        }));
            }
            else
            {
                /*int progress = e.ProgressPercentage;
                int maxProgress = progressBar1.Maximum;*/
                progressBar1.Value = e.ProgressPercentage;//Math.Min(progress, maxProgress);
            }
        }

    }
}
