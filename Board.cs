using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
	public class Board
	{
		public int[] board;
		public int selected_cell;
		public int turn; // 0 white, 1 black
		public int[] possibleMoves;
		public int[][] allPossibleMoves;
		public bool AI_WhiteON, AI_BlackON;
			//, whiteKingMoved, whiteRookLeftMoved, whiteRookRightMoved,
			//blackKingMoved, blackRookLeftMoved, blackRookRightMoved;
		public Board()
		{
			this.initializeBoard();
		}

		public void initializeTestBoard()
        {
			board = new int[] {
								38, 0, 0, 0, 40, 16, 17, 38,
								11, 11, 0, 11, 11, 11, 11, 11,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 8, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 18, 0, 0, 0, 19, 0,
								1, 1, 0, 0, 0, 1, 0, 1,
								28, 0, 0, 0, 30, 0, 0, 28,
								};
			this.selected_cell = -1;
			this.turn = 0;
			this.possibleMoves = new int[] {
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			this.allPossibleMoves = new int[64][];
			this.AI_BlackON = false;
			this.AI_WhiteON = false;
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
			this.possibleMoves = new int[] {
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			this.allPossibleMoves = new int[64][];
			this.AI_BlackON = true;
			this.AI_WhiteON = false;
			//this.whiteKingMoved = false; 
			//this.whiteRookLeftMoved = false;
			//this.whiteRookRightMoved = false;
			//this.blackKingMoved = false;
			//this.blackRookLeftMoved = false;
			//this.blackRookRightMoved = false;
		}
	}

}
