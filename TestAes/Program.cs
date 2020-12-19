using System;

namespace TestAes
{
    class Program
    {
        static void Main(string[] args)
        {
            AesManger manager = new AesManger();

            //string cOpenFile = @"./FileTest/File/test.txt";
            //string cSaveFile = @"E:\aes-implementation-master\TestAes\FileTest\FileEncrypt\text.txt";

            string cOpenFile = @"E:\aes-implementation-master\TestAes\FileTest\FileEncrypt\text.aes";
            string cSaveFile = @"E:\aes-implementation-master\TestAes\FileTest\FileDecrypt\text.txt";

            string cPassword = "123456";
            //check param successfully
            
            //Boolean bValue = manager.Encrypt(cOpenFile, cSaveFile, cPassword);
            Boolean bValue = manager.Decrypt(cOpenFile, cSaveFile, cPassword);

            Console.WriteLine(bValue);
            Console.ReadKey();

                
        }
    }
}


