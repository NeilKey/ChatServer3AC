using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ChatServer;

namespace ChatServer
{
    public class Client
    {
        // Client socket.
        public Socket socket;
        // Container
        private ChatServerForm csf;
        // Size of receive buffer.
        public static int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer;
        public String nickname;
        // Classe che gestisce il protocollo
        ChatProtocol protocol;

        public Client(ChatServerForm csf, Socket socket) {
            this.socket = socket;
            this.buffer = new byte[Client.BufferSize];
            this.csf = csf;
            this.protocol = new ChatProtocol(this);
        }

        internal void SetupReceiveCallback()
        {
            try
            {
                // Imposta la callback per la ricezione asincrona dei dati.
                AsyncCallback receiveData = new AsyncCallback(OnReceivedData);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, receiveData, this);
            }
            catch (Exception) { }
        }

        private void OnReceivedData(IAsyncResult ar)
        {
            // La variabile client non è utilizzata, corrisponde a this ed è stata passata tramite
            // il parametro "state" di BeginReceive().
            Client client = (Client)ar.AsyncState;

            byte[] arrData = GetReceivedData(ar);

            // Se nessun dato è stato ricevuto, allora la connessione è da ritenere chiusa
            if (arrData.Length <= 0)
            {
                socket.Close();
                csf.deleteClient(this);  // Elimina il client corrente dalla lista.
                return;
            }

            // In questo punto la thread corrente è quella del socket di ricezione, utilizzare
            // m_form.Invoke() per invocare il metodo associato al Delegate ed eseguirlo all'interno
            // della thread che ha creato il form (m_form).
            // Converti il buffer di byte ricevuti in una stringa Unicode.
            string sReceived = Encoding.Unicode.GetString(arrData, 0, arrData.Length);
            protocol.parseData(sReceived);
            //m_form.Invoke(m_form.SetMessageDelegate, new string[] { sReceived });

            // Riabilita la ricezione dal client.
            SetupReceiveCallback();
        }

        private byte[] GetReceivedData(IAsyncResult ar)
        {
            int nBytesRec = 0;

            try
            {
                // Disabilita la ricezione dati e ritorna il numero di byte ricevuti.
                nBytesRec = socket.EndReceive(ar);
            }
            catch
            {
                // Nessun dato ricevuto, ritorna un buffer vuoto.
                return new byte[0];
            }

            byte[] data = new byte[nBytesRec];
            Array.Copy(buffer, data, nBytesRec);

            // Se i byte ricevuti non hanno utilizzato completamente il buffer di
            // ricezione, ritorna.
            if (nBytesRec < buffer.Length) return data;

            // Controlla se ci sono ancora dei dati rimanenti (non trasferiti nel
            // buffer di ricezione perché già pieno), in caso concatenali.
            int nToBeRead = socket.Available;
            if (nToBeRead <= 0) return data;

            byte[] myData = new byte[nToBeRead];
            socket.Receive(myData);

            // Concatena i due buffer in uno solo e ritorna quest'ultimo.
            byte[] allDataReceived = new byte[nBytesRec + nToBeRead];
            Array.Copy(data, 0, allDataReceived, 0, nBytesRec);
            Array.Copy(myData, 0, allDataReceived, nBytesRec, nToBeRead);

            return allDataReceived;
        }

        public void Send(String data)
        {
            // Convert the string data to byte data using ASCII encoding.
            byte[] byteData = Encoding.Unicode.GetBytes(data);

            // Begin sending the data to the remote device.
            socket.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), socket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void Close() 
        {
            csf.deleteClient(this);
        }

        public void NotifyNewOnline(String nickname)
        {
            csf.NotifyNewOffline(this, nickname);
        }

        public void NotifyNewOffline(String nickname)
        {
            csf.NotifyNewOnline(this, nickname);
        }

        public void SendMessages(String from, String[] to, String message)
        {
            csf.SendMessages(from, to, message);
        }
    }
}