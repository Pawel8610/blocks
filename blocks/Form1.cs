using Code7248.word_reader;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace blocks
{
    public partial class Notes : Form
    {
        public List<int> NotesToDelete = new List<int>();
        Point PanelMouseDownLocation;
        int cLeft = 1;
        const string TEMP_FOLDER = "DragDropDemo";
        private string _tempFolder;
        private NAudio.Wave.WaveFileReader wave = null;
        private NAudio.Wave.DirectSoundOut output = null;
        public bool message = true;
        public Notes()
        {
            InitializeComponent();
            CreatenInitApplicationSpecificTempFolder();
            AddingTabs();
         }
        private void Form1_Load(object sender, EventArgs e)
        {
            if (!File.Exists(SaveXmlSettingsFile.XMLfileUsera))//jeśli nie istnieje plik tego użytkownika to go stwórz
            { SaveXmlSettingsFile.SaveNote("");
            CreatingNewNote("Your note...", "New note");//aby razem z pierwszym tabem powstała karteczka
            }
            else
            {
                LoadingAllNotes();//jeśli plik istnieje stwórz z nim notatki
            }
            InitTimer();
            setCheckBox();//ustawienie checkboxa 'Disable Sound' w zależności od wartości w rejestrze
            LockApplication();//blokowanie tabów jeśli nie ma licencji
        }
        private void CreatenInitApplicationSpecificTempFolder()
        {
            try
            {
                _tempFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), TEMP_FOLDER);
                Directory.CreateDirectory(_tempFolder);
            }
            catch (Exception ee)
            {
                MessageBox.Show("Problem with creating temp folder. Check your system permissions. " + ee.ToString());
            }
        }
          private void button1_Click(object sender, EventArgs e)//create new note
        {
            CreatingNewNote("Your note...", "New note");
        }
        public void LoadingAllNotes()
        {
            try
            {
                FileStream fs = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("Note");
               if (xmlnode.Count > 0)
                {
                    //for (int i = 0; i < xmlnode.Count; i++)
                    //{
                    //    AddNewTextBox(xmlnode[i].Attributes["NoteID"].Value, xmlnode[i].Attributes["panelTOP"].Value, xmlnode[i].Attributes["panelLEFT"].Value, xmlnode[i].Attributes["panelWIDTH"].Value, xmlnode[i].Attributes["panelHEIGHT"].Value, xmlnode[i].Attributes["panelCOLOR"].Value, xmlnode[i].Attributes["panelNAME"].Value, xmlnode[i].ChildNodes.Item(0).InnerText.ToString());
                    //}
                    var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
                    var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
                        from e in xml.Descendants("Note")
                        select e.Attribute("NoteID").Value;

                    foreach (string IDs in queryNoteID) //dla każdego ID nie będącego na liście NoteToDelete
                    {
                        if (!NotesToDelete.Contains(Int32.Parse(IDs))) //jeśli danej karteczki nie ma na liście do usunięcia to ją stwórz, jest to zabezpieczenie, aby jeśli metodę wykonam w trakcie pracy programu, a wcześniej coś usunąłem to wpadło na listę Notestodelete a z xmla zostanie dopiero usunięte przy zamknięciu programu
                        {
                            XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");
                            //if (t.ChildNodes.Item(0).InnerText.ToString()!="")
                            AddNewTextBox(t.Attributes["NoteID"].Value, t.Attributes["panelTOP"].Value, t.Attributes["panelLEFT"].Value, t.Attributes["panelWIDTH"].Value, t.Attributes["panelHEIGHT"].Value, t.Attributes["panelCOLOR"].Value, t.Attributes["DATE"].Value, t.Attributes["panelNAME"].Value, t.Attributes["Tabindex"].Value, t.Attributes["Tabname"].Value,t.ChildNodes.Item(0).InnerText.ToString());
                        }
                    }
                }
                  fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces
            }
            catch (Exception e) { 
                MessageBox.Show(e.ToString()); 
            }
         }
         public void AddNewTextBox(string NoteID, string panelTop, string panelLeft, string panelWidth, string panelHeight, string panelColor, string date, string name, string TabIndex, string Tabname, string content)
         {
             try
             {
                 int NoteNumber = Int32.Parse(NoteID);
                 int TOP = Int32.Parse(panelTop);
                 int LEFT = Int32.Parse(panelLeft);
                 int WIDTH = Int32.Parse(panelWidth);
                 int HEIGHT = Int32.Parse(panelHeight);
                 int Tabindex = Int32.Parse(TabIndex);
                 
                 System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
                 // tabControl1.TabIndex = Tabindex;
                 //  tabControl1.Controls.Add(panel);
                 // tabPage1
                 //tabControl1.SelectedTab.Controls.Add(panel);
                 tabControl1.TabPages[Tabindex].Controls.Add(panel);
                 // panel.Top = cLeft * 15;//aby nowy panel tworzył się przesunięty o 15px względem poprzedniego
                 panel.Top = TOP;             //25
                 panel.Left = LEFT;           //300
                 panel.Width = WIDTH;          //200
                 panel.Height = HEIGHT;         //150

                 if (panelColor == "Yellow")
                 { panel.BackColor = System.Drawing.Color.Yellow; }
                 else if (panelColor == "Red")
                 { panel.BackColor = System.Drawing.Color.Red; }
                 else if (panelColor == "Green")
                 { panel.BackColor = System.Drawing.Color.Green; }

                 panel.BringToFront();
                 panel.AllowDrop = true;
                 panel.Text = "Panel" + this.cLeft.ToString();
                 cLeft = cLeft + 1;

                 System.Windows.Forms.RichTextBox txt = new System.Windows.Forms.RichTextBox();//zawartość karteczki
                 txt.Top = 20;
                 txt.Left = 5;
                 txt.Width = panel.Width - 10;
                 txt.Height = panel.Height - 40;
                 txt.Text = content;
                 //if (content != null)
                 // { txt.Text = content; }
                 // else { txt.Text = ""; }
                 panel.Controls.Add(txt);
                 System.Windows.Forms.TextBox txtName = new System.Windows.Forms.TextBox();//nazwa karteczki
                 txtName.Top = 0;
                 txtName.Left = 45;
                 txtName.Width = panel.Width - 100;
                 txtName.Height = 5;
                 txtName.Text = name;
                 panel.Controls.Add(txtName);
                 System.Windows.Forms.Button but = new System.Windows.Forms.Button();//close button
                 but.Width = 18;
                 but.Height = 18;
                 but.Top = 0;
                 but.Left = panel.Width - 18;
                 but.Text = "X";
                 but.BackColor = System.Drawing.Color.GreenYellow;
                 panel.Controls.Add(but);

                 System.Windows.Forms.Button butResize = new System.Windows.Forms.Button();//resize button
                 butResize.Width = 20;
                 butResize.Height = 20;
                 butResize.Top = panel.Height - 20;
                 butResize.Left = panel.Width - 20;
                 butResize.Text = ">";
                 butResize.BackColor = System.Drawing.Color.GreenYellow;
                 //butResize.AutoSize = true;
                 panel.Controls.Add(butResize);

                 System.Windows.Forms.Button Yellowpanel = new System.Windows.Forms.Button();//save color button
                 Yellowpanel.Width = 18;
                 Yellowpanel.Height = 18;
                 Yellowpanel.Top = panel.Height - 18;
                 Yellowpanel.Left = 0;
                 Yellowpanel.BackColor = System.Drawing.Color.Yellow;
                 panel.Controls.Add(Yellowpanel);
                 System.Windows.Forms.Button Redpanel = new System.Windows.Forms.Button();//save color button
                 Redpanel.Width = 18;
                 Redpanel.Height = 18;
                 Redpanel.Top = panel.Height - 18;
                 Redpanel.Left = 18;
                 Redpanel.BackColor = System.Drawing.Color.Red;
                 panel.Controls.Add(Redpanel);
                 System.Windows.Forms.Button Greenpanel = new System.Windows.Forms.Button();//save color button
                 Greenpanel.Width = 18;
                 Greenpanel.Height = 18;
                 Greenpanel.Top = panel.Height - 18;
                 Greenpanel.Left = 36;
                 Greenpanel.BackColor = System.Drawing.Color.Green;
                 panel.Controls.Add(Greenpanel);

                 System.Windows.Forms.DateTimePicker dateTimePicker1 = new System.Windows.Forms.DateTimePicker();//zawartość karteczki
                 dateTimePicker1.Format = DateTimePickerFormat.Custom;
                 dateTimePicker1.CustomFormat = "dd-MM-yyyy HH:mm:ss";
                 dateTimePicker1.Width = 136;
                 dateTimePicker1.Height = 20;
                 dateTimePicker1.Top = panel.Height - 20;
                 dateTimePicker1.Left = 58;

                 System.Windows.Forms.Button butRevoke = new System.Windows.Forms.Button();//resize button
                 butRevoke.Width = 60;
                 butRevoke.Height = 22;
                 butRevoke.Top = panel.Height - 22;
                 butRevoke.Left = dateTimePicker1.Left + dateTimePicker1.Width + 2;   //192;
                 butRevoke.Text = "Revoke";
                 // butRevoke.BackColor = System.Drawing.Color.Green;
                 panel.Controls.Add(butRevoke);

                 if (date == "")
                 {
                     dateTimePicker1.Value = DateTime.Now;
                     butRevoke.BackColor = System.Drawing.Color.Green;
                 }
                 else
                 {
                     dateTimePicker1.Value = DateTime.Parse(date);
                     butRevoke.BackColor = System.Drawing.Color.Red;
                 }
                 panel.Controls.Add(dateTimePicker1);

                 panel.MouseDown += (object sender, MouseEventArgs e1) =>
                 {
                     if (e1.Button == MouseButtons.Left)
                     {
                         PanelMouseDownLocation.X = e1.X;  //pobranie położenia panela po naciśnieciu myszą
                         PanelMouseDownLocation.Y = e1.Y;
                         panel.BringToFront();
                         txt.Focus();
                     }
                 };
                 panel.MouseMove += (object sender, MouseEventArgs e2) =>
                 {
                     if (e2.Button == MouseButtons.Left)
                     {
                         panel.Left += e2.X - PanelMouseDownLocation.X;   //zmiana położenia panelu zgodnie z ruchem myszki
                         panel.Top += e2.Y - PanelMouseDownLocation.Y;
                         if (panel.Left > this.Size.Width || panel.Top > this.Size.Height || panel.Left < -panel.Width || panel.Top < -panel.Height) //jeśli przeciągnę karteczkę poza formę
                         {
                             string datee = "";//zabezpieczenie, aby nie dodać daty przeszłej jeżeli była aktualna w momencie przeciągania karteczki
                             if (dateTimePicker1.Value <= DateTime.Now)
                                 datee = "";
                             else { datee = dateTimePicker1.Value.ToString(); }
                             CreateNotepad2(panel, NoteNumber, panel.Width, panel.Height, panelColor, txt, txtName.Text.ToString(), datee);
                             //CreateNotepad(NoteNumber, panel.Width, panel.Height, panelColor, txt.Text.ToString(), txtName.Text.ToString());
                             //panel.Dispose(); //destroing object
                         }
                     }
                     if (panel.Left < this.Size.Width && panel.Top < this.Size.Height && panel.Left > -panel.Width && panel.Top > -panel.Height) //metoda zapisu pozycji ma działać tylko w obszarze formy ponieważ jeżeli przeciągnę karteczkę poza forma, a okaże się, że taki plik istnieje, a ja kliknę że nie chcę nadpisać zawartości, to metoda ta zapisze w xml pozycję poza formą, a więc notatka stanie się niewidoczna
                     {
                         SavingtoXmlNotePosition(NoteNumber, panel.Top, panel.Left, panel.Width, panel.Height);
                     }
                 };
                 but.Click += (object sender, EventArgs e3) =>
                 {
                     int countCB = 0;
                     foreach (Control c in tabControl1.SelectedTab.Controls) //liczę ilość kontrolek (paneli - karteczek) na aktualnym tabie
                     {
                        // if (tabControl1.GetType() == typeof(Panel))
                      //   {
                             countCB++;
                        // }
                     }
                     if (MessageBox.Show("Do you really want to delete this note?", "Deleting", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                     {
                         if (countCB <= 1 && tabControl1.TabCount <= 1)//jeśli jest więcej niż 1 tab i ta karteczka nie jest ostatnia, to pozwól na jej usunięcie
                         {
                             MessageBox.Show("You cannot delete last notes from last tab.");
                         }
                         else {
                             
                             NotesToDelete.Add(NoteNumber);//dadaje do listy elementy do usunięcia
                             panel.Visible = false;
                             panel.Dispose(); //destroing object
                             CheckingTabIndexes();//jeżeli usunę ostatnią karteczkę z taba to usuń taba
                                }
                     }
                 };
                 Yellowpanel.Click += (object sender, EventArgs esave) =>
                 {
                     SavingtoXmlNoteColorNote(NoteNumber, "Yellow");//zapis do xml
                     panel.BackColor = System.Drawing.Color.Yellow;//dynamicczna zmiana koloru panelu
                     panel.Refresh();
                     panelColor = "Yellow"; //po evencie dodatkowo update zmiennej, która potem będzie potrzebna przy wywołaniu metody CreateNotepad i ewentualnym cofnięciu i tworzeniu nowej karteczki (jeśli przed przeciągnięciem karteczki zmienię jej kolor i anuluję, aby qróciła z tym samym kolorem)
                 };
                 Redpanel.Click += (object sender, EventArgs esave) =>
                 {
                     SavingtoXmlNoteColorNote(NoteNumber, "Red");
                     panel.BackColor = System.Drawing.Color.Red;
                     panel.Refresh();
                     panelColor = "Red";
                 };
                 Greenpanel.Click += (object sender, EventArgs esave) =>
                 {
                     SavingtoXmlNoteColorNote(NoteNumber, "Green");
                     panel.BackColor = System.Drawing.Color.Green;
                     panel.Refresh();
                     panelColor = "Green";
                 };
                 butRevoke.Click += (object sender, EventArgs esave) =>
                 {
                     string datee = dateTimePicker1.Value.ToString();
                     DateTime Start = DateTime.Parse(datee);
                     //   MessageBox.Show(dateTimePicker1.Value.ToString("dd-MM-yyyy HH:mm"));
                     if (Start <= DateTime.Now)
                     {
                         MessageBox.Show("You have selected a past date.");
                         SavingDATEtoXmlNote(NoteNumber, "");
                         butRevoke.BackColor = System.Drawing.Color.Green;
                     }
                     else
                     {
                         SavingDATEtoXmlNote(NoteNumber, datee);
                         butRevoke.BackColor = System.Drawing.Color.Red;
                     } //jeśli jest to data przyszła to ją zapisz do atrybutu DATA
                     //MessageBox.Show(DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
                 };
                 butResize.MouseMove += (object sender, MouseEventArgs e4) =>
                 {
                     if (e4.Button == MouseButtons.Left)
                     {
                         panel.Height = butResize.Top + e4.Y;
                         panel.Width = butResize.Left + e4.X;
                         if (panel.Height < 50) //jeśli będę próbował zmniejszyć panel poniżej 50px
                         {
                             panel.Height = 50;    //to ustaw go na 50 (zabezpieczenie przed ustawieniem zbyt małego panelu)
                         }
                         if (panel.Width < 50)
                         {
                             panel.Width = 50;
                         }
                         //dla utrzymnia proporcji przy resizingu:
                         txt.Width = panel.Width - 10;
                         txt.Height = panel.Height - 40;
                         txtName.Width = panel.Width - 100;
                         but.Top = 0;
                         but.Left = panel.Width - 18;
                         butResize.Top = panel.Height - 20;
                         butResize.Left = panel.Width - 20;
                         Yellowpanel.Top = panel.Height - 18;
                         Yellowpanel.Left = 0;
                         Redpanel.Top = panel.Height - 18;
                         Redpanel.Left = 18;
                         Greenpanel.Top = panel.Height - 18;
                         Greenpanel.Left = 36;
                         dateTimePicker1.Top = panel.Height - 20;
                         dateTimePicker1.Left = 58;
                         butRevoke.Top = panel.Height - 22;
                         butRevoke.Left = dateTimePicker1.Left + dateTimePicker1.Width + 2;   //192;
                     }
                     SavingtoXmlNotePosition(NoteNumber, panel.Top, panel.Left, panel.Width, panel.Height);//zapis pozycji do pliku xmla
                 };
                 txt.SelectionChanged += (object sender, System.EventArgs ed) =>
                 {
                     EditNote(NoteNumber, txt.Text);  //event automatycznie zapisuje zmiany w notatkach do pliku xml
                 };
                 txtName.TextChanged += (object sender, System.EventArgs edd) =>
                 {
                     SavingNoteName(NoteNumber, txtName.Text); //event automatycznie zapisuje zmiany nazw notatek do pliku xml (nagłówek notatki)
                 };
                 txt.LinkClicked += (object sender, LinkClickedEventArgs ej) =>    //włączenie obsługi hiperłącz w notatce
                 {
                     System.Diagnostics.Process.Start(ej.LinkText);
                 };
                 txt.MouseDown += (object sender, MouseEventArgs e5) =>    //menu kontekstowe
                 {
                     if (e5.Button == MouseButtons.Right)
                     {
                         System.Windows.Forms.ContextMenu menu = new System.Windows.Forms.ContextMenu();

                         MenuItem menuItem = new MenuItem("Cut");
                         menuItem.Click += (object senderr, EventArgs ee) =>
                             {
                                 if (txt.SelectionLength > 0)
                                 {
                                     txt.Cut();
                                 }
                             };
                         menu.MenuItems.Add(menuItem);
                         menuItem = new MenuItem("Copy");
                         menuItem.Click += (object senderrr, EventArgs eee) =>
                         {
                             if (txt.SelectionLength > 0)
                             {
                                 Clipboard.SetText(txt.SelectedText);
                             }
                         };
                         menu.MenuItems.Add(menuItem);
                         menuItem = new MenuItem("Paste");
                         menuItem.Click += (object senderrrr, EventArgs eeee) =>
                         {
                             //if (Clipboard.ContainsText(TextDataFormat.Rtf))
                             //{
                             //    txt.SelectedRtf= Clipboard.GetData(DataFormats.Rtf).ToString();
                             //}
                             if (Clipboard.ContainsText())
                             {
                                 txt.Text += Clipboard.GetText(TextDataFormat.Text).ToString();
                             }
                         };
                         menu.MenuItems.Add(menuItem);
                         txt.ContextMenu = menu;
                         //if (txt.SelectionLength > 0)
                         //{
                         //    DataObject dto = new DataObject();
                         //    //  dto.SetText(txt.SelectedRtf, TextDataFormat.Rtf);
                         //    dto.SetText(txt.SelectedText, TextDataFormat.UnicodeText);
                         //    Clipboard.Clear();
                         //    Clipboard.SetDataObject(dto);
                         //}
                     }
                 };
             }
             catch (Exception ee)
             {
                 MessageBox.Show("Problem with adding note. " + ee.ToString());
             }
         }
            private void CreateNotepad(int NoteID, int panelWidth, int panelHeight, string panelcolor, string content, string NameOfnote, string date)//do tej metody pobieram tyle danych, bo nie mogę odświeżyć panela, muszę potem od nowa stworzyć jeśli przeciągnę go poza formę a kliknę nie nadpisuj w przypadku intnienia pliku o takiej samej nazwie
        {
            try
            {
                int Tabindex = tabControl1.SelectedIndex;
                string Tabname = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
                string Name="";
                if (File.Exists(@"Notes\"+NameOfnote.ToString()+".txt")) //sprawdzenie czy o takiej nazwie plik txt już istnieni w folderze Notes
                {
                    DialogResult dialogResult = MessageBox.Show("File with this name exist. Please rename your note or leave blank to auto generate serial number. Or click Yes if you want to overwrite existing file.", "Warning!!!", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)   //jeśli nadpisać zawartość istniejącego pliku
                    {
                        Name = NameOfnote.ToString() + ".txt";
                        NotesToDelete.Add(NoteID);    //usuwanie karteczki z pliku xmla (dodanie jej na listę z elementami do usunięcia)
                        TextWriter sw = new StreamWriter(@"Notes\" + Name);
                        sw.Write(content);
                        sw.Close();
                        Process notePad = new Process();  //otwarcie zapisanego pliku txt
                        notePad.StartInfo.FileName = "notepad.exe";
                        notePad.StartInfo.Arguments = @"Notes\" + Name;
                        notePad.Start();
                     }
                    else if (dialogResult == DialogResult.No) //jeśli nie nadpisać, to muszę ponownie notatkę pokazać w formie
                    {     //karteczka po przeciągnięciu została zniszczona, więc muszę ją stworzyć ponownie (przed zniszczeniem przekazałem jej atrybuty do tej metody)
                        AddNewTextBox(NoteID.ToString(), "25", "300", panelWidth.ToString(), panelHeight.ToString(), panelcolor, date, NameOfnote, Tabindex.ToString(), Tabname, content); //karteczkę tworzę w losowym miejscy formy
                    }
                }
                else
                {
                    if (NameOfnote == "") //jeśli karteczka nie ma nazwy, to zapodaj nazwę losową
                    {
                        Random randomname = new Random();
                        Name = "Note" + randomname.Next(0, 100000).ToString() + ".txt";
                    }
                    else { Name = NameOfnote.ToString() + ".txt"; }
                    NotesToDelete.Add(NoteID);

                    TextWriter sw = new StreamWriter(@"Notes\" + Name);
                    sw.Write(content);
                    sw.Close();
                    Process notePad = new Process();
                    notePad.StartInfo.FileName = "notepad.exe";
                    notePad.StartInfo.Arguments = @"Notes\" + Name;
                    notePad.Start();
                }
             }
            catch {MessageBox.Show("Problem with creating note. Please try again.");
            }
        }
        public void DeleteNote() //metoda usuwa wszystkie karteczki, których ID jest na liście: NotesToDelete
 {
     try
     {
         foreach (int Notess in NotesToDelete)
         {
             XmlDocument doc = new XmlDocument();
             doc.Load(SaveXmlSettingsFile.XMLfileUsera);
             XmlNode t = doc.SelectSingleNode("notes/Note[@NoteID='" + Notess + "']");
             t.ParentNode.RemoveChild(t);
             doc.Save(SaveXmlSettingsFile.XMLfileUsera);
         }
     }
     catch { }
 }
        private void CreateNotepad2(Panel panel, int NoteID, int panelWidth, int panelHeight, string panelcolor, RichTextBox content, string NameOfnote, string date)//do tej metody pobieram tyle danych, bo nie mogę odświeżyć panela, muszę potem od nowa stworzyć jeśli przeciągnę go poza formę a kliknę nie nadpisuj w przypadku intnienia pliku o takiej samej nazwie
        {
            try
            {
                int Tabindex = tabControl1.SelectedIndex;
                string Tabname = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
                string Name = "";
                                         
            if (NameOfnote == "") //jeśli karteczka nie ma nazwy, to zapodaj nazwę losową
            {
                  Random randomname = new Random();
                  Name = "Note" + randomname.Next(0, 100000).ToString() + ".txt";
            }
            else { Name = NameOfnote.ToString() + ".txt"; }

               string fullPath = System.IO.Path.Combine(_tempFolder, Name);
               TextWriter sw = new StreamWriter(fullPath);
             
               string[] str = content.Text.Split('\n');     //każdą linijkę wrzucam do tablicy str jako osobny element aby przy zapisie do pliku była taka sama struktura tekstu
               //for (int i = 0; i < str.Length; i++) //alternatywna pętla
              // {sw.WriteLine(str[i].ToString());}
                   foreach (string cont in str)
                   { sw.WriteLine(cont); } //zapisuję każdą linijkę osobno dlatego WriteLine zamiast Write
               
               sw.Close();
               DataObject dragObj = new DataObject();
               dragObj.SetFileDropList(new System.Collections.Specialized.StringCollection() { fullPath });
               this.DoDragDrop(dragObj, DragDropEffects.Copy);
               //Process notePad = new Process();  //otwarcie zapisanego pliku txt
               //notePad.StartInfo.FileName = "notepad.exe";
               //notePad.StartInfo.Arguments = fullPath;
               //notePad.Start();
               string content2 = content.Text.ToString(); //przed skasowaniem panelu, który zawiera RichTekstBox pobieram jego zawartość do zmiennej, aby potem ją przekazać do metody AddNewTextBox
               panel.Dispose(); 

                 DialogResult dialogResult = MessageBox.Show("Would you like to permanently delete your 'yellow card' from app?.", "Warning!!!", MessageBoxButtons.YesNo);
                 if (dialogResult == DialogResult.Yes)  
                 {
                    int countCB = 0;
                    foreach (Control c in tabControl1.SelectedTab.Controls) //liczę ilość kontrolek (paneli - karteczek) na aktualnym tabie
                    {
                        // if (tabControl1.GetType() == typeof(Panel))
                        //   {
                        countCB++;
                        // }
                    }
                      if (countCB <= 0 && tabControl1.TabCount <= 1)//jeśli jest więcej niż 1 tab i ta karteczka nie jest ostatnia, to pozwól na jej usunięcie, tu akutat countCB <= 0 a nie countCB <= 1 ponieważ karteczka w momencie przeciągnięcia na pulpit znika z taba
                    {
                        MessageBox.Show("You cannot delete last notes from last tab.");   //powrót karteczki na taba
                        AddNewTextBox(NoteID.ToString(), "25", "300", panelWidth.ToString(), panelHeight.ToString(), panelcolor, date, NameOfnote, Tabindex.ToString(), Tabname, content2);
                        }
                        else
                        {
                            NotesToDelete.Add(NoteID);
                            CheckingTabIndexes();//jeżeli usunę ostatnią karteczkę z taba to usuń taba
                        }
                   }
                 else
                 {  //powrót karteczki na taba
                     AddNewTextBox(NoteID.ToString(), "25", "300", panelWidth.ToString(), panelHeight.ToString(), panelcolor, date, NameOfnote, Tabindex.ToString(), Tabname, content2);
                 }
               File.Delete(fullPath); //czyszczenie folderu temp
            }
            catch
            {
                MessageBox.Show("Problem with creating note. Please try again.");
            }
        }
 public void SavingtoXmlNotePosition(int NoteID, int panelTop, int panelLeft, int panelWidth, int panelHeight) //zapis pozycji  i wymiarów karteczki jako atrybuty węzła pliku xml
 {
     try
     {
           //int Tabindex = tabControl1.SelectedIndex;
         XmlDocument xmldoc = new XmlDocument();
         xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
         XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + NoteID + "']");
         t.Attributes[1].Value = panelTop.ToString();
         t.Attributes[2].Value = panelLeft.ToString();
         t.Attributes[3].Value = panelWidth.ToString();
         t.Attributes[4].Value = panelHeight.ToString();
         xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
     }
     catch { MessageBox.Show("Problem with saving to xml note position."); }
 }
 public void SavingtoXmlNoteColorNote(int NoteID, string panelColor) //zapis koloru karteczki
 {
     try
     {
         XmlDocument xmldoc = new XmlDocument();
         xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
         XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + NoteID + "']");
         t.Attributes[5].Value = panelColor.ToString();
         xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
     }
     catch { MessageBox.Show("Problem with saving to xml note color."); }
 }
 public void SavingDATEtoXmlNote(int NoteID, string date) //zapis daty karteczki
 {
     try
     {
         XmlDocument xmldoc = new XmlDocument();
         xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
         XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + NoteID + "']");
         t.Attributes[7].Value = date.ToString();
         xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
     }
     catch { MessageBox.Show("Problem with saving date to xml."); }
 }
 public void FindNotesToRevoke()
 {
     try
     {
         var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
         var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
             from e in xml.Descendants("Note")
             select e.Attribute("NoteID").Value;

            foreach (string IDs in queryNoteID)
           {
             XmlDocument xmldoc = new XmlDocument();
             xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
             XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");
                    if (t.Attributes[7].Value != "")
                    {
                        DateTime RevokeDate = DateTime.Parse(t.Attributes[7].Value);//parsuje stringa na datę
                        if (RevokeDate <= DateTime.Now)
                        {
                            if (!checkBox1DisableSound.Checked)
                            { PlayNotification(); }
                            if (message == true)
                            {
                                message = false;
                                MessageBox.Show("You have revoking note with name: " + t.Attributes[6].Value);
                                SavingDATEtoXmlNote(Int32.Parse(IDs), ""); //po kliknięciu ok skasowanie daty z pliku xml
                                message = true;
                            }
                        }
                    }
                      
                }
  }
     catch (Exception ex)
     {
         MessageBox.Show(ex.ToString());
     }
 }
 public void EditNote(int NoteID, string txt) //zmiana zawartości karteczki
 {
     try
     {
             //  FileStream fs = new FileStream("note1.xml", FileMode.Open, FileAccess.Read);
             XmlDocument xmldoc = new XmlDocument();
             xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
             XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + NoteID + "']");
             if (txt!=""&&txt.TrimStart()!="")  //jeśli notatka nie jest pusta i nie ma samych spacji
             {
                 t.ChildNodes.Item(0).InnerText = txt;  //to ją updatuj
             }
             else { t.ChildNodes.Item(0).InnerText = "Empty Note..."; } //w przeciwnym razie ustaw jej zawartość "Empty Note..."
             xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
     }
     catch { MessageBox.Show("Problem with editing note."); }
 }
 public void SavingNoteName(int NoteID, string txtName) //zmiana nazwy karteczki
 {
     try
     {
         XmlDocument xmldoc = new XmlDocument();
         xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
         XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + NoteID + "']");
         t.Attributes[6].Value = txtName.ToString();
         xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
     }
     catch { MessageBox.Show("Problem with saving note name."); }
 }
 public void CreatingNewNote(string contentOfNote, string NameofNote)
 {
      // XDocument doc = XDocument.Load("note1.xml");
     // XElement root = new XElement("Note");
     //root.Add(new XAttribute("name", "name goes here"));
     // root.Add(new XElement("Note", "Note"));
     // doc.Element("notes").Add(root);
     // doc.Save("note1.xml");
     try
     {
         int Tabindex = tabControl1.SelectedIndex;
         string Tabname = tabControl1.TabPages[tabControl1.SelectedIndex].Text;
                int maxId;
         int NoteCount;
         string date = "";//default value null
         FileStream fss = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read);
         XmlDocument xmldoc3 = new XmlDocument();
         xmldoc3.Load(fss);
         XmlNodeList xmlnode2;
         xmlnode2 = xmldoc3.GetElementsByTagName("Note");
         NoteCount = xmlnode2.Count;
         fss.Dispose();

         XDocument doc = XDocument.Load(SaveXmlSettingsFile.XMLfileUsera);
         if (NoteCount > 0)
         {
             maxId = doc.Descendants("Note").Max(e => (int)e.Attribute("NoteID"));
         }
         else { maxId = -1; } //aby po +1 pierwsze NoteID było =0
         int NewID = maxId + 1;
         XElement root = new XElement("Note", contentOfNote);
         root.Add(new XAttribute("NoteID", NewID));
         root.Add(new XAttribute("panelTOP", 25));
         root.Add(new XAttribute("panelLEFT", 300));
         root.Add(new XAttribute("panelWIDTH", 255));
         root.Add(new XAttribute("panelHEIGHT", 150));
         root.Add(new XAttribute("panelCOLOR", "Yellow"));
         root.Add(new XAttribute("panelNAME", NameofNote+NewID));
         root.Add(new XAttribute("DATE", date));
         root.Add(new XAttribute("Tabindex", Tabindex));
         root.Add(new XAttribute("Tabname", Tabname));
         doc.Element("notes").Add(root);
         doc.Save(SaveXmlSettingsFile.XMLfileUsera);
         AddNewTextBox(NewID.ToString(), "25", "300", "255", "150", "Yellow","",NameofNote+NewID, Tabindex.ToString(), Tabname, contentOfNote);//defaultowe wartości wymiarów, położenia i koloru
     }
     catch (Exception ex)
     {
         MessageBox.Show("Problem with CreatingNewNote method " + ex.ToString());
     }
 }
        public int TabCount()
        {
          try
            {
                XDocument doc = XDocument.Load(SaveXmlSettingsFile.XMLfileUsera);
                int max = doc.Descendants("Note").Select(e => (int)e.Attribute("Tabindex")).Distinct().Count();
                return max;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public void AddingTabs() {
            try
            {
                int max = TabCount();
                for (int i = 0; i <= max - 1; i++)
                {
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
                    XmlNode t = xmldoc.SelectSingleNode("notes/Note[@Tabindex='" + i.ToString() + "']");
                    tabControl1.TabPages.Add(t.Attributes[9].Value);
                }
                if (max == 0)
                {
                    NewTab();
                    //tabControl1.TabPages.Add("Nowy0");
                }
            }
            catch { }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
 {
     //DialogResult result = MessageBox.Show("Would you like to exit?", "Exiting", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Stop);
     //if (result == DialogResult.Yes)
     //{
     //  DeleteNote();
     //}
     //else if (result == DialogResult.Cancel)
     //{
     //    // Stop the closing and return to the form
     //    e.Cancel = true;
     //}
     //else if (result == DialogResult.No)
     //{
     //    // this.Close();
     //    DeleteNote();
     //    e.Cancel = false;
     // }
     DeleteNote();
     performBackUp();//zrobienie kopii zapasowej
     SavetoRegistry();//zapisanie do rejestru ustawień
            //       CheckingTabIndexes();
        }
       public void RemovingAllPanels()
        {
                 for (int x = tabControl1.TabPages.Count - 1; x >= 0; x--)
         //      if (this.Controls[x] is Panel) this.Controls[x].Dispose();
                      tabControl1.TabPages.RemoveAt(x);
        }

 public void FindLostNotes()//and refreshing
 {
     try
     {
       var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
           var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
             from e in xml.Descendants("Note")
             select e.Attribute("NoteID").Value;

                //var childType = themes.Descendants("theme")
                //                         .Where(X => X.Attribute("name").Value == "Agile")
                //                         .Where(X => X.Descendants("root").Attributes("type").First().Value == "Project")
                //                         .Select(X => X.Descendants("node").Attributes("type").First().Value);

                foreach (string IDs in queryNoteID)
            //foreach (string IDs in queryNoteID.Where(b => !b.Equals(NotesToDelete))) //dal każdego ID nie będącego na liście NoteToDelete
         {
             XmlDocument xmldoc = new XmlDocument();
             xmldoc.Load(SaveXmlSettingsFile.XMLfileUsera);
             XmlNode t = xmldoc.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");
             int panelTop=Int32.Parse(t.Attributes[1].Value);
             int panelLeft=Int32.Parse(t.Attributes[2].Value);
             int panelWidth=Int32.Parse(t.Attributes[3].Value);
             int panelHeight = Int32.Parse(t.Attributes[4].Value);

             if (panelLeft > this.Size.Width-50 || panelTop > this.Size.Height-50 || panelLeft < -panelWidth+50 || panelTop < -panelHeight+50) //jeśłi obrzerza karteczki znajdują się około 50px od krawędzi formy, to przesuń je do pozycji 15;300
             {
                 t.Attributes[1].Value = "25";
                 t.Attributes[2].Value = "300";
                xmldoc.Save(SaveXmlSettingsFile.XMLfileUsera);
             }
         }
         RemovingAllPanels();
         this.Refresh();
         AddingTabs();
         LoadingAllNotes();
      }
     catch (Exception ex)
     {
         //MessageBox.Show(ex.ToString());
     }
 }
      
 private void Notes_DragDrop(object sender, DragEventArgs e)
 {
     try
     {
         string name = null;
         string content = null;
         string[] droppedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);//tablica przechowuje ścieżki wszystkich przeciągniętych plików

         foreach (string filepaths in droppedFiles)
         {
             name = getFileName(filepaths);
             content = getFileContent(filepaths);

             if (content != "" && content != "UnsupportedFileToDeleteeeeee" && content.TrimStart() != "")   //dla każdego przeciągniętego pliku, który jest obsługiwany stwórz karteczkę (nie może mieć też samych spacji)
             {
                 // MessageBox.Show("r"+content.ToString()+"r");//tak sprawdziłem, że content zwraca np. wiele spacji
                 CreatingNewNote(content, name);
             }
             else if (content == "UnsupportedFileToDeleteeeeee") //jeśli jest ineobsługiwany nie rób nic
             { }
             else if (content == "" || content.TrimStart() == "") //jeśli przeciągnę plik docx, który jest pusty, to niestety metoda extractor.ExtractText() zwraca kilka spacji, więc muszę przyciać je w warunku (jeżeli są same spacje)
                 CreatingNewNote("Empty note...", name); //przypadek kiedy plik jest obsługiwany, ale pusty lub ma same spacje
         }
     }
     catch { }
 }

 private void Notes_DragEnter(object sender, DragEventArgs e)
 {
     if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
     {
         e.Effect = DragDropEffects.All;
     }
 }
        public static string StripInvalidXmlCharacters(string str) //usuwanie ewentualnych wartości szestnastkowych i znaków
        {
            var invalidXmlCharactersRegex = new Regex("[^\u0009\u000a\u000d\u0020-\ud7ff\ue000-\ufffd]|([\ud800-\udbff](?![\udc00-\udfff]))|((?<![\ud800-\udbff])[\udc00-\udfff])");
            return invalidXmlCharactersRegex.Replace(str, "");
        }
 public string getFileContent(string path)
 {
     try
     {
         long length = new System.IO.FileInfo(path).Length; //pobranie info o rozmiarze pliku (w bajtach)
           if (length > 100000000)     //100MB
         {
             MessageBox.Show("Size file exceed 100MB.");
             return "UnsupportedFileToDeleteeeeee";
         }
           else if (System.IO.Path.GetExtension(path).Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
         {
             using (var streamReader = new StreamReader(path, Encoding.Default))
             {
                 return streamReader.ReadToEnd().ToString();
             }
         }
         else if (System.IO.Path.GetExtension(path).Equals(".docx", StringComparison.InvariantCultureIgnoreCase) || System.IO.Path.GetExtension(path).Equals(".doc", StringComparison.InvariantCultureIgnoreCase))
         {
             Code7248.word_reader.TextExtractor extractor = new TextExtractor(path);
                    
             return StripInvalidXmlCharacters(extractor.ExtractText().ToString());
         }
         else
         {
             MessageBox.Show("File: \"" + System.IO.Path.GetFileName(path).ToString() + "\" not supported. Only txt, doc and docx files are accepted.");
             return "UnsupportedFileToDeleteeeeee";//jeśli plik jest nieobsługiwany to metoda zwraca jakąś wartość, aby potem to sprawdzić. Najlepiej aby to była wartość, która jest mało prawdopodobna, że wystąpi w zawartości pliku
         }
     }
     catch {
        // MessageBox.Show("File: \"" + getFileName(path).ToString() + "\" not supported. Only txt, doc and docx files are accepted.");
     return "";
     }
 }
 public string getFileName(string path)
 {
     try
     {
         return System.IO.Path.GetFileNameWithoutExtension(path);
     }
     catch { return null; }
 }

 private void button1Find_Click(object sender, EventArgs e)
 {
     FindLostNotes();//refreshing
 }

 private void aboutMeToolStripMenuItem_Click(object sender, EventArgs e)
 {
     MessageBox.Show("Author: Paweł Andrzejczyk\npm8610@gmail.com");
 }

 private void readMeToolStripMenuItem_Click(object sender, EventArgs e)
 {
     try
     {
         Process notePad = new Process();
         notePad.StartInfo.FileName = "notepad.exe";
         notePad.StartInfo.Arguments = "ReadMe.txt";
         notePad.Start();
     }
     catch (Exception ee)
     {
         MessageBox.Show("Problem with opening file ReadMe.txt Plesase ensure thant file exist in main folder and retry. " + ee.ToString());
     }
 }
 public void performBackUp()
 {
     try
     {
         string sourcePath = @"";
         string destinationPath = @"BackUp";
         string sourceFileName = SaveXmlSettingsFile.XMLfileUsera;
         string destinationFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + SaveXmlSettingsFile.XMLfileUsera; // yyyyMMddHHmmss + note + IDUsera + .xml    ("yyyyMMddhhmmss" dla formatu godzinnego 12h) 
         string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
         string destinationFile = System.IO.Path.Combine(destinationPath, destinationFileName);

         if (!System.IO.Directory.Exists(destinationPath))
         {
             System.IO.Directory.CreateDirectory(destinationPath);
         }
         System.IO.File.Copy(sourceFile, destinationFile, true);
     }
     catch (Exception ee)
     {
         MessageBox.Show("Problem with performing backup. " + ee.ToString());
     }
 }
 public void RemoveOldBackUps()
 {
     try
     {
         string[] files = Directory.GetFiles(@"BackUp");
         Array.Sort(files);
         Array.Reverse(files);
         //files.Reverse();
         //foreach (string file in files)
         //{
         //    FileInfo fi = new FileInfo(file);
         //    if (fi.LastAccessTime < DateTime.Now.AddMonths(-3))  //usuwanie plików starszych niż 3 miesiące
         //        fi.Delete();
         //}

         //   if (files.Length > 0)
         //   {
         //       for (int i = 0; i < files.Length; i++)
         //       {

         //           FileInfo fi = new FileInfo(files[i]);
         //           if (fi.CreationTime.Date < DateTime.Now.AddMinutes(-1))
         //           {
         //                fi.Delete();
         //           }
         //       }
         //}

         if (files.Length > 0)   //jeśli coś jest w tablicy
         {
             for (int i = files.Length; i > 1; i--)
             {
                 FileInfo fi = new FileInfo(files[i - 1]);
                 fi.Delete(); //to usuń wszystko oprócz osatniego elementu (najnowszy, bo największy - tablic jest posortowana od największej wartości do najmniejszej)
             }
         }
     }
     catch (Exception ee)
     {
         MessageBox.Show("Problem with deleting old backups. " + ee.ToString());
     }
}
 
 private void performBackupToolStripMenuItem_Click(object sender, EventArgs e)
 {
     performBackUp();
 }

 private void deleteOldBackupsToolStripMenuItem_Click(object sender, EventArgs e)
 {
     RemoveOldBackUps();
 }

 private void openRestoringPointToolStripMenuItem_Click(object sender, EventArgs e)
 {
       performBackUp();//jeżeli w danej sesji coś stworzyłem, to aby nic nie stracić wykonuję backup w momencie otwarcia narzędzia
       Restore rest = new Restore();
        rest.ShowDialog();
 }
 //private Timer timer1;
 public void InitTimer()//methon run timer, that run proper method cyclic
 {
     timer1 = new Timer();
     timer1.Tick += new EventHandler(timer1_Tick);
     timer1.Interval = 5000; // in miliseconds
     timer1.Start();
 }
 private void timer1_Tick(object sender, EventArgs e)
 {
     FindNotesToRevoke();
 }
 public void PlayNotification()
 {
     try
     {
         DisposeWave();
         wave = new NAudio.Wave.WaveFileReader("notify.wav");
         output = new NAudio.Wave.DirectSoundOut();
         output.Init(new NAudio.Wave.WaveChannel32(wave));
         output.Play();
     }
     catch (Exception ee)
     {
         MessageBox.Show("Problem with playing notification sound. Please ensure that file notify.wav exist in main folder. " + ee.ToString());
     }
 }
 private void DisposeWave()
 {
            try
            {
                if (output != null)
                {
                    if (output.PlaybackState == NAudio.Wave.PlaybackState.Playing) output.Stop();
                    output.Dispose();
                    output = null;
                }
                if (wave != null)
                {
                    wave.Dispose();
                    wave = null;
                }
            }
            catch { }
 }
        public void NewTab() //adding new tab
        {
            try
            {
                string name = tabControl1.TabCount.ToString();
                if (textBox1.Text != "")
                { tabControl1.TabPages.Add(textBox1.Text); 
                tabControl1.SelectedIndex = Int32.Parse(name);//focus na przed chwilą stworzonego taba
                }
                else
                {
                    tabControl1.TabPages.Add("Nowy" + name);
                    textBox1.Text = "";
                    tabControl1.SelectedIndex = Int32.Parse(name);//focus na przed chwilą stworzonego taba
                }
             }
            catch (Exception ee)
            {
                MessageBox.Show("Problem with adding new tab. " + ee.ToString());
            }
        }
        private void button1_Click_1(object sender, EventArgs e) //dodawanie taba
        {
            NewTab();
            CreatingNewNote("Your note...", "New note");
        }
        public void RemoveTab()
        {
            try
            {
                FileStream fs = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("Note");
                DialogResult dialogResult = MessageBox.Show("Would you really like to permanently delete this tab with notes?.", "Warning!!!", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (xmlnode.Count > 0)
                    {
                        fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces

                        //var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
                        //var queryTabindex =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
                        //from k in xml.Descendants("Note")
                        //select k.Attribute("Tabindex").Value;
                        //        foreach (string index in queryTabindex) 
                        //{
                        //     if (Int32.Parse(index)== tabControl1.SelectedIndex)
                        //    {
                        //        XmlDocument xmldoc2 = new XmlDocument();
                        //        xmldoc2.Load(SaveXmlSettingsFile.XMLfileUsera);
                        //        XmlNode tt = xmldoc2.SelectSingleNode("notes/Note[@Tabindex='" + index + "']");
                        //       // NotesToDelete.Add(Int32.Parse(tt.Attributes[0].Value));
                        //            MessageBox.Show(tt.Attributes[0].Value);
                        //           // xmldoc2.Save(SaveXmlSettingsFile.XMLfileUsera);
                        //        }

                        if (tabControl1.TabCount == 1)
                        { MessageBox.Show("You cannot delete last tab."); }
                        else
                        {
                            var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
                            var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
                            from k in xml.Descendants("Note")
                            select k.Attribute("NoteID").Value;
                            foreach (string IDs in queryNoteID)
                            {
                                XmlDocument xmldoc2 = new XmlDocument();
                                xmldoc2.Load(SaveXmlSettingsFile.XMLfileUsera);
                                XmlNode tt = xmldoc2.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");

                                if ((Int32.Parse(tt.Attributes[8].Value) == tabControl1.SelectedIndex) && (!NotesToDelete.Contains(Int32.Parse(IDs))))//jeżeli karteczka jest na aktualnym tabie oraz nie jest na liście do usunięcia (aby uniknąć błędów)
                                {
                                   NotesToDelete.Add(Int32.Parse(tt.Attributes[0].Value));
                                }
                                if ((Int32.Parse(tt.Attributes[8].Value) > tabControl1.SelectedIndex) && (!NotesToDelete.Contains(Int32.Parse(IDs))))//jeżeli karteczka jest na następnym tabie oraz nie jest na liście do usunięcia, to przesuń ją na tab poprzedni (indexy tabów muszą być kompletne 0-n)
                                {
                                    tt.Attributes[8].Value = (Int32.Parse(tt.Attributes[8].Value) - 1).ToString();
                                    xmldoc2.Save(SaveXmlSettingsFile.XMLfileUsera);
                                }
                            }
                             tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);//usunięcie taba
                        }
                      }
                    //FindLostNotes();
                }
                fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces
            }
            catch (Exception ee)
            {
                MessageBox.Show("Problem with deleting tab. " + ee.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)  //usuwanie taba
        {
            RemoveTab();
        }
        public void CheckingTabIndexes()//przy usuwaniu pojedyńczej karteczki jeżeli jest ona ostatnia, to usuń taba i przesuń karteczki z następntch tabów -1
        {
            try
            {
                FileStream fs = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("Note");
                
                    if (xmlnode.Count > 0)
                    {
                        fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces

                        var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
                        var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
                        from k in xml.Descendants("Note")
                        select k.Attribute("NoteID").Value;

                        foreach (string IDs in queryNoteID)
                        {
                            XmlDocument xmldoc2 = new XmlDocument();
                            xmldoc2.Load(SaveXmlSettingsFile.XMLfileUsera);
                            XmlNode tt = xmldoc2.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");
//jeżeli są karteczki na następnych tabach oraz ten tab nie ma karteczek oraz danej notatki nie ma na liście do usunięcia                     
                            if ((Int32.Parse(tt.Attributes[8].Value) > tabControl1.SelectedIndex) && (!tabControl1.SelectedTab.HasChildren) && (!NotesToDelete.Contains(Int32.Parse(IDs))))
                            {
                                tt.Attributes[8].Value = (Int32.Parse(tt.Attributes[8].Value) - 1).ToString();
                                xmldoc2.Save(SaveXmlSettingsFile.XMLfileUsera);
                            }
                        }
                         if (!tabControl1.SelectedTab.HasChildren)
                          {
                        tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
                           }
                    }
                  fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces
            }
            catch (Exception ee)
            {
                MessageBox.Show("Problem with shifting notes across tabs. " + ee.ToString());
            }
      }
        public void RenameTab()
        {
            try
            {
                FileStream fs = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read);
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(fs);
                XmlNodeList xmlnode;
                xmlnode = xmldoc.GetElementsByTagName("Note");

                if (xmlnode.Count > 0)
                {
                    fs.Dispose(); //zamykam Filestream, abym potem miał dostęp do pliku przez inny proces

                    var xml = XElement.Load(SaveXmlSettingsFile.XMLfileUsera);
                    var queryNoteID =                            //wynikiem jest tablica wartości wszystkich atrybutów "NoteID"
                    from k in xml.Descendants("Note")
                    select k.Attribute("NoteID").Value;

                    foreach (string IDs in queryNoteID)
                    {
                        XmlDocument xmldoc2 = new XmlDocument();
                        xmldoc2.Load(SaveXmlSettingsFile.XMLfileUsera);
                        XmlNode tt = xmldoc2.SelectSingleNode("notes/Note[@NoteID='" + IDs + "']");
                        if (Int32.Parse(tt.Attributes[8].Value) == tabControl1.SelectedIndex && textBox1.Text != "")
                           tt.Attributes[9].Value = textBox1.Text;
                           xmldoc2.Save(SaveXmlSettingsFile.XMLfileUsera);
                    }
                }
                fs.Dispose();
                FindLostNotes();
                textBox1.Text = "";
            }
            catch (Exception ee)
            {
                MessageBox.Show("Problem with changing tab name. " + ee.ToString());
            }
        }
        private void button3_Click(object sender, EventArgs e) //changing tab name
        {
            RenameTab();
        }

        public void SavetoRegistry()
        {
            try {
                string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\" + Application.ProductName + "\\" + Application.ProductVersion;
                string ifSound;
                if (checkBox1DisableSound.Checked)
                { ifSound = "0"; }
                else { ifSound = "1"; }

                string keyName = "Sound";
                Microsoft.Win32.Registry.SetValue(key, keyName, ifSound, Microsoft.Win32.RegistryValueKind.String); }
            catch { }
        }
   public string ReadFromRegistry(string KeyName)
        {
                try
                {
                string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\" + Application.ProductName + "\\" + Application.ProductVersion;
                return (string)Microsoft.Win32.Registry.GetValue(key,KeyName.ToUpper(), Microsoft.Win32.RegistryValueKind.String);
                }
                catch (Exception e)
                {
               //  MessageBox.Show(e+ "Reading registry");//jeżeli nie ma jeszcze wpisu żeby nie wywalało błędu
                    return null;
                }
            }
        public void setCheckBox()
        {
            try {
                if (ReadFromRegistry("Sound") == "0")
                { checkBox1DisableSound.Checked = true; }
                else if (ReadFromRegistry("Sound") == "1")
                { checkBox1DisableSound.Checked = false; }
            }
            catch { }
        }
        public void LockApplication()
        {
            if (Authentification.CheckingKeyValidation(ReadFromRegistry("KEY")) == 0)
            { tabControl1.Dispose();
            MessageBox.Show("Input valid license key.");
            }
        }
        private void inputLicenseKeyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LicenseInput restt = new LicenseInput();
            restt.ShowDialog();
        }

        //string Timestamp = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
        //string key = "HKEY_LOCAL_MACHINE\\SOFTWARE\\" + Application.ProductName + "\\" + Application.ProductVersion;
        //string valueName = "Trial Period";
        ////MessageBox.Show(key);
        //Microsoft.Win32.Registry.SetValue(key, valueName, Timestamp, Microsoft.Win32.RegistryValueKind.String);

        //RegistryKey key = Registry.LocalMachine.OpenSubKey("Software", true);
        //key.CreateSubKey("AppName");
        //key = key.OpenSubKey("AppName", true);
        //key.CreateSubKey("AppVersion");
        //key = key.OpenSubKey("AppVersion", true);
        //key.SetValue("yourkey", "yourvalue");








    }
}
