using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Media;
using System.IO;

namespace DrawBallot
{
    public partial class MainForm : Form
    {
        DataForm dataForm = new DataForm();
        SQLiteConnection mConn;
        SQLiteDataAdapter mAdapter;
        DataTable mTable = new DataTable();
        List<string> IDList = new List<string>();
        int index;
        bool drawPushed = false;
        Timer timer = new Timer();
        List<Label> idList = new List<Label>();
        List<Label> LNList = new List<Label>();
        List<Label> FNList = new List<Label>();
        Setting setting = new Setting();
        History history = new History();
        public MainForm()
        {
            InitializeComponent();
            idList.Add(lID1);
            idList.Add(lID2);
            idList.Add(lID3);
            idList.Add(lID4);
            idList.Add(lID5);
            idList.Add(lID6);
            idList.Add(lID7);
            idList.Add(lID8);
            idList.Add(lID9);
            idList.Add(lID10);

            LNList.Add(lLN1);
            LNList.Add(lLN2);
            LNList.Add(lLN3);
            LNList.Add(lLN4);
            LNList.Add(lLN5);
            LNList.Add(lLN6);
            LNList.Add(lLN7);
            LNList.Add(lLN8);
            LNList.Add(lLN9);
            LNList.Add(lLN10);

            FNList.Add(lFN1);
            FNList.Add(lFN2);
            FNList.Add(lFN3);
            FNList.Add(lFN4);
            FNList.Add(lFN5);
            FNList.Add(lFN6);
            FNList.Add(lFN7);
            FNList.Add(lFN8);
            FNList.Add(lFN9);
            FNList.Add(lFN10);

            comboBox1.SelectedIndex = 0;
            mTable = history.mTable;
            for (int i=0;i<10;i++)
            {
                idList[i].Visible = false;
                FNList[i].Visible = false;
                LNList[i].Visible = false;
            }
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            Stream str = Properties.Resources.DrumRoll;
            SoundPlayer player = new SoundPlayer(str);
            player.Load();
            if (!drawPushed)
            {                
                player.PlayLooping();

                player.PlayLooping();
                for (int i = 0; i < 10; i++)
                {
                    if (i < comboBox1.SelectedIndex + 1)
                    {
                        idList[i].Visible = true;
                        FNList[i].Visible = true;
                        LNList[i].Visible = true;
                    }
                    else
                    {
                        idList[i].Visible = false;
                        FNList[i].Visible = false;
                        LNList[i].Visible = false;
                    }
                }
                string mDbPath = Application.StartupPath + "/DatabaseFinal.sqlite";
                mConn = new SQLiteConnection("Data Source=" + mDbPath);
                mConn.Open();
                SQLiteCommand sql = new SQLiteCommand("Select * From PARTICIPANT", mConn);
                SQLiteDataReader reader = sql.ExecuteReader();

                while (reader.Read())
                {
                    IDList.Add(reader["ID"].ToString());
                }

                //
                IDList.Shuffle();
                index = 0;
                
                //

                timer.Interval = 150;
                timer.Tick += new EventHandler(timer_Tick);
                timer.Start();
                drawButton.Text = "Stop";
                drawPushed = true;
            }
            else
            {
                player.Stop();
                timer.Stop();
                drawButton.Text = "Draw";
                drawPushed = false;
                int n = comboBox1.SelectedIndex + 1;
                for (int i = 0;i<n;i++)
                {
                    DataRow newrow = mTable.NewRow();
                    newrow["ID"] = idList[i].Text;
                    newrow["FIRSTNAME"] = FNList[i].Text;
                    newrow["LASTNAME"] = LNList[i].Text;
                    mTable.Rows.Add(newrow);
                }
                history.mTable = mTable;
                mConn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataButton_Click(object sender, EventArgs e)
        {

        }

        void timer_Tick(Object Sender, EventArgs e)
        {
            for (int i=0;i<10;i++)
            {
                this.idList[i].Text = IDList[(index+i)%IDList.Count];
                string strSql = "Select * From PARTICIPANT where ID =" + IDList[(index+i)%IDList.Count] + ";";
                SQLiteCommand sql = new SQLiteCommand(strSql, mConn);
                SQLiteDataReader reader = sql.ExecuteReader();
                while (reader.Read())
                {
                    FNList[i].Text = reader["FIRSTNAME"].ToString();
                    LNList[i].Text = reader["LASTNAME"].ToString();
                }
                index++;
                if (index >= IDList.Count)
                {
                    index = 0;
                }
            }
            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        
        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
           
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            dataForm.ShowDialog();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            setting.ShowDialog();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            string mDbPath = Application.StartupPath + "/DatabaseFinal.sqlite";
            mConn = new SQLiteConnection("Data Source=" + mDbPath);
            mConn.Open();
            using (SQLiteCommand mCmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS [PARTICIPANT] ('ROWID' INTEGER PRIMARY KEY ,'ID' varchar[50], 'FIRSTNAME' varchar[50], 'LASTNAME' varchar[50]);", mConn))
            {
                mCmd.ExecuteNonQuery();
            }
            using (SQLiteCommand mCmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS [WINNER] ('ROWID' INTEGER PRIMARY KEY ,'ID' varchar[50], 'FIRSTNAME' varchar[50], 'LASTNAME' varchar[50]);", mConn))
            {
                mCmd.ExecuteNonQuery();
            }
        }

        private void toolStripButton2_Click_1(object sender, EventArgs e)
        {
            history.ShowDialog();
        }

        private void optionsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            setting.ShowDialog();
        }
    }
}
