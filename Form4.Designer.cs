
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
            this.label2 = new System.Windows.Forms.Label();
            this.getStockfishExeBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.compareStockfishBtn = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.stockfishPathLabel = new System.Windows.Forms.Label();
            this.runPerftBulkBtn = new System.Windows.Forms.Button();
            this.ComparisonDisplay = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBox1.Location = new System.Drawing.Point(119, 16);
            this.textBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(571, 314);
            this.textBox1.TabIndex = 0;
            // 
            // runTestBtn
            // 
            this.runTestBtn.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.runTestBtn.Location = new System.Drawing.Point(18, 16);
            this.runTestBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.runTestBtn.Name = "runTestBtn";
            this.runTestBtn.Size = new System.Drawing.Size(88, 20);
            this.runTestBtn.TabIndex = 1;
            this.runTestBtn.Text = "Run perft test";
            this.runTestBtn.UseVisualStyleBackColor = true;
            // 
            // depthInput
            // 
            this.depthInput.Location = new System.Drawing.Point(20, 119);
            this.depthInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.depthInput.Name = "depthInput";
            this.depthInput.Size = new System.Drawing.Size(77, 23);
            this.depthInput.TabIndex = 2;
            this.depthInput.Text = "3";
            this.depthInput.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 102);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Test depth";
            // 
            // divideBtn
            // 
            this.divideBtn.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.divideBtn.Location = new System.Drawing.Point(18, 65);
            this.divideBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.divideBtn.Name = "divideBtn";
            this.divideBtn.Size = new System.Drawing.Size(88, 20);
            this.divideBtn.TabIndex = 5;
            this.divideBtn.Text = "Run perft divide";
            this.divideBtn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(121, 333);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 15);
            this.label2.TabIndex = 6;
            this.label2.Text = "Select path to";
            // 
            // getStockfishExeBtn
            // 
            this.getStockfishExeBtn.Location = new System.Drawing.Point(242, 335);
            this.getStockfishExeBtn.Name = "getStockfishExeBtn";
            this.getStockfishExeBtn.Size = new System.Drawing.Size(75, 23);
            this.getStockfishExeBtn.TabIndex = 7;
            this.getStockfishExeBtn.Text = "Open";
            this.getStockfishExeBtn.UseVisualStyleBackColor = true;
            this.getStockfishExeBtn.Click += new System.EventHandler(this.getStockfishExeBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(121, 347);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "Stockfish executable";
            // 
            // compareStockfishBtn
            // 
            this.compareStockfishBtn.AutoSize = true;
            this.compareStockfishBtn.Location = new System.Drawing.Point(449, 340);
            this.compareStockfishBtn.Name = "compareStockfishBtn";
            this.compareStockfishBtn.Size = new System.Drawing.Size(15, 14);
            this.compareStockfishBtn.TabIndex = 9;
            this.compareStockfishBtn.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(350, 333);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "Compare results";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(350, 348);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "with Stockfish";
            // 
            // stockfishPathLabel
            // 
            this.stockfishPathLabel.AutoSize = true;
            this.stockfishPathLabel.ForeColor = System.Drawing.Color.DarkGray;
            this.stockfishPathLabel.Location = new System.Drawing.Point(121, 366);
            this.stockfishPathLabel.Name = "stockfishPathLabel";
            this.stockfishPathLabel.Size = new System.Drawing.Size(71, 15);
            this.stockfishPathLabel.TabIndex = 12;
            this.stockfishPathLabel.Text = "Path: empty";
            // 
            // runPerftBulkBtn
            // 
            this.runPerftBulkBtn.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.runPerftBulkBtn.Location = new System.Drawing.Point(18, 40);
            this.runPerftBulkBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.runPerftBulkBtn.Name = "runPerftBulkBtn";
            this.runPerftBulkBtn.Size = new System.Drawing.Size(88, 20);
            this.runPerftBulkBtn.TabIndex = 13;
            this.runPerftBulkBtn.Text = "Run perft bulk";
            this.runPerftBulkBtn.UseVisualStyleBackColor = true;
            // 
            // ComparisonDisplay
            // 
            this.ComparisonDisplay.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ComparisonDisplay.Location = new System.Drawing.Point(117, 393);
            this.ComparisonDisplay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ComparisonDisplay.Multiline = true;
            this.ComparisonDisplay.Name = "ComparisonDisplay";
            this.ComparisonDisplay.ReadOnly = true;
            this.ComparisonDisplay.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ComparisonDisplay.Size = new System.Drawing.Size(571, 314);
            this.ComparisonDisplay.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(549, 376);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 15);
            this.label6.TabIndex = 15;
            this.label6.Text = "Comparison mismatches";
            // 
            // Form4
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 720);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ComparisonDisplay);
            this.Controls.Add(this.runPerftBulkBtn);
            this.Controls.Add(this.stockfishPathLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.compareStockfishBtn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.getStockfishExeBtn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.divideBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.depthInput);
            this.Controls.Add(this.runTestBtn);
            this.Controls.Add(this.textBox1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button getStockfishExeBtn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox compareStockfishBtn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label stockfishPathLabel;
        private System.Windows.Forms.Button runPerftBulkBtn;
        private System.Windows.Forms.TextBox ComparisonDisplay;
        private System.Windows.Forms.Label label6;
    }
}