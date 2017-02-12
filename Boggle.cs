using System;
using System.Collections.Generic;

namespace GoogleInterviewQuestions
{
	public static class Boggle
	{
		//Input is a boggle board of size mxn, and a string to search for.
		public static IEnumerable<Tile> BoggleSolver(char[,] board, string search)
		{
			int maxRow = board.GetLength(0);
			int maxCol = board.GetLength(1);

			Stack<Tile> path = new Stack<Tile>();

			//Search across the whole board, if first letter found, perform dfs
			for (int i = 0; i < maxRow; i++) 
			{
				for (int j = 0; j < maxCol; j++)
				{
					char tileValue = board[i, j];
					bool[,] marked = new bool[maxRow, maxCol];
					int d = 0;

					path.Push(new Tile(tileValue, i, j)); //This is first value to check in dfs

					if (dfs(board, search, path, d, marked))
					{
						//THe DFS found it, return path
						return path;
					}

					path.Pop();
				}
			}

			return null;
		}


		private static bool dfs(char[,] board, string search, Stack<Tile> path, int d, bool[,] marked)
		{
			Tile topTile = path.Peek();
			char curr = search[d];

			if (!topTile.Value.Equals(curr))
				return false;

			marked[topTile.Row, topTile.Column] = true;

			if (d == search.Length - 1)
				return true;

			foreach (Tile adj in GetAdjTile(topTile, board))
			{
				if (!marked[adj.Row, adj.Column])
				{
					path.Push(adj);
					//Keep on looking through this path.
					if (dfs(board, search, path, d + 1, marked))
					{
						return true;
					}

					path.Pop();
				}
			}

			return false;

		}

		private static IEnumerable<Tile> GetAdjTile(Tile tile, char[,] board)
		{
			//Move this elsewhere so it doesn't keep getting created
			List<Tile> potentialTiles = new List<Tile>()
			{
				new Tile(-1, -1), new Tile(-1, 0), new Tile(-1, 1),
				new Tile(0, -1), new Tile(0, 0), new Tile(0, 1), //Middle Tile is redundant here, not needed
				new Tile(1, -1), new Tile(1, 0), new Tile(1, 1)
			};				
							
			List<Tile> adjTiles = new List<Tile>();

			int maxRow = board.GetLength(0);
			int maxCol = board.GetLength(1);

			foreach(Tile pt in potentialTiles)
			{
				if (ValidateTile(tile.Row + pt.Row, tile.Column + pt.Column, maxCol, maxRow))
				{
					int row = tile.Row + pt.Row;
					int column = tile.Column + pt.Column;
					adjTiles.Add(new Tile(board[row, column], row, column));
				}
			}

			return adjTiles;
		}

		private static Boolean ValidateTile(int row, int column, int maxCol, int maxRow)
		{
			//Check if its in the bounds of the board game
			return !(row < 0 || row >= maxRow || column < 0 || column >= maxCol);
		}
	}

	public class Tile
	{
		public int Row;
		public int Column;
		public char Value;

		public Tile(int row, int col)
		{
			Row = row;
			Column = col;
		}

		public Tile(char value, int row, int col) 
			: this(row, col)
		{
			Value = value;
		}

	}
}
