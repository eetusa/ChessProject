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

            return moveArray;
        }

		public static int[][] getAllPossibleMovesBroad(int[] board)
		{

			int[][] superArray = new int[64][];
			int[] tempArray = new int[28];
			bool empty = true;

			for (int i = 0; i < superArray.Length; i++)
			{				
					tempArray = getPossibleMovesLow(board, i);
					if (tempArray[0] != -1)
					{
						superArray[i] = new int[28];
						Array.Copy(tempArray, superArray[i], 28);
						empty = false;
					}				
			}
			if (!empty) return superArray;
			else return null;
		}

		public static bool checkPawnUpdate(int[] board, bool aiTurn)
        {
			bool result2 = false;
			for (int i = 0; i < 8; i++)
            {
				if (board[i] == 1)
                {
					result2 = true;
					if (!aiTurn) {
						using (Form3 form = new Form3())
						{

							var result = form.ShowDialog();
							if (result == System.Windows.Forms.DialogResult.OK)
							{
								int returnValue = form.returnValue;            //values preserved after close
								if (returnValue == 6)
								{
									board[i] = 6;
								}
								if (returnValue == 7)
								{
									board[i] = 7;
								}
								if (returnValue == 8)
								{
									board[i] = 8;
								}
								if (returnValue == 9)
								{
									board[i] = 9;
								}

							}
						}
					} else
                    {
						board[i] = 9;
                    }
				}
            }
			for (int i = 56; i < 64; i++)
			{
				if (board[i] == 11)
				{
					result2 = true;
					if (!aiTurn)
					{
						using (Form3 form = new Form3())
						{

							var result = form.ShowDialog();
							if (result == System.Windows.Forms.DialogResult.OK)
							{
								int returnValue = form.returnValue;            //values preserved after close
								if (returnValue == 6)
								{
									board[i] = 16;
								}
								if (returnValue == 7)
								{
									board[i] = 17;
								}
								if (returnValue == 8)
								{
									board[i] = 18;
								}
								if (returnValue == 9)
								{
									board[i] = 19;
								}

							}
						}
                    }
                    else
                    {
						board[i] = 19;
                    }
				}
			}
			return result2;
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
			if (targetCell != originCell)
            {
				tempArr[targetCell] = tempArr[originCell];
				tempArr[originCell] = 0;
			}
           

            for (int i = 0; i < board.Length; i++)
            {
                if (tempArr[i] == 10 || tempArr[i] == 30 ) whiteking = i;
                if (tempArr[i] == 20 || tempArr[i] == 40 ) blackking = i;
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

		static bool kingThreatened(int[] board, int originCell, int targetCell, bool enpassant)
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
			if (targetCell - originCell == 7 || targetCell - originCell == -9)
            {
				tempArr[originCell - 1] = 0;
            }
			if (targetCell - originCell == -7 || targetCell - originCell == 9)
			{
				tempArr[originCell + 1] = 0;
			}

			for (int i = 0; i < board.Length; i++)
			{
				if (tempArr[i] == 10 || tempArr[i] == 30) whiteking = i;
				if (tempArr[i] == 20 || tempArr[i] == 40) blackking = i;
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

		static bool legalMoveNew(int[] board, int originCell, int targetCell, int type) // problem with board state
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
			if (piece == 10 || piece == 20 || piece == 30 || piece == 40)
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
			if (piece == 8 || piece == 18 || piece == 28 || piece == 38)
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
				if ((piece == 28 && board[targetCell] == 30) || (piece == 38 && board[targetCell] == 40))
                {
					return canRookCastle(board, originCell, targetCell);
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
					if (!kingThreatened(board, originCell, targetCell))
					{
						//SetupEnPassant(board, originCell, targetCell);
						return true;
					}
					return false;
				}
				else if ((movement == 7 || movement == 9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1 ) && targetOccupiedByEnemy)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if ((movement == 7 || movement == 9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1) && (!targetOccupiedByEnemy))
				{
					return CheckEnPassant(board, originCell, targetCell, movement);
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
					if (!kingThreatened(board,originCell,targetCell))
                    {
						//SetupEnPassant(board, originCell, targetCell);
						return true;
                    }
					return false;
				}
				else if ((movement == -7 || movement == -9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1) && (targetOccupiedByEnemy))
				{
					return !kingThreatened(board, originCell, targetCell);
				} else if ((movement == -7 || movement == -9 && Math.Abs((originCell / 8) - (targetCell / 8)) == 1) && (!targetOccupiedByEnemy)){
					return CheckEnPassant(board, originCell, targetCell, movement);
                }
			}


			return false;
		}

		static bool CheckEnPassant(int[] board, int originCell, int targetCell, int movement)
        {
			if (board[targetCell] == 0)
            {
				if (movement == 7 || movement == -9)
                {
					if (board[originCell+1] == 2 || board[originCell+1] == 12)
                    {
						if (getCellColor(originCell, board) != getCellColor(board[originCell + 1], board)) return !kingThreatened(board, originCell, targetCell, true);
                    }
                } else if (movement == -8 || movement == 9)
                {
					if (board[originCell - 1] == 2 || board[originCell - 1] == 12)
					{
						if (getCellColor(originCell, board) != getCellColor(board[originCell - 1], board)) return !kingThreatened(board, originCell, targetCell, true);
					}
				}

            }
			return false;
        }

		public static bool SetupEnpassantPawn(int[] board, int originCell, int targetCell)
        {
			if ((originCell/8 == 1 && targetCell/8 == 3) || (originCell / 8 == 6 && targetCell / 8 == 4))
            {
				if (board[originCell] == 1) { board[originCell] = 2; return true; }
				if (board[originCell] == 11) { board[originCell] = 12; return true; }
			}
			return false;
        }

		public static void CancelEnPassant(int[] board, int color)
        {
			//System.Diagnostics.Debug.Write(" COLOR: " + color + " \n");
			if (color == 1)
            {
				for (int i = 24; i < 32; i++)
				{
					if (board[i] == 12) board[i] = 11;
				}
			}
			if (color == 0)
			{
				for (int j = 32; j < 40; j++)
				{
					if (board[j] == 2) board[j] = 1;
				}
			}
		}

		public static int getCellColor(int cellIndex, int[] board)
		{
			int cellContentInt = board[cellIndex];

			if ((cellContentInt > 0 && cellContentInt < 11) || cellContentInt == 28 || cellContentInt == 30)
			{
				return 0; // white
			}
			else if (cellContentInt > 10 && cellContentInt < 21 || cellContentInt == 38 || cellContentInt == 40)
			{
				return 1; // black
			}
			return -1;
		}

		static bool canRookCastle(int[] board, int originCell, int targetCell)
        {
			//System.Diagnostics.Debug.Write("OriginCell: " + originCell + " targetCell: " + targetCell + "\n");
			if (targetCell > originCell)
            {
				for (int i = originCell+1; i < targetCell; i++)
                {
					
					if (board[i] != 0)
                    {
						return false;
                    }
                }
				for (int i = targetCell; i > targetCell - 3; i--)
                {
					System.Diagnostics.Debug.Write("targetcell > origincell. i: " + i + " targetcell: " +targetCell + " originCell: " + originCell +"\n");
					if (kingThreatened(board, targetCell, i)) return false;
                }
			}
            else
            {
				for (int i = originCell-1; i > targetCell; i--)
				{
					if (board[i] != 0)
					{
						return false;
					}
				}
				//System.Diagnostics.Debug.Write(targetCell + " x " + (targetCell - 3) + " ");
				for (int i = targetCell; i < targetCell + 3; i++)
				{
					System.Diagnostics.Debug.Write("targetcell < origincell. i: " + i + " targetcell: " + targetCell + " originCell: " + originCell + "\n");
					if (kingThreatened(board, targetCell, i)) return false;
				}
			}
			
			return true;
        }

		public static void castleMove(int[] board, int originCell, int targetCell, Form1.Cell[] cells)
        {
			if (originCell == 0 || originCell == 56)
            {
				board[targetCell - 1] = board[originCell]-20;
				board[targetCell - 2] = board[targetCell]-20;
				board[originCell] = 0;
				board[targetCell] = 0;
				cells[targetCell - 1].updated = false;
				cells[targetCell - 2].updated = false;
				cells[targetCell].updated = false;
				cells[originCell].updated = false;
			}
			if (originCell == 7 || originCell == 63)
            {
				board[targetCell + 1] = board[originCell] - 20;
				board[targetCell + 2] = board[targetCell] - 20;
				board[originCell] = 0;
				board[targetCell] = 0;
				cells[targetCell + 1].updated = false;
				cells[targetCell + 2].updated = false;
				cells[targetCell].updated = false;
				cells[originCell].updated = false;
			}
        }

		public static void castleMoveSoft(int[] board, int originCell, int targetCell)
		{
			if (originCell == 0 || originCell == 56)
			{
				board[targetCell - 1] = board[originCell] - 20;
				board[targetCell - 2] = board[targetCell] - 20;
				board[originCell] = 0;
				board[targetCell] = 0;

			}
			if (originCell == 7 || originCell == 63)
			{
				board[targetCell + 1] = board[originCell] - 20;
				board[targetCell + 2] = board[targetCell] - 20;
				board[originCell] = 0;
				board[targetCell] = 0;
			}
		}

		public static void CastleUnmoveSoft (int[] board, int originCell, int targetCell)
        {
			if (originCell == 0 || originCell == 56)
            {
				board[targetCell] = board[targetCell - 2] + 20;
				board[originCell] = board[targetCell - 1] + 20;
				board[targetCell - 1] = 0;
				board[targetCell - 2] = 0;
            }
			if (originCell == 7 || originCell == 63)
            {
				board[targetCell] = board[targetCell + 2] + 20;
				board[originCell] = board[targetCell + 1] + 20;
				board[targetCell + 1] = 0;
				board[targetCell + 2] = 0;
            }
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

		static void MakeMoveSoft(int[] board, int originCell, int targetCell)
        {
			if ((board[originCell] == 38 && board[targetCell] == 40) || (board[originCell] == 28 && board[targetCell] == 30))
			{
				castleMoveSoft(board, originCell, targetCell);
			}
            else
            {
				board[targetCell] = board[originCell];
				board[originCell] = 0;
            }

		}

		static void UnmakeMoveSoft(int[] board, int originCell, int targetCell, int targetValue)
        {
			if ((targetValue == 40 && (originCell == 0 || originCell == 7) ) || (targetValue == 30) && (originCell == 57 || originCell == 63) ) 
            {
				CastleUnmoveSoft(board, originCell, targetCell);
            } else
            {
				board[originCell] = board[targetCell];
				board[targetCell] = targetValue;
			}

        }

		public static string ReturnAlphabeticalCoordinate(int index)
        {
			int row = index / 8;
			int col = index - (index / 8) * 8;
			string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
			return letters[row] + col;
        }
		public static Int64 MoveGenerationTest(int depth, int[] board, int turn)
        {
			if (depth == 0)
            {
				return 1;
            }

			int[][] movesList = new int[64][];

			movesList = getAllPossibleMoves(board, turn);
			Int64 numPositions = 0;
			if (movesList != null)
            {
				for (int i = 0; i < movesList.Length; i++)
				{
					if (movesList[i] != null)
                    {	
						
						for (int j = 0; j < movesList[i].Length; j++)
                        {
							
							if (movesList[i][j] == -1)
                            {
								break;
                            }
							int tempCellValue = board[movesList[i][j]];

							MakeMoveSoft(board, i, movesList[i][j]);
							numPositions += MoveGenerationTest(depth - 1, board, (turn+1)%2);
							
							UnmakeMoveSoft(board, i, movesList[i][j], tempCellValue);

                        }
						
						
                    }
				}
			}
			return numPositions;
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