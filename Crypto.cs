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
       //
       //private static int m = alphabet.Length;
       

        public static bool CryptKeyTranspositionCipher(string path, int blockLen, int[] key)
        {
            return KeyTranspositionCipher(path, "crypted.txt", blockLen, key);
        }

        public static bool DecryptKeyTranspositionCipher(string path, int blockLen, int[] key)
        {
            return KeyTranspositionCipher(path, "decrypted.txt", blockLen, key);
        }
        private static bool KeyTranspositionCipher(string path, string outputPath, int blockLen, int[] key)
        {
            if (blockLen > 0)
            {
                int keyLen = 0;
                for (int i = 1; i <= blockLen; i++)
                {
                    if (key.Contains(i))
                        keyLen++;
                    else
                        return false;
                }

                if (keyLen != key.Length)
                    return false;
            }
            else
                return false;

            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter(outputPath, false, Encoding.UTF8);

            string sourceLine, sourceText = "";
            while ((sourceLine = input.ReadLine()) != null)
                sourceText += sourceLine ?? "";

            string sourceBlock = "", outputBlock;
            int sourceBlockI = 0;
            for(int i = 0; i < sourceText.Length; i++)
            {
                if(sourceBlockI < blockLen)
                {
                    if (alphabet.Contains(sourceText[i]))
                    {
                        sourceBlock += sourceText[i];
                        sourceBlockI++;
                    }
                } else
                {
                    outputBlock = "";
                    for(int j = 0; j < blockLen; j++)
                    {
                        outputBlock += sourceBlock[key[j] - 1];
                    }

                    output.Write(outputBlock);

                    sourceBlockI = 0;
                    sourceBlock = "";
                    i--;
                }
            }

            if(sourceBlockI < blockLen)
            {
                for (; sourceBlockI < blockLen; sourceBlockI++)
                {
                    sourceBlock += ".";
                }
            } 

            outputBlock = "";
            for (int j = 0; j < blockLen; j++)
            {
                outputBlock += sourceBlock[key[j] - 1];
            }
            output.Write(outputBlock);

            input.Close();
            output.Close();

            return true;
        }

        public static bool DecryptRailFenceCipher(string path, int columnNumber)
        {
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("decrypted.txt", false, Encoding.UTF8);

            string sourceLine;
            int stringNumber;

            double textLenght = Crypto.GetFileLenghtInLetters(path);
            int lastString = (int)textLenght % columnNumber;
            if (lastString == 0)
                lastString = columnNumber;

            char[,] strings = new char[(int)Math.Ceiling(textLenght / columnNumber), columnNumber];
            stringNumber = strings.Length / columnNumber;

            int stringsI = 0, stringSymbolsI = 0;
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (stringsI == stringNumber
                        || ((stringsI == stringNumber - 1) && (stringSymbolsI > lastString - 1)))
                    {
                        stringsI = 0;
                        stringSymbolsI++;
                    }

                    strings[stringsI, stringSymbolsI] = sourceLine[i];
                    stringsI++;
                }
            }

            string outputLine;
            for (int i = 0; i < stringNumber; i++)
            {
                outputLine = "";
                for (int j = 0; j < columnNumber; j++)
                {
                    if (strings[i, j] != '\0')
                        outputLine += strings[i, j];
                }

                output.WriteLine(outputLine);
            }

            input.Close();
            output.Close();

            return true;
        }

        public static bool EncodeRailFenceCipher(string path, int columnNumber)
        {
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            StreamWriter output = new StreamWriter("encoded.txt", false, Encoding.UTF8);

            string sourceLine;

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
            for (int i = 0; i < columnNumber; i++)
            {
                outputLine = "";
                for (int j = 0; j < strings.Count; j++)
                {
                    if (strings.ElementAt(j)[i] != '\0')
                        outputLine += strings.ElementAt(j)[i];
                }

                output.Write(outputLine);
            }

            input.Close();
            output.Close();

            return true;

        }

        public static int GetFileLenghtInLetters(string path)
        {
            int lenght = 0;
            StreamReader input = new StreamReader(path, Encoding.UTF8);

            string inputLine;
            while ((inputLine = input.ReadLine()) != null)
                lenght += inputLine.Length;

            return lenght;
        }
    }
}
