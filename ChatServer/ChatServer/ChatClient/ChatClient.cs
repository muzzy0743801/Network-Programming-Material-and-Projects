using System;
using System.IO;
using System.Net;
using System.Data;
using System.Text;
using System.Drawing;
using Chat = System.Net;
using System.Threading;
using System.Collections;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PC;
//*****************************************************************************************
//                           LICENSE INFORMATION
//*****************************************************************************************
//   PC_Chat 1.0.0.0
//   Creates a basic basic server/client chat application in C#
//
//   Copyright (C) 2007  
//   Richard L. McCutchen 
//   Email: richard@psychocoder.net
//   Created: 16SEP07
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program.  If not, see <http://www.gnu.org/licenses/>.
//*****************************************************************************************
namespace ChatClient
{
    public partial class ChatClient
    {
        public ChatClient()
        {
            //InitializeComponent();
        }
        static Form client;
        static Chat.Sockets.TcpClient tcpClient;

        [DllImport("kernel32.dll")]
        private static extern void ExitProcess(int a);

        public static void Main()
        {
            client = new Form();
            client.Text = "PCChat - Chat Client";

            client.Closing += new CancelEventHandler(ChatClient_Closing);
            client.Controls.Add(new TextBox());
            client.Controls[0].Dock = DockStyle.Fill;
            client.Controls.Add(new TextBox());
            client.Controls[1].Dock = DockStyle.Bottom;
            ((TextBox)client.Controls[0]).Multiline = true;
            ((TextBox)client.Controls[1]).Multiline = true;
            client.WindowState = FormWindowState.Maximized;
            client.Show();
            ((TextBox)client.Controls[1]).KeyUp += new KeyEventHandler(key_up);
            tcpClient = new Chat.Sockets.TcpClient();
            tcpClient.Connect("127.0.0.1", 4296);
            Thread chatThread = new Thread(new ThreadStart(run));
            chatThread.Start();
            while (true)
            {
                Application.DoEvents();
            }
        }

        private static void ChatClient_Closing(object s, CancelEventArgs e)
        {
            e.Cancel = false;
            //exit the application
            Application.Exit();
            //call the ExitProcess API
            ExitProcess(0);
        }

        private static void key_up(object s, KeyEventArgs e)
        {
            //create our textbox value variable
            TextBox txtChat = (TextBox)s;
            //check to make sure the length of the text
            //in the TextBox is greater than 1 (meaning it has text in it)
            if (txtChat.Lines.Length > 1)
            {
                //create a StreamWriter based on the current NetworkStream
                StreamWriter writer = new StreamWriter(tcpClient.GetStream());
                //write our message
                writer.WriteLine(txtChat.Text);
                //ensure the buffer is empty
                writer.Flush();
                //clear the textbox for our next message
                txtChat.Text = "";
                txtChat.Lines = null;
            }
        }

        private static void run()
        {
            //create our StreamReader Object, based on the current NetworkStream
            StreamReader reader = new StreamReader(tcpClient.GetStream());
            while (true)
            {
                //call DoEvents so other processes can process
                //simultaneously
                Application.DoEvents();
                //create a TextBox reference
                TextBox txtChat = (TextBox)client.Controls[0];
                //append the current value in the 
                //current NetworkStream to the chat window
                txtChat.AppendText(reader.ReadLine() + "\r\n");
                //place the cursor at the end of the
                //text in the textbox for typing our messages
                txtChat.SelectionStart = txtChat.Text.Length;
            }
        }
    }
}