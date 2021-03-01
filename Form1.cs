using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace ChessProject
{
    public partial class Form1 : Form
    {
		private TableLayoutPanel cells;
		public class Cell : PictureBox
		{
			public static readonly System.Drawing.Size CellSize = new System.Drawing.Size(80, 80);
			public readonly int row, col, index;
			public bool updated;
		
			

			public Cell(int row, int col) : base()
			{
				this.row = row; this.col = col;
				this.Size = CellSize;
				this.BackColor = (col % 2 == row % 2) ? Color.FromArgb(238, 238, 212) : Color.FromArgb(118, 150, 85);
				this.index = row * 8 + col;
				this.updated = false;

			}
			public override string ToString() { return "Cell(" + row + "," + col + ")"; }
			public Cell returnThis()
            {
				return this;
            }
		}


		private TableLayoutPanel GetBoard(Board board, Cell[] test_cells)
		{
			TableLayoutPanel b = new TableLayoutPanel();
			b.ColumnCount = 8;
			b.RowCount = 8;
			turn_label.Text = board.turn == 0 ? "White" : "Black";

			for (int i = 0; i < b.ColumnCount; i++)
			{
				b.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Cell.CellSize.Width));
			}

			for (int i = 0; i < b.RowCount; i++)
			{
				b.RowStyles.Add(new RowStyle(SizeType.Absolute, Cell.CellSize.Height));
			}
			int ii = 0;

			for (int row = 0; row < b.RowCount; row++)
			{
				for (int col = 0; col < b.ColumnCount; col++)
				{
					Cell cell = new Cell(row, col);
					cell.Click += delegate (object s, EventArgs e) {
						cell_Click(s, board, b, test_cells);
					};


					b.Controls.Add(cell, col, row);
					test_cells[ii] = cell;
					ii++;

					cell.SizeMode = PictureBoxSizeMode.StretchImage;

				}

			}
			b.Padding = new Padding(0);
			b.Size = new System.Drawing.Size(b.ColumnCount * Cell.CellSize.Width, b.RowCount * Cell.CellSize.Height);

			return b;
		}



		public Form1()
        {
    
			Board board = new Board();
			
			//FormBorderStyle = FormBorderStyle.None;
			//WindowState = FormWindowState.Maximized;
			//this.Size = new Size(1300, 1300);
			InitializeComponent();
			Cell[] cellArr = new Cell[64];
			cells = GetBoard(board, cellArr);
			this.Controls.Add(cells);

			startNewGame(board, cellArr);
			newgame_button.Click += (sender, EventArgs) =>
			{
				newgame_Click(board, cellArr);
				drawBoard(board, cellArr);
				printBoard(board);
			};

			testBoard1Btn.Click += (sender, EventArgs) =>
			{
				LoadTestBoard(board, cellArr);
				drawBoard(board, cellArr);
				printBoard(board);
			};
			blackAIButton.Click += (sender, EventArgs) =>
			{
				changeCheckValue(board);
			};
			whiteAIButton.Click += (sender, EventArgs) =>
			{
				changeCheckValue(board);
			};
			drawBoard(board, cellArr);
			printBoard(board);

			test_button.Click += (sender, EventArgs) =>
			{
				openTestForm(board);
			};
			printFENBtn.Click += (sender, EventArgs) =>
			{
				printFEN(board);
			};
			loadFENBtn.Click += (sender, EventArgs) =>
			{
				LoadFromFEN(board, cellArr);
			};
			copyFENBtn.Click += (sender, EventArgs) =>
			{
				CopyToClipboard();
			};


		}

		void openTestForm(Board board)
        {
			var m = new Form4(board);
			m.Show();
        }





private void cell_Click(object sender, Board board, TableLayoutPanel b, Cell[] cells)
		{
			//System.Diagnostics.Debug.Write(board.AI_BlackON + " " + board.AI_WhiteON + " " + board.turn);
			if ((board.AI_WhiteON && board.turn%2 == 0) || (board.AI_BlackON && board.turn%2==1 ))
            {
				doAITurn(board, cells);
            }
			Cell targetCell = (Cell)sender;

			if (board.allPossibleMoves != null) { 
				if (board.selected_cell == -1) // if no cell active selected
				{
					if (board.turn % 2 == ChessAI.getCellColor(targetCell.index, board.board)) // if correct color piece clicked on turn set cell as selected
					{
						selectCell(board, targetCell, cells);
					}
				}
				else
				{
					Cell originCell = cells[board.selected_cell];
					if (originCell == targetCell) // if clicked cell same as is active
					{
						deselectCell(board, originCell, cells);

					} else if (board.allPossibleMoves[originCell.index] != null && board.allPossibleMoves[originCell.index].Contains(targetCell.index)) 
					{
						doTurn(board, cells, originCell, targetCell, false);
						if (board.allPossibleMoves == null)
						{
							//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
							showCheckmate(board);
						}
					} 
				}

			} else
            {
				//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
			}
		}

		void showCheckmate(Board board)
        {
			String winner = board.turn % 2 == 1 ? "White" : "Black";
			var m = new Form2(winner);
			m.Show();
			wait(5000);
			m.Close();
        }
		void selectCell(Board board, Cell cell, Cell[] cells)
        {
			board.selected_cell = cell.index;
			requireUpdatedDrawing(cell);
			board.possibleMoves = board.allPossibleMoves[cell.index];
			printArray(board.possibleMoves, printBoxDebug);
			drawBoard(board, cells);
		}

		void deselectCell(Board board, Cell cell, Cell[] cells)
        {
			board.selected_cell = -1; // reset selection to none
			board.possibleMoves = resetPossibleMoves(board, cells);
			requireUpdatedDrawing(cell);
			drawBoard(board, cells);
			printArray(board.possibleMoves, printBoxDebug);
		}

		bool CheckIfKingCastled(int[] board, int originCell, int targetCell)
        {

			if (board[originCell] == 30 || board[originCell] == 40)
            {
				if (targetCell == 62 || targetCell == 6 || targetCell == 2 || targetCell == 58)
                {
					return true;
                }

            }
			return false;
        }
		void doTurn(Board board, Cell[] cells, Cell originCell, Cell targetCell, bool aiTurn) 
        {
			if (board.allPossibleMoves == null)
			{
				//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
				return;
			}
			// update gamestate
			
			if (CheckIfKingCastled(board.board, originCell.index, targetCell.index))
            {
				//System.Diagnostics.Debug.Write("castled \n");
				board.possibleMoves = resetPossibleMoves(board, cells);
				ChessAI.castleMove(board.board, originCell.index, targetCell.index, cells);
            } else
            {
				if (board.board[originCell.index] == 1 || board.board[originCell.index] == 11)
				{
					if (board.board[targetCell.index] == 0)
                    {
						int movement = (originCell.index - targetCell.index);
						if (movement == 7 || movement == -9)
                        {
							board.board[originCell.index + 1] = 0;
							requireUpdatedDrawing(cells[originCell.index + 1]);
                        }
						else if (movement == -7 || movement == 9)
						{
							board.board[originCell.index - 1] = 0;
							requireUpdatedDrawing(cells[originCell.index - 1]);
						}
					}

				}
				ChessAI.SetupEnpassantPawn(board.board, originCell.index, targetCell.index);

				board.possibleMoves = resetPossibleMoves(board, cells);
				ChessAI.checkFirstMoveByKingOrRook(board.board, originCell.index);
				board.board[targetCell.index] = board.board[originCell.index];
				board.board[originCell.index] = 0;
				requireUpdatedDrawing(originCell);
				requireUpdatedDrawing(targetCell);
			}

			board.selected_cell = -1;
			ChessAI.checkPawnUpdate(board.board, aiTurn);
            

			// change turn, draw all and get new moves
			if (board.turn % 2 == 0) { board.turn += 1; turn_label.Text = "Black"; }
			else
			{ board.turn += 1; turn_label.Text = "White"; }
			ChessAI.CancelEnPassant(board.board, board.turn % 2);
			printBoard(board);
			drawBoard(board, cells);
			board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, board.turn % 2);

			// if ai on for the turné
            if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				doAITurn(board, cells);
			}

		}


		void doAITurn(Board board, Cell[] cells)
        {
			int[] result = ChessAI.AITurn(board);
			wait(10);
			if (result != null)
			{
				doTurn(board, cells, cells[result[0]], cells[result[1]], true);
			}
			else
			{
				//System.Diagnostics.Debug.Write("Debug nulll;");
			}
		}

		void drawBoard(Board board, Cell[] cells)
        {
			for (int i = 0; i < board.board.Length; i++)
            {
				drawCell(board, cells[i]);
            }
        }

		void drawCell(Board board, Cell cell)
        {
			int index = cell.index;
			int cellValue = board.board[index];

	
				if (board.selected_cell == index)
                {
					setActiveCellColor(cell);
                } else if (board.possibleMoves != null && board.possibleMoves.Contains(index))
                {
					setLegalCellColor(cell);
                }		
				else
                {
					resetCellColor(cell);
                }
			if (!cell.updated)
			{
				if (board.board[index] > 0)
                {
					
					if (cellValue > 0 && cellValue < 6)
					{
						cell.ImageLocation = @"images\1.png";


					}
					else if (cellValue > 10 && cellValue < 16)
					{
						cell.ImageLocation = @"images\11.png";
					} else if (cellValue > 20) {
						cell.ImageLocation = $@"images\{cellValue-20}.png";
					}
					else
					{
						cell.ImageLocation = $@"images\{cellValue}.png";

					}
				} else
                {
					cell.ImageLocation = null;
                }
				cell.updated = true;
            }
        }

		public void requireUpdatedDrawing (Cell cell)
        {
			cell.updated = false;
        }

		void resetCellColor(Cell cell)
        {
			cell.BackColor = !(cell.col % 2 == cell.row % 2) ? Color.FromArgb(118, 150, 85) : Color.FromArgb(238, 238, 212);
		}

		void setActiveCellColor(Cell cell)
        {
			cell.BackColor = !(cell.col % 2 == cell.row % 2) ? Color.FromArgb(88, 133, 64) : Color.FromArgb(213, 213, 149);
		}

		void setLegalCellColor(Cell cell)
		{
			//cell.BackColor = !(cell.col % 2 == cell.row % 2) ? Color.FromArgb(216, 226, 205) : Color.FromArgb(250, 250, 241);
			cell.BackColor = Color.FromArgb(253, 217, 200);
		}
		public void wait(int milliseconds)
		{
			var timer1 = new System.Windows.Forms.Timer();
			if (milliseconds == 0 || milliseconds < 0) return;

			// Console.WriteLine("start wait timer");
			timer1.Interval = milliseconds;
			timer1.Enabled = true;
			timer1.Start();

			timer1.Tick += (s, e) =>
			{
				timer1.Enabled = false;
				timer1.Stop();
				// Console.WriteLine("stop wait timer");
			};

			while (timer1.Enabled)
			{
				Application.DoEvents();
			}
		}

		void changeCheckValue(Board board)
		{

			board.AI_WhiteON = whiteAIButton.Checked;
			board.AI_BlackON = blackAIButton.Checked;

		}

		int[] resetPossibleMoves(Board board, Cell[] cells)
        {
			int[] moveArray =
			{
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};

			return moveArray;
		}

		int getCellColor(Cell cell, Board board)
        {
			int cellContentInt = board.board[cell.index];

			if (cellContentInt > 0 && cellContentInt < 11)
			{
				return 0; // white
			}
			else if (cellContentInt > 10 && cellContentInt < 21)
			{
				return 1; // black
			}
			return -1;
        }

		void printBoard(Board board)
        {
			boardIntCont.Text = "";
			int temp = 0;
			for (int i = 0; i < board.board.Length; i++)
            {
				if (board.board[i] > 9)
                {
					boardIntCont.AppendText(board.board[i] + " ");
				} else
                {
					boardIntCont.AppendText(board.board[i] +  "  ");
				}
				
				temp++;
				if (temp == 8 && i < 63)
                {
					temp = 0;
					boardIntCont.AppendText(Environment.NewLine);
                }
            }
			boardIntCont.Enabled = false;
			boardIntCont.Enabled = true;	
        }

		void printBoard(int[] board, TextBox textbox)
		{
			textbox.Text = "";
			int temp = 0;
			for (int i = 0; i < board.Length; i++)
			{
				//System.Diagnostics.Debug.Write(board[i] + " ");
				if (board[i] > 9)
				{
					textbox.AppendText(board[i] + " ");
				}
				else
				{
					textbox.AppendText(board[i] + "  ");
				}

				temp++;
				if (temp == 8 && i < 63)
				{
					temp = 0;
					textbox.AppendText(Environment.NewLine);
				}
			}
			textbox.Enabled = false;
			textbox.Enabled = true;
		}
		void printArray(int[] array, TextBox textbox)
		{
			textbox.Text = "";
			if (array != null) { 
			for (int i = 0; i < array.Length; i++)
			{
				textbox.AppendText(array[i] + " ");
			}
			}
		}

		void newgame_Click(Board board, Cell[] cells)
		{
				startNewGame(board, cells);
		}


		void LoadTestBoard(Board board, Cell[] cells)
		{

			board.initializeTestBoard();
			foreach (Cell cell in cells)
			{
				requireUpdatedDrawing(cell);
			}

			board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, 0);
			blackAIButton.Checked = board.AI_BlackON;
			whiteAIButton.Checked = board.AI_WhiteON;
			turn_label.Text = (board.turn % 2 == 0) ? "White" : "Black";
			drawBoard(board, cells);
			printBoard(board);
		}

		void printFEN(Board board)
		{
			fenDisp.Text = ChessAI.BoardToFEN(board.board, board.turn);
		}

		void LoadFromFEN(Board board, Cell[] cells)
        {
			if (fenDisp.Text == "") return;
			ChessAI.FENToBoard(fenDisp.Text, board);
			SetBoardSettings(board, cells);
		}
		void SetBoardSettings(Board board, Cell[] cells)
        {
			printBoard(board);
			foreach (Cell cell in cells)
			{
				requireUpdatedDrawing(cell);
			}
			board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, 0);
			blackAIButton.Checked = board.AI_BlackON;
			whiteAIButton.Checked = board.AI_WhiteON;
			turn_label.Text = (board.turn % 2 == 0) ? "White" : "Black";
			drawBoard(board, cells);
			printBoard(board);
		}

		void startNewGame(Board board, Cell[] cells)
		{

			board.initializeBoard();
			SetBoardSettings(board, cells);
		}

		void CopyToClipboard()
        {
			Clipboard.SetText(fenDisp.Text);
        }


    }


}

