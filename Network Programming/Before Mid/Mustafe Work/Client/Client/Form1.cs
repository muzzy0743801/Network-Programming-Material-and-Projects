using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        byte[] b = new byte[1024];
        TcpClient client = new TcpClient();

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void ReadMsg(IAsyncResult ar)
        {
            NetworkStream ns = (NetworkStream)ar.AsyncState;
            int count = ns.EndRead(ar);
            txtDisplay.Text += ASCIIEncoding.ASCII.GetString(b, 0, count);
            ns.BeginRead(b, 0, b.Length, ReadMsg, ns);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine(txtName.Text + " Says: " + txtMsg.Text);
            sw.Flush();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            CheckForIllegalCrossThreadCalls = false;
            client.Connect(IPAddress.Loopback, 11000);

            NetworkStream ns = client.GetStream();
            StreamWriter sw = new StreamWriter(ns);
            sw.WriteLine("@name@" + txtName.Text);
            sw.Flush();
            ns.BeginRead(b, 0, b.Length, ReadMsg, ns);
        }
    }
}
