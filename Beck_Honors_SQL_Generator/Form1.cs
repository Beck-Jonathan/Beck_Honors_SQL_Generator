namespace Beck_Honors_SQL_Generator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
            settings.path = filepath;
        }
    }
}