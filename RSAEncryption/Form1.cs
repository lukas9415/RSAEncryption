using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RSAEncryption
{
    public partial class Form1 : Form
    {
        UnicodeEncoding ByteConverter = new UnicodeEncoding();
        //RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        RSAParameters rsaKeyInfo;

        public Form1()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = true;
            textBox1.Visible = true;
            encryptButton.Visible = true;

            label2.Visible = false;
            textBox2.Visible = false;
            decryptButton.Visible = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = false;
            textBox1.Visible = false;
            encryptButton.Visible = false;

            label2.Visible = true;
            textBox2.Visible = true;
            decryptButton.Visible = true;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            //Generate a public/private key pair.  
            RSA rsa = RSA.Create();
            //Save the public key information to an RSAParameters structure.  
            rsaKeyInfo = rsa.ExportParameters(false);
            resultTextBox.Clear();
            byte[] text = ByteConverter.GetBytes(textBox1.Text);
            byte[] encryptedtext = Encryption(text, rsaKeyInfo, false);
            resultTextBox.Text = ByteConverter.GetString(encryptedtext);
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            resultTextBox.Clear();
            byte[] texttodecrypt = ByteConverter.GetBytes(textBox2.Text);
            byte[] decryptedtext = Decryption(texttodecrypt, rsaKeyInfo, false);
            resultTextBox.Text = ByteConverter.GetString(decryptedtext);
        }


        /*---- Encryption and Decryption methods ----*/
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey); encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
                }
                return encryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        static public byte[] Decryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] decryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    decryptedData = RSA.Decrypt(Data, DoOAEPPadding);
                }
                return decryptedData;
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }

    }
}
