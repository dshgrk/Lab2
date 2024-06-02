namespace GUIbot
{
    partial class MainForm
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
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            cbBotRun = new CheckBox();
            SuspendLayout();
            // 
            // cbBotRun
            // 
            cbBotRun.AutoSize = true;
            cbBotRun.Location = new Point(209, 152);
            cbBotRun.Name = "cbBotRun";
            cbBotRun.Size = new Size(83, 24);
            cbBotRun.TabIndex = 0;
            cbBotRun.Text = "Bot Run";
            cbBotRun.UseVisualStyleBackColor = true;
            cbBotRun.CheckedChanged += OnRunClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(520, 357);
            Controls.Add(cbBotRun);
            Name = "MainForm";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox cbBotRun;
    }
}
