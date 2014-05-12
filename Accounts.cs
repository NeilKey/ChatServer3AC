using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatServer
{
    public partial class Accounts : Form
    {
        public Accounts()
        {
            InitializeComponent();
            ListViewItem lvi = new ListViewItem();
            lvi.SubItems.Add("lol");
            lvi.SubItems.Add("Pustaj");
            lvi.SubItems.Add("GLUP");
            lvi.SubItems.Add("TI SI");

            listView1.Items.Add(lvi);
        }

        private void listClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (listView1.FocusedItem.Bounds.Contains(e.Location) == true)
                {
                    rightClickMenu.Show(Cursor.Position);
                }
            }
        }
    }
}
