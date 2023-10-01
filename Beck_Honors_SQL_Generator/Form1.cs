using static Beck_Honors_SQL_Generator.Data_Objects;
using System.Data;

namespace Beck_Honors_SQL_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            //settings.app_path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            //settings.app_path = settings.app_path.Substring(0, settings.app_path.Length - 30);
            settings.TSQLMode = false;
            InitializeComponent();
            tbx_databasename.Text = "WFTDA_debug";
            settings.databaseName = tbx_databasename.Text;
            initialize_settings();
            for (int i = 0; i < 3; i++)
            {
                clb_options.SetItemChecked(i, true);
            }
            for (int i = 3; i < 12; i++)
            {
                clb_options.SetItemChecked(i, false);
            }
            btn_selectfile.Enabled = true;
            btn_read_table.Enabled = false;
            cbx_table_names.Enabled = false;
            btn_generateTable.Enabled = false;
            clb_options.Enabled = false;
        }

        private void btn_save_update_Click(object sender, EventArgs e)
        {
            String s = tbx_Update.Text;
            Logic_Layer.pass_note(s);
            MessageBox.Show("Update saved");
        }

        private void btn_choose_file_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "C:\\Users\\jjbec\\source\\repos\\Beck_Honors_SQL_Generator\\Beck_Honors_SQL_Generator\\Text_files\\";
            openFileDialog.ShowDialog();
            string filepath = openFileDialog.FileName;
            settings.applicationPath = filepath;
            btn_selectfile.Enabled = false;
            btn_read_table.Enabled = true;
            cbx_table_names.Enabled = false;
            btn_generateTable.Enabled = false;
            clb_options.Enabled = false;
        }

        private void btn_read_table_Click(object sender, EventArgs e)
        {
            file_read.readdata();

            settings.table_count = database.allTables.Count;
            for (int i = 0; i < settings.table_count; i++)
            {

            }
            foreach (table t in database.allTables)
            {
                settings.table_names.Add(t.name);
            }
            cbx_table_names.DataSource = settings.table_names;
            cbx_table_names.Enabled = true;
            btn_selectfile.Enabled = false;
            btn_read_table.Enabled = false;

            btn_generateTable.Enabled = true;
            clb_options.Enabled = true;
        }

        private void btn_generateTable_Click(object sender, System.EventArgs e)
        {

            String database_head = database.print_database_header();
            file_write.WriteBuddy.Write(database_head);
            int count = 0;
            List<Boolean> these_settings = new List<Boolean>();

            foreach (table t in database.allTables)
            {
                these_settings = settings.all_options[count];
                string s = "";
                List<String> st = new List<String>();
                // if selected, add rows to table
                if (these_settings[0])
                {
                    s = t.gen_columns();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add primary keys to table
                if (these_settings[1])
                {
                    s = t.gen_primary_keys();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add foreign keys to table
                if (these_settings[2])
                {
                    s = t.gen_foreign_keys();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, create a matching audit table
                if (these_settings[3])
                {
                    s = t.gen_audit_table();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add an SP_update to table
                if (these_settings[4] && !t.isLine)
                {
                    s = t.gen_update();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add an SP_delete to table
                if (these_settings[5] && !t.isLine)
                {
                    s = t.gen_delete();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add an SP_retreive that requires a PK
                if (these_settings[6] && !t.isLine)
                {
                    s = t.gen_retreive_by_key();
                    file_write.WriteBuddy.Write(s);
                }
                //if selected an an SP_retreive that shows all data in table
                if (these_settings[7] && !t.isLine)
                {
                    s = t.gen_retreive_by_all();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add SP_insert to add records to table
                if (these_settings[8] && !t.isLine)
                {
                    s = t.gen_insert();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add a trigger for inserts
                if (these_settings[9] && !t.isLine)
                {
                    s = t.gen_insert_trigger();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add a trigger for updates
                if (these_settings[10] && !t.isLine)
                {
                    s = t.gen_update_trigger();
                    file_write.WriteBuddy.Write(s);
                }
                // if selected, add a triger for delets
                if (these_settings[11])
                {
                    s = t.gen_delete_trigger();
                    file_write.WriteBuddy.Write(s);
                }




                count++;
            }
            file_write.WriteBuddy.Flush();
            MessageBox.Show("generation complete");
            this.Close();


        }
        private void initialize_settings()
        {
            for (int i = 0; i < settings.table_count; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    settings.all_options[i][j] = true;
                }

            }



        }

        private void clb_options_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Boolean> these_settings = new List<Boolean>();
            for (int i = 0; i < 12; i++)
            {
                these_settings.Add(clb_options.GetItemChecked(i));
            }
            settings.all_options[cbx_table_names.SelectedIndex] = these_settings;

        }
    }
}