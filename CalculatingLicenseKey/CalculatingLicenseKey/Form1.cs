using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatingLicenseKey
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //we właściwaościach dateTimePicker1 zmieniałem custom format na: yyyyMMddHHmmss
        private void button1_Click(object sender, EventArgs e)
        {
            string text = Convert.ToInt64(Math.Floor(HashKey(dateTimePicker1.Value.ToString("yyyyMMddHH")))).ToString();
            textBox1.Text = Reverse(text);
         }

        //public static string DescriptKey(string KEY)//metoda zamienia zakodowany klucz na datę (pierwiastkuje datę)
        //{
        //    double output = 0;
        //    output = Math.Sqrt(Double.Parse(KEY));
        //    return output.ToString();
        //}
        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }
        public double HashKey(string KEY)//metoda zamienia datę na zaszyfrowany klucz (podnosi datę do potęgi 2)
        {
            double output = 0;
            output = Math.Pow(Double.Parse(KEY), 2);
            return output;
        }
    }
}
