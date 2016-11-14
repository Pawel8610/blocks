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
using System.Xml;

namespace blocks
{
    public partial class Restore : Form
    {

        public string[] restorePoints;
        BindingSource itemsBinding = new BindingSource();//do wiązania danych na liście


        public Restore()
        {
            InitializeComponent();
      //      itemsBinding.DataSource = restorePoints;
       //     listBox1.DataSource = itemsBinding;
            listBox1.DataSource = restorePoints;
        }


       private void button1_Click(object sender, EventArgs e)
        {
            PerformRestoring(); 
        }
       public string CountNotes(string file) //method return notes quantity from particular xml file(s) 
        {
            try
            {
                FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("Note");
                fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces
                return xmlnode.Count.ToString();
            }
            catch
            {
                return "0";
            }
        }
      
       public void PerformRestoring()
       {
           try
           {
               if (listBox1.Items.Count > 0)
               {
                   if (listBox1.SelectedIndex >= 0)
                   {
                       for (int x = 0; x < listBox1.Items.Count; x++)
                       {
                           if (listBox1.GetSelected(x) == true) //jeśli zaznaczony
                           {
                               FileInfo fi = new FileInfo(restorePoints[x]);
                               string sourceFile = restorePoints[x];
                               string destinationFile = SaveXmlSettingsFile.XMLfileUsera;
                               System.IO.File.Copy(sourceFile, destinationFile, true);
                           }
                       }
                          MessageBox.Show("Your notes were sucessfully restored. To see effect click: 'Find lost notes', or just reopen this application.");
                   }
                   else { MessageBox.Show("First select point to restore and then click restore button."); }
               }
               else { MessageBox.Show("There is no restoring points."); }
         }
           catch { MessageBox.Show("There is no restoring points."); }
       
         }

        private void Restore_Load(object sender, EventArgs e)
        {
            string afterTrimming="";
            restorePoints = Directory.GetFiles(@"BackUp");
            Array.Sort(restorePoints);
            Array.Reverse(restorePoints);

            foreach (string str in restorePoints)
            {
                afterTrimming = str.Substring(7, 4) + "-" + str.Substring(11, 2) + "-" + str.Substring(13, 2) + " " + str.Substring(15, 2) + ":" + str.Substring(17, 2) + ":" + str.Substring(19, 2) + " (Notes count: " + CountNotes(str)+")";
                listBox1.Items.Add(afterTrimming);
            }
        }
    }
}
