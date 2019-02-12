using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SQLServerHelper helper = new SQLServerHelper();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.comboBox1.SelectedIndex = 0;
            if (this.dataGridView1.Rows.Count != 0)
            {
                IList<Song> list = (IList<Song>)this.dataGridView1.DataSource;
                for (int i = 0; i < list.Count; i++)
                {
                    list.RemoveAt(i);
                }
                this.dataGridView1.DataSource = null;
            }
            this.dataGridView1.DataSource =GetSongs();
        }

        private List<Song> GetSongs()
        {
            List<Song> songlist = new List<Song>();
            helper.OpenConnection();
            string sql = "SELECT song_info.song_id,song_info.song_name,song_info.song_ab,song_info.song_word_count,song_type.songtype_name,singer_info.singer_name,song_info.song_url,song_info.song_play_count FROM song_info" +
                " INNER JOIN song_type ON song_info.songtype_id = song_type.songtype_id" +
                " INNER JOIN singer_info ON song_info.singer_id = singer_info.singer_id";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, helper.Connection);
            try
            {
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Song song = new Song
                        {
                            Id = (int)dr["song_id"],
                            SongName = dr["song_name"].ToString(),
                            PinYin = dr["song_ab"].ToString(),
                            WordCount = (int)dr["song_word_count"],
                            Type = dr["songtype_name"].ToString(),
                            Singer = dr["singer_name"].ToString(),
                            Url = dr["song_url"].ToString(),
                            PlayCount = (int)dr["song_play_count"]
                        };
                        songlist.Add(song);
                    }
                    return songlist;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                helper.CloseConnection();
            }
        }

        private List<Song> Filtered(List<Song> list, string type)
        {
            var q = from u in list
                    select u;
            if (type != "全部")
            {
                q = q.Where(p => p.Type == type);
            }
            return q.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dataGridView1.Rows.Count != 0)
            {
                IList<Song> list = (IList<Song>)this.dataGridView1.DataSource;
                for (int i = 0; i < list.Count; i++)
                {
                    list.RemoveAt(i);
                }
                this.dataGridView1.DataSource = null;
            }
            this.dataGridView1.DataSource = Filtered(GetSongs(),this.comboBox1.Text);
        }
    }
}
