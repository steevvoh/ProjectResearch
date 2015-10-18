using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;

namespace containertest3
{


    public partial class Form1 : Form
    { 
        private UdpClient ucreciever;
        private IPEndPoint RemoteIpEndPoint;
        private Byte[] receivebytes;
        private data d;

        struct data
        {
            public float a;
            public float b;
            public float c;
            public int i;
        }

        public Form1()
        {
            InitializeComponent();
            this.ucreciever = new UdpClient(11000);
            //this.RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            //this.receivebytes = this.ucreciever.Receive(ref this.RemoteIpEndPoint);
            //this.label1.Text = Encoding.ASCII.GetString(this.receivebytes);
            this.ucreciever.BeginReceive(new AsyncCallback(recv), null);

        }

        private void recv(IAsyncResult res)
        {
            this.RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            this.receivebytes = this.ucreciever.EndReceive(res, ref RemoteIpEndPoint);
            this.ucreciever.BeginReceive(new AsyncCallback(recv), null);
            viewchange();
        }

        private void viewchange()
        {
            SetText( ByteArrayToString(this.receivebytes), this.label1);
            this.d = fromBytes(this.receivebytes);
            SetText(this.d.a.ToString(), this.label2);
            SetText(this.d.b.ToString(), this.label3);
            SetText(this.d.c.ToString(), this.label4);
            SetText(this.d.i.ToString(), this.label5);
        }

        delegate void SetTextCallback(string text, Label label);

        private void SetText(string text, Label label)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (label.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text, label });
            }
            else
            {
                label.Text = text;
            }
        }



        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        data fromBytes(byte[] arr)
        {
            data str = new data();

            int size = Marshal.SizeOf(str);
            IntPtr ptr = Marshal.AllocHGlobal(size);

            Marshal.Copy(arr, 0, ptr, size);

            str = (data)Marshal.PtrToStructure(ptr, str.GetType());
            Marshal.FreeHGlobal(ptr);

            return str;
        }
    }
}
