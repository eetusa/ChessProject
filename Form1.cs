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
			int cell_contentColor = getCellColor(targetCell, board);
			

			if (board.selected_cell == -1) // if no cell active selected
			{
				if (board.turn == cell_contentColor ) // if correct color piece clicked on turn set cell as selected
				{
					board.selected_cell = targetCell.index;
					requireUpdatedDrawing(targetCell);
					board.possibleMoves = getPossibleMoves(board, cells, targetCell);
					// draw possbile moves -> go through all indexes and check for legality
				}
            }
            else
            {
				Cell originCell = cells[board.selected_cell];
				
				if (originCell == targetCell) // if clicked cell same as is active
                {
					board.selected_cell = -1; // reset selection to none
					board.possibleMoves = resetPossibleMoves(board, cells);
					requireUpdatedDrawing(originCell);
                } else if (board.possibleMoves.Contains(targetCell.index))
				{
					board.possibleMoves = resetPossibleMoves(board, cells);
					board.board[targetCell.index] = board.board[board.selected_cell];
					board.board[board.selected_cell] = 0;
					requireUpdatedDrawing(originCell);
					requireUpdatedDrawing(targetCell);
					board.selected_cell = -1;

					if (board.turn == 0) { board.turn = 1; turn_label.Text = "Black"; }
					else
					{ board.turn = 0; turn_label.Text = "White"; }
					printBoard(board);
					
				}



            }
			drawBoard(board, cells);
			

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
                } else if (board.possibleMoves.Contains(index))
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

		bool checkStraightMovement(Board board, int origin, int target, int movement)
        {
			if (origin / 8 != target / 8 && (movement == 1 || movement == -1)) return false;
			if ((origin / 8 == target / 8) && (movement > 1 || movement < -1))
			{
                //System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
                return false;
			}

			int row_diff = Math.Abs((origin / 8) - (target / 8));

			if (origin > target && movement < 0)
			{
				if ((origin - target) % movement == 0)
				{
					int temp_position = origin;
					int temp_value = 0;
					while (true)
					{
						temp_position += movement;
						temp_value++;
						if (temp_position < 0 || temp_position > 63) { 
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
							break; 
						}

						if (board.board[temp_position] != 0 && temp_position != target)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
							return false;
						}
						if (temp_position == target)
						{
							//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
							if (row_diff == temp_value || row_diff == 0) 
							{ 
								if (getCellColor(target, board) == -1) {
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " +origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true; 
								}
								if (getCellColor(origin, board) != getCellColor(target, board)) { 
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true; 
								}
							}
						}
					}
				}
			}

			if (origin < target && movement > 0)
			{
				if ((target - origin) % movement == 0)
				{
					int temp_position = origin;
					int temp_value = 0;
					while (true)
					{
						temp_position += movement;
						temp_value++;
						if (temp_position < 0 || temp_position > 63) { 
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
							break; 
						}

						if (board.board[temp_position] != 0 && temp_position != target)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
							return false;
						}

						if (temp_position == target)
						{
							//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
							if (row_diff == temp_value || row_diff == 0)
							{
								if (getCellColor(target, board) == -1) { 
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true; 
								}
								if (getCellColor(origin, board) != getCellColor(target, board)) { 
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true; 
								}
							}
						}
					}
				}
			}
			
			return false;
        }


		bool checkStraightMovement(int[] board, int origin, int target, int movement)
		{
			if (origin / 8 != target / 8 && (movement == 1 || movement == -1)) return false;
			if ((origin / 8 == target / 8) && (movement > 1 || movement < -1))
			{
				//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
				return false;
			}

			int row_diff = Math.Abs((origin / 8) - (target / 8));

			if (origin > target && movement < 0)
			{
				if ((origin - target) % movement == 0)
				{
					int temp_position = origin;
					int temp_value = 0;
					while (true)
					{
						temp_position += movement;
						temp_value++;
						if (temp_position < 0 || temp_position > 63)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
							break;
						}

						if (board[temp_position] != 0 && temp_position != target)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
							return false;
						}
						if (temp_position == target)
						{
							//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
							if (row_diff == temp_value || row_diff == 0)
							{
								if (getCellColor(target, board) == -1)
								{
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " +origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true;
								}
								if (getCellColor(origin, board) != getCellColor(target, board))
								{
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true;
								}
							}
						}
					}
				}
			}

			if (origin < target && movement > 0)
			{
				if ((target - origin) % movement == 0)
				{
					int temp_position = origin;
					int temp_value = 0;
					while (true)
					{
						temp_position += movement;
						temp_value++;
						if (temp_position < 0 || temp_position > 63)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n"); 
							break;
						}

						if (board[temp_position] != 0 && temp_position != target)
						{
							//System.Diagnostics.Debug.Write("|| failure: row: " + origin / 8 + " " + " row: " + target / 8 + " " + movement + "\n");
							return false;
						}

						if (temp_position == target)
						{
							//System.Diagnostics.Debug.Write("** Row diff: " + row_diff + " temp_value: " + temp_value + "\n");
							if (row_diff == temp_value || row_diff == 0)
							{
								if (getCellColor(target, board) == -1)
								{
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true;
								}
								if (getCellColor(origin, board) != getCellColor(target, board))
								{
									//System.Diagnostics.Debug.Write("|| Sucess: Origin: " + origin + " Target: " + target + " Movement: " + movement + "\n"); 
									return true;
								}
							}
						}
					}
				}
			}

			return false;
		}

		int[] getPossibleMoves(Board board, Cell[] cells, Cell originCell)
        {
			int[] moveArray =
			{
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};

			if (board.selected_cell != -1)
            {
				int temp = 0;
				for (int i = 0; i < board.board.Length; i++)
                {
					
						if (legalMoveNew(board.board, originCell.index, i, 0))
                        {
							moveArray[temp] = i;
							requireUpdatedDrawing(cells[i]);
							temp++;
                        }
                    
                }
				//System.Diagnostics.Debug.Write("\n" + "possible moves length: " + temp + "\n");
            }
			printArray(moveArray, printBoxDebug);
			return moveArray;
        }

		int[] getPossibleMovesForThreat(Board board, Cell[] cells, Cell originCell)
		{
			int[] moveArray =
			{
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};

			if (board.selected_cell != -1)
			{
				int temp = 0;
				for (int i = 0; i < board.board.Length; i++)
				{
					if (i != originCell.index)
					{
						if (legalMoveNew(board.board, originCell.index, i, 0))
						{
							System.Diagnostics.Debug.Write("\n ** Cell: " + cells[i] + " i: " + temp + "** \n");
							moveArray[temp] = i;
							requireUpdatedDrawing(cells[i]);
							temp++;
						}
					}
				}
				//System.Diagnostics.Debug.Write("\n" + "possible moves length: " + temp + "\n");
			}
			
			return moveArray;
		}

		int[] resetPossibleMoves(Board board, Cell[] cells)
        {
			int[] moveArray =
			{
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			//for (int i = 0; i < board.possibleMoves.Length; i++)
   //         {
			//	if (board.possibleMoves[i] != -1) requireUpdatedDrawing(cells[i]);
			//	board.possibleMoves[i] = -1;
   //         }
			return moveArray;
		}

		//bool cellThreatened(Board board, Cell targetCell, Cell originCell, Cell[] cells)
  //      {
		//	int[] tempArray = new int[] {
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
		//		-1, -1, -1, -1,-1, -1, -1, -1
		//	};
		//	int cellColor = getCellColor(originCell, board);

		//	for (int i = 0; i < board.board.Length; i++)
  //          {
		//		int tempColor = getCellColor(cells[i], board);
		//		if (i != targetCell.index && i != originCell.index) { 
		//			if (cellColor != tempColor && tempColor != -1)
		//			{
		//				//System.Diagnostics.Debug.Write(" || cellcolor: " + cellColor + " tempcolor: " + tempColor);
		//				 //tempArray = getPossibleMovesForThreat(board, cells, cells[i]);
		//				if (legalMove(board, cells[i], originCell, cells, 2)) return true;
		//				//if (tempArray.Contains(targetCell.index))
		//				//{
		//				//	System.Diagnostics.Debug.Write("**  "+cells[i]+" ** "+ tempColor + " " +cellColor  +" \n");
		//				//	for (int j = 0; j < tempArray.Length; j++)
		//				//	{
		//				//		System.Diagnostics.Debug.Write(" " + tempArray[j]);
		//				//	}
		//				//	System.Diagnostics.Debug.Write(" ** \n");
		//				//	return true;
		//				//}
		//			}
		//		}

		//	}
		//	return false;
  //      }

		bool kingThreatened (int[] board, int originCell, int targetCell)
        {
			int[] tempArr = new int[64];
			int whiteking = 0;
			int blackking = 0;

			for (int i=0; i < board.Length; i++)
            {
				tempArr[i] = board[i];
            }

			int cellColor = getCellColor(originCell, board);
			tempArr[targetCell] = tempArr[originCell];
			tempArr[originCell] = 0;

			for (int i = 0; i < board.Length; i++)
			{
				if (tempArr[i] == 10) whiteking = i;
				if (tempArr[i] == 20) blackking = i;
			}

			for (int j = 0; j < tempArr.Length; j++)
            {
				int tempColor = getCellColor(j, board);
				if (tempColor == 0)
                {

					if (legalMoveNew(tempArr, j, blackking, 1))
                    {
						if (cellColor == 1) return true;
                    }
                } else if (tempColor == 1)
                {
					if (legalMoveNew(tempArr, j, whiteking, 1))
					{
						if (cellColor == 0) return true;
					}
				}
			}
			return false;
        }

		bool legalMoveNew(int[] board, int originCell, int targetCell, int type)
        {
			int piece = board[originCell];
			bool targetOccupied = board[targetCell] != 0 ? true : false;
			bool targetOccupiedByEnemy = ((getCellColor(originCell, board) != getCellColor(targetCell, board)) && getCellColor(targetCell, board) != -1) ? true : false;
			int movement = originCell - targetCell;


			// king
			if (piece == 10 || piece == 20)
            {
				int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
				if (movement == 1 || movement == -1)
                {
					if (rowdif != 0) return false;
					if (targetOccupiedByEnemy || !targetOccupied)
                    {
						if (type != 1)
                        {
							return !kingThreatened(board, originCell, targetCell);
						}
						
						return true;
                    }
                } else if (( movement == 7 || movement == 8 || movement == 9 || movement == -7 || movement == -8 || movement == -9 ) && rowdif == 1)
                {
					if ( targetOccupiedByEnemy || !targetOccupied )
					{
						if (type != 1)
						{
							return !kingThreatened(board, originCell, targetCell);
						}
						return true;
					}
				}
            }

			// bishop
			if (piece == 6 || piece == 16)
			{
				if (checkStraightMovement(board, originCell, targetCell, -7))
                {
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, -9))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;

				}
				if (checkStraightMovement(board, originCell, targetCell, 7))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 9))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
			}

			// rook
			if (piece == 8 || piece == 18)
			{
				if (checkStraightMovement(board, originCell, targetCell, -1))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, -8))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 1))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 8))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
			}

			// queen
			if (piece == 9 || piece == 19)
			{
				if (checkStraightMovement(board, originCell, targetCell, -7))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, -9))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 7))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 9))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, -1))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, -8))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 1))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
				if (checkStraightMovement(board, originCell, targetCell, 8))
				{
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
				}
			}

			// white pawn
			if (piece == 1)
			{
				if (movement == 8 && !targetOccupied && type != 1)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if (movement == 16 && originCell / 8 == 6 && !targetOccupied && type != 1)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if ((movement == 7 || movement == 9) && targetOccupiedByEnemy)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
			}

			// black pawn
			if (piece == 11)
			{
				if (movement == -8 && !targetOccupied && type != 1)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if (movement == -16 && originCell/8 == 1 && !targetOccupied  && type != 1)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if ((movement == -7 || movement == -9) && (targetOccupiedByEnemy))
				{
					return !kingThreatened(board, originCell, targetCell);
				}
			}

			return false;
        }

		//bool legalMove(Board board, Cell originCell, Cell targetCell, Cell[] cells, int callType)
  //      {
		//	int piece = board.board[originCell.index];
		//	bool targetOccupied = (board.board[targetCell.index] != 0) ? true : false;
		//	bool targetOccupiedByEnemy = (( getCellColor(originCell, board) != getCellColor(targetCell, board) ) && (getCellColor(targetCell, board) !=-1) ) ? true : false;
		//	int origin = originCell.index;
		//	int target = targetCell.index;
		//	int movement = origin - target;

		//	// king
		//	if (piece == 10 || piece == 20)
  //          {
		//		int[] temp = { 8, -8, 7, -7, 9, -9 };
		//		int rowdif = Math.Abs((origin / 8) - (target / 8));
		//		if (movement == 1 || movement == -1)
  //              {
		//			if (rowdif != 0)
  //                  {
		//				return false;
  //                  }
		//			if (targetOccupiedByEnemy || !targetOccupied)
  //                  {
		//				//return !kingThreatened(board, targetCell, originCell, cells);
		//				if (callType!=2) return !kingThreatened(board, targetCell, originCell, cells);
		//				return true;
  //                  }
  //              }
		//		if (temp.Contains(movement) && rowdif == 1)
  //              {
		//			if (targetOccupiedByEnemy || !targetOccupied)
		//			{
		//				if (callType != 2) return !kingThreatened(board, targetCell, originCell, cells);
		//				return true;
		//			}
		//		}
  //          }

		//	// bishop
		//	if (piece == 6 || piece == 16)
		//	{
		//		if (checkStraightMovement(board, origin, target, -7)) return true;
		//		if (checkStraightMovement(board, origin, target, -9)) return true;
		//		if (checkStraightMovement(board, origin, target, 7)) return true;
		//		if (checkStraightMovement(board, origin, target, 9)) return true;

		//	}

		//	// rook
		//	if (piece == 8 || piece == 18)
  //          {
		//		if (checkStraightMovement(board, origin, target, -1)) return true;
		//		if (checkStraightMovement(board, origin, target, -8)) return true;
		//		if (checkStraightMovement(board, origin, target, 1)) return true;
		//		if (checkStraightMovement(board, origin, target, 8)) return true;
		//	}

		//	// queen
		//	if (piece == 9 || piece == 19)
  //          {
		//		if (checkStraightMovement(board, origin, target, -7)) return true;
		//		if (checkStraightMovement(board, origin, target, -9)) return true;
		//		if (checkStraightMovement(board, origin, target, 7)) return true;
		//		if (checkStraightMovement(board, origin, target, 9)) return true;
		//		if (checkStraightMovement(board, origin, target, -1)) return true;
		//		if (checkStraightMovement(board, origin, target, -8)) return true;
		//		if (checkStraightMovement(board, origin, target, 1)) return true;
		//		if (checkStraightMovement(board, origin, target, 8)) return true;
		//	}

		//	// white pawn
		//	if (piece == 1)
  //          {
		//		if (movement == 8 && !targetOccupied)
  //              {
		//			return true;
  //              } else if (movement == 16 && originCell.row == 6 && !targetOccupied && callType!=2)
  //              {
		//			return true;
  //              } else if ((movement == 7 || movement == 9) && targetOccupiedByEnemy || callType == 2)
  //              {
		//			return true;
  //              }
  //          }

		//	// black pawn
		//	if (piece == 11)
		//	{
		//		if (movement == -8 && !targetOccupied)
		//		{
		//			return true;
		//		}
		//		else if (movement == -16 && originCell.row == 1 && !targetOccupied && callType != 2)
		//		{
		//			return true;
		//		}
		//		else if ((movement == -7 || movement == -9) && (targetOccupiedByEnemy || callType == 2))
		//		{
		//			return true;
		//		}
		//	}



		//	return false;
  //      }

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

		int getCellColor(int cellIndex, Board board)
        {
			int cellContentInt = board.board[cellIndex];

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

		int getCellColor(int cellIndex, int[] board)
		{
			int cellContentInt = board[cellIndex];

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
			for (int i = 0; i < array.Length; i++)
			{
				textbox.AppendText(array[i] + " ");
			}
		}
	}


}

