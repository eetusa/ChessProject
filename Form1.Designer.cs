
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
            this.whiteAIButton = new System.Windows.Forms.CheckBox();
            this.blackAIButton = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // turn_label
            // 
            this.turn_label.AutoSize = true;
            this.turn_label.Location = new System.Drawing.Point(1053, 32);
            this.turn_label.Name = "turn_label";
            this.turn_label.Size = new System.Drawing.Size(38, 20);
            this.turn_label.TabIndex = 0;
            this.turn_label.Text = "Turn";
            // 
            // boardIntCont
            // 
            this.boardIntCont.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.boardIntCont.Location = new System.Drawing.Point(940, 81);
            this.boardIntCont.Multiline = true;
            this.boardIntCont.Name = "boardIntCont";
            this.boardIntCont.ReadOnly = true;
            this.boardIntCont.Size = new System.Drawing.Size(170, 144);
            this.boardIntCont.TabIndex = 1;
            // 
            // printBoxDebug
            // 
            this.printBoxDebug.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.printBoxDebug.Location = new System.Drawing.Point(940, 257);
            this.printBoxDebug.Multiline = true;
            this.printBoxDebug.Name = "printBoxDebug";
            this.printBoxDebug.ReadOnly = true;
            this.printBoxDebug.Size = new System.Drawing.Size(170, 233);
            this.printBoxDebug.TabIndex = 2;
            // 
            // whiteAIButton
            // 
            this.whiteAIButton.AutoSize = true;
            this.whiteAIButton.Location = new System.Drawing.Point(1030, 499);
            this.whiteAIButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.whiteAIButton.Name = "whiteAIButton";
            this.whiteAIButton.Size = new System.Drawing.Size(88, 24);
            this.whiteAIButton.TabIndex = 3;
            this.whiteAIButton.Text = "White AI";
            this.whiteAIButton.UseVisualStyleBackColor = true;
            // 
            // blackAIButton
            // 
            this.blackAIButton.AutoSize = true;
            this.blackAIButton.Location = new System.Drawing.Point(1030, 532);
            this.blackAIButton.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.blackAIButton.Name = "blackAIButton";
            this.blackAIButton.Size = new System.Drawing.Size(84, 24);
            this.blackAIButton.TabIndex = 4;
            this.blackAIButton.Text = "Black AI";
            this.blackAIButton.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1125, 1055);
            this.Controls.Add(this.blackAIButton);
            this.Controls.Add(this.whiteAIButton);
            this.Controls.Add(this.printBoxDebug);
            this.Controls.Add(this.boardIntCont);
            this.Controls.Add(this.turn_label);
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
        private System.Windows.Forms.CheckBox whiteAIButton;
        private System.Windows.Forms.CheckBox blackAIButton;
    }
}

