using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
    public static class ChessEngine
    {
        static bool IsKingThreatened(int[] board, int from, int to, int color)
        {
            // make potential board from potential move
            int[] futureBoard = new int[64];
            Array.Copy(board, futureBoard, 64);

            if (to != from) { 
                futureBoard[to] = futureBoard[from];
                futureBoard[from] = 0;
            }
            int capturingColor = (color == 0 ? 1 : 0);
            int threatenedKing = -1;
            
            // find playing color's king
            if (color == 1)
            {
                for (int i = 0; i < futureBoard.Length; i++)
                {
                    if (futureBoard[i] == 20 || futureBoard[i] == 40)
                    {
                        threatenedKing = i;
                        break;
                    }
                }
            } else
            {
                for (int i = futureBoard.Length-1; i >= 0; i--) // assume higher index, start iteration from board end
                {
                    if (futureBoard[i] == 10 || futureBoard[i] == 30)
                    {
                        threatenedKing = i;
                        break;
                    }
                }
            }
            // check if opposite color pieces can threat king on potential board
            for (int i = 0; i < futureBoard.Length; i++)
            {
                if (GetCellColor(futureBoard[i]) == capturingColor)
                {
                    if (IsCapturingMove(futureBoard, i, threatenedKing)) return true;
                }
            }

            return false;
        }

        static bool IsCapturingMove(int[] board, int pieceIndex, int targetIndex)
        {
            int piece = board[pieceIndex];
            int movement = targetIndex - pieceIndex;
            int rowdif = targetIndex / 8 - pieceIndex / 8;

            if (piece == 10 || piece == 20 || piece == 30 || piece == 40) // king
            {
                if (rowdif == 1)
                {
                    if (movement == 7 || movement == 8 || movement == 9 || movement == -7 || movement == -8 || movement == -9) return true;
                } else if (rowdif == 0)
                {
                    if (movement == 1 || movement == -1) return true;
                }
            } else if (piece == 1 || piece == 2) // white pawn
            {
                if (rowdif == -1)
                {
                    if (movement == -7 || movement == -9) return true;
                }
            } else if (piece == 11 || piece == 12) // black pawn
            {
                if (rowdif == 1)
                {
                    if (movement == 7 || movement == 9) return true;
                }
            } else if (piece == 6 || piece == 16) // rook
            {
                if (movement % 7 == 0 || movement % 9 == 0)
                {
                    return CheckStraightMovement(board, pieceIndex, targetIndex, movement, rowdif);
                }
            } else if ( piece == 7 || piece == 17) // knight
            {
                if ((movement == -17 || movement == -15 || movement == 17 || movement == 15) && rowdif == 2)
                {
                    return true;
                }
                else if ((movement == -6 || movement == -10 || movement == 6 || movement == 10) && rowdif == 1)
                {
                    return true;
                }
            } else if ( piece == 9 || piece == 19) // queen
            {
                if (movement % 7 == 0 || movement % 9 == 0 || movement % 8 == 0 || (movement > -8 && movement <8))
                {
                    return CheckStraightMovement(board, pieceIndex, targetIndex, movement, rowdif);
                }
            } if ( piece == 8 || piece == 18 || piece == 28 || piece == 38) // rook
            {
                if (rowdif != 0)
                {
                    if (movement % 8 == 0)
                    {
                        return CheckStraightMovement (board, pieceIndex, targetIndex, movement, rowdif);
                    }
                }
                else
                {
                    return CheckStraightMovement (board, pieceIndex, targetIndex, movement, rowdif);
                }
            }

            return false;
        }

        static bool CheckStraightMovement(int[] board, int origin, int target, int movement, int rowdif)
        {
            
            int row_diff = Math.Abs(rowdif);
            if (row_diff > 0)
            {
                if (movement % 7 == 0)
                {
                    movement = movement > 0 ? 7 : -7;
                }
                else if (movement % 9 == 0)
                {
                    movement = movement > 0 ? 9 : -9;
                }
            } else
            {
                if (movement > 0)
                {
                    movement = 1;
                } else
                {
                    movement = -1;
                }
            }

            if (rowdif!=0 && (movement == 1 || movement == -1)) return false;
            if ((rowdif==0) && (movement > 1 || movement < -1)) return false;
            



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
                            return false;
                        }

                        if (board[temp_position] != 0 && temp_position != target)
                        {
                            return false;
                        }
                        if (temp_position == target)
                        {
                            if (row_diff == temp_value || row_diff == 0)
                            {
                                if (getCellColor(target, board) == -1)
                                {
                                    return true;
                                }
                                if (getCellColor(origin, board) != getCellColor(target, board))
                                {
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
                            return false;
                        }

                        if (board[temp_position] != 0 && temp_position != target)
                        {
                            return false;
                        }

                        if (temp_position == target)
                        {
                            if (row_diff == temp_value || row_diff == 0)
                            {
                                if (getCellColor(target, board) == -1)
                                {
                                    return true;
                                }
                                if (getCellColor(origin, board) != getCellColor(target, board))
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }
        static int[] GetMovesPawn(int[] board, int index, int color)
        {
            int[] resultArray = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            int direction = color == 0 ? -1 : 1;
            int running = 0;
            if (board[index + direction * 8] == 0)
            {
                if (!IsKingThreatened(board, index, index + direction * 8, color))
                {
                    resultArray[running] = index + direction * 8;
                    running++;
                    if ((index + direction * 8) / 8 == 0 || (index + direction * 8) / 8 == 7)
                    {
                        resultArray[running] = index + direction * 8;
                        running++;
                        resultArray[running] = index + direction * 8;
                        running++;
                        resultArray[running] = index + direction * 8;
                        running++;
                    }
                }
            }
            if ( ( (color == 0 && index / 8 == 6) || (color == 1 && index / 8 == 1) ) && running == 1)
            {
                if (board[index + direction * 16] == 0)
                {
                    if (!IsKingThreatened(board, index, index + direction * 16, color))
                    {
                        resultArray[running] = index + direction * 16;
                        running++;
                    }
                }
            }
            if (GetCellColor(board[index + direction * 7]) == (color == 0 ? 1 : 0))
            {
                if (index / 8 - (index + direction * 7) / 8 == -direction)
                {
                    if (!IsKingThreatened(board, index, index + direction * 7, color))
                    {
                        resultArray[running] = index + direction * 7;
                        running++;
                        if ((index + direction * 7) / 8 == 0 || (index + direction * 7) / 8 == 7)
                        {
                            resultArray[running] = index + direction * 7;
                            running++;
                            resultArray[running] = index + direction * 7;
                            running++;
                            resultArray[running] = index + direction * 7;
                            running++;
                        }
                    }
                }
                
            }
            if (GetCellColor(board[index + direction * 9]) == (color == 0 ? 1 : 0))
            {
                if (index / 8 - (index + direction * 9) / 8 == -direction)
                {
                    if (!IsKingThreatened(board, index, index + direction * 9, color))
                    {
                        resultArray[running] = index + direction * 9;
                        running++;
                        if ((index + direction * 9) / 8 == 0 || (index + direction * 9) / 8 == 7)
                        {
                            resultArray[running] = index + direction * 9;
                            running++;
                            resultArray[running] = index + direction * 9;
                            running++;
                            resultArray[running] = index + direction * 9;
                            running++;
                        }
                    }
                }
            }
            if (board[index+1] == (color == 0 ? 12 : 2) && board[index + (color==0?-7:9)] == 0)
            {
                if (index / 8 - (index + (color == 0 ? -7 : 9)) / 8 == -direction)
                {
                    if (!IsKingThreatened(board, index, index + (color == 0 ? -7 : 9), color))
                    {
                        resultArray[running] = index + (color == 0 ? -7 : 9);
                        running++;
                    }
                }

            }

            if (board[index - 1] == (color == 0 ? 12 : 2) && board[index + (color == 0 ? -9 : 7)] == 0)
            {
                if (index / 8 - (index + (color == 0 ? -9 : 7)) / 8 == -direction)
                {
                    if (!IsKingThreatened(board, index, index + (color == 0 ? -9 : 7), color))
                    {
                        resultArray[running] = index + (color == 0 ? -9 : 7);
                    }
                }
                    
            }

            return resultArray;


        }

        static int[] GetMovesBishop(int[] board, int index, int color)
        {
            int[] movement = { -9, -7, 7, 9 };
            int[] moves = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            return GetStraightMovement(board, index, color, movement, moves);
            //return null;
        }
        static int[] GetMovesKnight(int[] board, int index, int color)
        {
            int[] moves = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            //if ((movement == -17 || movement == -15 || movement == 17 || movement == 15) && rowdif == 2) { }
            //if ((movement == -6 || movement == -10 || movement == 6 || movement == 10) && rowdif == 1)
            int[] movement = { -17, -15, 17, 15, -6, -10, 6, 10 };
            int running = 0;
            for (int i = 0; i < 8; i++)
            {
                int move = movement[i];
                int targetIndex = index + move;
                int rowdif = index / 8 - targetIndex / 8;
                if (targetIndex >= 0 && targetIndex < 64)
                {
                    if (i < 4)
                    {
                        if (rowdif == 2 || rowdif == -2)
                        {
                            if (GetCellColor(board[targetIndex]) != color)
                            {
                                if (!IsKingThreatened(board, index, targetIndex, color))
                                {
                                    moves[running] = targetIndex;
                                    running++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rowdif == 1 || rowdif == -1)
                        {
                            if (GetCellColor(board[targetIndex]) != color)
                            {
                                if (!IsKingThreatened(board, index, targetIndex, color))
                                {
                                    moves[running] = targetIndex;
                                    running++;
                                }
                            }
                        }
                    }
                }
                
            }
                return moves;
        }
        static int[] GetMovesQueen(int[] board, int index, int color)
        {
            int[] movement = { -9, -7, 7, 9, 1, -1, 8, -8 };
            int[] moves = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            return GetStraightMovement(board, index, color, movement, moves);
            //return null;
        }
        static int[] GetMovesRook(int[] board, int index, int color)
        {
            int[] movement = { 1, -1, 8, -8 };
            int[] moves = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            return GetStraightMovement(board, index, color, movement, moves);
            //return null;
        }

        static int[] GetMovesKing(int[] board, int index, int color)
        {
            int[] movement = { 1, -1, 8, -8, 7, -7, 9, -9 };
            int[] moves = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            int running = 0;
            for (int i = 0; i < 8; i++)
            {
                int move = movement[i];
                int targetIndex = index + move;
                int rowdif = index / 8 - targetIndex / 8;
                

                if (targetIndex >= 0 && targetIndex < 64)
                {
                    if (i < 2)
                    {
                        if (rowdif == 0)
                        {
                            if (GetCellColor(board[targetIndex]) != color)
                            {
                                if (!IsKingThreatened(board, index, targetIndex, color))
                                {
                                    moves[running] = targetIndex;
                                    running++;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rowdif == 1 || rowdif == -1)
                        {
                            if (GetCellColor(board[targetIndex]) != color)
                            {
                                if (!IsKingThreatened(board, index, targetIndex, color))
                                {
                                    moves[running] = targetIndex;
                                    running++;
                                }
                            }
                        }
                    }
                }

            }
            return moves;
        }
        static int[] GetStraightMovement(int[] board, int index, int color, int[] movement, int[] moves)
        {
            int movesIndex = 0;
            for (int i = 0; i < movement.Length; i++)
            {
                int moveConst = movement[i];
                int move = movement[i];
                int initialRowDif = index / 8 - (index + move) / 8;
                if ( ( (move == 1 || move == -1) && initialRowDif == 0) || (( move > 1 || move < -1 ) && initialRowDif != 0))
                {
                    
                    while (true)
                    {
                        
                        if (index + move < 0 || index + move > 63) break;
                        bool okSquare = GetCellColor(board[index + move]) == color ? false : true;
                        bool emptySquare = GetCellColor(board[index + move]) == -1;
                        if (okSquare)
                        {
                            if (IsKingThreatened(board, index, index + move, color)) break;
                            moves[movesIndex] = index + move;
                            movesIndex++;
                            if ((index + move)/8 - (index + move + moveConst)/8 != initialRowDif) break;
                            if (!emptySquare) break;
                            move += moveConst;
                            

                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return moves;
        }
        public static int[] GetPossibleMovesCaller(int[] board, int index, int color)
        {
            int piece = board[index];
            int[] result = {    -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1, -1, -1,
                                -1, -1, -1, -1, -1, -1, -1, -1 };
            if (piece == 1 || piece == 2 || piece == 11 || piece == 12)
            {
                result = GetMovesPawn(board, index, color);
            }  else if ( piece == 6 || piece == 16 )
            {
                result = GetMovesBishop(board, index, color);
            } else if ( piece == 9 || piece == 19)
            {
                result = GetMovesQueen(board, index, color);
            } else if ( piece == 8 || piece == 18 || piece == 28 || piece == 38)
            {
                result = GetMovesRook(board, index, color);
            } else if ( piece == 7 || piece == 17)
            {
                result = GetMovesKnight(board, index, color);
            } else if ( piece == 10 || piece == 20 || piece == 30 || piece == 40)
            {
                result = GetMovesKing(board, index, color);
            }

            System.Diagnostics.Debug.WriteLine("--------");
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] != -1)
                {
                    System.Diagnostics.Debug.WriteLine("Possible move: " + result[i]);
                }
            }
            return null;
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
                    //tempArray = getPossibleMovesLow(board, i);
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
        public static int GetCellColor(int cellContentInt)
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

        static void UnMakeSoftNew(int[] board, List<KeyValuePair<int, int>> changed)
        {
            foreach (KeyValuePair<int, int> change in changed)
            {
                board[change.Key] = change.Value;
            }
        }

        public static List<KeyValuePair<int, int>> MakeMoveSoftNew(int[] board, int originCell, int targetCell)
        {
            List<KeyValuePair<int, int>> changed = new List<KeyValuePair<int, int>>();
            int movement = (originCell - targetCell);
            if ((board[originCell] == 40 || board[originCell] == 30) && (movement == 2 || movement == -2))
            {
                CancelEnPassantReturn(board, changed);
                CastleMoveSoftNew(board, originCell, targetCell, changed);
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
                            changed.Add(new KeyValuePair<int, int>(originCell - 1, board[originCell - 1]));

                            board[originCell - 1] = 0;
                        }
                    }
                }
                changed.Add(new KeyValuePair<int, int>(targetCell, board[targetCell]));
                changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
                CancelEnPassantReturn(board, changed);
                SetupEnpassantPawnNew(board, originCell, targetCell, changed);
                CheckFirstMoveByKingOrRookNew(board, originCell, changed);

                board[targetCell] = board[originCell];
                board[originCell] = 0;
            }
            return changed;
        }

        public static void CancelEnPassantReturn(int[] board, List<KeyValuePair<int, int>> changed)
        {
            for (int i = 24; i < 32; i++)
            {
                if (board[i] == 12)
                {
                    changed.Add(new KeyValuePair<int, int>(i, 12));
                    board[i] = 11;
                    return;
                }
            }

            for (int j = 32; j < 40; j++)
            {
                if (board[j] == 2)
                {
                    changed.Add(new KeyValuePair<int, int>(j, 2));
                    board[j] = 1;
                    return;
                }
            }

        }

        public static void CastleMoveSoftNew(int[] board, int originCell, int targetCell, List<KeyValuePair<int, int>> changed)
        {

            if (targetCell == 6 || targetCell == 62)
            {
                changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
                changed.Add(new KeyValuePair<int, int>(targetCell, board[targetCell]));
                changed.Add(new KeyValuePair<int, int>(targetCell - 1, board[targetCell - 1]));
                changed.Add(new KeyValuePair<int, int>(targetCell + 1, board[targetCell + 1]));

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

        public static bool SetupEnpassantPawnNew(int[] board, int originCell, int targetCell, List<KeyValuePair<int, int>> changed)
        {
            if ((originCell / 8 == 1 && targetCell / 8 == 3) || (originCell / 8 == 6 && targetCell / 8 == 4))
            {
                if (board[originCell] == 1) { board[originCell] = 2; changed.Add(new KeyValuePair<int, int>(originCell, 1)); return true; }
                if (board[originCell] == 11) { board[originCell] = 12; changed.Add(new KeyValuePair<int, int>(originCell, 11)); return true; }
            }
            return false;
        }

        public static void CheckFirstMoveByKingOrRookNew(int[] board, int originCell, List<KeyValuePair<int, int>> changed)
        {
            if (board[originCell] == 28 || board[originCell] == 30 || board[originCell] == 38 || board[originCell] == 40)
            {
                changed.Add(new KeyValuePair<int, int>(originCell, board[originCell]));
                board[originCell] = board[originCell] - 20;
            }
        }
    }
}
