using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace ChessProject
{
    public partial class Form4 : Form
    {
        public Form4(Board board)
        {
            InitializeComponent();
            runTestBtn.Click += (sender, EventArgs) =>
            {
                runTest(board);
            };

            divideBtn.Click += (sender, EventArgs) =>
            {
                runDivideTest(board);
            };
        }

        private void runTest(Board board)
        {
            
 
            textBox1.Text = "";
            int[] expectedResults =
            {
               0, 20, 400, 8902, 197281, 4865609, 119060324
            };
            //try (Int32.Parse())
            //label1.Text = ChessAI.MoveGenerationTest();
            try
            {
                int depth = Int32.Parse(depthInput.Text);
                if (depth > 10)
                {
                    depth = 10;
                }
                for (int i = 1; i < depth+1; i++)
                {
                    Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Restart();
                    Int64 result = ChessAI.MoveGenerationTest(i, board.board, board.turn);
                    sw.Stop();
                    textBox1.AppendText("Depth: " + i + "  Result: " + result +"  Time: "+ sw.Elapsed + "  Test: " + (result==expectedResults[i] ? "Pass" : "Fail"));
                    if (result != expectedResults[i])
                    {
                        textBox1.AppendText(". Expected result: " + expectedResults[i]);
                    }

                    textBox1.AppendText(Environment.NewLine);
                }
            }
            catch
            {

            }
        }

        private void runDivideTest(Board board)
        {

            textBox1.Text = "";
            try
            {
                int depth = Int32.Parse(depthInput.Text);
                if (depth > 10)
                {
                    depth = 10;
                }
                Stopwatch sw = new System.Diagnostics.Stopwatch();
                sw.Restart();
                Int64 result = ChessAI.MoveGenerationTestDivide(depth, board.board, board.turn, depth, textBox1);
                sw.Stop();                
            }
            catch
            {

            }

        }


    }
}
