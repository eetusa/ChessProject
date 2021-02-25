using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChessProject
{
    public partial class Form3 : Form
    {
        

        public Form3()
        {
            InitializeComponent();
            
        }

        public int returnValue { get; set; }

        private void button_queen_Click(object sender, EventArgs e)
        {
            this.returnValue = 9;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_bishop_Click(object sender, EventArgs e)
        {
            this.returnValue = 6;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_knight_Click_1(object sender, EventArgs e)
        {
            this.returnValue = 7;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_rook_Click(object sender, EventArgs e)
        {
            this.returnValue = 8;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
