using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace blocks
{
    class Authentification
    {
         public static string DescriptKey(string KEY)//metoda zamienia zakodowany klucz na datę (pierwiastkuje datę)
        {
            double output = 0;
            output = Math.Sqrt(Double.Parse(KEY));
            return output.ToString();
        }

         public string HashKey(string KEY)//metoda zamienia datę na zaszyfrowany klucz (podnosi datę do potęgi 2)
         {
             double output = 0;
             output = Math.Pow(Double.Parse(KEY),2);
             return output.ToString();
          }
         public static string Reverse(string s) //podawany string jest odwrócony, więc muszę go odwrócić ponownie za pomocą tej metody
         {
             char[] charArray = s.ToCharArray();
             Array.Reverse(charArray);
             return new string(charArray);
         }
         public static int CheckingKeyValidation(string KEY)
         {
             try
             {
                 string TimeNow = DateTime.Now.ToString("yyyyMMddHH");
                 int DateNow = Int32.Parse(TimeNow);
                 int ExpirationDate = Int32.Parse(DescriptKey(Reverse(KEY)));//parsuje stringa na datę
                 int MaxDate = 2059010101;//aby się zabezpieczyć że ktoś strzeli bardzo dużą liczbę i spełni poniższy warunek
                 if (ExpirationDate > DateNow && ExpirationDate < MaxDate)
                 {
                     return 1;
                 }
                 else { return 0; }
             }
             catch { return 0; }
         }
        









    }
}
