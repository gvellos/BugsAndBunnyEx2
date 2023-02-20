using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BugsAndBunnyChallenge
{
    public class Stats
    {
        Player player = new Player();
        public String connectionString = "Data source=csharp2022_2.db;Version=3";
        SQLiteConnection connection;

        public List<Player> ShowStats(Player player)
        {
            connection.Open();
            String selectSQL = "Select * from BugsAndBunny";
            SQLiteCommand command = new SQLiteCommand(selectSQL, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<Player> players = new List<Player>();
            while (reader.Read())
            {
                Player p = new Player();
                p.Username = reader.GetString(1);
                p.Winner = reader.GetString(2);
                p.Score = reader.GetInt32(3);
                players.Add(p);
            }
            players = players.OrderByDescending(p => p.Score).ToList();
            connection.Close();
            return players;
        }


        public void CreateTable()
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
            String createSQL = "Create table if not exists BugsAndBunny(BAB integer primary key autoincrement," +
                "Username Text,Winner Text,Score Int)";
            SQLiteCommand command = new SQLiteCommand(createSQL, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public void Insert(Player player)
        {
            connection.Open();
            String insertSQL = "Insert into BugsAndBunny(Username,Winner,Score) values(@username,@winner,@score)";
            SQLiteCommand command = new SQLiteCommand(insertSQL, connection);
            command.Parameters.AddWithValue("username", player.Username);
            command.Parameters.AddWithValue("winner", player.Winner);
            command.Parameters.AddWithValue("score", player.Score);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public Tuple<string, int> GetTopScorer()
        {
            connection.Open();
            string selectSQL = "SELECT Username, MAX(Score) as MaxScore FROM BugsAndBunny";
            SQLiteCommand command = new SQLiteCommand(selectSQL, connection);
            SQLiteDataReader reader = command.ExecuteReader();
            string topScorerUsername = null;
            int topScorerScore = -1;
            if (reader.Read())
            {
                topScorerUsername = reader.GetString(0);
                topScorerScore = reader.GetInt32(1);
            }
            connection.Close();
            return Tuple.Create(topScorerUsername, topScorerScore);
        }

    }
}
