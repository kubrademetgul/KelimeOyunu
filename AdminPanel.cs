using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kelime_Oyunu
{
    public partial class AdminPanel : Form
    {
        private DatabaseController databaseController = new DatabaseController();
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void addWordInput_TextChanged(object sender, EventArgs e)
        {
            addInfoLbl.Text = "";
            if (addWordInput.Text.Length<3)
            {
                addWordBtn.Enabled = false;
            }
            else
            {
                addWordBtn.Enabled = true;
            }
        }

        private void addWordBtn_Click(object sender, EventArgs e)
        {
            Boolean status = databaseController.insertValuesIntoTable(addWordInput.Text.Trim());
            if (status)
            {
                addWordInput.Clear();
                addWordInput.Focus();
                addInfoLbl.Text = "Kelime eklendi.";

            }
            else
            {
                addWordInput.Clear();
                addWordInput.Focus();
                addInfoLbl.Text = "Kelime eklenemedi!";
            }

        }

        private void updateWordList_Click(object sender, EventArgs e)
        {
            List<String> wordList = databaseController.getAllWords();
            totalWordInfo.Text = "Toplam Kelime Sayısı:" + databaseController.getWordCount().ToString();
            wordPoolList.DataSource = wordList;
        }

        private void searchWordBtn_Click(object sender, EventArgs e)
        {
            List<String> wordList = databaseController.searchByWord(textSearchInput.Text.Trim());
            searchedWordList.DataSource = wordList;
            if (wordList.Count>0)
            {
                deleteWordBtn.Enabled = true;
            }
            else
            {
                deleteWordBtn.Enabled = false;
            }

        }

        private void deleteWordBtn_Click(object sender, EventArgs e)
        {
            if (searchedWordList.Items.Count>0 &&  searchedWordList.SelectedIndex>-1)
            {
                String selectedWord = searchedWordList.SelectedValue.ToString();
                Boolean isDeleted = databaseController.deleteValuesFromTable(selectedWord);
                if (isDeleted)
                {
                    searchedWordList.DataSource = null;
                    List<String> wordList = databaseController.searchByWord(textSearchInput.Text);
                    searchedWordList.DataSource = wordList;
                    if (wordList.Count > 0)
                    {
                        deleteWordBtn.Enabled = true;
                    }
                    else
                    {
                        deleteWordBtn.Enabled = false;
                    }
                    MessageBox.Show("Kelime silindi.","Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
        }

        private void totalWordInfo_Click(object sender, EventArgs e)
        {

        }
    }
}
