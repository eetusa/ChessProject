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
		public Board()
		{
			board = new int[] {
								18, 17, 16, 19, 20, 16, 17, 18,
								11, 11, 11, 11, 11, 11, 11, 11,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								1, 1, 1, 1, 1, 1, 1, 1,
								8, 7, 6, 9, 10, 6, 7, 8,
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
		}
	}
}
