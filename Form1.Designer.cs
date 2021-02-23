
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
            this.printBoxDebug = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // turn_label
            // 
            this.turn_label.AutoSize = true;
            this.turn_label.Location = new System.Drawing.Point(921, 24);
            this.turn_label.Name = "turn_label";
            this.turn_label.Size = new System.Drawing.Size(31, 15);
            this.turn_label.TabIndex = 0;
            this.turn_label.Text = "Turn";
            // 
            // boardIntCont
            // 
            this.boardIntCont.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boardIntCont.Location = new System.Drawing.Point(842, 61);
            this.boardIntCont.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.boardIntCont.Multiline = true;
            this.boardIntCont.Name = "boardIntCont";
            this.boardIntCont.ReadOnly = true;
            this.boardIntCont.Size = new System.Drawing.Size(130, 109);
            this.boardIntCont.TabIndex = 1;
            // 
            // printBoxDebug
            // 
            this.printBoxDebug.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.printBoxDebug.Location = new System.Drawing.Point(842, 193);
            this.printBoxDebug.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.printBoxDebug.Multiline = true;
            this.printBoxDebug.Name = "printBoxDebug";
            this.printBoxDebug.ReadOnly = true;
            this.printBoxDebug.Size = new System.Drawing.Size(130, 109);
            this.printBoxDebug.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 961);
            this.Controls.Add(this.printBoxDebug);
            this.Controls.Add(this.boardIntCont);
            this.Controls.Add(this.turn_label);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "ö";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label turn_label;
        private System.Windows.Forms.TextBox boardIntCont;
        private System.Windows.Forms.TextBox printBoxDebug;
    }
}

