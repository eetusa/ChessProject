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
        string stockfishPath;
        List<string> test;

        public Form4(Board board)
        {
            this.stockfishPath = "";
            
            InitializeComponent();
            runTestBtn.Click += (sender, EventArgs) =>
            {
                runTest(board);
            };

            divideBtn.Click += (sender, EventArgs) =>
            {
                runDivideTest(board);
            };

            runPerftBulkBtn.Click += (sender, EventArgs) =>
            {
                RunBulkTest(board);
            };

            test = new List<string>();
            
        }

        private void runTest(Board board)
        {
            ComparisonDisplay.Text = "";
            textBox1.Text = "";
            int[] expectedResults =
            {
               0, 20, 400, 8902, 197281, 4865609, 119060324
            };

            List<Dictionary<string, Int64>> ownResults = new List<Dictionary<string, Int64>>();
            List<Dictionary<string, Int64>> stockResults = new List<Dictionary<string, Int64>>();


            try
            {
                int depth = Int32.Parse(depthInput.Text);
                if (depth > 5)
                {
                    depth = 6;
                }
                for (int i = 1; i < depth+1; i++)
                {
                    Dictionary<string, Int64> ownPerftResult = new Dictionary<string, Int64>();
                    Stopwatch sw = new Stopwatch();
                    sw.Restart();
                    Int64 result = ChessAI.MoveGenerationTest(i, board.board, board.turn);
                    ownPerftResult.Add("nodes", result);
                    sw.Stop();
                    ownPerftResult.Add("time", sw.ElapsedMilliseconds);
                    ownResults.Add(ownPerftResult);
                    if (!compareStockfishBtn.Checked)
                    {
                        textBox1.AppendText("Depth: " + depth + "  Nodes: " + result + "  Time: " + sw.Elapsed);
                        textBox1.AppendText(Environment.NewLine);
                    }
                }

                if (compareStockfishBtn.Checked)
                {
                    for (int i = 1; i < depth + 1; i++)
                    {
                        Dictionary<string, Int64> stockPerftResult = new Dictionary<string, Int64>();
                        callStockfish(ChessAI.BoardToFEN(board.board, board.turn), i, stockPerftResult);
                        stockResults.Add(stockPerftResult);
                    }
                    for (int i = 0; i < depth; i++)
                    {
                        ComparePerfts(stockResults[i], ownResults[i], 0, i);
                    }
                } 
            }
            catch
            {

            }
        }


        private void ComparePerfts(Dictionary<string, Int64> result, Dictionary<string, Int64> ownPerftResult, int typeOfCall, int depthMinus)
        {

            if (typeOfCall == 1)
            {
                ComparisonDisplay.Text = "";
                foreach (KeyValuePair<string, Int64> entry in result)
                {
                    //Debug.WriteLine(entry.Key + " : " + entry.Value);
                    if (ownPerftResult.ContainsKey(entry.Key))
                    {
                        if (ownPerftResult[entry.Key] != entry.Value)
                        {
                            ComparisonDisplay.AppendText(entry.Key + " " + ownPerftResult[entry.Key] + " - " + entry.Value);
                            ComparisonDisplay.AppendText(Environment.NewLine);
                        }
                    }
                    else
                    {
                        ComparisonDisplay.AppendText(entry.Key + " not found in own results");
                        ComparisonDisplay.AppendText(Environment.NewLine);
                    }
                }
                foreach (KeyValuePair<string, Int64> entry in ownPerftResult)
                {
                    //Debug.WriteLine(entry.Key + " : " + entry.Value);
                    if (!result.ContainsKey(entry.Key) && entry.Key!="time")
                    {
                        ComparisonDisplay.AppendText(entry.Key + " not found in comparison results");
                    }
                }

                PrintPerft(ownPerftResult);
                depthMinus--;
            }
            
            Int64 stockNodes = result["nodes"];
            Int64 ownNodes = ownPerftResult["nodes"];
            Int64 time = ownPerftResult["time"];
            int depth = depthMinus + 1;
            

            textBox1.AppendText("Depth: " + depth + "  Nodes: " + ownNodes + "  Time: " + time + "  Test: " + (ownNodes == stockNodes ? "Pass" : "Fail"));
            if (stockNodes != ownNodes)
            {
                textBox1.AppendText(". Expected result: " + stockNodes);
            }
            textBox1.AppendText(Environment.NewLine);
            
            
            

        }

        private void ComparePerfts(Dictionary<string, Int64> result, int depth)
        {

            //foreach (KeyValuePair<string, Int64> entry in result)
            //{
            //    if (ownPerftResult.ContainsKey(entry.Key))
            //    {
            //        if (ownPerftResult[entry.Key] != entry.Value)
            //        {
            //            test.Add(entry.Key + " " + ownPerftResult[entry.Key] + " - " + entry.Value);
            //        }
            //    }
            //    else
            //    {
            //        test.Add(entry.Key + " not found in own results");
            //    }
            //}
            //foreach (KeyValuePair<string, Int64> entry in ownPerftResult)
            //{
            //    if (!result.ContainsKey(entry.Key))
            //    {
            //        test.Add(entry.Key + " not found in comparison results");
            //    }
            //}
            //Int64 nodes = result["nodes"];
            //Int64 ownNodes = ownPerftResult["nodes"];
            //FinalizeComparison(nodes, ownNodes, depth);
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
                Dictionary<string, Int64> ownPerftResult = new Dictionary<string, Int64>();

                
                Stopwatch sw = new System.Diagnostics.Stopwatch();


                sw.Restart();
                Int64 result = ChessAI.MoveGenerationTestDivide(depth, board.board, board.turn, depth, ownPerftResult);
                ownPerftResult.Add("nodes", result);
                sw.Stop();
                ownPerftResult.Add("time", sw.ElapsedMilliseconds);

                
                if (compareStockfishBtn.Checked)
                {
                    Dictionary<string, Int64> stockPerftResult = new Dictionary<string, Int64>();
                    callStockfish(ChessAI.BoardToFEN(board.board, board.turn), depth, stockPerftResult);
                    ComparePerfts(stockPerftResult, ownPerftResult, 1, depth);
                } else
                {
                    PrintPerft(ownPerftResult);
                }

                
            }
            catch
            {

            }

        }

        private void PrintPerft(Dictionary<string, Int64> perft)
        {
            foreach (KeyValuePair<string, Int64> entry in perft)
            {
                textBox1.AppendText(entry.Key + ": " + entry.Value + Environment.NewLine);

            }
            textBox1.AppendText(Environment.NewLine);
        }

        private void RunBulkTest(Board board)
        {
            textBox1.Text = "";
            int[] expectedResults =
{
               0, 20, 400, 8902, 197281, 4865609, 119060324
            };
            try
            {
                int depth = Int32.Parse(depthInput.Text);
                if (depth > 5)
                {
                    depth = 6;
                }
                for (int i = 1; i < depth + 1; i++)
                {
                    Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Restart();
                    Int64 result = ChessAI.MoveGenerationTestBulk(i, board.board, board.turn);
                    sw.Stop();
                    textBox1.AppendText("Depth: " + i + "  Result: " + result + "  Time: " + sw.Elapsed + "  Test: " + (result == expectedResults[i] ? "Pass" : "Fail"));
                    if (result != expectedResults[i])
                    {
                        textBox1.AppendText(". Expected result: " + expectedResults[i]);
                    }

                    textBox1.AppendText(Environment.NewLine);
                }
                //callStockfish(ChessAI.BoardToFEN(board.board, board.turn), depth);
            }
            catch
            {

            }
        }

        private void callStockfish(string fen, int depth, Dictionary<string, Int64> result)
        {
            ProcessStartInfo si = new ProcessStartInfo()
            {
                FileName = stockfishPath,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true
            };
            Process myProcess = new Process();
            myProcess.StartInfo = si;
            try
            {
                // throws an exception on win98
                myProcess.PriorityClass = ProcessPriorityClass.BelowNormal;
            }
            catch { }

            //myProcess.OutputDataReceived += new DataReceivedEventHandler(myProcess_OutputDataReceived);
            myProcess.OutputDataReceived += (sender, EventArgs) =>
            {
                OutPutHandler(sender, EventArgs, result);
            };

            myProcess.Start();
            myProcess.BeginErrorReadLine();
            myProcess.BeginOutputReadLine();

            SendLine("position fen " + fen, myProcess);
            SendLine("go perft " + depth, myProcess);
            SendLine("quit", myProcess);
            myProcess.WaitForExit();
            
 
        }
        void OutPutHandler (object sendingPorcess, DataReceivedEventArgs e, Dictionary<string, Int64> result)
        {
            string text = e.Data;
            if (text == null) return;
            if (text.Length > 3)
            {
                if (text[4].Equals(':'))
                {
                    string key = text.Substring(0, 4);
                    string value = text.Substring(6);
                    result.Add(key, Int64.Parse(value));
                } else if (text[5].Equals(':'))
                {
                    string key = text.Substring(0, 5);
                    string value = text.Substring(7);
                    result.Add(key, Int64.Parse(value));
                }
                if (text.Contains("Nodes searched"))
                {
                    string value = text.Substring(16);
                    result.Add("nodes", Int64.Parse(value));
                    //ComparePerfts(result, ownPerfResult);
                }
            }
        }
        private void SendLine(string command, Process myProcess)
        {
            myProcess.StandardInput.WriteLine(command);
            myProcess.StandardInput.Flush();
        }

        //private void myProcess_OutputDataReceived(object sender, DataReceivedEventArgs e, Dictionary<string, Int64> result)
        //{
        //    string text = e.Data;
          
        //    if (text.Length > 3)
        //    {
        //        if (text[4].Equals(':'))
        //        {
        //            string key = text.Substring(0, 4);
        //            string value = text.Substring(6);
        //            this.stockfishResults.Add(key, Int64.Parse(value));
        //        }
        //        if (text.Contains("Nodes searched"))
        //        {
        //            string value = text.Substring(16);
        //            this.stockfishResults.Add("nodes", Int64.Parse(value));
        //            FinalizeComparison();
        //        }
        //    }
        //    Debug.WriteLine(stockfishResults.Count);

        //}


        private void getStockfishExeBtn_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    stockfishPath = filePath;
                    if (filePath.Length > 30)
                    {
                        stockfishPathLabel.Text = "Path: ..." + stockfishPath.Substring(filePath.Length - 30);
                    } else
                    {
                        stockfishPathLabel.Text = "Path: " + filePath;
                    }
                    

                }
            }
        }
    }
}
