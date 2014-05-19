using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlServerCe;
using ChatServer.Properties;
using System.Windows.Forms;

namespace ChatServer
{
    class DbManagement
    {
        private SqlCeConnection conn = null;
        private SqlCeCommand cmd = null;

        public DbManagement() { }

        public List<String[]> getAccounts()
        {
            List<String[]> l = new List<String[]>(); 

            try
            {
                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("SELECT * FROM [Users]", conn);

                SqlCeDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();

                // Per ragioni di performances, utilizzare GetOrdinal() all'esterno del loop while(reader.Read())
                int idUsername = reader.GetOrdinal("Username");
                int idPassword = reader.GetOrdinal("Password");
                int idNickname = reader.GetOrdinal("Nickname");
                int idEnabled = reader.GetOrdinal("Enabled");

                while (reader.Read())
                {
                    l.Add(new String[] {
                        reader.GetString(idNickname),
                        reader.GetString(idUsername),
                        reader.GetString(idPassword),
                        (reader.GetBoolean(idEnabled)) ? "Yes" : "No"
                    });
                }

                conn.Close();
            }
            catch (Exception e)
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();

                MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return l;
        }

        public int checkLogin(String username, String password)
        {
            try
            {
                if (checkUser(username))
                    return -1;

                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("SELECT * FROM [Users] WHERE Username = @Username AND Password = @Password;", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlCeDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                return 1;
            } catch(Exception) {}

            return 0;
        }

        public bool checkUser(String username)
        {
            try
            {
                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("SELECT * FROM [Users] WHERE Username = @Username;", conn);
                cmd.Parameters.AddWithValue("@Username", username);

                SqlCeDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                return reader.HasRows;
            }
            catch (Exception) { }

            return false;
        }

        public bool checkNickname(String nickname)
        {
            try
            {
                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("SELECT * FROM [Users] WHERE Nickname = @Nickname;", conn);
                cmd.Parameters.AddWithValue("@Nickname", nickname);

                SqlCeDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();
                return reader.HasRows;
            }
            catch (Exception) { }

            return false;
        }

        public int registerUser(String username, String password, String nickname)
        {
            try
            {
                if (checkUser(username))
                    return -1;

                if (checkNickname(nickname))
                    return -2;

                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("INSERT INTO [Users] VALUES (@Username, @Password, @Nickname);", conn);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Nickname", nickname);

                int n = cmd.ExecuteNonQuery();
                cmd.Dispose();
                return 1;
            }
            catch (Exception) { }

            return 0;
        }

        public String getNickname(String username)
        {
            try
            {
                conn = new SqlCeConnection(Settings.Default.AccountsConnectionString);
                conn.Open();

                cmd = new SqlCeCommand("SELECT nickname FROM [Users] WHERE Username = @Username;", conn);
                cmd.Parameters.AddWithValue("@Username", username);

                SqlCeDataReader reader = cmd.ExecuteReader();
                cmd.Dispose();

                int idNickname = reader.GetOrdinal("Nickname");

                return reader.GetString(idNickname);
            }
            catch (Exception) { }

            return null;
        }
    }
}
