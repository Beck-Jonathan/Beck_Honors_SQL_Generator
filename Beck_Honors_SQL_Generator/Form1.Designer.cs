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
            btn_choose_file = new Button();
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
            // btn_choose_file
            // 
            btn_choose_file.Location = new Point(61, 110);
            btn_choose_file.Name = "btn_choose_file";
            btn_choose_file.Size = new Size(75, 23);
            btn_choose_file.TabIndex = 2;
            btn_choose_file.Text = "choose_file";
            btn_choose_file.UseVisualStyleBackColor = true;
            btn_choose_file.Click += btn_choose_file_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btn_choose_file);
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
        private Button btn_choose_file;
    }
}