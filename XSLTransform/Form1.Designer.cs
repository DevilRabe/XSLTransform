namespace XSLTransform
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
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            BtnRun = new Button();
            SuspendLayout();
            // 
            // BtnRun
            // 
            BtnRun.Location = new Point(116, 75);
            BtnRun.Name = "BtnRun";
            BtnRun.Size = new Size(100, 29);
            BtnRun.TabIndex = 0;
            BtnRun.Text = "Преобразовать";
            BtnRun.UseVisualStyleBackColor = true;
            BtnRun.Click += BtnRun_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnRun);
            Name = "Form1";
            Text = "XSLTransform";
            ResumeLayout(false);
        }

        #endregion

        private Button BtnRun;
    }
}
