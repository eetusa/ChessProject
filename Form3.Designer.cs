
namespace ChessProject
{
    partial class Form3
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
            this.button_queen = new System.Windows.Forms.Button();
            this.button_bishop = new System.Windows.Forms.Button();
            this.button_rook = new System.Windows.Forms.Button();
            this.button_knight = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Promote pawn to";
            // 
            // button_queen
            // 
            this.button_queen.Location = new System.Drawing.Point(37, 81);
            this.button_queen.Name = "button_queen";
            this.button_queen.Size = new System.Drawing.Size(94, 29);
            this.button_queen.TabIndex = 1;
            this.button_queen.Text = "Queen";
            this.button_queen.UseVisualStyleBackColor = true;
            this.button_queen.Click += new System.EventHandler(this.button_queen_Click);
            // 
            // button_bishop
            // 
            this.button_bishop.Location = new System.Drawing.Point(162, 81);
            this.button_bishop.Name = "button_bishop";
            this.button_bishop.Size = new System.Drawing.Size(94, 29);
            this.button_bishop.TabIndex = 2;
            this.button_bishop.Text = "Bishop";
            this.button_bishop.UseVisualStyleBackColor = true;
            this.button_bishop.Click += new System.EventHandler(this.button_bishop_Click);
            // 
            // button_rook
            // 
            this.button_rook.Location = new System.Drawing.Point(412, 81);
            this.button_rook.Name = "button_rook";
            this.button_rook.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button_rook.Size = new System.Drawing.Size(94, 29);
            this.button_rook.TabIndex = 3;
            this.button_rook.Text = "Rook";
            this.button_rook.UseVisualStyleBackColor = true;
            this.button_rook.Click += new System.EventHandler(this.button_rook_Click);
            // 
            // button_knight
            // 
            this.button_knight.Location = new System.Drawing.Point(287, 81);
            this.button_knight.Name = "button_knight";
            this.button_knight.Size = new System.Drawing.Size(94, 29);
            this.button_knight.TabIndex = 4;
            this.button_knight.Text = "Knight";
            this.button_knight.UseVisualStyleBackColor = true;
            this.button_knight.Click += new System.EventHandler(this.button_knight_Click_1);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 158);
            this.Controls.Add(this.button_knight);
            this.Controls.Add(this.button_rook);
            this.Controls.Add(this.button_bishop);
            this.Controls.Add(this.button_queen);
            this.Controls.Add(this.label1);
            this.Name = "Form3";
            this.Text = "Promotion";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_queen;
        private System.Windows.Forms.Button button_bishop;
        private System.Windows.Forms.Button button_rook;
        private System.Windows.Forms.Button button_knight;
    }
}