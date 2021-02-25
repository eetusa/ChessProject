using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ChessProject
{
    public partial class Form2 : Form
    {
        public Form2(String str)
        {
            InitializeComponent();
            label1.Text = "Checkmate. " + str + " won!";
        }


    }
}
