using System;
using System.Data.SqlServerCe;
using System.Windows.Forms;
using ChatServer.Properties;
using System.Collections.Generic;

namespace ChatServer
{
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();

            PopulateAccountList();
        }

        private DbManagement db = new DbManagement();

        private void PopulateAccountList()
        {
            accountListView.Items.Clear();

            List<String[]> accounts = db.getAccounts();

            foreach (String[] account in accounts) {
                accountListView.Items.Add(account[0]).SubItems.AddRange(new string[] {
                    account[1],
                    account[2],
                    account[3]
                });
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
