namespace TicTacToe_Project
{
    partial class SettingsForm
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
            this.sizeL = new System.Windows.Forms.Label();
            this.winL = new System.Windows.Forms.Label();
            this.xL = new System.Windows.Forms.Label();
            this.oL = new System.Windows.Forms.Label();
            this.sizeN = new System.Windows.Forms.NumericUpDown();
            this.winN = new System.Windows.Forms.NumericUpDown();
            this.colorD = new System.Windows.Forms.ColorDialog();
            this.xB = new System.Windows.Forms.Button();
            this.oB = new System.Windows.Forms.Button();
            this.backB = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.sizeN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.winN)).BeginInit();
            this.SuspendLayout();
            // 
            // sizeL
            // 
            this.sizeL.Location = new System.Drawing.Point(14, 26);
            this.sizeL.Name = "sizeL";
            this.sizeL.Size = new System.Drawing.Size(105, 23);
            this.sizeL.TabIndex = 0;
            this.sizeL.Text = "Размер поля:";
            // 
            // winL
            // 
            this.winL.AutoSize = true;
            this.winL.Location = new System.Drawing.Point(14, 84);
            this.winL.Name = "winL";
            this.winL.Size = new System.Drawing.Size(135, 17);
            this.winL.TabIndex = 1;
            this.winL.Text = "Фигур для победы:";
            // 
            // xL
            // 
            this.xL.AutoSize = true;
            this.xL.Location = new System.Drawing.Point(14, 133);
            this.xL.Name = "xL";
            this.xL.Size = new System.Drawing.Size(116, 17);
            this.xL.TabIndex = 2;
            this.xL.Text = "Цвет крестиков:";
            // 
            // oL
            // 
            this.oL.AutoSize = true;
            this.oL.Location = new System.Drawing.Point(14, 184);
            this.oL.Name = "oL";
            this.oL.Size = new System.Drawing.Size(103, 17);
            this.oL.TabIndex = 3;
            this.oL.Text = "Цвет ноликов:";
            // 
            // sizeN
            // 
            this.sizeN.Location = new System.Drawing.Point(155, 24);
            this.sizeN.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.sizeN.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.sizeN.Name = "sizeN";
            this.sizeN.Size = new System.Drawing.Size(100, 22);
            this.sizeN.TabIndex = 4;
            this.sizeN.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // winN
            // 
            this.winN.Location = new System.Drawing.Point(155, 82);
            this.winN.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.winN.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.winN.Name = "winN";
            this.winN.Size = new System.Drawing.Size(100, 22);
            this.winN.TabIndex = 5;
            this.winN.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // xB
            // 
            this.xB.Location = new System.Drawing.Point(155, 125);
            this.xB.Name = "xB";
            this.xB.Size = new System.Drawing.Size(100, 32);
            this.xB.TabIndex = 6;
            this.xB.UseVisualStyleBackColor = true;
            this.xB.Click += new System.EventHandler(this.xB_Click);
            // 
            // oB
            // 
            this.oB.Location = new System.Drawing.Point(155, 176);
            this.oB.Name = "oB";
            this.oB.Size = new System.Drawing.Size(100, 32);
            this.oB.TabIndex = 7;
            this.oB.UseVisualStyleBackColor = true;
            this.oB.Click += new System.EventHandler(this.oB_Click);
            // 
            // backB
            // 
            this.backB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.backB.Location = new System.Drawing.Point(72, 225);
            this.backB.Name = "backB";
            this.backB.Size = new System.Drawing.Size(124, 42);
            this.backB.TabIndex = 8;
            this.backB.Text = "Назад";
            this.backB.UseVisualStyleBackColor = true;
            this.backB.Click += new System.EventHandler(this.backB_Click);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(273, 279);
            this.Controls.Add(this.backB);
            this.Controls.Add(this.oB);
            this.Controls.Add(this.xB);
            this.Controls.Add(this.winN);
            this.Controls.Add(this.sizeN);
            this.Controls.Add(this.oL);
            this.Controls.Add(this.xL);
            this.Controls.Add(this.winL);
            this.Controls.Add(this.sizeL);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки";
            ((System.ComponentModel.ISupportInitialize)(this.sizeN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.winN)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label sizeL;
        private System.Windows.Forms.Label winL;
        private System.Windows.Forms.Label xL;
        private System.Windows.Forms.Label oL;
        private System.Windows.Forms.NumericUpDown sizeN;
        private System.Windows.Forms.NumericUpDown winN;
        private System.Windows.Forms.ColorDialog colorD;
        private System.Windows.Forms.Button xB;
        private System.Windows.Forms.Button oB;
        private System.Windows.Forms.Button backB;
    }
}