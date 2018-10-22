namespace GamingMacros
{
    partial class Main
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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.programEnterBtn = new System.Windows.Forms.Button();
            this.ProgramListBox = new System.Windows.Forms.ListBox();
            this.ProgramSearchBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.StartMacroButton = new System.Windows.Forms.Button();
            this.macroKeys = new System.Windows.Forms.ListBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(281, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "label1";
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(169, 320);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(75, 23);
            this.startBtn.TabIndex = 1;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 39.02743F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60.97257F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.programEnterBtn, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.ProgramListBox, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.ProgramSearchBox, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.StartMacroButton, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.macroKeys, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40.81633F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 59.18367F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 94F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 278F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(802, 452);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(307, 19);
            this.label2.TabIndex = 0;
            this.label2.Text = "Select Program";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // programEnterBtn
            // 
            this.programEnterBtn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.programEnterBtn.Location = new System.Drawing.Point(119, 145);
            this.programEnterBtn.Name = "programEnterBtn";
            this.programEnterBtn.Size = new System.Drawing.Size(75, 23);
            this.programEnterBtn.TabIndex = 2;
            this.programEnterBtn.Text = "Select";
            this.programEnterBtn.UseVisualStyleBackColor = true;
            this.programEnterBtn.Click += new System.EventHandler(this.programEnterBtn_Click);
            // 
            // ProgramListBox
            // 
            this.ProgramListBox.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ProgramListBox.FormattingEnabled = true;
            this.ProgramListBox.Location = new System.Drawing.Point(96, 50);
            this.ProgramListBox.Name = "ProgramListBox";
            this.ProgramListBox.Size = new System.Drawing.Size(120, 82);
            this.ProgramListBox.TabIndex = 1;
            // 
            // ProgramSearchBox
            // 
            this.ProgramSearchBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgramSearchBox.Location = new System.Drawing.Point(96, 23);
            this.ProgramSearchBox.Name = "ProgramSearchBox";
            this.ProgramSearchBox.Size = new System.Drawing.Size(120, 20);
            this.ProgramSearchBox.TabIndex = 3;
            this.ProgramSearchBox.Text = "Search";
            this.ProgramSearchBox.Click += new System.EventHandler(this.ProgramSearchBox_Click);
            this.ProgramSearchBox.TextChanged += new System.EventHandler(this.ProgramSearchBox_TextChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(520, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Record Macro";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.UseMnemonic = false;
            // 
            // StartMacroButton
            // 
            this.StartMacroButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.StartMacroButton.Location = new System.Drawing.Point(520, 22);
            this.StartMacroButton.Name = "StartMacroButton";
            this.StartMacroButton.Size = new System.Drawing.Size(75, 22);
            this.StartMacroButton.TabIndex = 5;
            this.StartMacroButton.Text = "Start";
            this.StartMacroButton.UseVisualStyleBackColor = true;
            this.StartMacroButton.Click += new System.EventHandler(this.StartMacroButton_Click);
            // 
            // macroKeys
            // 
            this.macroKeys.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.macroKeys.FormattingEnabled = true;
            this.macroKeys.Location = new System.Drawing.Point(497, 50);
            this.macroKeys.Name = "macroKeys";
            this.macroKeys.Size = new System.Drawing.Size(120, 82);
            this.macroKeys.TabIndex = 6;
            this.macroKeys.Visible = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.KeyPreview = true;
            this.Name = "Main";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Main_KeyPress);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Main_KeyPressEvent);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox ProgramListBox;
        private System.Windows.Forms.Button programEnterBtn;
        private System.Windows.Forms.TextBox ProgramSearchBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button StartMacroButton;
        private System.Windows.Forms.ListBox macroKeys;
    }
}

