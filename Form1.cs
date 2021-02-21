﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessProject
{
    public partial class Form1 : Form
    {
		private TableLayoutPanel cells;
		class Cell : PictureBox
		{
			public static readonly System.Drawing.Size CellSize = new System.Drawing.Size(80, 80);
			public readonly int row, col, index;
		
			

			public Cell(int row, int col) : base()
			{
				this.row = row; this.col = col;
				this.Size = CellSize;
				this.BackColor = (col % 2 == row % 2) ? Color.FromArgb(118, 150, 85) : Color.FromArgb(238, 238, 212);
				this.index = row * 8 + col;
			}
			public override string ToString() { return "Cell(" + row + "," + col + ")"; }
			public Cell returnThis()
            {
				return this;
            }
		}



		public Form1()
        {
    
			Board board = new Board();

			//FormBorderStyle = FormBorderStyle.None;
			//WindowState = FormWindowState.Maximized;
			//this.Size = new Size(1300, 1300);
			InitializeComponent();
			Cell[] test_cells = new Cell[64];
			cells = GetBoard(board,test_cells);
			this.Controls.Add(cells);
			
		}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

		private TableLayoutPanel GetBoard(Board board, Cell[] test_cells)
		{
			TableLayoutPanel b = new TableLayoutPanel();
			b.ColumnCount = 8;
			b.RowCount = 8;
			turn_label.Text = board.turn == 0 ? "White" : "Black";

			for (int i = 0; i < b.ColumnCount; i++)
			{
				b.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, Cell.CellSize.Width));
			}

			for (int i = 0; i < b.RowCount; i++)
			{
				b.RowStyles.Add(new RowStyle(SizeType.Absolute, Cell.CellSize.Height));
			}
			int ii = 0;
			
			for (int row = 0; row < b.RowCount; row++)
			{
				for (int col = 0; col < b.ColumnCount; col++)
				{
					Cell cell = new Cell(row, col);
					int temp_cell = board.board[row * 8 + col];
					
					//System.Diagnostics.Debug.Write(test_cells[temp_cell].ToString());

					//cell.Click += new EventHandler(cell_Click(cell, selected_cell));
					cell.Click += delegate (object s, EventArgs e) {
						cell_Click(s, board, b, test_cells);
					};
					
					
					b.Controls.Add(cell, col, row);
					test_cells[ii] = cell;
					ii++;

					cell.SizeMode = PictureBoxSizeMode.StretchImage;
					if (temp_cell != 0)
					{
						if (temp_cell > 0 && temp_cell < 6)
						{
							cell.ImageLocation = @"images\1.png";
							

						}
						else if (temp_cell > 10 && temp_cell < 16)
						{
							cell.ImageLocation = @"images\11.png";
						}
						else
						{
							cell.ImageLocation = $@"images\{temp_cell}.png";

						}
						//cell.imagelocation = @"images\1.png";
						//cell.sizemode = pictureboxsizemode.stretchimage;
					}


				}

			}
			b.Padding = new Padding(0);
			b.Size = new System.Drawing.Size(b.ColumnCount * Cell.CellSize.Width, b.RowCount * Cell.CellSize.Height);
			printBoard(board);
			return b;
		}


		private void cell_Click(object sender, Board board, TableLayoutPanel b, Cell[] test_cells)
		{

			Cell targetCell = (Cell)sender;
			//int cell_contentInt = board.board[targetCell.index];
			int cell_contentColor = getCellColor(targetCell, board);
			

			if (board.selected_cell == -1)
			{
				if (board.turn == cell_contentColor )
				{
					board.selected_cell = targetCell.index;
					setActiveCellColor(targetCell);
					// draw possible moves?
				}
            }
            else
            {
				Cell originCell = test_cells[board.selected_cell];

				if (originCell == targetCell)
                {
					board.selected_cell = -1;
					resetCellColor(originCell);
                } else if (legalMove(board, originCell, targetCell))
				{
					updateCellDrawing(originCell, targetCell, test_cells);

					board.board[targetCell.index] = board.board[board.selected_cell];
					board.board[board.selected_cell] = 0;
					board.selected_cell = -1;

					if (board.turn == 0) { board.turn = 1; turn_label.Text = "Black"; }
					else
					{ board.turn = 0; turn_label.Text = "White"; }
					printBoard(board);
	
				}



            }

		}

		void updateCellDrawing(Cell originCell, Cell destinationCell, Cell[] test_cells)
		{
			resetCellColor(destinationCell);
			resetCellColor(originCell);
			destinationCell.ImageLocation = originCell.ImageLocation;
			originCell.ImageLocation = null;
		}

		void resetCellColor(Cell cell)
        {
			cell.BackColor = (cell.col % 2 == cell.row % 2) ? Color.FromArgb(118, 150, 85) : Color.FromArgb(238, 238, 212);
		}

		void setActiveCellColor(Cell cell)
        {
			cell.BackColor = (cell.col % 2 == cell.row % 2) ? Color.FromArgb(88, 133, 64) : Color.FromArgb(213, 213, 149);
		}

		bool legalMove(Board board, Cell originCell, Cell targetCell)
        {
			return true;
        }

		int getCellColor(Cell cell, Board board)
        {
			int cellContentInt = board.board[cell.index];

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

		void printBoard(Board board)
        {
			boardIntCont.Text = "";
			int temp = 0;
			for (int i = 0; i < board.board.Length; i++)
            {
				if (board.board[i] > 9)
                {
					boardIntCont.AppendText(board.board[i] + " ");
				} else
                {
					boardIntCont.AppendText(board.board[i] +  "   ");
				}
				
				temp++;
				if (temp == 8 && i < 63)
                {
					temp = 0;
					boardIntCont.AppendText(Environment.NewLine);
                }
            }
        }


 
    }
	
}
