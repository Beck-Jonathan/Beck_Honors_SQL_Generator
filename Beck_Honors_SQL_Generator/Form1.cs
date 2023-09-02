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
        }
    }
}