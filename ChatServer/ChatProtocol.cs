using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    class ChatProtocol
    {
        // contenitore
        private Client c;
        public ChatProtocol(Client c)
        {
            this.c = c;
        }

        public void parseData(String s){
            if(s.Contains("HELO")){
                c.Send("ChatCPT:Version 1.0");
            }
            else if(s.Contains("Login:")) {
                String[] p = s.Split(':');
                MessageBox.Show(p[1]);
                c.Send("Pwd");
            }
            else if (s.Contains("Pwd:")) {
                String[] p = s.Split(':');
                MessageBox.Show(p[1]);
            }
        }
    }
}
