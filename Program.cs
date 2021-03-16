using System;
using LB2_Cryptography;

namespace LB2_Cryptography
{
    class Program
    {
        static void Main(string[] args)
        {
           int num =  Crypto.EncodeHedgeCipher("in.txt", 4, 0);
            Console.WriteLine(num);
        }
    }
}
