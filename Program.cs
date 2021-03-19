using System;
using LB2_Cryptography;

namespace LB2_Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
            /*  Crypto.EncryptRailFenceCipher("in.txt", 200);
              Crypto.DecryptRailFenceCipher("crypted.txt", 200);
  */
            /* Crypto.CryptKeyTranspositionCipher("in.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
             Crypto.DecryptKeyTranspositionCipher("crypted.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
 */

            /*Crypto.EncryptColumnarTranspositionCipher("in.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
            Crypto.DecryptColumnarTranspositionCipher("crypted.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });*/

            Crypto.EncryptDoubleTanspositionCipher("in.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
            Crypto.DecryptDoubleTanspositionCipher("crypted.txt", 7, new int[] { 7, 6, 5, 4, 3, 2, 1 });
        }
    }
}
