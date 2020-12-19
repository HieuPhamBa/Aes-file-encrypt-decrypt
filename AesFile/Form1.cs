using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AesFile
{
    public partial class frmAESDemo : Form
    {
        AesManger aesManger;

        public frmAESDemo()
        {
            aesManger = new AesManger();
            InitializeComponent();
        }

        private void btnBrowser_Click(object sender, EventArgs e)
        {
            OpenFileDialog od = new OpenFileDialog();
            od.Multiselect = false;
            if (od.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = od.FileName;
            }
        }

        private void rdEncrypt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdEncrypt.Checked)
            {
                rdDecypt.Checked = false;
            }
        }

        private void rdDecypt_CheckedChanged(object sender, EventArgs e)
        {
            if (rdDecypt.Checked)
            {
                rdEncrypt.Checked = false;
            }
        }

        private void btnExcute_Click(object sender, EventArgs e)
        {
            if (!File.Exists(txtFilePath.Text))
            {
                MessageBox.Show("File does not exist.");
                return;
            }

            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Password empty. Please enter your password");
                return;
            }

            // Get file content and key for encrypt/decrypt
            try
            {
                string filePath = txtFilePath.Text;
                string password = txtPassword.Text;

                String fileExt = Path.GetExtension(txtFilePath.Text);
                SaveFileDialog sd = new SaveFileDialog();

                double timeExcute = 0;
                bool actionResult = false;

                sd.Filter = "Files (*" + fileExt + ") | *" + fileExt;

                if (sd.ShowDialog() == DialogResult.OK)
                {
                    if (rdEncrypt.Checked)
                    {
                        actionResult = aesManger.Encrypt(filePath, sd.FileName, password, ref timeExcute);
                        txtTime.Text = timeExcute.ToString();
                    }
                    else
                    {
                        actionResult = aesManger.Decrypt(filePath, sd.FileName, password, ref timeExcute);
                        txtTime.Text = timeExcute.ToString();
                    }
                }

                MessageBox.Show(actionResult ? "Action completed!" : "Action false");
            }
            catch
            {
                MessageBox.Show("File is in use.Close other program is using this file and try again.");
                return;
            }
        }

    }
}
