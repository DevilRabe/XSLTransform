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
            dgvEmployees = new DataGridView();
            dgvMonthly = new DataGridView();
            lblEmployees = new Label();
            lblMonthly = new Label();
            BtnAdd = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvMonthly).BeginInit();
            SuspendLayout();
            // 
            // BtnRun
            // 
            BtnRun.Location = new Point(12, 12);
            BtnRun.Name = "BtnRun";
            BtnRun.Size = new Size(100, 29);
            BtnRun.TabIndex = 0;
            BtnRun.Text = "Преобразовать";
            BtnRun.UseVisualStyleBackColor = true;
            BtnRun.Click += BtnRun_Click;
            // 
            // dgvEmployees
            // 
            dgvEmployees.AllowUserToAddRows = false;
            dgvEmployees.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.Location = new Point(10, 70);
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.ReadOnly = true;
            dgvEmployees.Size = new Size(380, 300);
            dgvEmployees.TabIndex = 2;
            dgvEmployees.SelectionChanged += dgvEmployees_SelectionChanged;
            // 
            // dgvMonthly
            // 
            dgvMonthly.AllowUserToAddRows = false;
            dgvMonthly.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvMonthly.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvMonthly.Location = new Point(400, 70);
            dgvMonthly.Name = "dgvMonthly";
            dgvMonthly.ReadOnly = true;
            dgvMonthly.Size = new Size(380, 300);
            dgvMonthly.TabIndex = 4;
            // 
            // lblEmployees
            // 
            lblEmployees.AutoSize = true;
            lblEmployees.Location = new Point(10, 50);
            lblEmployees.Name = "lblEmployees";
            lblEmployees.Size = new Size(76, 15);
            lblEmployees.TabIndex = 1;
            lblEmployees.Text = "Сотрудники:";
            // 
            // lblMonthly
            // 
            lblMonthly.AutoSize = true;
            lblMonthly.Location = new Point(400, 50);
            lblMonthly.Name = "lblMonthly";
            lblMonthly.Size = new Size(129, 15);
            lblMonthly.TabIndex = 3;
            lblMonthly.Text = "Выплаты по месяцам:";
            // 
            // BtnAdd
            // 
            BtnAdd.Location = new Point(118, 15);
            BtnAdd.Name = "BtnAdd";
            BtnAdd.Size = new Size(108, 23);
            BtnAdd.TabIndex = 5;
            BtnAdd.Text = "Добавить запись";
            BtnAdd.UseVisualStyleBackColor = true;
            BtnAdd.Click += BtnAdd_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(BtnAdd);
            Controls.Add(BtnRun);
            Controls.Add(lblEmployees);
            Controls.Add(dgvEmployees);
            Controls.Add(lblMonthly);
            Controls.Add(dgvMonthly);
            Name = "Form1";
            Text = "XSLTransform";
            ((System.ComponentModel.ISupportInitialize)dgvEmployees).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvMonthly).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button BtnRun;
        private DataGridView dgvEmployees;
        private DataGridView dgvMonthly;
        private Label lblEmployees;
        private Label lblMonthly;
        private Button BtnAdd;
    }
}
