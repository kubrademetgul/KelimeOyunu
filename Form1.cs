using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kelime_Oyunu
{
    public partial class Form1 : Form
    {
        private DatabaseController databaseController = new DatabaseController();
        private GameLogic gameLogic = new GameLogic();
        private static Random shuffleRandomObj = new Random();
        String wordOptions = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        List<String> selectedWords = new List<String>();
        Double counter = 0f;
        int gameMinute = 0, gameSecond = 0,totalPoint=0,totalTrueAnswer=0, totalWrongAnswer=0;
        Boolean isGameStart = false;

        private void kapatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public Form1()
        {
            InitializeComponent();
            
        }

        private void harfUretButton_Click(object sender, EventArgs e)
        {
            if (!isGameStart) {
                oyunSayac.Start();
                gameMinute = 0;
                gameSecond = 0;
                counter = 0f;
                totalTrueAnswer = 0;
                totalWrongAnswer = 0;
                startStopBtn.Text = "Oyunu Bitir";
                pasBtn.Enabled = true;
                shuffleWordsBtn.Enabled = true;
                isGameStart = true;
                textInput.Enabled = true;
                generateWords();
            }else
            {
                oyunSayac.Stop();
                startStopBtn.Text = "Oyunu Başlat";
                tahminEtBtn.Enabled = false;
                pasBtn.Enabled = false;
                shuffleWordsBtn.Enabled = false;
                textInput.Enabled = false;
                isGameStart = false;
                initArrayOfWords();
            }
        }
        
        private void initArrayOfWords()
        {
            List<String> clearList = new List<String>();
            for (int i=0;i<8;i++)
            {
                clearList.Add("-");
            }
            putCharactersIntoScreen(clearList);
        }

        private void oyunSayac_Tick(object sender, EventArgs e)
        {
            counter++;
            if (counter % 60==0)
            {
                gameMinute++;
                counter = 0;
            }
            gameSecond = (int)counter;
            if (gameMinute==0) {
                b1.Text = "Geçen Süre: "+gameSecond+"sn";
            }
            else{
                b1.Text = "Geçen Süre: "+gameMinute+"dk "+ gameSecond + "sn";
            }
        }

        private void shuffleWordsBtn_Click(object sender, EventArgs e)
        {
            shuffleList(selectedWords);
        }

        private void pasBtn_Click(object sender, EventArgs e)
        {
            generateWords();
        }

        private void tahminEtBtn_Click(object sender, EventArgs e)
        {
            String pointGainStatus = "";
            if (gameLogic.isWordRegular(selectedWords, textInput.Text.Trim()))
            {
                int point = gameLogic.calculatePoint(textInput.Text.Trim());
                Console.WriteLine("Kelime kurallara uygun.");
                if (databaseController.searchByWordIsFound(textInput.Text.Trim()))
                {
                    wordStatusLabel.Text = "Bulunan kelime doğru. " + point + " puan kazanıldı.";
                    wordStatusLabel.ForeColor = Color.Green;
                    totalTrueAnswer++;
                    totalPoint += point;
                    pointGainStatus = "T";
                }
                else
                {
                    wordStatusLabel.Text = "Bulunan kelime anlamlı değil.";
                    wordStatusLabel.ForeColor = Color.Orange;
                    Console.WriteLine("Kelime kurallara uygun.");
                    pointGainStatus = "F";
                    totalWrongAnswer++;
                }
            }
            else
            {
                wordStatusLabel.Text = "Bulunan kelime kurallara uygun değil.";
                wordStatusLabel.ForeColor = Color.Red;
                Console.WriteLine("Kelime kurallara uygun değil.");
                pointGainStatus = "W";
                totalWrongAnswer++;
            }

            updateInfoLabelsAndList(textInput.Text.Trim(), pointGainStatus);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            databaseController.initializeDatabase();
        }

        private void adminPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.ShowDialog();
        }

        private void textInput_TextChanged(object sender, EventArgs e)
        {
            if (textInput.Text.Length>2)
            {
                tahminEtBtn.Enabled = true;
            }
            else
            {
                tahminEtBtn.Enabled = false;
            }
        }

        

        

        public void putCharactersIntoScreen(List<String> wordList)
        {
            l1.Text = wordList[0];
            l2.Text = wordList[1];
            l3.Text = wordList[2];
            l4.Text = wordList[3];
            l5.Text = wordList[4];
            l6.Text = wordList[5];
            l7.Text = wordList[6];
            l8.Text = wordList[7];
        }
        public void generateWords()
        {
            Random wordRandomObject = new Random();
            selectedWords.Clear();
            for (int i = 0; i < 8; i++)
            {
                selectedWords.Add(wordOptions[wordRandomObject.Next(wordOptions.Length)].ToString());
            }
            putCharactersIntoScreen(selectedWords);
        }

        public  void shuffleList(List<String> list)
        {
            List<String> tmpList = list.GetRange(0, list.Count);
            List<String> arrReturn = new List<String> ();

            while (tmpList.Count > 0)
            {
                int rnd = shuffleRandomObj.Next(0, tmpList.Count);
                arrReturn.Add(tmpList[rnd]);
                tmpList.RemoveAt(rnd);
            }

            putCharactersIntoScreen(arrReturn);
        }

        private void updateInfoLabelsAndList(String inputText, String isPointGained)
        {
            totalTrueLabel.Text = "Doğru Deneme: "+totalTrueAnswer.ToString();
            totalWrongLabel.Text = "Hatalı Deneme: " + totalWrongAnswer.ToString();
            totalPointLabel.Text = "Puan: " + totalPoint;
            if (isPointGained=="T")
                previousInputs.Items.Insert(0,inputText+"\t-DOĞRU-");
            else if (isPointGained == "F")
                previousInputs.Items.Insert(0, inputText + "\t-ANLAMSIZ-");
            else if (isPointGained == "W")
                previousInputs.Items.Insert(0, inputText + "\t-HATALI-");
            else
                previousInputs.Items.Insert(0,"-");
        }
    }
}
