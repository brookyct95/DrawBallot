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

namespace DrawBallot
{
    public partial class History : Form
    {
        SQLiteConnection mConn;
        SQLiteDataAdapter mAdapter;
        public DataTable mTable;
        public History()
        {
            InitializeComponent();
            string mDbPath = Application.StartupPath + "/DatabaseFinal.sqlite";
            mConn = new SQLiteConnection("Data Source=" + mDbPath);
            mConn.Open();
            mAdapter = new SQLiteDataAdapter("SELECT * FROM [WINNER]", mConn);
            mTable = new DataTable(); // Don't forget initialize!
            mAdapter.Fill(mTable);
            new SQLiteCommandBuilder(mAdapter);
            dataGridView1.DataSource = mTable;
        }

        private void History_Load(object sender, EventArgs e)
        {
            
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            mTable.Clear();
            var a = 1;
        }
    }
}
