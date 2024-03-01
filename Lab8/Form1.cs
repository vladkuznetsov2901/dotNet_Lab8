using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace Lab8
{
    [Serializable]
    public class MultiTranslationDictionary
    {
        private Dictionary<string, List<string>> dictionary;

        public MultiTranslationDictionary()
        {
            dictionary = new Dictionary<string, List<string>>();
        }

        public void AddTranslation(string word, List<string> translations)
        {
            if (dictionary.ContainsKey(word))
            {
                dictionary[word].AddRange(translations);
            }
            else
            {
                dictionary[word] = translations;
            }
        }

        public List<string> GetTranslations(string word)
        {
            return dictionary.ContainsKey(word) ? dictionary[word] : new List<string>();
        }

        public void SaveToFileBinary(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static MultiTranslationDictionary ReadFromFileBinary(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                IFormatter formatter = new BinaryFormatter();
                return (MultiTranslationDictionary)formatter.Deserialize(fs);
            }
        }

        public void SaveToFileSoap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                IFormatter formatter = new SoapFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static MultiTranslationDictionary ReadFromFileSoap(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                IFormatter formatter = new SoapFormatter();
                return (MultiTranslationDictionary)formatter.Deserialize(fs);
            }
        }
    }

    public partial class Form1 : Form
    {
        private MultiTranslationDictionary dictionary;

        private TextBox txtWord;
        private TextBox txtTranslations;
        private Button btnAddTranslation;
        private Button btnGetTranslations;

        public Form1()
        {
            dictionary = new MultiTranslationDictionary();

            txtWord = new TextBox();
            txtWord.Location = new System.Drawing.Point(10, 10);

            txtTranslations = new TextBox();
            txtTranslations.Location = new System.Drawing.Point(10, 40);

            btnAddTranslation = new Button();
            btnAddTranslation.Text = "Add Translation";
            btnAddTranslation.Location = new System.Drawing.Point(10, 70);
            btnAddTranslation.Click += BtnAddTranslation_Click;

            btnGetTranslations = new Button();
            btnGetTranslations.Text = "Get Translations";
            btnGetTranslations.Location = new System.Drawing.Point(10, 100);
            btnGetTranslations.Click += BtnGetTranslations_Click;

            Controls.Add(txtWord);
            Controls.Add(txtTranslations);
            Controls.Add(btnAddTranslation);
            Controls.Add(btnGetTranslations);
        }

        private void BtnAddTranslation_Click(object sender, EventArgs e)
        {
            string word = txtWord.Text;
            string[] translationsArray = txtTranslations.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> translations = new List<string>(translationsArray);

            dictionary.AddTranslation(word, translations);
            MessageBox.Show("Translation added successfully!");
        }

        private void BtnGetTranslations_Click(object sender, EventArgs e)
        {
            string word = txtWord.Text;
            List<string> translations = dictionary.GetTranslations(word);
            MessageBox.Show($"Translations of '{word}': {string.Join(", ", translations)}");
        }
    }

}
