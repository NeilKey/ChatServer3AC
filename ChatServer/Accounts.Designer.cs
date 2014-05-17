namespace ChatServer
{
    partial class Accounts
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.accountListView = new System.Windows.Forms.ListView();
            this.Nickname = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Username = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Password = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Enable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.rightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshButton = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.enableButton = new System.Windows.Forms.ToolStripMenuItem();
            this.deletedButton = new System.Windows.Forms.ToolStripMenuItem();
            this.rightClickMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountListView
            // 
            this.accountListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Nickname,
            this.Username,
            this.Password,
            this.Enable});
            this.accountListView.FullRowSelect = true;
            this.accountListView.Location = new System.Drawing.Point(12, 27);
            this.accountListView.Name = "accountListView";
            this.accountListView.Size = new System.Drawing.Size(484, 223);
            this.accountListView.TabIndex = 0;
            this.accountListView.UseCompatibleStateImageBehavior = false;
            this.accountListView.View = System.Windows.Forms.View.Details;
            this.accountListView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.listClick);
            // 
            // Nickname
            // 
            this.Nickname.Text = "Nickname";
            this.Nickname.Width = 105;
            // 
            // Username
            // 
            this.Username.Text = "Username";
            this.Username.Width = 105;
            // 
            // Password
            // 
            this.Password.Text = "Password";
            this.Password.Width = 102;
            // 
            // Enable
            // 
            this.Enable.Text = "Enable";
            this.Enable.Width = 136;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Accounts:";
            // 
            // rightClickMenu
            // 
            this.rightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshButton,
            this.toolStripSeparator1,
            this.enableButton,
            this.deletedButton});
            this.rightClickMenu.Name = "contextMenuStrip1";
            this.rightClickMenu.Size = new System.Drawing.Size(212, 76);
            // 
            // refreshButton
            // 
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(211, 22);
            this.refreshButton.Text = "Refresh";
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // enableButton
            // 
            this.enableButton.Name = "enableButton";
            this.enableButton.Size = new System.Drawing.Size(211, 22);
            this.enableButton.Text = "Enable seleced accounts";
            this.enableButton.Click += new System.EventHandler(this.enableButton_Click);
            // 
            // deletedButton
            // 
            this.deletedButton.Name = "deletedButton";
            this.deletedButton.Size = new System.Drawing.Size(211, 22);
            this.deletedButton.Text = "Deleted selected accounts";
            // 
            // Accounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(508, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accountListView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Accounts";
            this.ShowInTaskbar = false;
            this.Text = "Accounts";
            this.rightClickMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView accountListView;
        private System.Windows.Forms.ColumnHeader Nickname;
        private System.Windows.Forms.ColumnHeader Username;
        private System.Windows.Forms.ColumnHeader Password;
        private System.Windows.Forms.ColumnHeader Enable;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip rightClickMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem enableButton;
        private System.Windows.Forms.ToolStripMenuItem deletedButton;
    }
}