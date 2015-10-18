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
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace DataSourceTest1
{
    public partial class Form1 : Form
    {
        struct data
        {
            public float a;
            public float b;
            public float c;
            public int i;
        }

        private data d;
        private UdpClient uc;
        private Byte[] sendBytes;
        private ulong packetcount;
        private Stopwatch _stopwatch;

        public Form1()
        {
            InitializeComponent();
            this.uc = new UdpClient();
            this.uc.Connect("localhost", 11000);
            this._stopwatch = new Stopwatch();
            this._stopwatch.Start();
            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            packdata();
            senddata();
        }

        private void packdata()
        {
            this.d.a = singen(3.0f, 0.2f);
            this.d.b = singen(5.5f, 1.0f);
            this.d.c = singen(1.1f, 0.4f);
            this.d.i = (int)singen(16.0f, 0.5f);
                
        }

        private float singen(float amplitude, float period)
        {
            double angle;
            angle = this._stopwatch.ElapsedMilliseconds / (period * 1000.0);
            return (float)Math.Sin(angle) * amplitude;
        }

        private void senddata()
        {
            this.sendBytes = _getBytes(this.d);
            this.uc.Send(this.sendBytes, this.sendBytes.Length);
            this.packetcount++;
            this.label1.Text = this.packetcount.ToString();
        }

        private byte[] _getBytes(data _data)
        {
            int size = Marshal.SizeOf(_data);
            byte[] arr = new byte[size];

            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(_data, ptr, true);
            Marshal.Copy(ptr, arr, 0, size);
            Marshal.FreeHGlobal(ptr);
            return arr;
        }
    }
}
