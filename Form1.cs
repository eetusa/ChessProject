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
		class Cell : PictureBox
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



		public Form1()
        {
    
			Board board = new Board();

			//FormBorderStyle = FormBorderStyle.None;
			//WindowState = FormWindowState.Maximized;
			//this.Size = new Size(1300, 1300);
			InitializeComponent();
			Cell[] test_cells = new Cell[64];
			cells = GetBoard(board,test_cells);
			this.Controls.Add(cells);
			board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, 0);
			blackAIButton.Checked = board.AI_BlackON;
			whiteAIButton.Checked = board.AI_WhiteON;
			drawBoard(board, test_cells);
			printBoard(board);


		}

        private void Form1_Load(object sender, EventArgs e)
        {

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


		private void cell_Click(object sender, Board board, TableLayoutPanel b, Cell[] cells)
		{

			Cell targetCell = (Cell)sender;

			if (board.allPossibleMoves != null) { 
				if (board.selected_cell == -1) // if no cell active selected
				{
					if (board.turn % 2 == getCellColor(targetCell, board)) // if correct color piece clicked on turn set cell as selected
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
						doTurn(board, cells, originCell, targetCell);
						if (board.allPossibleMoves == null)
						{
							System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
						}
					} 
				}

			} else
            {
				System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
			}



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

		void doTurn(Board board, Cell[] cells, Cell originCell, Cell targetCell) 
        {
			if (board.allPossibleMoves == null)
			{
				System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
				return;
			}
			// update gamestate
			board.possibleMoves = resetPossibleMoves(board, cells);
			board.board[targetCell.index] = board.board[originCell.index];
			board.board[originCell.index] = 0;
			requireUpdatedDrawing(originCell);
			requireUpdatedDrawing(targetCell);
			board.selected_cell = -1;

			// change turn, draw all and get new moves
			if (board.turn % 2 == 0) { board.turn += 1; turn_label.Text = "Black"; }
			else
			{ board.turn += 1; turn_label.Text = "White"; }
			printBoard(board);
			drawBoard(board, cells);
			board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, board.turn % 2);

            if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				int[] result = ChessAI.AITurn(board);
				wait(10);
                if (result != null)
				{
					doTurn(board, cells, cells[result[0]], cells[result[1]]);
				}
				else
				{
					System.Diagnostics.Debug.Write("Debug nulll;");
				}
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

		void requireUpdatedDrawing (Cell cell)
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


		//bool checkStraightMovement(Board board, int origin, int target, int movement)
		//      {
		//	if (origin / 8 != target / 8 && (movement == 1 || movement == -1)) return false;
		//	if ((origin / 8 == target / 8) && (movement > 1 || movement < -1))
		//	{
		//              //System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//              return false;
		//	}

		//	int row_diff = Math.Abs((origin / 8) - (target / 8));

		//	if (origin > target && movement < 0)
		//	{
		//		if ((origin - target) % movement == 0)
		//		{
		//			int temp_position = origin;
		//			int temp_value = 0;
		//			while (true)
		//			{
		//				temp_position += movement;
		//				temp_value++;
		//				if (temp_position < 0 || temp_position > 63) { 
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
		//					break; 
		//				}

		//				if (board.board[temp_position] != 0 && temp_position != target)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//					return false;
		//				}
		//				if (temp_position == target)
		//				{
		//					//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
		//					if (row_diff == temp_value || row_diff == 0) 
		//					{ 
		//						if (getCellColor(target, board) == -1) {
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " +origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true; 
		//						}
		//						if (getCellColor(origin, board) != getCellColor(target, board)) { 
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true; 
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}

		//	if (origin < target && movement > 0)
		//	{
		//		if ((target - origin) % movement == 0)
		//		{
		//			int temp_position = origin;
		//			int temp_value = 0;
		//			while (true)
		//			{
		//				temp_position += movement;
		//				temp_value++;
		//				if (temp_position < 0 || temp_position > 63) { 
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
		//					break; 
		//				}

		//				if (board.board[temp_position] != 0 && temp_position != target)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//					return false;
		//				}

		//				if (temp_position == target)
		//				{
		//					//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
		//					if (row_diff == temp_value || row_diff == 0)
		//					{
		//						if (getCellColor(target, board) == -1) { 
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true; 
		//						}
		//						if (getCellColor(origin, board) != getCellColor(target, board)) { 
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true; 
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}

		//	return false;
		//      }


		//bool checkStraightMovement(int[] board, int origin, int target, int movement)
		//{
		//	if (origin / 8 != target / 8 && (movement == 1 || movement == -1)) return false;
		//	if ((origin / 8 == target / 8) && (movement > 1 || movement < -1))
		//	{
		//		//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//		return false;
		//	}

		//	int row_diff = Math.Abs((origin / 8) - (target / 8));

		//	if (origin > target && movement < 0)
		//	{
		//		if ((origin - target) % movement == 0)
		//		{
		//			int temp_position = origin;
		//			int temp_value = 0;
		//			while (true)
		//			{
		//				temp_position += movement;
		//				temp_value++;
		//				if (temp_position < 0 || temp_position > 63)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
		//					break;
		//				}

		//				if (board[temp_position] != 0 && temp_position != target)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//					return false;
		//				}
		//				if (temp_position == target)
		//				{
		//					//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
		//					if (row_diff == temp_value || row_diff == 0)
		//					{
		//						if (getCellColor(target, board) == -1)
		//						{
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " +origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true;
		//						}
		//						if (getCellColor(origin, board) != getCellColor(target, board))
		//						{
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true;
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}

		//	if (origin < target && movement > 0)
		//	{
		//		if ((target - origin) % movement == 0)
		//		{
		//			int temp_position = origin;
		//			int temp_value = 0;
		//			while (true)
		//			{
		//				temp_position += movement;
		//				temp_value++;
		//				if (temp_position < 0 || temp_position > 63)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
		//					break;
		//				}

		//				if (board[temp_position] != 0 && temp_position != target)
		//				{
		//					//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
		//					return false;
		//				}

		//				if (temp_position == target)
		//				{
		//					//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
		//					if (row_diff == temp_value || row_diff == 0)
		//					{
		//						if (getCellColor(target, board) == -1)
		//						{
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true;
		//						}
		//						if (getCellColor(origin, board) != getCellColor(target, board))
		//						{
		//							//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
		//							return true;
		//						}
		//					}
		//				}
		//			}
		//		}
		//	}

		//	return false;
		//}

		//int[][] getAllPossibleMoves(int[] board, int color)
		//      {

		//	int[][] superArray = new int[64][];
		//	int[] tempArray = new int[28];
		//	bool empty = true;

		//	for (int i = 0; i < superArray.Length; i++)
		//          {
		//		int gotColor = getCellColor(i, board);
		//		if  ( gotColor == color)
		//              {
		//			tempArray = getPossibleMovesLow(board, i);
		//			if (tempArray[0] != -1) {
		//				superArray[i] = new int[28];
		//				Array.Copy(tempArray, superArray[i], 28);
		//				empty = false;
		//			}

		//		}
		//          }
		//	if (!empty) return superArray;
		//	else return null;
		//      }

		//int[] getPossibleMovesLow(int[] board, int originCell)
		//{
		//	int[] moveArray =
		//	{
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1
		//	};


		//		int temp = 0;
		//		for (int i = 0; i < board.Length; i++)
		//		{

		//			if (legalMoveNew(board, originCell, i, 0))
		//			{
		//				moveArray[temp] = i;
		//				temp++;
		//			}

		//		}
		//		//System.Diagnostics.Debug.Write("\n" + "possible moves length: " + temp + "\n");

		//	//printArray(moveArray, printBoxDebug);
		//	return moveArray;
		//}

		//int[] getPossibleMoves(Board board, Cell[] cells, Cell originCell)
		//      {
		//	int[] moveArray =
		//	{
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1
		//	};

		//	if (board.selected_cell != -1)
		//          {
		//		int temp = 0;
		//		for (int i = 0; i < board.board.Length; i++)
		//              {

		//				if (legalMoveNew(board.board, originCell.index, i, 0))
		//                      {
		//					moveArray[temp] = i;
		//					requireUpdatedDrawing(cells[i]);
		//					temp++;
		//                      }

		//              }
		//		//System.Diagnostics.Debug.Write("\n" + "possible moves length: " + temp + "\n");
		//          }
		//	printArray(moveArray, printBoxDebug);
		//	return moveArray;
		//      }


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


		// tai label ja \n
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
				System.Diagnostics.Debug.Write(board[i] + " ");
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

    }


}

