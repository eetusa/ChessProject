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


		private TableLayoutPanel GetBoard(Board board)
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
						cell_Click(s, board, b);
					};


					b.Controls.Add(cell, col, row);
					//test_cells[ii] = cell;
					
					board.cells[ii] = cell;
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
			//Cell[] cellArr = new Cell[64];
			cells = GetBoard(board);
			this.Controls.Add(cells);

			startNewGame(board);
			newgame_button.Click += (sender, EventArgs) =>
			{
				newgame_Click(board);
				drawBoard(board);
				printBoard(board);
			};

			testBoard1Btn.Click += (sender, EventArgs) =>
			{
				LoadTestBoard(board);
				drawBoard(board);
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
			drawBoard(board);
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
				LoadFromFEN(board);
				PrintToConsolePieces(board);
			};
			copyFENBtn.Click += (sender, EventArgs) =>
			{
				CopyToClipboard();
			};
			printPieces.Click += (sender, EventArgs) =>
			{
				PrintToConsolePieces(board);
				DrawBoardFromBoardPieces(board);
			};
			undoButton.Click += (sender, EventArgs) =>
			{
				undoTurn(board);
				
			};
		}

		void DrawBoardFromBoardPieces(Board board)
        {
			Cell[] cells = board.cells;
			foreach (Cell cell in cells)
            {
				cell.ImageLocation = null;
			}
			int pieceType = -1;
			foreach (List<int> pieceList in board.whitePieces)
            {
				if (pieceList == board.pawns[0])
                {
					pieceType = 1;
                }
				else if (pieceList == board.knights[0])
				{
					pieceType = 7;
				}
				else if (pieceList == board.bishops[0])
				{
					pieceType = 6;
				}
				else if (pieceList == board.rooks[0])
				{
					pieceType = 8;
				}
				else if (pieceList == board.queens[0])
				{
					pieceType = 9;
				}
				else if (pieceList == board.kings[0])
				{
					pieceType = 10;
				}
				foreach (int piece in pieceList)
                {
					if (pieceType > 0 && pieceType < 6)
					{
						cells[piece].ImageLocation = @"images\1.png";
					}
					else if (pieceType > 10 && pieceType < 16)
					{
						cells[piece].ImageLocation = @"images\11.png";
					}
					else if (pieceType > 20)
					{
						cells[piece].ImageLocation = $@"images\{piece - 20}.png";
					}
					else
					{
						cells[piece].ImageLocation = $@"images\{pieceType}.png";

					}
				}			
				
			}
			pieceType = -1;
			foreach (List<int> pieceList in board.blackPieces)
			{
				if (pieceList == board.pawns[1])
				{
					pieceType = 11;
				}
				else if (pieceList == board.knights[1])
				{
					pieceType = 17;
				}
				else if (pieceList == board.bishops[1])
				{
					pieceType = 16;
				}
				else if (pieceList == board.rooks[1])
				{
					pieceType = 18;
				}
				else if (pieceList == board.queens[1])
				{
					pieceType = 19;
				}
				else if (pieceList == board.kings[1])
				{
					pieceType = 20;
				}
				foreach (int piece in pieceList)
				{
					if (pieceType > 0 && pieceType < 6)
					{
						cells[piece].ImageLocation = @"images\1.png";
					}
					else if (pieceType > 10 && pieceType < 16)
					{
						cells[piece].ImageLocation = @"images\11.png";
					}
					else if (pieceType > 20)
					{
						cells[piece].ImageLocation = $@"images\{piece - 20}.png";
					}
					else
					{
						cells[piece].ImageLocation = $@"images\{pieceType}.png";

					}
				}

			}

		}
		void PrintToConsolePieces(Board board)
        {
			foreach (List<int> color in board.pawns)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("pawns " + board.pawns.IndexOf(color) + ": " + temp);
			}

			foreach (List<int> color in board.rooks)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("rooks " + board.rooks.IndexOf(color) + ": " + temp);
			}

			foreach (List<int> color in board.queens)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("queen " + board.queens.IndexOf(color) + ": " + temp);
			}
			foreach (List<int> color in board.kings)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("kings " + board.kings.IndexOf(color) + ": " + temp);
			}
			foreach (List<int> color in board.knights)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("knigh " + board.knights.IndexOf(color) + ": " + temp);
			}
			foreach (List<int> color in board.bishops)
			{
				int temp = 0;
				foreach (int piece in color)
				{
					temp++;
				}
				System.Diagnostics.Debug.WriteLine("bish " + board.bishops.IndexOf(color) + ": " + temp);
			}
		}
		void openTestForm(Board board)
        {
			var m = new Form4(board);
			m.Show();
        }

		public void DoTurn(Board board, int originCell, int targetCell, bool aiTurn, int promotion)
		{
			//if (board.allPossibleMoves == null) return;
			ChessEngine.MakeMove(board.board, originCell, targetCell, aiTurn, promotion);
			board.turn++;
			board.selected_cell = -1;
			board.resetPossibleMoves();
			drawBoard2(board);
			board.allPossibleMoves = ChessEngine.GetAllPossibleMoves(board.board, board.turn % 2);
			if (board.allPossibleMoves == null)
			{
				showCheckmate(board);
				return;
			}

			if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				DoAITurn(board);
			}

		}

		void DoAITurn(Board board)
		{
			int[] result = ChessAI.AITurn(board);
			wait(10);
			if (result != null)
			{
				DoTurn(board, result[0], result[1], true, result[2]); //
			}
		}

		private void cell_Click(object sender, Board board, TableLayoutPanel b)
		{
			if ((board.AI_WhiteON && board.turn % 2 == 0) || (board.AI_BlackON && board.turn % 2 == 1))
			{
				DoAITurn(board);
			}
			Cell targetCell = (Cell)sender;
			//ChessEngine.GetPossibleMovesCaller(board.board, targetCell.index, board.turn%2);
			if (board.allPossibleMoves != null)
			{
				if (board.selected_cell == -1) // if no cell active selected
				{
					if (board.turn % 2 == ChessEngine.getCellColor(targetCell.index, board.board)) // if correct color piece clicked on turn set cell as selected
					{
						selectCell(board, targetCell.index);
						//System.Diagnostics.Debug.WriteLine("moves amount: " + board.possibleMoves.Length);
						drawBoard2(board);
					}
				}
				else
				{
					Cell originCell = board.cells[board.selected_cell];
					if (originCell == targetCell) // if clicked cell same as is active
					{
						deselectCell(board);
						drawBoard2(board);
						return;
					}
                    if (board.allPossibleMoves == null)
                    {
                        showCheckmate(board);
                    }
                    else if (board.allPossibleMoves[originCell.index] != null &&board.allPossibleMoves[originCell.index].Contains(targetCell.index))
                    {
                        DoTurn(board, originCell.index, targetCell.index, false, -1);

                        if (board.allPossibleMoves == null)
                        {
                            showCheckmate(board);
                        }
                    }
                }

				}
			else
			{
				//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
			}
			
		}
		//private void cell_Click(object sender, Board board, TableLayoutPanel b, Cell[] cells)
		//{
		//	//System.Diagnostics.Debug.Write(board.AI_BlackON + " " + board.AI_WhiteON + " " + board.turn);
		//	if ((board.AI_WhiteON && board.turn%2 == 0) || (board.AI_BlackON && board.turn%2==1 ))
		//          {
		//		doAITurn(board, cells);
		//          }
		//	Cell targetCell = (Cell)sender;

		//	if (board.allPossibleMoves != null) { 
		//		if (board.selected_cell == -1) // if no cell active selected
		//		{
		//			if (board.turn % 2 == ChessAI.getCellColor(targetCell.index, board.board)) // if correct color piece clicked on turn set cell as selected
		//			{
		//				selectCell(board, targetCell, cells);
		//			}
		//		}
		//		else
		//		{
		//			Cell originCell = cells[board.selected_cell];
		//			if (originCell == targetCell) // if clicked cell same as is active
		//			{
		//				deselectCell(board, originCell, cells);

		//			} else if (board.allPossibleMoves[originCell.index] != null && board.allPossibleMoves[originCell.index].Contains(targetCell.index)) 
		//			{
		//				doTurn(board, cells, originCell, targetCell, false);
		//				if (board.allPossibleMoves == null)
		//				{
		//					//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
		//					showCheckmate(board);
		//				}
		//			} 
		//		}

		//	} else
		//          {
		//		//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
		//	}
		//}

		void showCheckmate(Board board)
        {
			String winner = board.turn % 2 == 1 ? "White" : "Black";
			var m = new Form2(winner);
			m.Show();
			wait(5000);
			m.Close();
        }
		void setPossibleMoves(Board board, int cell)
        {
			//foreach (KeyValuePair<int, int> move in board.allPossibleMovesKV)
			//{
			//    if (move.Key == cell)
			//    {
			//        board.possibleMovesList.Add(move.Value);
			//    }
			//}
			board.possibleMoves = board.allPossibleMoves[cell];

   //         foreach (Move move in board.allMoves)
   //         {
			//	if (move.from == cell)
   //             {
			//		board.possibleMovesList.Add(move.to);

			//	}
			//}
        }
		void selectCell(Board board, int cell)
        {
			board.selected_cell = cell;
			//requireUpdatedDrawing(cell);
			//setPossibleMoves(board, cell);
			board.possibleMoves = board.allPossibleMoves[cell];
			printArray(board.possibleMoves, printBoxDebug);
			//drawBoard(board);
		}

		void deselectCell(Board board)
        {
			board.selected_cell = -1; // reset selection to none
			board.resetPossibleMoves();
			//requireUpdatedDrawing(cell);
			//drawBoard(board);
			//printArray(board.possibleMoves, printBoxDebug);
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
			
		void undoTurn(Board board)
        {
			//ChessAI.UnmakeMove(board);
			if (board.moveStack.Count == 0) return;
			ChessAI.UnmakeMoveNew(board, board.moveStack[board.moveStack.Count-1]);
			
			board.resetPossibleMoves();
			drawBoard2(board);
			board.allMoves = ChessAI.getAllPossibleMovesNew(board, board.turn % 2);
			
		}

		void doTurn3(Board board, Move move, bool aiTurn)
		{

			// update gamestate
			ChessAI.MakeMoveNew(board, move, aiTurn);
			board.selected_cell = -1;

			// change turn, draw all and get new moves

			board.resetPossibleMoves();
			board.allMoves = ChessAI.getAllPossibleMovesNew(board, board.turn % 2);
			//board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, board.turn % 2);
			drawBoard2(board);
			// if ai on for the turné
			if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				doAITurn3(board);
			}
		}

		void doTurn2(Board board, int originCell, int targetCell, bool aiTurn)
		{
			if (board.allPossibleMovesKV == null)
			{
				//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
				return;
			}
			// update gamestate

			ChessAI.MakeMoveSoft(board, originCell, targetCell, aiTurn);
		
			board.selected_cell = -1;

			// change turn, draw all and get new moves
			
			board.resetPossibleMoves();
			board.allPossibleMovesKV = board.allPossibleMovesKV = ChessAI.getAllPossibleMovesLowV2(board, board.turn % 2);
			//board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, board.turn % 2);
			drawBoard2(board);
			// if ai on for the turné
			if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				doAITurn2(board);
			}
		}

		void doTurn4(Board board, int originCell, int targetCell, bool aiTurn, int promotion = -1)
		{
			if (board.allPossibleMovesKV == null)
			{
				//System.Diagnostics.Debug.Write("\n ** IT'S A CHECKMATE, MATE ** \n");
				return;
			}
			// update gamestate
		
			ChessAI.MakeMoveSoft(board, originCell, targetCell, aiTurn, promotion);

			board.selected_cell = -1;

			
			// change turn, draw all and get new moves

			board.resetPossibleMoves();
			board.allPossibleMovesKV =  ChessAI.getAllPossibleMovesLowV2(board, board.turn % 2);
			if (board.allPossibleMovesKV.Count == 0)
			{
				showCheckmate(board);
				return;
			}
			//board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, board.turn % 2);
			drawBoard2(board);
			// if ai on for the turné
			if ((board.turn % 2 == 1 && blackAIButton.Checked) || board.turn % 2 == 0 && whiteAIButton.Checked)
			{
				// check if moves left!!!!
				doAITurn4(board);
			}
		}


		void doTurn(Board board, Cell[] cells, Cell originCell, Cell targetCell, bool aiTurn) 
        {
			if (board.allPossibleMoves == null)
			{
				return;
			}
			// update gamestate
			
			if (CheckIfKingCastled(board.board, originCell.index, targetCell.index))
            {
				//System.Diagnostics.Debug.Write("castled \n");
				resetPossibleMoves(board);
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

				resetPossibleMoves(board);
				ChessAI.checkFirstMoveByKingOrRook(board.board, originCell.index);
				board.board[targetCell.index] = board.board[originCell.index];
				board.board[originCell.index] = 0;
				requireUpdatedDrawing(originCell);
				requireUpdatedDrawing(targetCell);
			}

			board.selected_cell = -1;
			ChessAI.checkPawnUpdate(board, aiTurn);
            

			// change turn, draw all and get new moves
			if (board.turn % 2 == 0) { board.turn += 1; turn_label.Text = "Black"; }
			else
			{ board.turn += 1; turn_label.Text = "White"; }
			ChessAI.CancelEnPassant(board.board, board.turn % 2);
			printBoard(board);
			drawBoard(board);
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

		void doAITurn2(Board board)
		{
			int[] result = ChessAI.AITurn(board);
			wait(10);
			if (result != null)
			{
				doTurn2(board, result[0], result[1], true);
			}
			else
			{
				//System.Diagnostics.Debug.Write("Debug nulll;");
			}
		}

		void doAITurn4(Board board)
		{
			int[] result = ChessAI.AITurn(board);
			wait(10);
			if (result != null)
			{
				doTurn4(board, result[0], result[1], true, result[2]);
			}
			else
			{
				//System.Diagnostics.Debug.Write("Debug nulll;");
			}
		}

		void doAITurn3(Board board)
		{
			Move result = ChessAI.AITurn2(board);
			ChessAI.AITurn(board);
			wait(10);
			if (result != null)
			{
				doTurn3(board, result, true);
			}
			else
			{
				//System.Diagnostics.Debug.Write("Debug nulll;");
			}
		}
		void drawBoard2(Board board)
        {
			for (int i = 0; i < board.board.Length; i++)
            {
				drawCell2(board, board.cells[i]);
            }
			printBoard(board);
			if (board.turn % 2 == 0) { turn_label.Text = board.turn + " white"; }
			else
			{ turn_label.Text = board.turn + " black"; }
		}


		void drawBoard(Board board)
		{
			for (int i = 0; i < board.board.Length; i++)
			{
				drawCell(board, board.cells[i]);
			}
		}
		void drawCell(Board board, Cell cell)
		{
			int index = cell.index;
			int cellValue = board.board[index];


			if (board.selected_cell == index)
			{
				setActiveCellColor(cell);
			}
			else if (board.possibleMoves != null && board.possibleMoves.Contains(index))
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
					else if (cellValue > 20)
					{
						cell.ImageLocation = $@"images\{cellValue - 20}.png";
					}
					else
					{
						cell.ImageLocation = $@"images\{cellValue}.png";

					}
				}
				else
				{
					cell.ImageLocation = null;
				}
				cell.updated = true;
			}
		}

		void drawCell2(Board board, Cell cell)
        {
			int index = cell.index;
			int cellValue = board.board[index];

	
				if (board.selected_cell == index)
                {
					setActiveCellColor(cell);
                } else if (board.possibleMoves != null && board.possibleMoves.Contains(cell.index))
                {
					setLegalCellColor(cell);
                }		
				else
                {
					resetCellColor(cell);
                }
			
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

		void resetPossibleMoves(Board board)
        {
			int[] moveArray =
			{
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			board.possibleMoves = moveArray;
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

		void newgame_Click(Board board)
		{
			startNewGame(board);
		}


		void LoadTestBoard(Board board)
		{

			board.initializeTestBoard();
			foreach (Cell cell in board.cells)
			{
				requireUpdatedDrawing(cell);
			};

			//board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, 0);
			//board.allPossibleMovesKV = ChessAI.getAllPossibleMovesLowV2(board, board.turn % 2);
		    board.allPossibleMoves = ChessEngine.GetAllPossibleMoves(board.board, board.turn % 2);
			blackAIButton.Checked = board.AI_BlackON;
			whiteAIButton.Checked = board.AI_WhiteON;
			turn_label.Text = (board.turn % 2 == 0) ? "White" : "Black";
			drawBoard(board);
			printBoard(board);
		}

		void printFEN(Board board)
		{
			fenDisp.Text = ChessAI.BoardToFEN(board.board, board.turn);
		}

		void LoadFromFEN(Board board)
        {
			if (fenDisp.Text == "") return;
			ChessAI.FENToBoard(fenDisp.Text, board);
			SetBoardSettings(board);
		}
		void SetBoardSettings(Board board)
        {
			printBoard(board);
			Cell[] cells = board.cells;
			foreach (Cell cell in cells)
			{
				requireUpdatedDrawing(cell);
			}
			//board.allPossibleMoves = ChessAI.getAllPossibleMoves(board.board, 0);
			//board.allPossibleMovesKV = ChessAI.getAllPossibleMovesLowV2(board, 0);
			//board.allMoves = ChessAI.getAllPossibleMovesNew(board, board.turn%2);
			board.allPossibleMoves = ChessEngine.GetAllPossibleMoves(board.board, board.turn % 2);
			blackAIButton.Checked = board.AI_BlackON;
			whiteAIButton.Checked = board.AI_WhiteON;
			turn_label.Text = (board.turn % 2 == 0) ? "White" : "Black";
			board.CountPieces();
			drawBoard(board);
			printBoard(board);

		}

		void startNewGame(Board board)
		{
			
			board.initializeBoard();
			SetBoardSettings(board);
		}

		void CopyToClipboard()
        {
			Clipboard.SetText(fenDisp.Text);
        }


    }


}

