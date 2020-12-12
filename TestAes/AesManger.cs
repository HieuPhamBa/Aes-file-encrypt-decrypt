using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TestAes
{
    class AesManger
    {

        public const int MAX_BLOCK_LENGTH = 16;
        public const int MAX_KEY_LENGTH = 16;

        private enum ErrorID
        {
            NullFilePath = 0,
            NUllFileSave,
            NullPWD,
            OtherError,
            Success,
        };

        private string[] ErrorString = new string[]{
            "No File be Selected",
            "No File be Saved",
            "No Password input",
            "Other Error",
            "OK"};

        private enum Direction
        {
            Encrypt = 0,
            Decrypt,
        };


        private KeySize ekeySize;
        private BlockSize eblockSize;
        private Direction eDirEnDe;

        public AesManger()
        {
            ekeySize = KeySize.Bits256;
            eblockSize = BlockSize.Bits256;
            eDirEnDe = Direction.Encrypt;
        }

        public Boolean Encrypt(string cOpenFile, string cSaveFile, string cPassword)
        {
            //check param
            if (("" == cOpenFile) ||
                ("" == cSaveFile) ||
                ("" == cPassword))
            {
                return false;
            }

            if (false == File.Exists(cOpenFile))
            {
                return false;
            }

            while (true == File.Exists(cSaveFile))
            {
                cSaveFile = Rename(cSaveFile);
            }

            byte[] plainText = new byte[MAX_BLOCK_LENGTH];
            byte[] cipherText = new byte[MAX_BLOCK_LENGTH];
            byte[] bzkey = new byte[MAX_KEY_LENGTH];

            //get password
            bzkey = Encoding.Unicode.GetBytes(cPassword);

            //get bytes from file
            FileStream fileStream = new FileStream(cOpenFile, FileMode.Open);
            fileStream.Seek(0, SeekOrigin.Begin);

            //get the file stream for save 
            using (FileStream saveStream = new FileStream(cSaveFile, FileMode.Append))
            {
                //set length of the file
                long lFileLength = fileStream.Length;
                //set position of the file
                long lPostion = fileStream.Position;

                //Read byte and Encrypt
                while (lPostion < lFileLength)
                {
                    //Initialize  the buffer
                    Initialize(plainText, MAX_BLOCK_LENGTH);

                    long lHasRead = fileStream.Read(plainText, 0, MAX_BLOCK_LENGTH);
                    if (0 >= lHasRead)
                    {
                        break;
                    }
                    //set current cursor position
                    lPostion = fileStream.Position;

                    //Encrypt
                    Aes aes = new Aes(ekeySize, bzkey, eblockSize);

                    //Initialize  the buffer
                    Initialize(cipherText, MAX_BLOCK_LENGTH);

                    aes.Cipher(plainText, cipherText);
                    saveStream.Write(cipherText, 0, MAX_BLOCK_LENGTH);
                }

                saveStream.Close();
                fileStream.Close();
                return true;
            }
        }

        public Boolean Decrypt(string cOpenFile, string cSaveFile, string cPassword)
        {
            //check param
            if (("" == cOpenFile) ||
                ("" == cSaveFile) ||
                ("" == cPassword))
            {
                return false;
            }

            if (0 > cOpenFile.LastIndexOf(".aes"))
            {
                return false;
            }

            if (false == File.Exists(cOpenFile))
            {
                return false;
            }

            while (true == File.Exists(cSaveFile))
            {
                cSaveFile = Rename(cSaveFile);
            }

            byte[] plainText = new byte[MAX_BLOCK_LENGTH];
            byte[] cipherText = new byte[MAX_BLOCK_LENGTH];
            byte[] bzkey = new byte[MAX_KEY_LENGTH];

            //get password
            bzkey = Encoding.Unicode.GetBytes(cPassword);

            //get bytes from file
            FileStream fileStream = new FileStream(cOpenFile, FileMode.Open);
            fileStream.Seek(0, SeekOrigin.Begin);

            //get the file stream for save 
            FileStream saveStream = new FileStream(cSaveFile, FileMode.Append);

            //set length of the file
            long lFileLength = fileStream.Length;
            //set position of the file
            long lPostion = fileStream.Position;

            //Read byte and Decrypt
            while (lPostion < lFileLength)
            {
                //Initialize  the buffer
                Initialize(plainText, MAX_BLOCK_LENGTH);

                long lHasRead = fileStream.Read(plainText, 0, MAX_BLOCK_LENGTH);
                if (0 >= lHasRead)
                {
                    break;
                }
                //set current cursor position
                lPostion = fileStream.Position;

                //Encrypt
                Aes aes = new Aes(ekeySize, bzkey, eblockSize);

                //Initialize  the buffer
                Initialize(cipherText, MAX_BLOCK_LENGTH);
                //Decrypt
                aes.InvCipher(plainText, cipherText);
                saveStream.Write(cipherText, 0, MAX_BLOCK_LENGTH);
            }


            saveStream.Close();
            fileStream.Close();

            return true;
        }

        private void Initialize(byte[] pByte, int iLength)
        {
            int iIndex = 0;
            for (iIndex = 0; iIndex < iLength; iIndex++)
            {
                pByte[iIndex] = 0;
            }
        }

        private string Rename(string cName)
        {
            string cFileName = cName;
            string cExtentName = "";
            int iIndex = cName.IndexOf(".");
            if (iIndex > 0)
            {
                cFileName = cName.Substring(0, iIndex);
                cExtentName = cName.Substring(iIndex);
            }
            cFileName += "aes";
            cFileName += cExtentName;
            return cFileName;
        }

    }
}
