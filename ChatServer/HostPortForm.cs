using System;
using System.Windows.Forms;
using ChatServer.Properties;

namespace ChatServer
{
    public partial class HostPortForm : Form
    {
        private int m_serverPort;

        public HostPortForm(int serverPort)
        {
            m_serverPort = serverPort;

            InitializeComponent();

            this.hostPortText.Text = Convert.ToString(serverPort);
        }

        private void OnSave(object sender, EventArgs e)
        {
            // Aggiorna i dati dell'host
            int serverPort;
            if (Int32.TryParse(hostPortText.Text, out serverPort))  // Non modificare il Port se il parsing fallisce
            {
                Settings.Default.serverPort = serverPort;
                Settings.Default.Save();
            }
        }
    }
}
