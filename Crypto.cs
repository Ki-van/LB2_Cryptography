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
        private static string alphabet = "абвгдеёжзийклмнопрстуфхцчшщъьыэюя_,.АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЭЮЯ ";
       //
       //private static int m = alphabet.Length;
        private static List<char []> ToTableLineByLine(string path, int columnNumber)
        {
            StreamReader input = new StreamReader(path, Encoding.UTF8);
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

            input.Close();

            return strings;
        }

        private static char[,] ToTableColumnByColumn(string path, int columnNumber, int[] order)
        {
            if (!Crypto.CheckBlockTranspositionCipherKey(columnNumber, order))
                return null;

            StreamReader input = new StreamReader(path, Encoding.UTF8);

            string sourceLine;
            int stringNumber;

            double textLenght = Crypto.GetFileLenghtInLetters(path);
            int lastString = (int)textLenght % columnNumber;
            if (lastString == 0)
                lastString = columnNumber;

            char[,] strings = new char[(int)Math.Ceiling(textLenght / columnNumber), columnNumber];
            stringNumber = strings.Length / columnNumber;

            int stringsI = 0, stringSymbolsI, keyI = 0;
            stringSymbolsI = order[keyI] - 1;
            while ((sourceLine = input.ReadLine()) != null)
            {
                for (int i = 0; i < sourceLine.Length; i++)
                {
                    if (stringsI == stringNumber
                        || ((stringsI == stringNumber - 1) && (stringSymbolsI > lastString - 1)))
                    {
                        stringsI = 0;
                        keyI++;
                        stringSymbolsI = order[keyI] - 1;
                    }

                    strings[stringsI, stringSymbolsI] = sourceLine[i];
                    stringsI++;
                }
            }

            input.Close();

            return strings;
        }

        private static bool CheckBlockTranspositionCipherKey(int blockLen, int [] key)
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

            return true;
        }

        private static string[] ToWords(string path)
        {
            int wordCount = 0;
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            Stream inputBase = input.BaseStream;
            inputBase.Position = 0;
            string line;
            while ((line = input.ReadLine()) != null)
            {
                line = line.Trim();
                if (line.Length > 0) 
                {
                    for (int i = 0; i < line.Length; i++)
                    {
                        if (line[i] == ' ')
                            wordCount++;
                    }
                    wordCount++;
                }
            }
            inputBase.Position = 0;

            string[] words = new string[wordCount];
            for(int i = 0; i < words.Length; i++)
            {
                line = input.ReadLine().Trim();
                
                for(int j = 0; j < line.Length; j++)
                {
                    if (line[j] != ' ')
                        words[i] += line[j];
                    else
                        i++;
                }
            }
            input.Close();

            return words;
        }   

        public static int GetFileLenghtInLetters(string path)
        {
            StreamReader input = new StreamReader(path, Encoding.UTF8);
            string inputLine = input.ReadToEnd();
            input.Close();
            return inputLine.Length;
        }

        private static (bool, int) BinarySearch<T>(T[] sorted_values, string value, bool descending = false) where T : IComparable
        {
            int left = 0;
            for (int right = sorted_values.Length, middle; left < right;)
            {
                if (sorted_values[left].CompareTo(value) == 0)
                    return (true, left);
                middle = left + (right - left) / 2;
                var c = sorted_values[middle].CompareTo(value);
                if (c == 0)
                {
                    if (middle == left + 1)
                        return (true, middle);
                    right = middle + 1;
                }
                if ((c < 0) == descending)
                    right = middle;
                else
                    left = middle + 1;
            }
            return (false, left);
        }

        public static int[] HackColumnarTranspositionCipher(string path, string dictionaryPath, double match = 0.7, 
            string outputPath = "decrypted.txt", int maxKeyLen = 50)
        {
            bool compare(int val_1, int val_2)
            {
                if (val_1 > val_2)
                    return true;
                else
                    return false;
            }
            
            int[] key = null;
            try
            {
                StreamReader decrypted = new StreamReader(outputPath, Encoding.UTF8);
                string[] dictionary = ToWords(dictionaryPath);
                for (int keyLen = 2; keyLen <= maxKeyLen; keyLen++)
                {
                    key = new int[keyLen];
                    for (int i = 0; i < keyLen; i++)
                        key[i] = i + 1;
                    do
                    {
                        Crypto.DecryptColumnarTranspositionCipher(path, keyLen, key);
                        decrypted.BaseStream.Position = 0;
                        
                        string decryptedLine, word = "";
                        double actualMatch = 0, wordCount = 0;
                        while((decryptedLine = decrypted.ReadLine()) != null)
                        {
                            decryptedLine = decryptedLine.Trim();
                            for(int i = 0; i < decryptedLine.Length; i++)
                            {
                                while (decryptedLine[i] != ' ' && i < decryptedLine.Length)
                                {
                                    word += decryptedLine[i];
                                    i++;
                                }
                                i++;
                                wordCount++;
                                if (BinarySearch<string>(dictionary, word).Item1)
                                    actualMatch++;

                            }
                        }
                        actualMatch = wordCount / actualMatch;

                        if (actualMatch >= match)
                        {
                            Console.WriteLine("Key:");
                            for (int i = 0; i < key.Length; i++)
                                Console.Write(key[i] + " ");
                            Console.WriteLine("Press 'y' to continue");
                            char c = (char)Console.Read();

                            if (c != 'y')
                                break;
                            else
                                Console.WriteLine("Key found, check decrypted.txt");
                        }
                        else
                            actualMatch = 0;
                        
                    } while (Narayana.NextPermutation<int>(key, compare));
                    
                }
                Console.WriteLine("Faild to find a key");
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
            return key;
        }

        public static bool EncryptDoubleTanspositionCipher(string path, int columnNumber, int[] key)
        {
            bool result = true;
            result = result && Crypto.EncryptColumnarTranspositionCipher(path, columnNumber, key, "tmp.txt");
            result = result && Crypto.EncryptColumnarTranspositionCipher("tmp.txt", columnNumber, key);

            FileInfo fTmp = new FileInfo("tmp.txt");
            fTmp.Delete();
            return result;
        }

        public static bool DecryptDoubleTanspositionCipher(string path, int columnNumber, int[] key)
        {
            bool result = true;
            result = result && Crypto.DecryptColumnarTranspositionCipher(path, columnNumber, key, "tmp.txt");
            result = result && Crypto.DecryptColumnarTranspositionCipher("tmp.txt", columnNumber, key);

            FileInfo fTmp = new FileInfo("tmp.txt");
          //  fTmp.Delete();

            return result;
        }

        public static bool DecryptColumnarTranspositionCipher(string path, int columnNumber, int[] key,
            string outputPath = "decrypted.txt")
        {
            if (!Crypto.CheckBlockTranspositionCipherKey(columnNumber, key))
                return false;

            StreamWriter output = new StreamWriter(outputPath, false, Encoding.UTF8);

            char[,]? strings = Crypto.ToTableColumnByColumn(path, columnNumber, key);
            if (strings == null)
            {
                output.Close();
                return false;
            }
            int stringNumber = strings.Length / columnNumber;

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

            output.Close();
            return true;
        }
        public static bool EncryptColumnarTranspositionCipher(string path, int columnNumber, 
            int[] key, string outputPath = null)
        {
            if (!Crypto.CheckBlockTranspositionCipherKey(columnNumber, key))
                return false;

            if (outputPath == null)
                outputPath = "crypted.txt";
            StreamWriter output = new StreamWriter(outputPath, false, Encoding.UTF8);

            List<char[]> strings = Crypto.ToTableLineByLine(path, columnNumber);

            string outputLine;
            for (int i = 0; i < columnNumber; i++)
            {
                outputLine = "";
                for (int j = 0; j < strings.Count; j++)
                {
                    if (strings.ElementAt(j)[key[i] - 1] != '\0')
                        outputLine += strings.ElementAt(j)[key[i] - 1];
                }

                output.Write(outputLine);
            }

            output.Close();

            return true;
        }

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

            if (!Crypto.CheckBlockTranspositionCipherKey(blockLen, key))
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
            StreamWriter output = new StreamWriter("decrypted.txt", false, Encoding.UTF8);

            int[] order = new int[columnNumber];
            for (int i = 1; i <= order.Length; i++)
                order[i - 1] = i;

            char[,]? strings = Crypto.ToTableColumnByColumn(path, columnNumber, order);
            if (strings == null)
                return false;
            
            int stringNumber = strings.Length / columnNumber;

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

            output.Close();
            return true;
        }

        public static bool EncryptRailFenceCipher(string path, int columnNumber)
        {
            StreamWriter output = new StreamWriter("crypted.txt", false, Encoding.UTF8);
            List<char[]> strings = Crypto.ToTableLineByLine(path, columnNumber);

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

            output.Close();
            return true;
        }
    }
}
