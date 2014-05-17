using System;
using System.Net;
using System.Windows.Forms;
using System.Net.Sockets;
using ChatServer.Properties;
using System.Collections.Generic;
using System.Text;

namespace ChatServer
{
    // parte relativa ai socket: http://msdn.microsoft.com/en-us/library/fx6588te%28v=vs.110%29.aspx
    public partial class ChatServerForm : Form
    {
        private String hostname;
        private String hostIP;

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
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.
            IPAddress ipAddress = IPAddress.Parse(hostIP);
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, Settings.Default.serverPort);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

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
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            Client c = new Client();
            c.workSocket = handler;
            handler.BeginReceive(c.buffer, 0, Client.BufferSize, 0,
                new AsyncCallback(ReadCallback), c);
        }

        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket
            // from the asynchronous state object.
            Client c = (Client)ar.AsyncState;
            Socket handler = c.workSocket;

            // Read data from the client socket. 
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.
                c.sb.Append(Encoding.ASCII.GetString(
                    c.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read 
                // more data.
                content = c.sb.ToString();
                if (content.IndexOf("<EOF>") > -1)
                {
                    // All the data has been read from the 
                    // client. Display it on the console.

                    // Gestione del pacchetto ricevuto
                    // prova
                    MessageBox.Show(content);
                    // Echo the data back to the client.
                    Send(handler, content);
                }
                else
                {
                    // Not all data received. Get more.
                    handler.BeginReceive(c.buffer, 0, Client.BufferSize, 0,
                    new AsyncCallback(ReadCallback), c);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

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
    }
}
