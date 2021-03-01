
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
            this.newgame_button = new System.Windows.Forms.Button();
            this.test_button = new System.Windows.Forms.Button();
            this.testBoard1Btn = new System.Windows.Forms.Button();
            this.printFENBtn = new System.Windows.Forms.Button();
            this.fenDisp = new System.Windows.Forms.TextBox();
            this.loadFENBtn = new System.Windows.Forms.Button();
            this.copyFENBtn = new System.Windows.Forms.Button();
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
            this.boardIntCont.Location = new System.Drawing.Point(822, 61);
            this.boardIntCont.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.boardIntCont.Multiline = true;
            this.boardIntCont.Name = "boardIntCont";
            this.boardIntCont.ReadOnly = true;
            this.boardIntCont.Size = new System.Drawing.Size(149, 109);
            this.boardIntCont.TabIndex = 1;
            // 
            // printBoxDebug
            // 
            this.printBoxDebug.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.printBoxDebug.Location = new System.Drawing.Point(822, 193);
            this.printBoxDebug.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.printBoxDebug.Multiline = true;
            this.printBoxDebug.Name = "printBoxDebug";
            this.printBoxDebug.ReadOnly = true;
            this.printBoxDebug.Size = new System.Drawing.Size(149, 176);
            this.printBoxDebug.TabIndex = 2;
            // 
            // whiteAIButton
            // 
            this.whiteAIButton.AutoSize = true;
            this.whiteAIButton.Location = new System.Drawing.Point(901, 374);
            this.whiteAIButton.Name = "whiteAIButton";
            this.whiteAIButton.Size = new System.Drawing.Size(71, 19);
            this.whiteAIButton.TabIndex = 3;
            this.whiteAIButton.Text = "White AI";
            this.whiteAIButton.UseVisualStyleBackColor = true;
            // 
            // blackAIButton
            // 
            this.blackAIButton.AutoSize = true;
            this.blackAIButton.Location = new System.Drawing.Point(901, 399);
            this.blackAIButton.Name = "blackAIButton";
            this.blackAIButton.Size = new System.Drawing.Size(68, 19);
            this.blackAIButton.TabIndex = 4;
            this.blackAIButton.Text = "Black AI";
            this.blackAIButton.UseVisualStyleBackColor = true;
            // 
            // newgame_button
            // 
            this.newgame_button.Location = new System.Drawing.Point(892, 474);
            this.newgame_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.newgame_button.Name = "newgame_button";
            this.newgame_button.Size = new System.Drawing.Size(82, 22);
            this.newgame_button.TabIndex = 5;
            this.newgame_button.Text = "New game";
            this.newgame_button.UseVisualStyleBackColor = true;
            // 
            // test_button
            // 
            this.test_button.Location = new System.Drawing.Point(896, 512);
            this.test_button.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.test_button.Name = "test_button";
            this.test_button.Size = new System.Drawing.Size(82, 22);
            this.test_button.TabIndex = 6;
            this.test_button.Text = "Tests";
            this.test_button.UseVisualStyleBackColor = true;
            // 
            // testBoard1Btn
            // 
            this.testBoard1Btn.Location = new System.Drawing.Point(732, 474);
            this.testBoard1Btn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.testBoard1Btn.Name = "testBoard1Btn";
            this.testBoard1Btn.Size = new System.Drawing.Size(82, 22);
            this.testBoard1Btn.TabIndex = 7;
            this.testBoard1Btn.Text = "test board1";
            this.testBoard1Btn.UseVisualStyleBackColor = true;
            // 
            // printFENBtn
            // 
            this.printFENBtn.Location = new System.Drawing.Point(670, 588);
            this.printFENBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.printFENBtn.Name = "printFENBtn";
            this.printFENBtn.Size = new System.Drawing.Size(82, 22);
            this.printFENBtn.TabIndex = 8;
            this.printFENBtn.Text = "Print FEN";
            this.printFENBtn.UseVisualStyleBackColor = true;
            // 
            // fenDisp
            // 
            this.fenDisp.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fenDisp.Location = new System.Drawing.Point(670, 553);
            this.fenDisp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.fenDisp.Name = "fenDisp";
            this.fenDisp.Size = new System.Drawing.Size(308, 20);
            this.fenDisp.TabIndex = 10;
            // 
            // loadFENBtn
            // 
            this.loadFENBtn.Location = new System.Drawing.Point(758, 588);
            this.loadFENBtn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.loadFENBtn.Name = "loadFENBtn";
            this.loadFENBtn.Size = new System.Drawing.Size(82, 22);
            this.loadFENBtn.TabIndex = 11;
            this.loadFENBtn.Text = "Load FEN";
            this.loadFENBtn.UseVisualStyleBackColor = true;
            // 
            // copyFENBtn
            // 
            this.copyFENBtn.Location = new System.Drawing.Point(847, 588);
            this.copyFENBtn.Name = "copyFENBtn";
            this.copyFENBtn.Size = new System.Drawing.Size(75, 23);
            this.copyFENBtn.TabIndex = 12;
            this.copyFENBtn.Text = "Copy FEN";
            this.copyFENBtn.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 782);
            this.Controls.Add(this.copyFENBtn);
            this.Controls.Add(this.loadFENBtn);
            this.Controls.Add(this.fenDisp);
            this.Controls.Add(this.printFENBtn);
            this.Controls.Add(this.testBoard1Btn);
            this.Controls.Add(this.test_button);
            this.Controls.Add(this.newgame_button);
            this.Controls.Add(this.blackAIButton);
            this.Controls.Add(this.whiteAIButton);
            this.Controls.Add(this.printBoxDebug);
            this.Controls.Add(this.boardIntCont);
            this.Controls.Add(this.turn_label);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Chess engine";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label turn_label;
        private System.Windows.Forms.TextBox boardIntCont;
        private System.Windows.Forms.TextBox printBoxDebug;
        private System.Windows.Forms.CheckBox whiteAIButton;
        private System.Windows.Forms.CheckBox blackAIButton;
        private System.Windows.Forms.Button newgame_button;
        private System.Windows.Forms.Button test_button;
        private System.Windows.Forms.Button testBoard1Btn;
        private System.Windows.Forms.Button printFENBtn;
        private System.Windows.Forms.TextBox fenDisp;
        private System.Windows.Forms.Button loadFENBtn;
        private System.Windows.Forms.Button copyFENBtn;
    }
}

