namespace Beck_Honors_SQL_Generator
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editoc.
        /// </summary>
        private void InitializeComponent()
        {
            tbx_Update = new TextBox();
            btn_save_update = new Button();
            btn_selectfile = new Button();
            tbx_databasename = new TextBox();
            cbx_table_names = new ComboBox();
            btn_generateTable = new Button();
            cbxTSql = new CheckBox();
            clb_options = new CheckedListBox();
            btn_read_table = new Button();
            SuspendLayout();
            // 
            // tbx_Update
            // 
            tbx_Update.Location = new Point(307, 334);
            tbx_Update.Name = "tbx_Update";
            tbx_Update.Size = new Size(228, 23);
            tbx_Update.TabIndex = 0;
            // 
            // btn_save_update
            // 
            btn_save_update.Location = new Point(556, 334);
            btn_save_update.Name = "btn_save_update";
            btn_save_update.Size = new Size(75, 23);
            btn_save_update.TabIndex = 1;
            btn_save_update.Text = "save update";
            btn_save_update.UseVisualStyleBackColor = true;
            btn_save_update.Click += btn_save_update_Click;
            // 
            // btn_selectfile
            // 
            btn_selectfile.Location = new Point(61, 110);
            btn_selectfile.Name = "btn_selectfile";
            btn_selectfile.Size = new Size(75, 23);
            btn_selectfile.TabIndex = 2;
            btn_selectfile.Text = "choose_file";
            btn_selectfile.UseVisualStyleBackColor = true;
            btn_selectfile.Click += btn_choose_file_Click;
            // 
            // tbx_databasename
            // 
            tbx_databasename.Location = new Point(61, 155);
            tbx_databasename.Name = "tbx_databasename";
            tbx_databasename.Size = new Size(100, 23);
            tbx_databasename.TabIndex = 3;
            // 
            // cbx_table_names
            // 
            cbx_table_names.FormattingEnabled = true;
            cbx_table_names.Location = new Point(190, 155);
            cbx_table_names.Name = "cbx_table_names";
            cbx_table_names.Size = new Size(121, 23);
            cbx_table_names.TabIndex = 4;
            // 
            // btn_generateTable
            // 
            btn_generateTable.Location = new Point(527, 47);
            btn_generateTable.Name = "btn_generateTable";
            btn_generateTable.Size = new Size(75, 23);
            btn_generateTable.TabIndex = 5;
            btn_generateTable.Text = "Generate Database";
            btn_generateTable.UseVisualStyleBackColor = true;
            btn_generateTable.Click += btn_generateTable_Click;
            // 
            // cbxTSql
            // 
            cbxTSql.AutoSize = true;
            cbxTSql.Location = new Point(62, 272);
            cbxTSql.Name = "cbxTSql";
            cbxTSql.Size = new Size(165, 19);
            cbxTSql.TabIndex = 6;
            cbxTSql.Text = "TSQLMode? (Experimental";
            cbxTSql.UseVisualStyleBackColor = true;
            // 
            // clb_options
            // 
            clb_options.FormattingEnabled = true;
            clb_options.Items.AddRange(new object[] { "Table", "Primary Keys", "Foreign Keys", "Audit Table", "SP_Update", "SP_Delete", "SP_Retreive_By_Key", "SP_Retreive_by_All", "SP_insert", "Trigger_Update", "Trigger_Insert", "Trigger_Delete" });
            clb_options.Location = new Point(477, 97);
            clb_options.Name = "clb_options";
            clb_options.Size = new Size(208, 220);
            clb_options.TabIndex = 7;
            clb_options.SelectedIndexChanged += clb_options_SelectedIndexChanged;
            // 
            // btn_read_table
            // 
            btn_read_table.Location = new Point(197, 109);
            btn_read_table.Name = "btn_read_table";
            btn_read_table.Size = new Size(75, 23);
            btn_read_table.TabIndex = 8;
            btn_read_table.Text = "Read File";
            btn_read_table.UseVisualStyleBackColor = true;
            btn_read_table.Click += btn_read_table_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btn_read_table);
            Controls.Add(clb_options);
            Controls.Add(cbxTSql);
            Controls.Add(btn_generateTable);
            Controls.Add(cbx_table_names);
            Controls.Add(tbx_databasename);
            Controls.Add(btn_selectfile);
            Controls.Add(btn_save_update);
            Controls.Add(tbx_Update);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbx_Update;
        private Button btn_save_update;
        private Button btn_selectfile;
        private TextBox tbx_databasename;
        private ComboBox cbx_table_names;
        private Button btn_generateTable;
        private CheckBox cbxTSql;
        private CheckedListBox clb_options;
        private Button btn_read_table;
    }
}