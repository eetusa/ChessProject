using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
	public class Board
	{
		public Form1.Cell[] cells;
		public int[] board;
		public int selected_cell;
		public int turn; // 0 white, 1 black
		public int[] possibleMoves;
		public int[][] allPossibleMoves;
		public List<List<int>> pawns, bishops, queens, kings, rooks, knights;
		public List<List<int>>  whitePieces, blackPieces;
		public bool AI_WhiteON, AI_BlackON;
		public List<KeyValuePair<int, int>> allPossibleMovesKV;
		public List<int> possibleMovesList;
			//, whiteKingMoved, whiteRookLeftMoved, whiteRookRightMoved,
			//blackKingMoved, blackRookLeftMoved, blackRookRightMoved;
		public Board()
		{
			cells = new Form1.Cell[64];
			allPossibleMovesKV = new List<KeyValuePair<int, int>>();
			possibleMovesList = new List<int>();
			this.initializeBoard();
		}

		public void setBoard(int[] board, int turn, int halfmove)
        {

			this.board = board;
			this.turn = turn;
			this.selected_cell = -1;
			resetPossibleMoves();
			this.allPossibleMoves = new int[64][];
			this.AI_BlackON = false;
			this.AI_WhiteON = false;
		}

		public void resetPossibleMoves()
        {
			this.possibleMoves = new int[] {
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			if (possibleMovesList != null) possibleMovesList.Clear();
		}

		private void InitializePieceLists()
        {
			whitePieces = new List<List<int>>();
			blackPieces = new List<List<int>>();

			pawns = new List<List<int>>();
			pawns.Add(new List<int>());
			pawns.Add(new List<int>());

			whitePieces.Add(pawns[0]);
			blackPieces.Add(pawns[1]);

			rooks = new List<List<int>>();
			rooks.Add(new List<int>());
			rooks.Add(new List<int>());

			whitePieces.Add(rooks[0]);
			blackPieces.Add(rooks[1]);

			bishops = new List<List<int>>();
			bishops.Add(new List<int>());
			bishops.Add(new List<int>());

			whitePieces.Add(bishops[0]);
			blackPieces.Add(bishops[1]);

			queens = new List<List<int>>();
			queens.Add(new List<int>());
			queens.Add(new List<int>());

			whitePieces.Add(queens[0]);
			blackPieces.Add(queens[1]);

			kings = new List<List<int>>();
			kings.Add(new List<int>());
			kings.Add(new List<int>());

			whitePieces.Add(kings[0]);
			blackPieces.Add(kings[1]);

			knights = new List<List<int>>();
			knights.Add(new List<int>());
			knights.Add(new List<int>());

			whitePieces.Add(knights[0]);
			blackPieces.Add(knights	[1]);
		}
		public void CountPieces()
        {
			InitializePieceLists();

			for (int i = 0; i < board.Length; i++)
            {
				if (board[i] != 0)
                {
					if (board[i] == 38 || board[i] == 18)
                    {
						rooks[1].Add(i);
                    } else if (board [i] == 28 || board[i] == 8)
                    {
						rooks[0].Add(i);
                    } else if (board[i] == 17)
                    {
						knights[1].Add(i);
                    }
					else if (board[i] == 7)
					{
						knights[0].Add(i);
					}
					else if (board[i] == 16)
					{
						bishops[1].Add(i);
					}
					else if (board[i] == 6)
					{
						bishops[0].Add(i);
					}
					else if (board[i] == 19)
					{
						queens[1].Add(i);
					}
					else if (board[i] == 9)
					{
						queens[0].Add(i);
					}
					else if (board[i] == 20 || board[i] == 40)
					{
						kings[1].Add(i);
					}
					else if (board[i] == 10 || board[i] == 30)
					{
						kings[0].Add(i);
					}
					else if (board[i] == 11 || board[i] == 12)
					{
						pawns[1].Add(i);
					}
					else if (board[i] == 1 || board[i] == 2)
					{
						pawns[0].Add(i);
					}
				}
            }
        }
		public void initializeTestBoard()
        {
			board = new int[] {
								38, 17, 16, 19, 0, 20, 0, 38,
								11, 11, 0, 1, 16, 11, 11, 11,
								0, 0, 11, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 6, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								1, 1, 1, 0, 7, 17, 1, 1,
								28, 7, 6, 9, 30, 0, 0, 28,
								};
			this.selected_cell = -1;
			this.turn = 0;
			resetPossibleMoves();
			this.allPossibleMoves = new int[64][];
			this.AI_BlackON = false;
			this.AI_WhiteON = false;
			CountPieces();

		}

		public void initializeBoard()
		{
			board = new int[] {
								38, 17, 16, 19, 40, 16, 17, 38,
								11, 11, 11, 11, 11, 11, 11, 11,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								1, 1, 1, 1, 1, 1, 1, 1,
								28, 7, 6, 9, 30, 6, 7, 28,
								};
			this.selected_cell = -1;
			this.turn = 0;
			resetPossibleMoves();
			this.allPossibleMoves = new int[64][];
			this.AI_BlackON = true;
			this.AI_WhiteON = false;
			CountPieces();

			//this.whiteKingMoved = false; 
			//this.whiteRookLeftMoved = false;
			//this.whiteRookRightMoved = false;
			//this.blackKingMoved = false;
			//this.blackRookLeftMoved = false;
			//this.blackRookRightMoved = false;
		}
	}

}
