using System;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;
using ChatServer.Properties;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    // spunto parte relativa ai socket: http://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx
    // poi adattato al modello visto in classe
    public partial class ChatServerForm : Form
    {
        private String hostname;
        private String hostIP;
        private List<Client> clients = new List<Client>();

        public ChatServerForm()
        {
            InitializeComponent();
            this.hostname = System.Environment.MachineName;
            this.hostIP = LocalIPAddress();
            this.hostNameLabel.Text = hostname;
            this.hostIPLabel.Text = hostIP;
            this.hostPortLabel.Text = Settings.Default.serverPort.ToString();
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

        public void StartListening()
        {
            // Establish the local endpoint for the socket.
            IPAddress ipAddress = IPAddress.Parse(hostIP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Settings.Default.serverPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(10);

                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            NewConnection(listener.EndAccept(ar));

            listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);
        }

        public void NewConnection(Socket clientSock) {
            // creo nuovo oggetto
            Client c = new Client(this, clientSock);
            // aggiungo alla lista di clienti connessi
            clients.Add(c);
            // ascolto per dati in entrata
            c.SetupReceiveCallback();
        } //OK

        private void OnStartClick(object sender, EventArgs e)
        {
            if (this.start.Text == "Start")
            {
                StartListening();
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

        private void OnHostPortChange(object sender, EventArgs e)
        {
            HostPortForm dlg = new HostPortForm(Settings.Default.serverPort);
            if (dlg.ShowDialog() != DialogResult.OK) return;

            this.hostPortLabel.Text = Settings.Default.serverPort.ToString();
        }

        public void deleteClient(Client c) {
            clients.Remove(c);
        }

        public void NotifyNewOnline(Client cOnline, String nickname) 
        {
            foreach (Client client in clients)
            {
                if(client != cOnline)
                    client.Send("NewUser:" + nickname);
            }
        }

        public void NotifyNewOffline(Client cOffline, String nickname)
        {
            deleteClient(cOffline);
            foreach (Client client in clients)
            {
                    client.Send("UserLeave:" + nickname);
            }
        }

        public void SendMessages(String from, String[] to, String message)
        {
            foreach (Client c in clients)
            {
                foreach (String nickname in to)
                {
                    if (nickname == c.nickname)
                    {
                        c.Send(String.Format("Msg:{0}:{1}", from, message));
                        break;
                    }
                }
            }
        }
    }
}
