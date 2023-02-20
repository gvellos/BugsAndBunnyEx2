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

namespace BugsAndBunnyChallenge
{
    public partial class Form3 : Form
    {
        Player player = new Player();
        String connectionString = "Data source=csharp2022_2.db;Version=3";
        SQLiteConnection connection;
        Stats stats = new Stats();
        bool form1Or2;
        public Form3()
        {
            InitializeComponent();
            form1Or2 = false;
        }
        public Form3(Player player)
        {
            InitializeComponent();
            this.player = player;
            form1Or2 = true;
        }

        private void buttoneExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonShowStats_Click(object sender, EventArgs e)
        {
            stats.CreateTable();
            richTextBox1.Clear();
            foreach (Player player in stats.ShowStats(player))
            {
                richTextBox1.AppendText("Username: " + player.Username + "     ");
                richTextBox1.AppendText("Winner: " + player.Winner + "    ");
                richTextBox1.AppendText("Score: " + player.Score + "\n");
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {

            if (form1Or2)
            {
                stats.CreateTable();
                stats.Insert(player);
                //insert();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 formHome = new Form1();
            formHome.Show();
            formHome.StartPosition = FormStartPosition.Manual;
            formHome.Location = this.Location;
            formHome.Size = this.Size;
        }
    }
}
