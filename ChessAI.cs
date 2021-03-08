using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Linq;
using System.Diagnostics;

namespace ChessProject
{
    public static class ChessAI
    {
		const int pawnValue = 100;
		const int knightValue = 300;
		const int bishopValue = 300;
		const int rookvalue = 500;
		const int queenValue = 900;

        public static int[] AITurn(Board Cboard)
        {
            int turn = Cboard.turn;
            int[] board = Cboard.board;
			//int[] minimaxResult = minimax(Cboard, 3, Cboard.turn % 2, -1, -1, int.MinValue,	 int.MaxValue);
			int[] minimaxResult = Minimax(Cboard, 3, Cboard.turn % 2, -1, -1);
			int[] result = { minimaxResult[1], minimaxResult[2], minimaxResult[3] };
			Debug.WriteLine("minmax: "+minimaxResult[0] + " " + minimaxResult[1] + " " + minimaxResult[2]);
			return result;
        }

		public static Move AITurn2(Board Cboard)
		{

			return randomMove2(Cboard);

		}
		static int CountMaterial (Board board, int color)
        {
			int material = 0;

			material += pawnValue * board.pawns[color].Count;
			material += knightValue * board.knights[color].Count;
			material += bishopValue * board.bishops[color].Count;
			material += queenValue * board.queens[color].Count;
			material += rookvalue * board.rooks[color].Count;

			return material;

        }
		static int GetMaterialValue(int piece, int color)
        {
			if (getCellColor(piece) == color)
            {
				if (piece == 1 || piece == 2 || piece == 11 || piece == 12) return pawnValue;
				if (piece == 6 || piece == 16) return bishopValue;
				if (piece == 7 || piece == 17) return knightValue;
				if (piece == 8 || piece == 18 || piece == 28 || piece == 38) return rookvalue;
				if (piece == 9 || piece == 19) return queenValue;
            }

			return 0;
        }
		static int CountMaterial2(Board boardC, int color)
		{
			int material = 0;

			int[] board = boardC.board;
			for (int i = 0; i < board.Length; i++)
            {
				material += GetMaterialValue(board[i], color);
            }
			//material += pawnValue * board.pawns[color].Count;
			//material += knightValue * board.knights[color].Count;
			//material += bishopValue * board.bishops[color].Count;
			//material += queenValue * board.queens[color].Count;
			//material += rookvalue * board.rooks[color].Count;

			return material;

		}

		static int EvaluateMove(Board board, int turn, int from, int to)
        {
			int result = 0;
			int piece = board.board[to];
			int movement = from - to;
			if (piece == 1 || piece == 2){
				if (movement > 0) result += 50;
            }
			else if (piece == 11 || piece == 12)
			{
				if (movement < 0) result += 50;
			}
			
			return result;
        }
		static int Evaluate(Board board, int turn, int from, int to)
        {
			int whiteValue = CountMaterial2(board, 0);
			int blackValue = CountMaterial2(board, 1);

			int moveEval = EvaluateMove(board, turn, from, to);
			if (turn % 2 == 1)
			{
				whiteValue += moveEval;
			} else
			{
				 blackValue += moveEval;
			}
			//Debug.WriteLine("from: " + from + " to: " + to + " whiteeval: " + whiteValue + " blackeval: " + blackValue);
			int evaluation = whiteValue - blackValue;
			int perspective = (turn % 2 == 0) ? 1 : -1;
			return perspective * evaluation;
        }

		static int[] alphabeta(Board boardC, int depth, int maximizingPlayer, int from, int to, int alpha, int beta)
        {
			int[] result = { -1, -1, -1, -1 };
			if (depth == 0)
            {
				result[0] = Evaluate(boardC, maximizingPlayer, from, to);
				return result;
            }
			int[] board = boardC.board;
			int turn = maximizingPlayer % 2;
			int[][] movesList;
			movesList = getAllPossibleMoves(board, turn);
			if (movesList == null)
            {
				result[0] = int.MinValue;
				return result;
            }

			int bestEval = int.MinValue;

			if (movesList != null)
			{
				for (int i = 0; i < movesList.Length; i++)
				{
					if (movesList[i] != null)
					{
						int tempTemp = 0;
						for (int j = 0; j < movesList[i].Length; j++)
						{
							if (movesList[i][j] == -1)
							{
								break;
							}
							int[] copyBoard = new int[64];

							if ((board[i] == 1 && movesList[i][j] / 8 == 0) || (board[i] == 11 && movesList[i][j] / 8 == 7))
							{
								Array.Copy(board, copyBoard, 64);
								MakeMoveSoft(board, i, movesList[i][j], tempTemp);
								int evaluation;
								if (to != -1)
                                {
									 evaluation = -alphabeta(boardC, depth - 1, maximizingPlayer + 1, from, to, -beta, -alpha)[0];
								} else
                                {
									evaluation = -alphabeta(boardC, depth - 1, maximizingPlayer + 1, i, movesList[i][j], -beta, -alpha)[0];
								}
								tempTemp++;
								Array.Copy(copyBoard, board, 64);
								if (evaluation >= beta)
                                {
									result[0] = beta;
									result[1] = i;
									result[2] = movesList[i][j];
									result[3] = tempTemp;
									return result;
								}

								if (evaluation > alpha)
                                {
									alpha = evaluation;
                                }
								
								
							}
							else
							{
								List<KeyValuePair<int, int>> changed = MakeMoveSoftNew(board, i, movesList[i][j]);
								int evaluation;
								if (to != -1)
								{
									evaluation = -alphabeta(boardC, depth - 1, maximizingPlayer + 1, from, to, -beta, -alpha)[0];
								}
								else
								{
									evaluation = -alphabeta(boardC, depth - 1, maximizingPlayer + 1, i, movesList[i][j], -beta, -alpha)[0];
								}

								UnMakeSoftNew(board, changed);
								if (evaluation >= beta)
								{
									result[0] = beta;
									result[1] = i;
									result[2] = movesList[i][j];

									return result;
								}

								if (evaluation > alpha)
								{
									alpha = evaluation;
								}
								
								
							}

						}


					}
				}
			}
			result[0] = alpha;
			return result;
        }


		static int[] Minimax(Board boardC, int depth, int maximizingPlayer, int from, int to)
		{
			int[] result = { -1, -1, -1, -1 };
			if (depth == 0)
			{
				result[0] = Evaluate(boardC, maximizingPlayer, from, to);
				return result;
			}
			int[] board = boardC.board;
			int turn = maximizingPlayer % 2;
			int[][] movesList;
			movesList = ChessEngine.GetAllPossibleMoves(board, turn);
			if (movesList == null)
			{
				result[0] = int.MinValue;
				return result;
			}

			int bestEval = int.MinValue;

			if (movesList != null)
			{
				for (int i = 0; i < movesList.Length; i++)
				{
					if (movesList[i] != null)
					{
						int tempTemp = 0;
						for (int j = 0; j < movesList[i].Length; j++)
						{
							if (movesList[i][j] == -1)
							{
								break;
							}
							int[] copyBoard = new int[64];

							if ((board[i] == 1 && movesList[i][j] / 8 == 0) || (board[i] == 11 && movesList[i][j] / 8 == 7))
							{
								Array.Copy(board, copyBoard, 64);
								MakeMoveSoft(board, i, movesList[i][j], tempTemp);
								int evaluation;
								if (to != -1)
								{
									evaluation = -Minimax(boardC, depth - 1, maximizingPlayer + 1, from, to)[0];
								}
								else
								{
									evaluation = -Minimax(boardC, depth - 1, maximizingPlayer + 1, i, movesList[i][j])[0];
								}
								tempTemp++;
								Array.Copy(copyBoard, board, 64);
								if (evaluation > bestEval)
								{
									bestEval = evaluation;
									result[1] = i;
									result[2] = movesList[i][j];
									result[3] = tempTemp;
									
								}

								


							}
							else
							{
								Array.Copy(board, copyBoard, 64);
								List<KeyValuePair<int, int>> changed = ChessEngine.MakeMove(board, i, movesList[i][j], true, -1);
								int evaluation;
								if (to != -1)
								{
									evaluation = -Minimax(boardC, depth - 1, maximizingPlayer + 1, from, to)[0];
								}
								else
								{
									evaluation = -Minimax(boardC, depth - 1, maximizingPlayer + 1, i, movesList[i][j])[0];
								}

								UnMakeSoftNew(board, changed);
								bool isEqual = Enumerable.SequenceEqual(board, copyBoard);
								if (!isEqual) Debug.WriteLine("not matching after undo");
								if (evaluation > bestEval)
								{
									bestEval = evaluation;
									result[1] = i;
									result[2] = movesList[i][j];
									result[3] = -1;

								}


							}

						}


					}
				}
			}
			result[0] = bestEval;
			return result;
		}
		static int[] randomMove(Board board)
        {
			Random rand = new Random();
			if (board.allPossibleMovesKV.Count == 0)
            {
				return null;
            }
			int moveIndex = rand.Next(board.allPossibleMovesKV.Count);

			int origin = board.allPossibleMovesKV[moveIndex].Key;
			int target = board.allPossibleMovesKV[moveIndex].Value;
			int[] result = { origin, target };

			return result;
        }

		static Move randomMove2(Board board)
		{
			Random rand = new Random();
			if (board.allMoves.Count == 0)
			{
				return null;
			}
			int moveIndex = rand.Next(board.allMoves.Count);

			return board.allMoves[moveIndex];
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


		public static List<Move> getAllPossibleMovesNew(Board boardC, int color)
		{
			List<Move> list = new List<Move>();
			List<Move> tempArray;
			int[] board = boardC.board;

			if (color == 0)
			{
				foreach (List<int> pieces in boardC.whitePieces)
				{
					foreach (int piece in pieces)
					{
						tempArray = GetPossibleMovesNew(board, piece);

						if (tempArray != null)
						{
							foreach (Move move in tempArray)
							{
								//move.toString();
								list.Add(move);
							}

						}
					}
				}
			}
			else
			{
				foreach (List<int> pieces in boardC.blackPieces)
				{
					foreach (int piece in pieces)
					{
						tempArray = GetPossibleMovesNew(board, piece);
						if (tempArray != null)
						{
							foreach (Move move in tempArray)
							{
								list.Add(move);
							}
						}
					}
				}
			}
			//System.Diagnostics.Debug.WriteLine("All moves count: " + list.Count);
			return list;
		}

		public static List<KeyValuePair<int, int>> getAllPossibleMovesLowV2(Board boardC, int color)
		{
			var list = new List<KeyValuePair<int, int>>();
			List<int> tempArray;
			int[] board = boardC.board;

			if (color == 0)
            {
				foreach (List<int> pieces in boardC.whitePieces)
                {
					foreach (int piece in pieces)
                    {
						tempArray = GetPossibleMovesLowV2(board, piece);
						
						if (tempArray != null)
                        {
							foreach (int move in tempArray)
                            {
								list.Add(new KeyValuePair<int, int>(piece, move));
							}
							
                        }
					}
                }
            } else
            {
				foreach (List<int> pieces in boardC.blackPieces	)
				{
					foreach (int piece in pieces)
					{
						tempArray = GetPossibleMovesLowV2(board, piece);
						if (tempArray != null)
						{
							foreach (int move in tempArray)
							{
								list.Add(new KeyValuePair<int, int>(piece, move));
							}
						}
					}
				}
			}

			return list;
		}
		public static List<KeyValuePair<int, int>> GetAllPossibleMovesV2(int[] board, int color)
		{

			var list = new List<KeyValuePair<int, int>>();
			List<int> tempArray = new List<int>();
			
			for (int i = 0; i < 64; i++)
			{
				int gotColor = getCellColor(i, board);
				if (gotColor == color)
				{
					tempArray = GetPossibleMovesLowV2(board, i);
					foreach(int move in tempArray)
                    {
						list.Add( new KeyValuePair<int, int>(i, move) );
                    }


				}
			}
			return list;
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

		static List<int> GetPossibleMovesLowV2(int[] board, int originCell)
		{
			List<int> moveArray = new List<int>();

			for (int i = 0; i < board.Length; i++)
			{

				if (legalMoveNew(board, originCell, i, 0))
				{
					if ((board[originCell] == 1 && (i / 8) == 0) || board[originCell] == 11 && (i / 8) == 7)
					{
						moveArray.Add(i);
						moveArray.Add(i);
						moveArray.Add(i);

					}
					moveArray.Add(i);
				}

			}

			return moveArray;
		}

		static List<Move> GetPossibleMovesNew(int[] board, int originCell)
		{
			List<Move> moveArray = new List<Move>();

			for (int i = 0; i < board.Length; i++)
			{
				int[] result = legalMoveNewNew(board, originCell, i, 0);
				if (result != null)
				{


					if ((board[originCell] == 1 && (i / 8) == 0) || board[originCell] == 11 && (i / 8) == 7)
					{
						moveArray.Add(new Move(originCell, i, result[0] != -1 ? result[0] : -1, (result[1] != -1 ? result[1] : -1), false, result[2], 0));
						moveArray.Add(new Move(originCell, i, result[0] != -1 ? result[0] : -1, (result[1] != -1 ? result[1] : -1), false, result[2], 1));
						moveArray.Add(new Move(originCell, i, result[0] != -1 ? result[0] : -1, (result[1] != -1 ? result[1] : -1), false, result[2], 2));
						moveArray.Add(new Move(originCell, i, result[0] != -1 ? result[0] : -1, (result[1] != -1 ? result[1] : -1), false, result[2], 3));
					} else
                    {
						moveArray.Add(new Move(originCell, i, (result[0] != -1 ? result[0] : -1), (result[1] != -1 ? result[1] : -1), (result[4] == 1 ? true : false), result[2], -1, (result[3] == 1 ? true : false)));
					}
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

		public static void checkPawnUpdate(Board boardC, bool aiTurn)
		{
			int[] board = boardC.board;
			bool result2 = false;
			for (int i = 0; i < 8; i++)
			{
				if (board[i] == 1)
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
					}
					else
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
		}



		public static void checkPawnUpdatePromotion(Board boardC, bool aiTurn, int promotion)
		{
			int[] board = boardC.board;
			bool result2 = false;
			for (int i = 0; i < 8; i++)
			{
				if (board[i] == 1)
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
					}
					else
					{
						board[i] = 9-promotion;
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
						board[i] = 19-promotion;
					}
				}
			}
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
					if (originCell == 5 && board[originCell] == 20 && j<20)
					{
						//System.Diagnostics.Debug.WriteLine("target: " + targetCell + " . " + blackking + " . " + j + " = " + legalMoveNew(tempArr, j, blackking, 1));
					}
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
			//if (originCell == 5 && board[originCell]==20) { System.Diagnostics.Debug.WriteLine("nytx"); }
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
				} else if ((movement == 2 || movement == -2) && (piece==30 || piece == 40))
                {
					return canKingCastle(board, originCell, targetCell);
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
				//if ((piece == 28 && board[targetCell] == 30) || (piece == 38 && board[targetCell] == 40))
    //            {
				//	return canRookCastle(board, originCell, targetCell);
    //            }
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
					//return !kingThreatened(board, originCell, targetCell);
					//////////
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
					///
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
					//return !kingThreatened(board, originCell, targetCell);
					//////////
					if (type != 1)
					{
						return !kingThreatened(board, originCell, targetCell);
					}
					return true;
					///
				}
				else if (((movement == -7 || movement == -9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && (!targetOccupiedByEnemy))
				{
					return CheckEnPassant(board, originCell, targetCell, movement);
                }
			}


			return false;
		}

		public static int[] legalMoveNewNew(int[] board, int originCell, int targetCell, int type) // problem with board state
		{
			int piece = board[originCell];
			bool targetOccupied = board[targetCell] != 0 ? true : false;
			bool targetOccupiedByEnemy = ((getCellColor(originCell, board) != getCellColor(targetCell, board)) && getCellColor(targetCell, board) != -1) ? true : false;
			int movement = originCell - targetCell;
			int[] result = { -1, -1, -1, -1, -1};

			// knight
			if (piece == 7 || piece == 17)
			{
				int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
				if ((movement == -17 || movement == -15 || movement == 17 || movement == 15) && rowdif == 2)
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						result[1] = targetCell;
						return result;
					} else if (!targetOccupied)
                    {
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if ((movement == -6 || movement == -10 || movement == 6 || movement == 10) && rowdif == 1)
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						result[1] = targetCell;
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
			}

			// king
			if (piece == 10 || piece == 20 || piece == 30 || piece == 40)
			{
				int rowdif = Math.Abs((originCell / 8) - (targetCell / 8));
				if (movement == 1 || movement == -1)
				{
					if (rowdif != 0) return null;
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 30 || piece == 40)
                        {
							
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 30 || piece == 40)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				else if ((movement == 7 || movement == 8 || movement == 9 || movement == -7 || movement == -8 || movement == -9) && rowdif == 1)
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 30 || piece == 40)
						{
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;

					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 30 || piece == 40)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;

					}
				}
				else if ((movement == 2 || movement == -2) && (piece == 30 || piece == 40))
				{

					if (!canKingCastle(board, originCell, targetCell)) return null;
					
					result[4] = 1;
					result[2] = CancelEnPassantNew(board);
					return result;
				}
			}

			// bishop
			if (piece == 6 || piece == 16)
			{
				if (checkStraightMovement(board, originCell, targetCell, -7))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, -9))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}

				}
				if (checkStraightMovement(board, originCell, targetCell, 7))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 9))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
			}

			// rook
			if (piece == 8 || piece == 18 || piece == 28 || piece == 38)
			{
				if (checkStraightMovement(board, originCell, targetCell, -1))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, -8))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 1))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 8))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						if (piece == 28 || piece == 38)
						{
							result[3] = 1;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}

			}

			// queen
			if (piece == 9 || piece == 19)
			{
				if (checkStraightMovement(board, originCell, targetCell, -7))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, -9))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							//return !kingThreatened(board, originCell, targetCell);
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[1] = targetCell;

						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 7))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 9))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, -1))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, -8))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 1))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
				if (checkStraightMovement(board, originCell, targetCell, 8))
				{
					if (targetOccupiedByEnemy)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}

						result[1] = targetCell;
						result[2] = CancelEnPassantNew(board);
						result[0] = board[targetCell];
						return result;
					}
					else if (!targetOccupied)
					{
						if (type != 1)
						{
							if (kingThreatened(board, originCell, targetCell)) return null;
						}
						result[2] = CancelEnPassantNew(board);
						return result;
					}
				}
			}

			// white pawn
			if (piece == 1)
			{

				if (movement == 8 && !targetOccupied && type != 1)
				{
					if (kingThreatened(board, originCell, targetCell)) return null;
					result[2] = CancelEnPassantNew(board);
					return result;
				}
				else if (movement == 16 && originCell / 8 == 6 && !targetOccupied && type != 1 && board[originCell - 8] == 0)
				{
					if (kingThreatened(board, originCell, targetCell)) return null;
					result[2] = CancelEnPassantNew(board);
					return result;
				}
				else if (((movement == 7 || movement == 9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && targetOccupiedByEnemy)
				{

					if (type != 1)
					{
						if (kingThreatened(board, originCell, targetCell)) return null;
					}
					
					result[0] = board[targetCell];
					result[1] = targetCell;
					result[2] = CancelEnPassantNew(board);
					return result;
					///
				}
				else if (((movement == 7 || movement == 9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && (!targetOccupiedByEnemy))
				{
					int check = CheckEnPassantNew(board, originCell, targetCell, movement);
					if (check != -1)
					{
						result[2] = CancelEnPassantNew(board);
						result[0] = board[check];

						result[1] = check;
						return result;
					}
				}
			}

			// black pawn
			if (piece == 11)
			{
				if (movement == -8 && !targetOccupied && type != 1)
				{
					if (kingThreatened(board, originCell, targetCell)) return null;
					result[2] = CancelEnPassantNew(board);
					return result;
				}
				else if (movement == -16 && originCell / 8 == 1 && !targetOccupied && type != 1 && board[originCell + 8] == 0)
				{
					if (kingThreatened(board, originCell, targetCell)) return null;
					result[2] = CancelEnPassantNew(board);
					return result;
				}
				else if (((movement == -7 || movement == -9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && targetOccupiedByEnemy)
				{
					//return !kingThreatened(board, originCell, targetCell);
					//////////
					if (type != 1)
					{
						if (kingThreatened(board, originCell, targetCell)) return null;
					}
					result[0] = board[targetCell];
					result[1] = targetCell;
					result[2] = CancelEnPassantNew(board);
					return result;
					///
				}
				else if (((movement == -7 || movement == -9) && (Math.Abs((originCell / 8) - (targetCell / 8)) == 1)) && (!targetOccupiedByEnemy))
				{
					int check = CheckEnPassantNew(board, originCell, targetCell, movement);
					if (check != -1)
					{
						result[2] = CancelEnPassantNew(board);
						result[0] = board[check];

						result[1] = check;
						return result;
					}
				}
			}


			return null;
		}
		static int CheckEnPassantNew(int[] board, int originCell, int targetCell, int movement)
		{
			if (board[targetCell] == 0)
			{
				if (movement == 7 || movement == -9)
				{
					if (board[originCell + 1] == 2 || board[originCell + 1] == 12)
					{
						if (getCellColor(originCell, board) != getCellColor(originCell + 1, board))
						{
							return originCell + 1;
						}
					}
				}
				else if (movement == -7 || movement == 9)
				{
					if (board[originCell - 1] == 2 || board[originCell - 1] == 12)
					{
						//System.Diagnostics.Debug.Write(getCellColor(originCell, board) + " " + getCellColor(board[originCell - 1], board));
						if (getCellColor(originCell, board) != getCellColor(originCell - 1, board))
						{
							return originCell - 1;
						}
					}
				}

			}
			return -1;
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

		public static bool SetupEnpassantPawnNew(int[] board, int originCell, int targetCell, List<KeyValuePair<int,int>> changed)
		{
			if ((originCell / 8 == 1 && targetCell / 8 == 3) || (originCell / 8 == 6 && targetCell / 8 == 4))
			{
				if (board[originCell] == 1) { board[originCell] = 2; changed.Add(new KeyValuePair<int, int>(originCell, 1)); return true; }
				if (board[originCell] == 11) { board[originCell] = 12; changed.Add(new KeyValuePair<int, int>(originCell, 11)); return true; }
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
		public static bool SetupEnpassantPawn(Board boardC, int originCell, int targetCell)
		{
			int[] board = boardC.board;
			if ((originCell / 8 == 1 && targetCell / 8 == 3) || (originCell / 8 == 6 && targetCell / 8 == 4))
			{
				if (board[originCell] == 1) { boardC.addOldSquare(originCell, board[originCell]); board[originCell] = 2; return true; }
				if (board[originCell] == 11) { boardC.addOldSquare(originCell, board[originCell]); board[originCell] = 12; return true; }
			}
			return false;
		}
		public static void CancelEnPassant(Board boardC)
		{
			
			int[] board = boardC.board;
			
				for (int i = 24; i < 32; i++)
				{
					if (board[i] == 12)
					{
						
						boardC.addOldSquare(i, board[i]);
						board[i] = 11;
					}
				}
			
			
				for (int j = 32; j < 40; j++)
				{
					if (board[j] == 2)
                    {
						
						boardC.addOldSquare(j, board[j]);
						board[j] = 1;
					}
				}
			
		}
		public static int CancelEnPassantNew(int[] board)
		{
			for (int i = 24; i < 32; i++)
			{
				if (board[i] == 12)
				{
					//board[i] = 11;
					return i;
				}
			}

			for (int j = 32; j < 40; j++)
			{
				if (board[j] == 2)
				{
					//board[j] = 1;
					return j;
				}
			}
			return -1;
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

		public static void CancelEnPassantReturn(int[] board, List<KeyValuePair<int,int>> changed)
		{
			for (int i = 24; i < 32; i++)
			{
				if (board[i] == 12) {
					changed.Add(new KeyValuePair<int, int>(i, 12));
					board[i] = 11; 
					return; 
				}
			}

			for (int j = 32; j < 40; j++)
			{
				if (board[j] == 2) {
					changed.Add(new KeyValuePair<int, int>(j, 2));
					board[j] = 1;
					return; 
				}
			}
			
		}
		public static int getCellColor(int cellContentInt)
		{

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

		static bool canKingCastle(int[] board, int originCell, int targetCell)
		{
			
			if (targetCell > originCell)
			{
				for (int i = originCell + 1; i < targetCell+1; i++)
				{
					if (board[i] != 0)
					{
						return false;
					}
				}
				if (board[targetCell+1] != 28 && board[targetCell +1] != 38)
                {
					return false;
                }

				for (int i = originCell; i <= targetCell; i++)
				{
					if (kingThreatened(board, originCell, i)) return false;
				}
			}
			else
			{
				for (int i = originCell - 1; i >= targetCell - 1; i--)
				{
					if (board[i] != 0)
					{
						return false;
					}
				}
				for (int i = originCell; i >= targetCell; i--)
				{
					if (kingThreatened(board, originCell, i)) return false;
				}
			}

			return true;
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
			if (targetCell == 6 || targetCell == 62)
			{
				board[targetCell] = board[originCell] - 20;
				board[targetCell - 1] = board[targetCell+1] - 20;
				board[originCell] = 0;
				board[targetCell + 1 ] = 0;
				cells[targetCell - 1].updated = false;
				cells[targetCell + 1].updated = false;
				cells[targetCell].updated = false;
				cells[originCell].updated = false;
			}
			if (targetCell == 2 || targetCell == 58 ) // not worked
			{
				board[targetCell] = board[originCell] - 20;
				board[targetCell + 1] = board[targetCell - 2] - 20;
				board[originCell] = 0;
				board[targetCell - 2 ] = 0;


				cells[targetCell].updated = false;
				cells[targetCell - 2].updated = false;
				cells[targetCell + 1].updated = false;
				cells[originCell].updated = false;
			}
		}

		public static void castleMoveOld(int[] board, int originCell, int targetCell, Form1.Cell[] cells)
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

			if (targetCell == 6 || targetCell == 62)
			{
				board[targetCell] = board[originCell] - 20;
				board[targetCell - 1] = board[targetCell + 1] - 20;
				board[originCell] = 0;
				board[targetCell + 1] = 0;

			}
			if (targetCell == 2 || targetCell == 58) // not worked
			{
				board[targetCell] = board[originCell] - 20;
				board[targetCell + 1] = board[targetCell - 2] - 20;
				board[originCell] = 0;
				board[targetCell - 2] = 0;

			}
		}

		public static void castleMoveSoftNew(int[] board, int originCell, int targetCell, List<KeyValuePair<int,int>> changed)
		{

			if (targetCell == 6 || targetCell == 62)
			{
				changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
				changed.Add(new KeyValuePair<int, int>(targetCell, board[targetCell]));
				changed.Add(new KeyValuePair<int, int>(targetCell - 1, board[targetCell-1]));
				changed.Add(new KeyValuePair<int, int>(targetCell + 1, board[targetCell+1]));

				board[targetCell] = board[originCell] - 20;
				board[targetCell - 1] = board[targetCell + 1] - 20;
				board[originCell] = 0;
				board[targetCell + 1] = 0;

			}
			if (targetCell == 2 || targetCell == 58) 
			{
				changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
				changed.Add(new KeyValuePair<int, int>(targetCell, board[targetCell]));
				changed.Add(new KeyValuePair<int, int>(targetCell - 2, board[targetCell - 2]));
				changed.Add(new KeyValuePair<int, int>(targetCell + 1, board[targetCell + 1]));

				board[targetCell] = board[originCell] - 20;
				board[targetCell + 1] = board[targetCell - 2] - 20;
				board[originCell] = 0;
				board[targetCell - 2] = 0;

			}
		}

		public static void castleMoveSoft(Board boardC, int originCell, int targetCell)
		{
			int[] board = boardC.board;

			if (targetCell == 6 || targetCell == 62)
			{
				boardC.addOldSquare(originCell, board[originCell]);
				boardC.addOldSquare(targetCell - 1, board[targetCell - 1]);
				boardC.addOldSquare(targetCell, board[targetCell]);
				boardC.addOldSquare(targetCell + 1, board[targetCell + 1]);
				UpgradePieceLists(boardC, originCell, targetCell);
				UpgradePieceLists(boardC, targetCell + 1, targetCell - 1);

				board[targetCell] = board[originCell] - 20;
				board[targetCell - 1] = board[targetCell + 1] - 20;
				board[originCell] = 0;
				board[targetCell + 1] = 0;


			}
			if (targetCell == 2 || targetCell == 58) 
			{
				boardC.addOldSquare(originCell, board[originCell]);
				boardC.addOldSquare(targetCell +1 , board[targetCell + 1]);
				boardC.addOldSquare(targetCell, board[targetCell]);
				boardC.addOldSquare(targetCell - 2, board[targetCell - 2]);
				UpgradePieceLists(boardC, originCell, targetCell);
				UpgradePieceLists(boardC, targetCell - 2, targetCell + 1);

				board[targetCell] = board[originCell] - 20;
				board[targetCell + 1] = board[targetCell - 2] - 20;
				board[originCell] = 0;
				board[targetCell - 2] = 0;

			}
		}

		public static void UnmakeCastleMoveNew(Board boardC, int originCell, int targetCell)
		{
			int[] board = boardC.board;
			List<int> targetlist = GetPieceList(boardC, targetCell);

			if (targetCell == 6 || targetCell == 62)
			{
				List<int> rooklist = GetPieceList(boardC, targetCell - 1);
				targetlist.Add(originCell);
				targetlist.Remove(targetCell);
				rooklist.Add(targetCell + 1);
				rooklist.Remove(targetCell - 1);

				board[targetCell + 1] = board[targetCell - 1] + 20;
				board[originCell] = board[targetCell] + 20;
				board[targetCell] = 0;
				board[targetCell - 1] = 0;

			}
			if (targetCell == 2 || targetCell == 58)
			{
				List<int> rooklist = GetPieceList(boardC, targetCell + 1);
				targetlist.Add(originCell);
				targetlist.Remove(targetCell);
				rooklist.Add(targetCell - 2);
				rooklist.Remove(targetCell + 1);

				board[originCell] = board[targetCell] + 20;
				board[targetCell - 2] = board[targetCell + 1] + 20;
				board[targetCell] = 0;
				board[targetCell + 1] = 0;
			}
		}

		public static void castleMoveNew(Board boardC, int originCell, int targetCell)
		{
			int[] board = boardC.board;
			List<int> targetlist = GetPieceList(boardC, originCell);
			

			if (targetCell == 6 || targetCell == 62)
			{
				List<int> rooklist = GetPieceList(boardC, targetCell + 1);
				targetlist.Remove(originCell);
				targetlist.Add(targetCell);
				rooklist.Remove(targetCell + 1);
				rooklist.Add(targetCell - 1);

				board[targetCell] = board[originCell] - 20;
				board[targetCell - 1] = board[targetCell + 1] - 20;
				board[originCell] = 0;
				board[targetCell + 1] = 0;


			}
			if (targetCell == 2 || targetCell == 58)
			{
				List<int> rooklist = GetPieceList(boardC, targetCell - 2);
				targetlist.Remove(originCell);
				targetlist.Add(targetCell);
				rooklist.Remove(targetCell -2);
				rooklist.Add(targetCell +1);

				board[targetCell] = board[originCell] - 20;
				board[targetCell + 1] = board[targetCell - 2] - 20;
				board[originCell] = 0;
				board[targetCell - 2] = 0;
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
				//int cancelPassant = CancelEnPassantReturn(board);
				//if (cancelPassant != -1)
				//{
				//	returnArray.Add(cancelPassant);
				//}
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

		public static void checkFirstMoveByKingOrRookNew(int[] board, int originCell, List<KeyValuePair<int,int>> changed)
		{
			if (board[originCell] == 28 || board[originCell] == 30 || board[originCell] == 38 || board[originCell] == 40)
			{
				changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
				board[originCell] = board[originCell] - 20;
			}
		}

		static void UpgradePieceLists(Board boardC, int originIndex, int newIndex)
        {
			int[] board = boardC.board;
			List<int> targetList = new List<int>();
			if (board[originIndex] == 1 || board[originIndex] == 2)
            {
				targetList = boardC.pawns[0];
			} 
			else if (board[originIndex] == 11 || board[originIndex] == 12)
            {
				targetList = boardC.pawns[1];
			} 
			else if (board[originIndex] == 6)
            {
				targetList = boardC.bishops[0];
            }
			else if (board[originIndex] == 16)
			{
				targetList = boardC.bishops[1];
			}
			else if (board[originIndex] == 7)
			{
				targetList = boardC.knights[0];
			}
			else if (board[originIndex] == 17)
			{
				targetList = boardC.knights[1];
			}
			else if (board[originIndex] == 18 || board[originIndex] == 38)
			{
				targetList = boardC.rooks[1];
			}
			else if (board[originIndex] == 8 || board[originIndex] == 28)
			{
				targetList = boardC.rooks[0];
			}
			else if (board[originIndex] == 9)
			{
				targetList = boardC.queens[0];
			}
			else if (board[originIndex] == 19)
			{
				targetList = boardC.queens[1];
			}
			else if (board[originIndex] == 10 || board[originIndex] == 30)
			{
				targetList = boardC.kings[0];
			}
			else if (board[originIndex] == 20 || board[originIndex] == 40)
			{
				targetList = boardC.kings[1];
			}


			if (targetList != null)
            {
				UpgradePiece(targetList, originIndex, newIndex);
			}
			
		}

		static void UpgradePiece(List<int> piece, int originIndex, int newIndex)
        {
			piece.Remove(originIndex);
			if (newIndex != -1)
            {
				piece.Add(newIndex);
            }


        }
		public static void MakeMoveSoft(Board boardC, int originCell, int targetCell, bool aiTurn)
		{
			boardC.targetCell = targetCell;
			boardC.removeOldSquares();
			boardC.changedSquares = new List<KeyValuePair<int, int>>();
			int[] board = boardC.board;
			int movement = (originCell - targetCell);

			if ((board[originCell] == 40 || board[originCell] == 30) && (movement == 2 || movement == -2))
			{

				CancelEnPassant(boardC);
				castleMoveSoft(boardC, originCell, targetCell);
				// upgrade piecelist done in castlemovesoft
			}
			else
			{
				boardC.addOldSquare(originCell, board[originCell]);
				boardC.addOldSquare(targetCell, board[targetCell]);
				if (board[originCell] == 1 || board[originCell] == 11)
				{
					if (board[targetCell] == 0)
					{

						if (movement == 7 || movement == -9)
						{
							board[originCell + 1] = 0;
							UpgradePieceLists(boardC, originCell + 1, -1);
							boardC.addOldSquare(originCell+1, board[originCell + 1]);
						}
						else if (movement == -7 || movement == 9)
						{
							board[originCell - 1] = 0;
							UpgradePieceLists(boardC, originCell - 1, -1);
							boardC.addOldSquare(originCell - 1, board[originCell - 1]);
						}
					}
				}
				CancelEnPassant(boardC);

				SetupEnpassantPawn(board, originCell, targetCell);
				checkFirstMoveByKingOrRook(board, originCell);



				UpgradePieceLists(boardC, originCell, targetCell);
				UpgradePieceLists(boardC, targetCell, -1);
				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}
			checkPawnUpdate(boardC, aiTurn);
			boardC.turn++;
		}

		public static void MakeMoveSoft(Board boardC, int originCell, int targetCell, bool aiTurn, int promotion)
		{
			boardC.targetCell = targetCell;
			boardC.removeOldSquares();
			boardC.changedSquares = new List<KeyValuePair<int, int>>();
			int[] board = boardC.board;
			int movement = (originCell - targetCell);

			if ((board[originCell] == 40 || board[originCell] == 30) && (movement == 2 || movement == -2))
			{

				CancelEnPassant(boardC);
				castleMoveSoft(boardC, originCell, targetCell);
				// upgrade piecelist done in castlemovesoft
			}
			else
			{
				boardC.addOldSquare(originCell, board[originCell]);
				boardC.addOldSquare(targetCell, board[targetCell]);
				if (board[originCell] == 1 || board[originCell] == 11)
				{
					if (board[targetCell] == 0)
					{

						if (movement == 7 || movement == -9)
						{
							board[originCell + 1] = 0;
							UpgradePieceLists(boardC, originCell + 1, -1);
							boardC.addOldSquare(originCell + 1, board[originCell + 1]);
						}
						else if (movement == -7 || movement == 9)
						{
							board[originCell - 1] = 0;
							UpgradePieceLists(boardC, originCell - 1, -1);
							boardC.addOldSquare(originCell - 1, board[originCell - 1]);
						}
					}
				}
				CancelEnPassant(boardC);

				SetupEnpassantPawn(board, originCell, targetCell);
				checkFirstMoveByKingOrRook(board, originCell);



				UpgradePieceLists(boardC, originCell, targetCell);
				UpgradePieceLists(boardC, targetCell, -1);
				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}
			checkPawnUpdatePromotion(boardC, aiTurn, promotion);
			boardC.turn++;
		}




		static void MakeMoveSoft(Board boardC, int originCell, int targetCell, int promotion)
		{
			boardC.targetCell = targetCell;
			int[] board = boardC.board;
			CancelEnPassant(boardC);
			boardC.addOldSquare(originCell, board[originCell]);
			boardC.addOldSquare(targetCell, board[targetCell]);
			
			if (originCell / 8 == 1)
			{
				if (promotion % 4 == 0)
				{
					board[targetCell] = 9;
				}
				else if (promotion % 4 == 1)
				{
					board[targetCell] = 8;
				}
				else if (promotion % 4 == 2)
				{
					board[targetCell] = 6;
				}
				else if (promotion % 4 == 3)
				{
					board[targetCell] = 7;
				}
			}
			else if (originCell / 8 == 6)
			{
				if (promotion % 4 == 0)
				{
					board[targetCell] = 19;
				}
				else if (promotion % 4 == 1)
				{
					board[targetCell] = 18;
				}
				else if (promotion % 4 == 2)
				{
					board[targetCell] = 16;
				}
				else if (promotion % 4 == 3)
				{
					board[targetCell] = 17;
				}
			}
			else
			{
				board[targetCell] = board[originCell];

			}
			board[originCell] = 0;
			boardC.turn++;


		}

	
		public static void UnmakeMoveNew(Board boardC, Move move)
        {
			if (boardC.moveStack.Any())
            {
				boardC.moveStack.RemoveAt(boardC.moveStack.Count - 1);
            }
			int[] board = boardC.board;
			boardC.turn--;
			
			if (move.firstMove)
            {
				board[move.to] += 20;
            }
			
			List<int> targetlist = GetPieceListFromValue(boardC, board[move.to]);
			if (move.castle)
			{
				UnmakeCastleMoveNew(boardC, move.from, move.to);
			}
			else if (move.promotion > -1)
			{
				targetlist.Remove(move.to);
				board[move.from] = board[move.to] < 10 ? 1 : 11;
				board[move.to] = 0;

				targetlist = GetPieceListFromValue(boardC, board[move.from]);
				targetlist.Add(move.from);

				if (move.capturedValue != -1)
				{
					board[move.capturedIndex] = move.capturedValue;
					targetlist = GetPieceListFromValue(boardC, move.capturedValue);
					targetlist.Add(move.capturedIndex);
				}
				

			}
			else
			{
				if (move.from - move.to == 16 && board[move.to] == 2)
				{
					board[move.to] = 1;
				}
				if (move.from - move.to == -16 && board[move.to] == 12)
				{
					board[move.to] = 11;
				}
				targetlist = GetPieceListFromValue(boardC, board[move.to]);
				board[move.from] = board[move.to];
				board[move.to] = 0;

				targetlist.Remove(move.to);
				targetlist.Add(move.from);


				if (move.capturedValue != -1)
				{
					board[move.capturedIndex] = move.capturedValue;
					targetlist = GetPieceListFromValue(boardC, move.capturedValue);
					targetlist.Add(move.capturedIndex);
				}
			}
			if (move.enpassantRemoved != -1) board[move.enpassantRemoved] = boardC.turn % 2 == 1 ? 2 : 12;
		}
		public static void MakeMoveNew(Board boardC, Move move, bool aiTurn)
		{
			boardC.moveStack.Add(move);
			int[] board = boardC.board;
			List<int> targetlist = GetPieceList(boardC, move.from);
			boardC.turn++;
			if (move.firstMove)
            {
				board[move.from] = board[move.from] - 20;
            }
			if (move.enpassantRemoved != -1)
			{
				board[move.enpassantRemoved] = board[move.enpassantRemoved] == 12 ? 11 : 1;
			}

			if (move.capturedIndex != -1)
			{
				List <int> targetlist2 = GetPieceList(boardC, move.capturedIndex);
				targetlist2.Remove(move.capturedIndex);
				board[move.capturedIndex] = 0;
			}


			if (move.castle)
            {
				castleMoveNew(boardC, move.from, move.to);
            } 
			else if (move.promotion > -1)
            {
				targetlist.Remove(move.from);
				if (aiTurn) { 
					board[move.to] = board[move.from] + 8 - move.promotion;
				} else
                {
					board[move.to] = board[move.from];
					checkPawnUpdate(boardC, false);
                }
				board[move.from] = 0;
				targetlist = GetPieceList(boardC, move.to);
				targetlist.Add(move.to);

			} else
            {
				targetlist.Remove(move.from);
				targetlist.Add(move.to);
				board[move.to] = board[move.from];
				board[move.from] = 0;
				
				if (move.from - move.to == 16 && board[move.to] == 1)
                {
					board[move.to] = 2;
                }
				if (move.from - move.to == -16 && board[move.to] == 11)
				{
					board[move.to] = 12;
				}
			}
		}

		public static void UnmakeMove(Board boardC)
        {
			boardC.turn--;
			int targetCell = boardC.targetCell;
			foreach (KeyValuePair<int,int> oldmove in boardC.changedSquares)
            {
				// Listat
				// jos oldmove.value 0, niin katso nykysen boardin arvo oldmove.key (jos > 0), aseta lista ja poista oldmove.key
				// jos oldmove.value != 0,
				//		jos oldmove.value == 12 || oldmove.value == 2 ja oldmove.key != targetcell
				//				-> ei tee mitään
				// jos oldmove.value > 0
				//		etsi board[targetcell] lista ja poista targetcell
				//		etsi oldmove.value ja aseta oldmove.key
				if (oldmove.Value == 0)
                {
					
					List<int> targetList = GetPieceList(boardC, oldmove.Key);
					if (targetList != null) {
						targetList.Remove(oldmove.Key);
					}
				}

				if (oldmove.Value > 0) //??
                {
					if ( (oldmove.Value == 12 || oldmove.Value ==2 ) && oldmove.Key != targetCell)
                    {

                    } else
                    {
						List<int> targetList = GetPieceListPiece(boardC, boardC.board[targetCell]);
						if (targetList != null)
						{
							targetList.Remove(oldmove.Key);
						}
						targetList = GetPieceListPiece(boardC, oldmove.Value);
						targetList.Add(oldmove.Key);
					}
				}

				boardC.board[oldmove.Key] = oldmove.Value;
				//List<int> targetList = new List<int>();
				



			}

			
			//List<int> targetList = new List<int>();



			//if (targetList != null)
			//{
			//	UpgradePiece(targetList, originIndex, newIndex);
			//}

		}
		public static List<int> GetPieceListPiece(Board boardC, int piece)
		{
			List<int> list = new List<int>();
			int[] board = boardC.board;
		
			if (piece == 0 || piece == -1) return null;

			if (piece == 1 || piece == 2)
			{
				list = boardC.pawns[0];
			}
			else if (piece == 6)
			{
				list = boardC.bishops[0];
			}
			else if (piece == 7)
			{
				list = boardC.knights[0];
			}
			else if (piece == 8 || piece == 28)
			{
				list = boardC.rooks[0];
			}
			else if (piece == 9)
			{
				list = boardC.queens[0];
			}
			else if (piece == 10 || piece == 30)
			{
				list = boardC.kings[0];
			}
			else if (piece == 11 || piece == 12)
			{
				list = boardC.pawns[1];
			}
			else if (piece == 16)
			{
				list = boardC.bishops[1];
			}
			else if (piece == 17)
			{
				list = boardC.knights[1];
			}
			else if (piece == 18 || piece == 38)
			{
				list = boardC.rooks[1];
			}
			else if (piece == 19)
			{
				list = boardC.queens[1];
			}
			else if (piece == 20 || piece == 40)
			{
				list = boardC.kings[1];
			}


			return list;
		}

		public static List<int> GetPieceListFromValue(Board boardC, int piece)
		{
			List<int> list = new List<int>();
			int[] board = boardC.board;
	
			if (piece == 0 || piece == -1) return null;

			if (piece == 1 || piece == 2)
			{
				list = boardC.pawns[0];
			}
			else if (piece == 6)
			{
				list = boardC.bishops[0];
			}
			else if (piece == 7)
			{
				list = boardC.knights[0];
			}
			else if (piece == 8 || piece == 28)
			{
				list = boardC.rooks[0];
			}
			else if (piece == 9)
			{
				list = boardC.queens[0];
			}
			else if (piece == 10 || piece == 30)
			{
				list = boardC.kings[0];
			}
			else if (piece == 11 || piece == 12)
			{
				list = boardC.pawns[1];
			}
			else if (piece == 16)
			{
				list = boardC.bishops[1];
			}
			else if (piece == 17)
			{
				list = boardC.knights[1];
			}
			else if (piece == 18 || piece == 38)
			{
				list = boardC.rooks[1];
			}
			else if (piece == 19)
			{
				list = boardC.queens[1];
			}
			else if (piece == 20 || piece == 40)
			{
				list = boardC.kings[1];
			}


			return list;
		}
		public static List<int> GetPieceList(Board boardC, int index)
        {
			List<int> list = new List<int>();
			int[] board = boardC.board;
			int piece = board[index];
			if (piece == 0 || piece == -1) return null;

			if (piece==1 || piece == 2)
            {
				list = boardC.pawns[0];
            } else if (piece == 6)
            {
				list = boardC.bishops[0];
            } else if ( piece == 7)
            {
				list = boardC.knights[0];
            } else if ( piece == 8 || piece == 28 )
            {
				list = boardC.rooks[0];
            } else if (piece == 9)
            {
				list = boardC.queens[0];
            } else if (piece == 10 || piece == 30)
            {
				list = boardC.kings[0];
            } else if (piece == 11 || piece == 12)
			{
				list = boardC.pawns[1];
			}
			else if (piece == 16)
			{
				list = boardC.bishops[1];
			}
			else if (piece == 17)
			{
				list = boardC.knights[1];
			}
			else if (piece == 18 || piece == 38)
			{
				list = boardC.rooks[1];
			}
			else if (piece == 19)
			{
				list = boardC.queens[1];
			}
			else if (piece == 20 || piece == 40)
			{
				list = boardC.kings[1];
			}


			return list;
        }

		static void UnMakeSoftNew(int[] board, List<KeyValuePair<int,int>> changed)
        {
			foreach (KeyValuePair<int,int> change in changed)
            {
				board[change.Key] = change.Value;
            }
        }
		public static List<KeyValuePair<int, int>> MakeMoveSoftNew(int[] board, int originCell, int targetCell)
		{
			List < KeyValuePair<int, int> > changed = new List<KeyValuePair<int, int>>();
			int movement = (originCell - targetCell);
			if ((board[originCell] == 40 || board[originCell] == 30) && (movement == 2 || movement == -2))
			{
				CancelEnPassantReturn(board, changed);
				castleMoveSoftNew(board, originCell, targetCell, changed);
			}
			else
			{
				if (board[originCell] == 1 || board[originCell] == 11)
				{
					if (board[targetCell] == 0)
					{

						if (movement == 7 || movement == -9)
						{
							changed.Add(new KeyValuePair<int, int>(originCell + 1, board[originCell + 1]));
							board[originCell + 1] = 0;
						}
						else if (movement == -7 || movement == 9)
						{
							changed.Add(new KeyValuePair<int, int>(originCell - 1 , board[originCell - 1]));

							board[originCell - 1] = 0;
						}
					}
				}
				changed.Add(new KeyValuePair<int, int>(targetCell, board[targetCell]));
				changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
				CancelEnPassantReturn(board, changed);
				SetupEnpassantPawnNew(board, originCell, targetCell, changed);
				checkFirstMoveByKingOrRookNew(board, originCell, changed);

				board[targetCell] = board[originCell];
				board[originCell] = 0;
			}
			return changed;
		}
		public static void MakeMoveSoft(int[] board, int originCell, int targetCell)
		{
			int movement = (originCell - targetCell);
			if ((board[originCell] == 40 || board[originCell] == 30) && (movement == 2 || movement == -2))
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

		static void MakeMoveSoft(int[] board, int originCell, int targetCell,  int promotion)
		{

			CancelEnPassant(board);
			if (originCell/8 == 1)
                {
					if (promotion%4 == 0)
                    {
						board[targetCell] = 9;
                    } else if (promotion % 4 == 1)
                    {
						board[targetCell] = 8;
					}
					else if (promotion % 4 == 2)
					{
						board[targetCell] = 7;
					}
					else if (promotion % 4 == 3)
					{
						board[targetCell] = 6;
					}
				} else if (originCell/8 == 6)
                {
					if (promotion % 4 == 0)
					{
						board[targetCell] = 19;
					}
					else if (promotion % 4 == 1)
					{
						board[targetCell] = 18;
					}
					else if (promotion % 4 == 2)
					{
						board[targetCell] = 17;
					}
					else if (promotion % 4 == 3)
					{
						board[targetCell] = 16;
					}
                }
                else
                {
					board[targetCell] = board[originCell];
					
				}
				board[originCell] = 0;


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

			movesList = ChessEngine.GetAllPossibleMoves(board, turn);
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
							
							int[] copyBoard = new int[64];
							Array.Copy(board, copyBoard, 64);

							if ((board[i] == 1 && movesList[i][j]/8 == 0) || (board[i] == 11 && movesList[i][j]/8 == 7)){
								MakeMoveSoft(board, i, movesList[i][j], tempTemp);
								tempTemp++;
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							} else
                            {
								MakeMoveSoft(board, i, movesList[i][j]);
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}
						}
                    } 
				}
			}
			return numPositions;
        }

		public static Int64 MoveGenerationTest(int depth, Board board)
		{
			if (depth == 0)
			{
				return 1;
			}
			int turn = board.turn % 2;
			Stopwatch sw = new Stopwatch();
			sw.Restart();
			List<Move> moves = getAllPossibleMovesNew(board, turn);
			sw.Stop();
			if (depth == 3)
            {
				Debug.WriteLine("New: " +sw.Elapsed);
            }
			Int64 numPositions = 0;
			if (moves.Any())
			{
				//int[] boardCopy = new int[64];
				foreach (Move move in moves)
				{
					//Array.Copy(board.board, boardCopy, 64);
					MakeMoveNew(board, move, true);
					Int64 tempCount = MoveGenerationTest(depth - 1, board);
					numPositions += tempCount;
					UnmakeMoveNew(board, move);
					//bool isEqual = Enumerable.SequenceEqual(board.board, boardCopy);
					//if (!isEqual) 
     //               {
					//	move.toString();
					//}
				}				
			}
			return numPositions;
		}

		public static Int64 MoveGenerationTestBulk(int depth, int[] board, int turn)
		{

			turn = turn % 2;
			List<KeyValuePair<int, int>> movesList;

			Stopwatch sw = new Stopwatch();
			sw.Restart();
			movesList = GetAllPossibleMovesV2(board, turn);
			sw.Stop();
			if (depth == 3)
            {
				Debug.WriteLine("Bulk: " + sw.Elapsed);
            }
			if (movesList == null) return 0;
			int moveCount = movesList.Count();
			
			
			if (depth == 1)
			{
				return moveCount;
				
			}
			Int64 numPositions = 0;
			if (movesList != null)
			{
				foreach (KeyValuePair<int,int> move in movesList)
                {
					int tempTemp = 0;
					Int64 subcount = 0;
					int[] copyBoard = new int[64];
					Array.Copy(board, copyBoard, 64);


					if ((board[move.Key] == 1 && move.Value / 8 == 0) || (board[move.Key] == 11 && move.Value / 8 == 7))
					{
						MakeMoveSoft(board, move.Key, move.Value, tempTemp);
						tempTemp++;
						Int64 tempCount = MoveGenerationTestBulk(depth - 1, board, (turn + 1) % 2);
						
						numPositions += tempCount;
						Array.Copy(copyBoard, board, 64);
						subcount += tempCount;
					}
					else
					{
						MakeMoveSoft(board, move.Key, move.Value);
						Int64 tempCount = MoveGenerationTestBulk(depth - 1, board, (turn + 1) % 2);
						numPositions += tempCount;
						Array.Copy(copyBoard, board, 64);
						subcount += tempCount;
					}

				}
			}
			return numPositions;
		}

		public static Int64 MoveGenerationTestDivide(int depth, int[] board, int turn, int originalDepth, Dictionary<string, Int64> perftResults)
		{
			if (depth == 0)
			{
				return 1;
			}

			int[][] movesList;
			turn = turn % 2;

			Stopwatch sw = new Stopwatch();
			sw.Restart();
			movesList = ChessEngine.GetAllPossibleMoves(board, turn);
			sw.Stop();
			if (depth == 3)
            {
				Debug.WriteLine("Divide: " + sw.Elapsed);
            }
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
					
							int[] copyBoard = new int[64];
							
							if ((board[i] == 1 && movesList[i][j] / 8 == 0) || (board[i] == 11 && movesList[i][j] / 8 == 7))
							{
								Array.Copy(board, copyBoard, 64);
								ChessEngine.MakeMovePawnPromotion(board, i, movesList[i][j], tempTemp);
								tempTemp++;
								Int64 tempCount = MoveGenerationTestDivide(depth - 1, board, (turn + 1) % 2, originalDepth, perftResults);
								numPositions += tempCount;
								
								Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}
							else
							{
								//System.Diagnostics.Debug.WriteLine(i + " " + movesList[i][j] );
								List<KeyValuePair<int, int>> changed = ChessEngine.MakeMove(board, i, movesList[i][j], true, -1);
					
								Int64 tempCount = MoveGenerationTest(depth - 1, board, (turn + 1) % 2);
								numPositions += tempCount;
								//UnmakeMoveSoft(board, i, movesList[i][j], tempTargetValue, tempOriginValue);
								ChessEngine.UnMakeMove(board, changed);
								//Array.Copy(copyBoard, board, 64);
								subcount += tempCount;
							}


							//subcount += numPositikons;


							if (depth == originalDepth) 
							{
                                string str = ReturnAlphabeticalCoordinate(i) + ReturnAlphabeticalCoordinate(movesList[i][j]);
                                if (perftResults.ContainsKey(str))
                                {
									



									if (!perftResults.ContainsKey(str + "r"))
                                    {
                                        perftResults.Add(str + "r", subcount);
                                    }
                                    else if (!perftResults.ContainsKey(str + "n"))
                                    {
                                        perftResults.Add(str + "n", subcount);
                                    }
                                    else if (!perftResults.ContainsKey(str + "b"))
                                    {
                                        perftResults.Add(str + "b", subcount);
										perftResults.Add(str + "q", perftResults[str]);
										perftResults.Remove(str);
                                    }

                                }
                                else
                                {
                                    perftResults.Add(str, subcount);
                                }




                                //textbox.AppendText(ReturnAlphabeticalCoordinate(i) + ReturnAlphabeticalCoordinate(movesList[i][j]));
                                //textbox.AppendText(": " + subcount);
                                //textbox.AppendText(Environment.NewLine);
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
