using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
    public static class ChessAI
    {


        public static int[] AITurn(Board Cboard)
        {
            int turn = Cboard.turn;
            int[] board = Cboard.board;

            testi(board);
			

            return randomMove(board, turn, Cboard.allPossibleMoves);
        }

		static int[] randomMove(int[] board, int turn, int[][] allMoves)
        {
			int availPieces = 0;
			if (allMoves == null) return null;
			for (int i = 0; i < board.Length; i++)
            {
				if (getCellColor(i, board) == turn % 2 && allMoves[i] != null)
				{
					availPieces++;
				}
			}
			int[] avaivablePieces = new int[availPieces];
			int temp = 0;
			for (int i = 0; i < board.Length; i++)
            {
				if (getCellColor(i, board) == turn % 2 && allMoves[i]!=null)
                {
					avaivablePieces[temp] = i;
					temp++;
                }
            }

			if (avaivablePieces[0] == -1) return null;

			Random rand = new Random();
			
			int pieceIndex = avaivablePieces[rand.Next(0, avaivablePieces.Length)];
			
			int[] moves = allMoves[pieceIndex];
			int pituus = 0;
			for (int j = 0; j < moves.Length; j++)
            {
				if (moves[j] == -1) break;
				pituus++;
            }
			int moveIndex = rand.Next(0, pituus);
			int[] result = { pieceIndex, moves[moveIndex] };

			return result;
        }

        static void testi(int[] board)
        {

        }
        public static int[][] getAllPossibleMoves(int[] board, int color)
        {

            int[][] superArray = new int[64][];
            int[] tempArray = new int[28];
            bool empty = true;

            for (int i = 0; i < superArray.Length; i++)
            {
                int gotColor = getCellColor(i, board);
                if (gotColor == color)
                {
                    tempArray = getPossibleMovesLow(board, i);
                    if (tempArray[0] != -1)
                    {
                        superArray[i] = new int[28];
                        Array.Copy(tempArray, superArray[i], 28);
                        empty = false;
                    }

                }
            }
            if (!empty) return superArray;
            else return null;
        }

        static int[] getPossibleMovesLow(int[] board, int originCell)
        {
            int[] moveArray =
            {
                -1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
                -1, -1, -1, -1,-1, -1, -1, -1, -1,-1,
                -1, -1, -1, -1,-1, -1, -1, -1
            };


            int temp = 0;
            for (int i = 0; i < board.Length; i++)
            {

                if (legalMoveNew(board, originCell, i, 0))
                {
                    moveArray[temp] = i;
                    temp++;
                }

            }
            //System.Diagnostics.Debug.Write("\n" + "possible moves length: " + temp + "\n");

            //printArray(moveArray, printBoxDebug);
            return moveArray;
        }

		static int[] checkPawnUpdate(int[] board)
        {
			for (int i = 0; i < 8; i++)
            {
				//if (board[i] == 0) ;
            }
			return null;
        }
        static bool kingThreatened(int[] board, int originCell, int targetCell)
        {
            int[] tempArr = new int[64];
            int whiteking = 0;
            int blackking = 0;

            for (int i = 0; i < board.Length; i++)
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
                }
                else if (tempColor == 1)
                {
                    if (legalMoveNew(tempArr, j, whiteking, 1))
                    {
                        if (cellColor == 0) return true;
                    }
                }
            }
            return false;
        }

		// to fix: pawn changes rank, rook castles
		// get all possible moves after movement. compare shit against that until moved
		static bool legalMoveNew(int[] board, int originCell, int targetCell, int type)
		{
			int piece = board[originCell];
			bool targetOccupied = board[targetCell] != 0 ? true : false;
			bool targetOccupiedByEnemy = ((getCellColor(originCell, board) != getCellColor(targetCell, board)) && getCellColor(targetCell, board) != -1) ? true : false;
			int movement = originCell - targetCell;

			// knight
			if (piece == 7 || piece == 17)
			{
				int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
				if ((movement == -17 || movement == -15 || movement == 17 || movement == 15) && rowdif == 2)
				{
					if (targetOccupiedByEnemy || !targetOccupied)
					{
						if (type != 1)
						{
							return !kingThreatened(board, originCell, targetCell);
						}

						return true;
					}
				}
				if ((movement == -6 || movement == -10 || movement == 6 || movement == 10) && rowdif == 1)
				{
					if (targetOccupiedByEnemy || !targetOccupied)
					{
						if (type != 1)
						{
							return !kingThreatened(board, originCell, targetCell);
						}

						return true;
					}
				}
			}

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
				}
				else if ((movement == 7 || movement == 8 || movement == 9 || movement == -7 || movement == -8 || movement == -9) && rowdif == 1)
				{
					if (targetOccupiedByEnemy || !targetOccupied)
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
				else if ((movement == 7 || movement == 9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1 ) && targetOccupiedByEnemy)
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
				else if (movement == -16 && originCell / 8 == 1 && !targetOccupied && type != 1)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if ((movement == -7 || movement == -9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1) && (targetOccupiedByEnemy))
				{
					return !kingThreatened(board, originCell, targetCell);
				}
			}


			return false;
		}
		static int getCellColor(int cellIndex, int[] board)
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

		static bool checkStraightMovement(int[] board, int origin, int target, int movement)
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
	}


}




//bool kingThreatened (int[] board, int originCell, int targetCell)
//      {
//	int[] tempArr = new int[64];
//	int whiteking = 0;
//	int blackking = 0;

//	for (int i=0; i < board.Length; i++)
//          {
//		tempArr[i] = board[i];
//          }

//	int cellColor = getCellColor(originCell, board);
//	tempArr[targetCell] = tempArr[originCell];
//	tempArr[originCell] = 0;

//	for (int i = 0; i < board.Length; i++)
//	{
//		if (tempArr[i] == 10) whiteking = i;
//		if (tempArr[i] == 20) blackking = i;
//	}

//	for (int j = 0; j < tempArr.Length; j++)
//          {
//		int tempColor = getCellColor(j, board);
//		if (tempColor == 0)
//              {

//			if (legalMoveNew(tempArr, j, blackking, 1))
//                  {
//				if (cellColor == 1) return true;
//                  }
//              } else if (tempColor == 1)
//              {
//			if (legalMoveNew(tempArr, j, whiteking, 1))
//			{
//				if (cellColor == 0) return true;
//			}
//		}
//	}
//	return false;
//      }

//// to fix: pawn changes rank, rook castles
//// get all possible moves after movement. compare shit against that until moved
//bool legalMoveNew(int[] board, int originCell, int targetCell, int type)
//      {
//	int piece = board[originCell];
//	bool targetOccupied = board[targetCell] != 0 ? true : false;
//	bool targetOccupiedByEnemy = ((getCellColor(originCell, board) != getCellColor(targetCell, board)) && getCellColor(targetCell, board) != -1) ? true : false;
//	int movement = originCell - targetCell;

//	// knight
//	if (piece == 7 || piece == 17)
//          {
//		int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
//		if ((movement == -17 || movement == -15 || movement == 17 || movement == 15) && rowdif == 2)
//              {
//			if (targetOccupiedByEnemy || !targetOccupied)
//			{
//				if (type != 1)
//				{
//					return !kingThreatened(board, originCell, targetCell);
//				}

//				return true;
//			}
//		}
//		if ( (movement == -6 || movement == -10 || movement == 6 || movement == 10) && rowdif == 1)
//              {
//			if (targetOccupiedByEnemy || !targetOccupied)
//			{
//				if (type != 1)
//				{
//					return !kingThreatened(board, originCell, targetCell);
//				}

//				return true;
//			}
//		}
//	}

//	// king
//	if (piece == 10 || piece == 20)
//          {
//		int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
//		if (movement == 1 || movement == -1)
//              {
//			if (rowdif != 0) return false;
//			if (targetOccupiedByEnemy || !targetOccupied)
//                  {
//				if (type != 1)
//                      {
//					return !kingThreatened(board, originCell, targetCell);
//				}

//				return true;
//                  }
//              } else if (( movement == 7 || movement == 8 || movement == 9 || movement == -7 || movement == -8 || movement == -9 ) && rowdif == 1)
//              {
//			if ( targetOccupiedByEnemy || !targetOccupied )
//			{
//				if (type != 1)
//				{
//					return !kingThreatened(board, originCell, targetCell);
//				}
//				return true;
//			}
//		}
//          }

//	// bishop
//	if (piece == 6 || piece == 16)
//	{
//		if (checkStraightMovement(board, originCell, targetCell, -7))
//              {
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, -9))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;

//		}
//		if (checkStraightMovement(board, originCell, targetCell, 7))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 9))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//	}

//	// rook
//	if (piece == 8 || piece == 18)
//	{
//		if (checkStraightMovement(board, originCell, targetCell, -1))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, -8))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 1))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 8))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//	}

//	// queen
//	if (piece == 9 || piece == 19)
//	{
//		if (checkStraightMovement(board, originCell, targetCell, -7))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, -9))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 7))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 9))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, -1))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, -8))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 1))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//		if (checkStraightMovement(board, originCell, targetCell, 8))
//		{
//			if (type != 1)
//			{
//				return !kingThreatened(board, originCell, targetCell);
//			}
//			return true;
//		}
//	}

//	// white pawn
//	if (piece == 1)
//	{
//		if (movement == 8 && !targetOccupied && type != 1)
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//		else if (movement == 16 && originCell / 8 == 6 && !targetOccupied && type != 1)
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//		else if ((movement == 7 || movement == 9) && targetOccupiedByEnemy)
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//	}

//	// black pawn
//	if (piece == 11)
//	{
//		if (movement == -8 && !targetOccupied && type != 1)
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//		else if (movement == -16 && originCell/8 == 1 && !targetOccupied  && type != 1)
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//		else if ((movement == -7 || movement == -9) && (targetOccupiedByEnemy))
//		{
//			return !kingThreatened(board, originCell, targetCell);
//		}
//	}


//	return false;
//      }