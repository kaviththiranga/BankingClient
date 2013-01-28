using System;
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
using System.Runtime.InteropServices;

namespace BankingClient
{
    public partial class FullView : Form
    {

        MainWindow main;

        public delegate void Notify(string note);

        public Notify notify;

        public FullView(MainWindow main)
        {
            InitializeComponent();
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            this.main = main;
            StartPosition = FormStartPosition.CenterScreen;
            notify = note;
            pictureBox8.Visible = false;

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
            //String accNo = (String)comboBox3.SelectedValue;
            //DebitCard card = main.currentCard;
            //String balance = main.server.AccountService.getBalance(card.CardNumber, card.Pin, accNo).ToString("N");
            //label6.Text = "Balance is Rs. " + balance;
            while (backgroundWorker2.IsBusy) {
                Thread.Sleep(1000);
            }
            backgroundWorker2.RunWorkerAsync();
            
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
            /*double amount = Double.Parse(textBox1.Text);
            String ac1 = (String)comboBox1.SelectedItem;
            String ac2 = (String)comboBox2.SelectedItem;
            Thread thread = new Thread(() => transferMoney(amount, ac1, ac2));
            thread.Start();*/

            while (backgroundWorker3.IsBusy)
            {
                Thread.Sleep(1000);
            }
            backgroundWorker3.RunWorkerAsync();
        }

        private void FullView_FormClosing(object sender, FormClosingEventArgs e)
        {
            main.Show();
            this.Dispose();
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
            //pictureBox8.Visible = true;
            //CDItem cd = CDList.getCdByTitleAndID((String)comboBox4.SelectedItem);
            
            //DebitCard card = main.currentCard;

            //Transaction tr = main.server.AccountService.executeTransaction(card.CardNumber, card.Pin,
            //                        (String)comboBox6.SelectedItem,
            //                         new Transaction("debit",cd.Price));

            //if(tr.Success){
            //    notify("Transaction Successfully. Please collect the cd from the tray.");
            //}else{
            //    notify("Transaction failed. Please try again.");
            //}

            //pictureBox8.Visible = false;
            while (backgroundWorker1.IsBusy)
            {
                Thread.Sleep(1000);
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork_1(object sender, DoWorkEventArgs e)
        {
            
            String title = ""; 
            String acc = "";

            if(comboBox1.InvokeRequired || comboBox6.InvokeRequired || pictureBox8.InvokeRequired){
                Invoke(new Action(
                        delegate()
                        {
                            title = (String)comboBox4.SelectedItem;
                            acc = (String)comboBox6.SelectedItem;
                            pictureBox8.Visible = true;
                        }));
            }
            CDItem cd = CDList.getCdByTitleAndID(title);

            DebitCard card = main.currentCard;

            Transaction tr = main.server.AccountService.executeTransaction(card.CardNumber, card.Pin, acc,
                                     new Transaction("debit", cd.Price));
            e.Result = tr;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Transaction tr = (Transaction)e.Result;

            if (tr.Success)
            {
                notify("Transaction Successfull. Please collect the cd from the tray.");
                main.logWindow.logger("Transaction Successfull. Please collect the cd from the tray.");
            }
            else
            {
                notify("Transaction failed. Please try again.");
                main.logWindow.logger("Transaction failed. Please try again.");
            }

            if (pictureBox8.InvokeRequired)
            {
                Invoke(new Action(
                        delegate()
                        {
                            pictureBox8.Visible = false;
                            Refresh();
                        }));
            }
            else {
                pictureBox8.Visible = false;
            }

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            String accNo = "";
            DebitCard card = main.currentCard;

            if (comboBox3.InvokeRequired || pictureBox7.InvokeRequired)
            {
                Invoke(new Action(
                        delegate()
                        {
                            accNo = (String)comboBox3.SelectedValue;
                            pictureBox7.Visible = true;
                        }));
            }
            Thread.Sleep(1500);
            e.Result = main.server.AccountService.getBalance(card.CardNumber, card.Pin, accNo).ToString("N");
            
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            label6.Text = "Balance is Rs. " + (string)e.Result;
            pictureBox7.Visible = false;
        }

        private void backgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            double amount =0;
            String ac1 = "";
            String ac2 = "";

            if (textBox1.InvokeRequired || comboBox1.InvokeRequired || comboBox2.InvokeRequired || pictureBox6.InvokeRequired)
            {
                Invoke(new Action(
                        delegate()
                        {
                            amount = Double.Parse(textBox1.Text);
                            ac1 = (String)comboBox1.SelectedItem;
                            ac2 = (String)comboBox2.SelectedItem;
                            pictureBox6.Visible = true;
                        }));
            }

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

        private void backgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            pictureBox6.Visible = false;
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
         );

        private void FullView_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, this.ClientRectangle, Color.DarkSlateGray, 5, ButtonBorderStyle.Solid, Color.DarkSlateGray, 5, ButtonBorderStyle.Solid, Color.DarkSlateGray, 5, ButtonBorderStyle.Solid, Color.DarkSlateGray, 5, ButtonBorderStyle.Solid);
        }

  
    }
}
