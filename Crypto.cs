using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LB2_Cryptography
{
    class Crypto
    {
        private static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя_,.АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ";
        private static int m = alphabet.Length;

        public static bool EncodeHedgeCipher(string path, int columnNumber, int stringNumber)
        {
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("encoded.txt", false, Encoding.UTF8);

            string sourceLine;

            //Построчно
            if (columnNumber > 0 && stringNumber == 0)
            {
                List<char[]> strings = new List<char[]>();
                int stringsI = 0;
                strings.Add(new char[columnNumber]);

                int stringSymbolsI = 0;
                while ((sourceLine = input.ReadLine()) != null)
                {
                    for (int i = 0; i < sourceLine.Length; i++)
                    {
                        if (alphabet.Contains(sourceLine[i]))
                        {
                            if (stringSymbolsI == columnNumber)
                            {
                                stringSymbolsI = 0;
                                strings.Add(new char[columnNumber]);
                                stringsI++;
                            }

                            strings.ElementAt(stringsI)[stringSymbolsI] = sourceLine[i];
                            stringSymbolsI++;
                        }
                    }
                }

                string outputLine;
                for(int i = 0; i < columnNumber - 1; i++)
                {
                    outputLine = "";
                    for(int j = 0; j < strings.Count; j++)
                    {
                        if(strings.ElementAt(j)[i] != '\0')
                            outputLine += strings.ElementAt(j)[i];
                    }

                    output.WriteLine(outputLine);
                }

                input.Close();
                output.Close();

                return true;
            }
            else
            {    //По столбцам
                if (stringNumber > 0 && columnNumber == 0)
                {

                    return true;
                }
            }

            

            return false;
        }


    }
}
