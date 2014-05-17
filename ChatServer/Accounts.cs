using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using ChatServer.Properties;

namespace ChatServer
{
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();

            PopulateAccountList();
        }

        private void PopulateAccountList()
        {
            accountListView.Items.Clear();

            SqlCeConnection conn = null;
            SqlCeCommand cmd = null;

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
                    accountListView.Items.Add(reader.GetString(idNickname)).SubItems.AddRange(new string[] {
                        reader.GetString(idUsername),
                        reader.GetString(idPassword),
                        (reader.GetBoolean(idEnabled)) ? "Yes" : "No" });
                }

                conn.Close();
            }
            catch (Exception e)
            {
                if (cmd != null) cmd.Dispose();
                if (conn != null) conn.Close();

                MessageBox.Show(e.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (accountListView.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    rightClickMenu.Show(Cursor.Position);
                }
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            PopulateAccountList();
        }

        private void enableButton_Click(object sender, EventArgs e)
        {
            var selectedItems = accountListView.SelectedItems;
            foreach (ListViewItem selectedItem in selectedItems)
            {
                MessageBox.Show(selectedItem.SubItems[0].Text);
                // query al database, where username = selectedItem.SubItems[0].Text --> enabled = false
                // cambiare stato su rightclickmenu
                PopulateAccountList();
            }   
        }
    }
}
