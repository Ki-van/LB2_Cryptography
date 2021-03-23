using System;
using System.IO;
using System.Text;
using LB2_Cryptography;

namespace LB2_Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(Crypto.GetFileLenghtInLetters("in.txt"));
            /*  Crypto.EncryptRailFenceCipher("in.txt", 2);
              Crypto.DecryptRailFenceCipher("crypted.txt", 2);*/

            /*Crypto.CryptKeyTranspositionCipher("in.txt", 5, new int[] { 5, 4, 3, 2, 1 });
            Crypto.DecryptKeyTranspositionCipher("crypted.txt", 5, new int[] { 5, 4, 3, 2, 1 });*/


            /*Crypto.EncryptColumnarTranspositionCipher("in.txt", 5, new int[] { 5, 4, 3, 2, 1 });
            Crypto.DecryptColumnarTranspositionCipher("crypted.txt", 5, new int[] {  5, 4, 3, 2, 1 });
*/
            /*  Crypto.EncryptDoubleTanspositionCipher("in.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
              Crypto.DecryptDoubleTanspositionCipher("crypted.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });*/

            /* Crypto.EncryptColumnarTranspositionCipher("in.txt", 4, new int[] { 4, 3, 2, 1 });
             Crypto.HackCipher("crypted.txt", Crypto.DecryptColumnarTranspositionCipher, "russian.txt");*/


            /* Crypto.CryptKeyTranspositionCipher("in.txt", 4, new int[] { 4, 3, 2, 1 });
             Crypto.DecryptKeyTranspositionCipher("crypted.txt", 4, new int[] { 4, 3, 2, 1 });
 */
            Crypto.EncryptColumnarTranspositionCipher("in.txt", 5, new int[] { 5, 4, 3, 2, 1 });
            Crypto.HackCipher("crypted.txt", Crypto.DecryptColumnarTranspositionCipher, "russian.txt", 0.15);

             //Crypto.DecryptKeyTranspositionCipher("task3.txt", 8 ,new int[] { 5, 6, 1, 8, 2, 4, 7, 3 });

        }
    }
}
