using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoteTaking
{
    public partial class NoteTaking : Form
    {
        string path = "notes.json";
        List<Note> notes;
        DataTable table;
        public NoteTaking()
        {
            InitializeComponent();
        }

        private void NoteTaking_Load(object sender, EventArgs e)
        {
            table = new DataTable();
            table.Columns.Add("Title", typeof(String));
            table.Columns.Add("Messages", typeof(String));

            dataGridView1.DataSource = table;

            dataGridView1.Columns["Messages"].Visible = false;
            dataGridView1.Columns["Title"].Width = 240;

            notes = InitNotes();
        }

        private List<Note> InitNotes()
        {
            if(File.Exists(path))
            {
                using(StreamReader sr = new StreamReader(path))
                {
                    string json = sr.ReadToEnd();
                    List<Note> notes = JsonConvert.DeserializeObject<List<Note>>(json);
                    if(notes == null)
                    {
                        return new List<Note>();
                    }
                    else
                    {
                        foreach(Note note in notes)
                        {
                            table.Rows.Add(note.Title, note.Message);
                        }
                        return notes;
                    }
                }
            }
            else
            {
                using (FileStream fs = new FileStream(path, FileMode.Create)) { }
                return new List<Note>();
                    
            }
        }

        private void buttonNew_Click(object sender, EventArgs e)
        {
            textTitle.Clear();
            textMessage.Clear();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            table.Rows.Add(textTitle.Text, textMessage.Text);
            notes.Add(new Note(textTitle.Text, textMessage.Text));
            textTitle.Clear();
            textMessage.Clear();
        }

        private void buttonRead_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;

            if(index > -1)
            {
                textTitle.Text = table.Rows[index].ItemArray[0].ToString();
                textMessage.Text = table.Rows[index].ItemArray[1].ToString();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if(dataGridView1.CurrentCell != null)
            {
                int index = dataGridView1.CurrentCell.RowIndex;
                notes.RemoveAt(index);
                table.Rows[index].Delete();
            }
            else
            {
                return;
            }
        }

        private void NoteTaking_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                string json = JsonConvert.SerializeObject(notes, Formatting.Indented);
                sw.WriteLine(json);
            }
        }
    }
}
