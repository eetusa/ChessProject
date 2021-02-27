using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;


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

		static int[] minimax(int[] board, int turn, int[][] allMoves, int depth)
        {
			if (depth == 0 || allMoves == null)
            {
				return null;
            }
			return null;
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
					if ((board[originCell] == 1 && (i/8) == 0) || board[originCell] == 11 && (i/8) == 7)
                    {
						moveArray[temp] = i;
						temp++;
						moveArray[temp] = i;
						temp++;
						moveArray[temp] = i;
						temp++;
					}
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
				else if (movement == 16 && originCell / 8 == 6 && !targetOccupied && type != 1 && board[originCell - 8] == 0)
				{
					
					if (!kingThreatened(board, originCell, targetCell))
					{
						//SetupEnPassant(board, originCell, targetCell);
						return true;
					}
					return false;
				}
				else if (( (movement == 7 || movement == 9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1) ) && targetOccupiedByEnemy)
				{
					return !kingThreatened(board, originCell, targetCell);
				}
				else if (( (movement == 7 || movement == 9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1) ) && (!targetOccupiedByEnemy))
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
				else if (movement == -16 && originCell / 8 == 1 && !targetOccupied && type != 1 && board[originCell + 8] == 0)
				{
					if (!kingThreatened(board,originCell,targetCell))
                    {
						//SetupEnPassant(board, originCell, targetCell);
						return true;
                    }
					return false;
				}
				else if (((movement == -7 || movement == -9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && targetOccupiedByEnemy)
				{
					return !kingThreatened(board, originCell, targetCell);
				} else if (((movement == -7 || movement == -9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && (!targetOccupiedByEnemy))
				{
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
					if (originCell / 8 != 1 && originCell / 8 != 6)
					{
						//System.Diagnostics.Debug.Write(originCell + " " + targetCell + " " + getCellColor(originCell - 1, board) + " " + getCellColor(board[originCell + 1], board) + "\n");
					}
					if (board[originCell+1] == 2 || board[originCell+1] == 12)
                    {
						if (getCellColor(originCell, board) != getCellColor(originCell + 1, board)) return !kingThreatened(board, originCell, targetCell, true);
                    }
                } else if (movement == -7 || movement == 9)
                {
					if (board[originCell - 1] == 2 || board[originCell - 1] == 12)
					{
						//System.Diagnostics.Debug.Write(getCellColor(originCell, board) + " " + getCellColor(board[originCell - 1], board));
						if (getCellColor(originCell, board) != getCellColor(originCell - 1, board)) return !kingThreatened(board, originCell, targetCell, true);
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

		public static void CancelEnPassant(int[] board)
		{
			//System.Diagnostics.Debug.Write(" COLOR: " + color + " \n");
				for (int i = 24; i < 32; i++)
				{
					if (board[i] == 12) board[i] = 11;
				}
			

				for (int j = 32; j < 40; j++)
				{
					if (board[j] == 2) board[j] = 1;
				}
		}

		public static int CancelEnPassantReturn(int[] board)
		{
			for (int i = 24; i < 32; i++)
			{
				if (board[i] == 12) { board[i] = 11; return i; }
			}

			for (int j = 32; j < 40; j++)
			{
				if (board[j] == 2) { board[j] = 1; return j; }
			}
			return -1;
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
					//System.Diagnostics.Debug.Write("targetcell > origincell. i: " + i + " targetcell: " +targetCell + " originCell: " + originCell +"\n");
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
					//System.Diagnostics.Debug.Write("targetcell < origincell. i: " + i + " targetcell: " + targetCell + " originCell: " + originCell + "\n");
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
				CancelEnPassant(board);
				castleMoveSoft(board, originCell, targetCell);
			}
            else
            {
				if (board[originCell] == 1 || board[originCell] == 11)
				{
					if (board[targetCell] == 0)
					{
						int movement = (originCell - targetCell);
						if (movement == 7 || movement == -9)
						{
							board[originCell + 1] = 0;
						}
						else if (movement == -7 || movement == 9)
						{
							board[originCell - 1] = 0;
						}
					}
				}
				CancelEnPassant(board);
				SetupEnpassantPawn(board, originCell, targetCell);
				checkFirstMoveByKingOrRook(board, originCell);
				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}
		}

		public static string BoardToFEN(int[] board, int turn)
        {
			string result = "";
			bool blackKingSideRookMoved = true;
			bool blackQueenSideRookMoved = true;
			bool blackKingMoved = true;

			bool whiteKingSideRookMoved = true;
			bool whiteQueenSideRookMoved = true;
			bool whiteKingMoved = true;

			string enpassant = "-";

			for (int i = 0; i < board.Length; i++)
            {
				if (i != 0 && i % 8 == 0)
				{
					
					result += "/";
				}

				if (board[i] == 0)
				{
					//empty++;
					//if (i != 0 && i % 8 == 0)
					//{
					//	result += empty + "/";
					//	empty = 0;
					//}
					try
					{
						int lastNumber = Int32.Parse(""+result[result.Length - 1]);
						result = result.Remove(result.Length - 1, 1);
						result += lastNumber+1;						
					} catch
                    {
						result += "1";
                    }
				} else {
									
					if (board[i] == 18 || board[i] == 38)
					{
						result += "r";
						if (board[i] == 38 && i == 0) blackQueenSideRookMoved = false;
						if (board[i] == 38 && i == 7) blackKingSideRookMoved = false;
					}
					else if (board[i] == 8 || board[i] == 28)
					{
						result += "R";
						if (board[i] == 28 && i == 56) whiteQueenSideRookMoved = false;
						if (board[i] == 28 && i == 63) whiteKingSideRookMoved = false;
					}
					else if (board[i] == 7)
					{
						result += "N";
					}
					else if (board[i] == 17)
					{
						result += "n";
					}
					else if (board[i] == 16)
					{
						result += "b";
					}
					else if (board[i] == 6)
					{
						result += "B";
					}
					else if (board[i] == 9)
					{
						result += "Q";
					}
					else if (board[i] == 19)
					{
						result += "q";
					}
					else if (board[i] == 10 || board[i] == 30)
					{
						result += "K";
						if (board[i] == 30) whiteKingMoved = false;
					}
					else if (board[i] == 20 || board[i] == 40)
					{
						result += "k";
						if (board[i] == 40) blackKingMoved = false;
					}
					if (board[i] == 1 || board[i] == 2)
					{
						result += "P";
						if (board[i] == 2)
						{
							enpassant = ReturnAlphabeticalCoordinate(i + 8);
						}
					}
					else if (board[i] == 11 || board[i] == 12)
					{
						result += "p";
						if (board[i] == 12)
						{
							enpassant = ReturnAlphabeticalCoordinate(i - 8);
						}
					}
				}
			}

			result += " ";
			//section
			result += (turn % 2 == 0) ? "w" : "b";

			result += " ";
			//section
			if (!whiteKingMoved)
            {
				if (!whiteKingSideRookMoved)
                {
					result += "K";
                }
                if (!whiteQueenSideRookMoved)
                {
					result += "Q";
                }
            }
			if (!blackKingMoved)
            {
				if (!blackKingSideRookMoved)
                {
					result += "k";
                }
				if (!blackQueenSideRookMoved)
                {
					result += "q";
                }
            }
			if (whiteKingMoved && blackKingMoved)
            {
				result += "-";
            }

			result += " ";
			//section
			result += enpassant;

			//section half-clock pseudo
			result += " 0";

			//section fullmove number
			result += " ";
			result += turn/2 + 1;
            

			return result;
        }

		public static void FENToBoard(String FEN, Board board)
        {
			if (FEN == "") return;
			int[] result = new int[64];
			int index = 0;
			int turnTotal = 0;
			int sectionEnd = FEN.IndexOf(" ");
			int color = 0;
			int halfmove = 0;
			
		
			for (int i = 0; i < sectionEnd; i++){
				try
                {
					int numberOfEmpty = Int32.Parse(""+FEN[i]);
					for (int j = 0; j < numberOfEmpty; j++)
                    {
						result[index] = 0;
						index++;
						
                    }
                }
                catch
                {
					String current = ""+FEN[i];
					if (!current.Equals("/"))
                    {
						if (current == "R")
						{
							result[index] = 8;
						}
						else if (current == "r")
						{
							result[index] = 18;
						}
						else if (current == "N")
						{
							result[index] = 7;
						}
						else if (current == "n")
						{
							result[index] = 17;
						}
						else if (current == "B")
						{
							result[index] = 6;
						}
						else if (current == "b")
						{
							result[index] = 16;
						}
						else if (current == "Q")
						{
							result[index] = 9;
						}
						else if (current == "q")
						{
							result[index] = 19;
						}
						else if (current == "K")
						{
							result[index] = 10;
						}
						else if (current == "k")
						{
							result[index] = 20;
						} else if (current == "P")
                        {
							result[index] = 1;
                        } else if (current == "p")
                        {
							result[index] = 11;
                        }
						index++;
					}

				}
            }

			if (FEN[sectionEnd+1].Equals('b'))
            {
				color = 1;
            }
			//System.Diagnostics.Debug.Write(color + " - "+ FEN[sectionEnd + 1].Equals('b') + FEN[sectionEnd+1] + " _ " + " x \n");

			sectionEnd = FEN.IndexOf(" ", sectionEnd + 1);
			int sectionStart = sectionEnd;
			sectionEnd = FEN.IndexOf(" ", sectionEnd + 1);

			for (int i = sectionStart; i < sectionEnd; i++)
            {
				string current = ""+FEN[i];
				if (current == "K")
                {
					result[63] = 28;
					result[60] = 30;
                }
				else if (current == "Q")
                {
					result[56] = 28;
					result[60] = 30;
                } else if (current == "k")
                {
					result[7] = 38;
					result[4] = 40;
                } else if (current == "q")
                {
					result[0] = 38;
					result[4] = 40;
                }
            }

			sectionStart = sectionEnd;
			sectionEnd = FEN.IndexOf(" ", sectionEnd + 1);

			if (!(FEN[sectionStart + 1]+"" == "-"))
            {
				string current = FEN.Substring(sectionStart + 1, 2);
				char colChar = current[0];
				int rowWrong = current[1];
				try
                {
					rowWrong = Int32.Parse(""+current[1]);
                } catch
                {
					return;
                }
				char[] chars = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h' };

				int row = Math.Abs(8 - rowWrong);
				int col = Array.IndexOf(chars, colChar);
				int cellIndex = row * 8 + col;
				if (color == 0)
                {
					result[cellIndex + 8] = 12;
                } else
                {
					result[cellIndex - 8] = 2;
                }
            }

			sectionStart = sectionEnd;
			sectionEnd = FEN.IndexOf(" ", sectionEnd + 1);
			try
            {
				halfmove = Int32.Parse(""+FEN[sectionStart + 1]);
            } catch
            {
				return;
            }

			try
            {
				turnTotal = Int32.Parse(FEN.Substring(sectionEnd + 1));
            }
            catch
            {
				return;
            }

			turnTotal--;
			turnTotal = turnTotal * 2;


			board.setBoard(result, turnTotal + color, halfmove);
        }
		static int[] MakeMoveSoftReturn(int[] board, int originCell, int targetCell)
		{
			List<int> returnArray = new List<int>();

			if ((board[originCell] == 38 && board[targetCell] == 40) || (board[originCell] == 28 && board[targetCell] == 30))
			{
				int cancelPassant = CancelEnPassantReturn(board);
				if (cancelPassant != -1)
				{
					returnArray.Add(cancelPassant);
				}
				castleMoveSoft(board, originCell, targetCell);
			}
			else
			{
				if (board[originCell] == 1 || board[originCell] == 11)
				{
					if (board[targetCell] == 0)
					{
						int movement = (originCell - targetCell);
						if (movement == 7 || movement == -9)
						{
							board[originCell + 1] = 0;
						}
						else if (movement == -7 || movement == 9)
						{
							board[originCell - 1] = 0;
						}
					}
				}
				SetupEnpassantPawn(board, originCell, targetCell);
				checkFirstMoveByKingOrRook(board, originCell);
				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}
			return null;
		}

		public static void checkFirstMoveByKingOrRook(int[] board, int originCell)
		{
			if (board[originCell] == 28 || board[originCell] == 30 || board[originCell] == 38 || board[originCell] == 40)
			{
				board[originCell] = board[originCell] - 20;
			}
		}
		static void MakeMoveSoft(int[] board, int originCell, int targetCell,  int promotion)
		{
			if ((board[originCell] == 38 && board[targetCell] == 40) || (board[originCell] == 28 && board[targetCell] == 30))
			{
				castleMoveSoft(board, originCell, targetCell);
			}
			else
			{
				if (originCell/8 == 0)
                {
					if (promotion == 0)
                    {
						board[targetCell] = 9;
                    } else if (promotion == 1)
                    {
						board[targetCell] = 8;
					}
					else if (promotion == 2)
					{
						board[targetCell] = 7;
					}
					else if (promotion == 3)
					{
						board[targetCell] = 6;
					}
				} else if (originCell/8 == 7)
                {
					if (promotion == 0)
					{
						board[targetCell] = 19;
					}
					else if (promotion == 1)
					{
						board[targetCell] = 18;
					}
					else if (promotion == 2)
					{
						board[targetCell] = 17;
					}
					else if (promotion == 3)
					{
						board[targetCell] = 16;
					}
                }
                else
                {

                }
				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}

		}

		static void UnmakeMoveSoft(int[] board, int originCell, int targetCell, int targetValue, int originValue)
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

		static void UnmakeMoveSoft(int[] board, int originCell, int targetCell, int targetValue, int originValue, int promotion)
		{

				if (originCell/8 == 0)
                {
					board[originCell] = 1;
				} else if (originCell/8 == 7)
                {
					board[originCell] = 11;
                }
				
				board[targetCell] = targetValue;
			

		}
		public static string ReturnAlphabeticalCoordinate(int index)
        {
			int row = 8 - index / 8;
			int col = index - (index / 8) * 8;
			string[] letters = { "a", "b", "c", "d", "e", "f", "g", "h" };
			return letters[col] + row;
        }
		public static Int64 MoveGenerationTest(int depth, int[] board, int turn)
        {
			if (depth == 0)
            {
				return 1;
            }
			turn = turn % 2;
			int[][] movesList;			

			movesList = getAllPossibleMoves(board, turn);
			Int64 numPositions = 0;
			if (movesList != null)
            {
				for (int i = 0; i < movesList.Length; i++)
				{
					if (movesList[i] != null)
                    {
						int tempOriginValue = board[i];
						int tempTemp = 0;
						for (int j = 0; j < movesList[i].Length; j++)
                        {
							Int64 subcount = 0;
							if (movesList[i][j] == -1)
                            {
								
								break;
                            }
							
							//int tempCellValue = board[movesList[i][j]];
							int[] copyBoard = new int[64];
							Array.Copy(board, copyBoard, 64);

							if ((board[i] == 1 && movesList[i][j]/8 == 0) || (board[i] == 11 && movesList[i][j]/8 == 7)){
								MakeMoveSoft(board, i, movesList[i][j], tempTemp);
								tempTemp++;
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								//UnmakeMoveSoft(board, i, movesList[i][j], tempCellValue, 0);
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							} else
                            {
								MakeMoveSoft(board, i, movesList[i][j]);
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								//UnmakeMoveSoft(board, i, movesList[i][j], tempCellValue, tempOriginValue);
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}


                            //subcount += numPositikons;


                            //if (depth == 4) System.Diagnostics.Debug.Write(i + " - " + ReturnAlphabeticalCoordinate(i)+ReturnAlphabeticalCoordinate(movesList[i][j]) + ": " + subcount + "\n");
                        }
						
						
                    } 
				}
			}
			else
			{
				//System.Diagnostics.Debug.Write("checkmate: " + (turn - 1) % 2 + "\n");
			}
			return numPositions;
        }

		public static Int64 MoveGenerationTestDivide(int depth, int[] board, int turn, int originalDepth, System.Windows.Forms.TextBox textbox)
		{
			if (depth == 0)
			{
				return 1;
			}

			int[][] movesList;
			turn = turn % 2;

			movesList = getAllPossibleMoves(board, turn);
			Int64 numPositions = 0;
			if (movesList != null)
			{
				for (int i = 0; i < movesList.Length; i++)
				{
					if (movesList[i] != null)
					{
						int tempTemp = 0;
						for (int j = 0; j < movesList[i].Length; j++)
						{
							Int64 subcount = 0;
							if (movesList[i][j] == -1)
							{
								break;
							}
							int tempTargetValue = board[movesList[i][j]];
							int tempOriginValue = board[i];
							int[] copyBoard = new int[64];
							Array.Copy(board, copyBoard, 64);
							if ((board[i] == 1 && movesList[i][j] / 8 == 0) || (board[i] == 11 && movesList[i][j] / 8 == 7))
							{
								MakeMoveSoft(board, i, movesList[i][j], tempTemp);
								tempTemp++;
								Int64 tempCount = MoveGenerationTestDivide(depth - 1, board, (turn + 1) % 2, originalDepth, textbox);
								numPositions += tempCount;
								//UnmakeMoveSoft(board, i, movesList[i][j], tempTargetValue, tempOriginValue, 0);
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}
							else
							{
								MakeMoveSoft(board, i, movesList[i][j]);
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								//UnmakeMoveSoft(board, i, movesList[i][j], tempTargetValue, tempOriginValue);
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}


							//subcount += numPositikons;


							if (depth == originalDepth) 
							{
								textbox.AppendText(ReturnAlphabeticalCoordinate(i) + ReturnAlphabeticalCoordinate(movesList[i][j]));
								textbox.AppendText(": " + subcount);
								textbox.AppendText(Environment.NewLine);
							}
							}


					}
				}
			}
			else
			{
				//System.Diagnostics.Debug.Write("checkmate: " + (turn - 1) % 2 + "\n");
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