
namespace ChessProject
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
            this.turn_label = new System.Windows.Forms.Label();
            this.boardIntCont = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // turn_label
            // 
            this.turn_label.AutoSize = true;
            this.turn_label.Location = new System.Drawing.Point(714, 61);
            this.turn_label.Name = "turn_label";
            this.turn_label.Size = new System.Drawing.Size(38, 20);
            this.turn_label.TabIndex = 0;
            this.turn_label.Text = "Turn";
            // 
            // boardIntCont
            // 
            this.boardIntCont.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boardIntCont.Location = new System.Drawing.Point(640, 111);
            this.boardIntCont.Multiline = true;
            this.boardIntCont.Name = "boardIntCont";
            this.boardIntCont.ReadOnly = true;
            this.boardIntCont.Size = new System.Drawing.Size(148, 144);
            this.boardIntCont.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 771);
            this.Controls.Add(this.boardIntCont);
            this.Controls.Add(this.turn_label);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label turn_label;
        private System.Windows.Forms.TextBox boardIntCont;
    }
}

