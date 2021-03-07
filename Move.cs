using System;
using System.Collections.Generic;
using System.Text;

namespace ChessProject
{
    public class Move
    {
        public int from, to;
        public int capturedIndex, capturedValue, enpassantRemoved, promotion;
        public bool castle, firstMove;

        public Move(int from, int to, int capturedValue = -1, int capturedIndex = -1,
            bool castle = false, int enpassantRemoved = -1, int promotion = -1,
            bool firstMove = false)
        {
            this.from = from;
            this.to = to;
            this.capturedValue = capturedValue;
            this.capturedIndex = capturedIndex;
            this.castle = castle;
            this.enpassantRemoved = enpassantRemoved;
            this.promotion = promotion;
            this.firstMove = firstMove;
        }

        public void toString()
        {
            System.Diagnostics.Debug.WriteLine("From: " + from + " to: " + to + " captureValue: " + capturedValue + " captureIndex: " + capturedIndex + " castle: " + castle + " promotion: " + promotion
                + " firstmoveMadeByPiece: " + firstMove + " enpass removed: " + enpassantRemoved);
        }
    }
}
