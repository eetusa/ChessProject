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
		public bool whiteKingThreatened, blackKingThreatened;
		public Board()
		{
			board = new int[] {
				18, 17, 16, 20, 19, 16, 17, 18,
								11, 11, 11, 11, 11, 11, 11, 11,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								0, 0, 0, 0, 0, 0, 0, 0,
								1, 1, 1, 1, 1, 1, 1, 1,
								8, 7, 6, 10, 9, 6, 7, 8,
								};
			this.selected_cell = -1;
			this.turn = 0;
			this.possibleMoves = new int[] {
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
				-1, -1, -1, -1,-1, -1, -1, -1
			};
			this.whiteKingThreatened = false;
			this.blackKingThreatened = false;
		}
	}
}
