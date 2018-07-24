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

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string errorMsg = string.Empty;
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                
            }
            if (chkSaveLogin.Checked)
            {
                Properties.Settings.Default.Password = txtPassword.Text.Trim();
                Properties.Settings.Default.Username = txtusername.Text.Trim();
                Properties.Settings.Default.Servername = txtServername.Text.Trim();

            }
        }
    }
}
