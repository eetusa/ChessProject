
namespace ChessProject
{
    partial class Form4
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.runTestBtn = new System.Windows.Forms.Button();
            this.depthInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.divideBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(136, 21);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(652, 417);
            this.textBox1.TabIndex = 0;
            // 
            // runTestBtn
            // 
            this.runTestBtn.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.runTestBtn.Location = new System.Drawing.Point(21, 21);
            this.runTestBtn.Name = "runTestBtn";
            this.runTestBtn.Size = new System.Drawing.Size(100, 26);
            this.runTestBtn.TabIndex = 1;
            this.runTestBtn.Text = "Run perft test";
            this.runTestBtn.UseVisualStyleBackColor = true;
            // 
            // depthInput
            // 
            this.depthInput.Location = new System.Drawing.Point(21, 129);
            this.depthInput.Name = "depthInput";
            this.depthInput.Size = new System.Drawing.Size(87, 27);
            this.depthInput.TabIndex = 2;
            this.depthInput.Text = "1";
            this.depthInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Test depth";
            // 
            // divideBtn
            // 
            this.divideBtn.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.divideBtn.Location = new System.Drawing.Point(21, 52);
            this.divideBtn.Name = "divideBtn";
            this.divideBtn.Size = new System.Drawing.Size(100, 26);
            this.divideBtn.TabIndex = 5;
            this.divideBtn.Text = "Run perft divide";
            this.divideBtn.UseVisualStyleBackColor = true;
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.divideBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.depthInput);
            this.Controls.Add(this.runTestBtn);
            this.Controls.Add(this.textBox1);
            this.Name = "Form4";
            this.Text = "Perft test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button runTestBtn;
        private System.Windows.Forms.TextBox depthInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button divideBtn;
    }
}