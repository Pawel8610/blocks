
using System.Xml.Serialization;
namespace blocks
{
    public class notes
    {
        
        private string note;

        public string Note//metada zapisująca/odczytująca pole background
        {
            get { return note; }
            set { note = value; }
        }
      
    }
}
