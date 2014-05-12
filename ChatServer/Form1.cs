using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChatServer.Properties;
using System.Net.Sockets;

namespace ChatServer
{
    public partial class Form1 : Form
    {
        private String hostname;
        private String hostIP;
        private int port = Properties.Settings.Default.port;

        public Form1()
        {
            InitializeComponent();
            this.hostname = System.Environment.MachineName;
            this.hostIP = LocalIPAddress();
            this.hostNameLabel.Text = hostname;
            this.hostIPLabel.Text = hostIP;
            this.hostPortLabel.Text = port.ToString();
        }

        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
        }

        private void OnStartClick(object sender, EventArgs e)
        {
            if (this.start.Text == "Start")
            {
                this.start.Text = "ShutDown";
            }
            else
            {
                this.start.Text = "Start";
            }
        }

        private void OnCloseClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void OnAccountsClick(object sender, EventArgs e)
        {
            Accounts f_accounts = new Accounts();
            f_accounts.Show();

        }
    }
}
