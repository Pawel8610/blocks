using System;
using System.Drawing;
using System.IO;
using System.Xml.Serialization;

namespace blocks
{
    public class SaveXmlSettingsFile
    {

       //public static string pobierzIDUsera = "note"+LogIn.UserID+ ".xml";//generator nazwy pliku xml, w zależności od zalogowanej osoby, ustawienia będą ładowały się
        public static string XMLfileUsera = "note1.xml";

        public static void SaveData(object obj, string filename)
        {
            try
            {
                XmlSerializer sr = new XmlSerializer(obj.GetType());
                TextWriter writer = new StreamWriter(filename);
                sr.Serialize(writer, obj);
                writer.Close();
            }
            catch { }
        }

      public static void SaveNote(string note)
        {
                notes not = new notes();
                if (note != "")
                {
                    not.Note = note;
                }
                SaveData(not, XMLfileUsera);
        }
//zrobić, żeby w zależności od indeksu i  czytało daną notatkę
        public static string readingNote()
        {
            try
            {
                if (File.Exists(SaveXmlSettingsFile.XMLfileUsera))
                {
                    XmlSerializer xs = new XmlSerializer(typeof(notes));
                    FileStream read = new FileStream(SaveXmlSettingsFile.XMLfileUsera, FileMode.Open, FileAccess.Read, FileShare.Read);
                    if (read.CanRead == true)
                    {
                        notes info = (notes)xs.Deserialize(read);
                        read.Close();
                        return info.Note;
                    }
                    else return "";
                }
                return "";
            }
            catch { return ""; }
        }

       


    }
}