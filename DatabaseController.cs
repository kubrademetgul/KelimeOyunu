using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace Kelime_Oyunu
{
    class DatabaseController
    {
        SQLiteConnection connection;

        public DatabaseController()
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
        }
        public SQLiteConnection connectToDatabase()
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            connection.Open();
            return connection;
        }
        public void closeConnectionFromDatabase()
        {
            connection.Close();
        }
        public void initializeDatabase()
        {
            if (!File.Exists("game_words.db"))
            {
                SQLiteConnection.CreateFile("game_words.db");
                createMainTable();
      
            }
        }
        private void createMainTable()
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            using (connection)
            {
                SQLiteCommand tableCreatingCommand = new SQLiteCommand(@"CREATE TABLE IF NOT EXISTS words (
                ID INTEGER PRIMARY KEY AUTOINCREMENT, word varchar(9))");
                tableCreatingCommand.Connection = connection;
                connection.Open();
                tableCreatingCommand.ExecuteNonQuery();
                connection.Close();
            }
        }
        public Boolean searchByWordIsFound(String word)
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            using (connection)
            {
                SQLiteCommand dataSearcherCommand = new SQLiteCommand(@"SELECT * FROM words WHERE word='" + word + "';");
                dataSearcherCommand.Connection = connection;
                connection.Open();
                SQLiteDataReader dataReader = dataSearcherCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    connection.Close();
                    return true;
                }
                connection.Close();
                return false;
            }
        }

        public object getWordCount()
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            using (connection)
            {
                SQLiteCommand dataCountCommand = new SQLiteCommand(@"SELECT COUNT(word) FROM words;");
                dataCountCommand.Connection = connection;      
                connection.Open();
                object wordcount = dataCountCommand.ExecuteScalar(); 
                connection.Close();
                return wordcount;
            }
        }
        public List<String> getAllWords()
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            List<String> wordList = new List<String>();
            using (connection)
            {
                SQLiteCommand dataSearcherCommand = new SQLiteCommand(@"SELECT * FROM words ORDER BY word ASC");
                dataSearcherCommand.Connection = connection;
                connection.Open();
                SQLiteDataReader dataReader = dataSearcherCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine(dataReader.GetString(1));
                    wordList.Add(dataReader.GetString(1));
                }
                connection.Close();
                return wordList;
            }
        }
        public List<String> searchByWord(String word)
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            List<String> foundedWords = new List<String>();
            using (connection)
            {
                SQLiteCommand dataSearcherCommand = new SQLiteCommand(@"SELECT * FROM words WHERE word LIKE '%" + word + "%' ORDER BY word ASC;");
                dataSearcherCommand.Connection = connection;
                connection.Open();
                SQLiteDataReader dataReader = dataSearcherCommand.ExecuteReader();
                while(dataReader.Read()){
                    foundedWords.Add(dataReader[1].ToString());
                }
                connection.Close();
                return foundedWords;
            }
        }
        public Boolean deleteValuesFromTable(String word)
        {
            try
            {
                connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
                using (connection)
                {
                    SQLiteCommand deleteDataCommand = new SQLiteCommand(@"DELETE FROM words WHERE word='" + word + "';");
                    deleteDataCommand.Connection = connection;
                    connection.Open();
                    deleteDataCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public Boolean insertValuesIntoTable(String word)
        {
            connection = new SQLiteConnection("Data Source=game_words.db;Version=3;");
            try
            {
                using (connection)
                {
                    SQLiteCommand insertDataCommand = new SQLiteCommand(@"INSERT INTO words(word) VALUES ('" + word + "');");
                    insertDataCommand.Connection = connection;
                    connection.Open();
                    insertDataCommand.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch(Exception exc)
            {
                return false;
            }
        }
        
    }
}
