using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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
        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
        byte[] plaintext;
        byte[] encryptedtext;

        bool toFile = false;
        bool fromFile = false;
        public Form1()
        {
            InitializeComponent();
            textBox1.MaxLength = 58;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = true;
            textBox1.Visible = true;
            encryptButton.Visible = true;

            label2.Visible = false;
            textBox2.Visible = false;
            decryptButton.Visible = false;

            checkBox1.Visible = false;
            checkBox2.Visible = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label1.Visible = false;
            textBox1.Visible = false;
            encryptButton.Visible = false;

            label2.Visible = true;
            textBox2.Visible = true;
            decryptButton.Visible = true;
            checkBox1.Visible = true;
            checkBox2.Visible = false;

            textBox2.Text = resultTextBox.Text;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string Path = null;

                Encoding encoding = Encoding.GetEncoding("437");
                plaintext = encoding.GetBytes(textBox1.Text);
                encryptedtext = Encryption(plaintext, RSA.ExportParameters(false), false);
                if (toFile == true)
                {
                    using (var sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "txt files (*.txt)|*.txt";
                        sfd.RestoreDirectory = true;
                        if (sfd.ShowDialog() == DialogResult.OK)
                        {
                            Path = sfd.FileName;
                        }
                    }
                    using (StreamWriter writer = new StreamWriter(Path))
                    {
                        writer.Write(encoding.GetString(encryptedtext));
                    }
                }
                resultTextBox.Text = encoding.GetString(encryptedtext);
            }
            catch (ArgumentNullException)
            {
                Console.WriteLine("Encryption failed.");
            }
        }

        private void decryptButton_Click(object sender, EventArgs e)
        {
            Encoding encoding = Encoding.GetEncoding("437");

            if (fromFile == true)
            {
                var content = string.Empty; ;
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    string path;
                    ofd.Filter = "txt files (*.txt)|*.txt";
                    ofd.RestoreDirectory = true;
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        path = ofd.FileName;
                        var data = ofd.OpenFile();
                        using (StreamReader sr = new StreamReader(data))
                        {
                            content = sr.ReadToEnd();
                        }
                    }
                }
                try
                {

                    byte[] decryptedtex = Decryption(encoding.GetBytes(content),
                    RSA.ExportParameters(true), false);
                    resultTextBox.Text = encoding.GetString(decryptedtex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine("Encryption failed.");
                }
            }
            else
            {
                try
                {
                    plaintext = encoding.GetBytes(textBox2.Text);
                    byte[] decryptedtex = Decryption(plaintext,
                    RSA.ExportParameters(true), false);
                    resultTextBox.Text = encoding.GetString(decryptedtex);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    Console.WriteLine("Encryption failed.");
                }
            }
        }


        /*---- Encryption and Decryption methods ----*/
        static public byte[] Encryption(byte[] Data, RSAParameters RSAKey, bool DoOAEPPadding)
        {
            try
            {
                byte[] encryptedData;
                using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
                {
                    RSA.ImportParameters(RSAKey);
                    string test = RSA.ToString();
                    encryptedData = RSA.Encrypt(Data, DoOAEPPadding);
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

        private void button1_Click(object sender, EventArgs e)
        {

       
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (fromFile == false)
            {
                fromFile = true;
            }
            else
            {
                fromFile = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (toFile == false)
            {
                toFile = true;
            }
            else
            {
                toFile = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
