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
        private int stateStack = 0;
        private String username;
        private String nickname;
        private DbManagement db = new DbManagement();
        private bool isLogged = false;

        public ChatProtocol(Client c)
        {
            this.c = c;
        }

        public void parseData(String s)
        {
            if (!isLogged)
            {
                switch (stateStack)
                {
                    case 0:
                        if (s.Equals("HELO"))
                        {
                            c.Send("ChatCPT:Version 1.0");
                            stateStack++;
                        }
                        break;
                    case 1:
                        if (s.StartsWith("Login:"))
                        {
                            String[] p = s.Split(':');
                            username = p[1];
                            c.Send("Pwd");
                            stateStack++;
                        }
                        else if (s.StartsWith("Reg:"))
                        {
                            String[] p = s.Split(':');
                            if(p.Length != 3)
                            {
                                c.Send("Reg-Err:Err_InvalidRegParams");
                                return;
                            }
                            
                            switch (db.registerUser(p[1], p[2], p[3])) {
                                case 1: 
                                    c.Send("Login-OK:" + username);
                                    stateStack = 0;
                                    isLogged = true;
                                    c.nickname = nickname = db.getNickname(username);
                                    c.NotifyNewOnline(nickname);
                                    break;
                                case -1:
                                    c.Send("Reg-Err:Err_UsernameAlreadyUsed");
                                    break;
                                case -2:
                                    c.Send("Reg-Err:Err_NicknameAlreadyUsed");
                                    break;
                                case 0:
                                    c.Send("Reg-Err:Err_DatabaseError");
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case 2:
                        if (s.StartsWith("Pwd:"))
                        {
                            String[] p = s.Split(':');
                            switch (db.checkLogin(username, p[1])) {
                                case 1:
                                    c.Send("Login-OK:" + username);
                                    stateStack = 0;
                                    isLogged = true;
                                    c.nickname = nickname = db.getNickname(username);
                                    c.NotifyNewOnline(nickname);
                                    break;
                                case -1:
                                    c.Send("Login-Err:Err_NoUserFound");
                                    break;
                                case 0:
                                    c.Send("Login-Err:Err_DatabaseError");
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (s.StartsWith("Msg:"))
                {
                    String[] p = s.Split(':');
                    String[] nicknames = p[1].Split(',');
                    String message = p[2];
                    c.SendMessages(nickname, nicknames, message);
                }
                else if(s.Equals("Logout"))
                {
                    c.socket.Close();
                    c.Close();
                }
            }
        }
    }
}
