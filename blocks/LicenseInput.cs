using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace blocks
{
    public partial class LicenseInput : Form
    {
        public LicenseInput()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SavetoRegistry();
        }
        public void SavetoRegistry()
        {
            try
            {
                string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\" + Application.ProductName + "\\" + Application.ProductVersion;
                string keyName = "KEY";
                Microsoft.Win32.Registry.SetValue(key, keyName, textBox1.Text, Microsoft.Win32.RegistryValueKind.String);
                MessageBox.Show("Restart your application.");
            }
            catch { }
        }
    }
}
