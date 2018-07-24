using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUNDBBACKUP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            

            txtPassword.Text = Properties.Settings.Default.Password;
            txtServername.Text = Properties.Settings.Default.Servername;
            txtusername.Text = Properties.Settings.Default.Username;
        }

        private async void btnBackup_Click(object sender, EventArgs e)
        {
            string errorMsg = null;
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                errorMsg += "User Password is required." + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(txtServername.Text))
            {
                errorMsg += "Database Servername is required." + Environment.NewLine;
            }
            if (string.IsNullOrEmpty(txtusername.Text))
            {
                errorMsg += "Database username is required." + Environment.NewLine;
            }
            if (errorMsg != null)
            {
                MessageBox.Show("Please correct the following error(s) to continue " + Environment.NewLine + errorMsg);
            }
            if (chkSaveLogin.Checked)
            {
                Properties.Settings.Default.Password = txtPassword.Text.Trim();
                Properties.Settings.Default.Username = txtusername.Text.Trim();
                Properties.Settings.Default.Servername = txtServername.Text.Trim();
                string server = txtServername.Text.Trim();
                string password = txtPassword.Text.Trim();
                string username = txtusername.Text.Trim();

                string folderPath = txtFolderPath.Text.Trim();
                string conString = string.Format($"Server={server};User ID={username};Password={password}");
                try
                {
                    BackupService backup = new BackupService(conString, folderPath);

                    await backup.BackupAllUserDatabasesAsync();


                    MessageBox.Show("Backup ");
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Something had happened. Please check." + Environment.NewLine + ex.Message);
                }
                

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string folder = string.Empty;
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                txtFolderPath.Text = folderBrowser.SelectedPath;
            }
        }
    }
}
