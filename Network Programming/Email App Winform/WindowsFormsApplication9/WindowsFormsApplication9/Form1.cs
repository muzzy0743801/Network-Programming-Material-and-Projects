using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Mail;

namespace WindowsFormsApplication9
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Add("theomyway@gmail.com");
            listBox1.Items.Add("beingfaisalmalik@gmail.com");
            listBox1.Items.Add("muxxamil0743801@gmail.com");

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smptpserver = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("theomway@gmail.com");
                string selectedItem = listBox1.Items[listBox1.SelectedIndex].ToString(); 
                mail.To.Add(selectedItem);
               
                mail.Subject = textBox1.Text;
                mail.Body = textBox2.Text;

                smptpserver.Port = 587;
                listBox1.SelectionMode = SelectionMode.MultiSimple;
                
                
                smptpserver.Credentials = new System.Net.NetworkCredential("theomyway@gmail.com", textBox4.Text);
                Console.WriteLine("theomyway says:");
                string msg = textBox2.Text;
                listBox2.Items.Add("theomyway@gmail.com to:" + selectedItem + ":" + msg);


                smptpserver.EnableSsl = true;
                smptpserver.Send(mail);
                MessageBox.Show("SUCCESSFULLY SENT!");






            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
        }
    }
}
