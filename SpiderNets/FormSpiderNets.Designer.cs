namespace SpiderNets
{
    partial class FormSpiderNets
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        private void InitializeComponent()
        {
            pnlContent = new Panel();
            SuspendLayout();
            // 
            // pnlContent
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(0, 0);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(243, 140);
            pnlContent.TabIndex = 0;
            // 
            // Monitor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            TopMost = false;
            ClientSize = new Size(243, 140);
            Controls.Add(pnlContent);
            Name = "Monitor";
            Text = "Form1";

            ResumeLayout(false);
        }

        #endregion

        private Panel pnlContent;
    }
}