using System.Collections.Generic;
using Game.Core.BoardBase;
using Game.Core.Enums;
using UnityEngine;

namespace Game.Mechanics
{
	public class MatchFinder
	{
		private readonly bool[,] _visitedCells = new bool[Board.Rows, Board.Cols];
		
		/// <summary>
		/// Returns the list of matching cells according to the given match type.
		/// Returns only the given cell if there are no neighbour items with the given match type.
		/// </summary>
		public List<Cell> FindMatches(Cell cell, MatchType matchType)
		{
			var resultCells = new List<Cell>();
			ClearVisitedCells();
			FindMatches(cell, matchType, resultCells);

			return resultCells;
		}
		public List<Cell> FindHintMatches(Cell cell, MatchType matchType)
		{
			var resultCells = new List<Cell>();
			ClearVisitedCells();
			FindHintMatches(cell, matchType, resultCells);

			return resultCells;
		}
		
		public List<Cell> FindComboMatches(Cell cell, MatchType matchType)
		{
			var resultCells = new List<Cell>();
			ClearVisitedCells();
			FindComboMatches(cell, matchType, resultCells);

			return resultCells;
		}
		
		private void FindMatches(Cell cell, MatchType matchType, List<Cell> resultCells)
		{
			if (cell == null || cell.Item == null) return;

			int x = cell.X;
			int y = cell.Y;
			if (_visitedCells[x, y]) return;
			
			if (cell.HasItem()
			    && (cell.Item.GetMatchType() == matchType || (int) cell.Item.GetMatchType() == (int) matchType + 5)  || cell.Item.GetMatchType() == MatchType.MatchWithAll
			    && cell.Item.GetMatchType() != MatchType.None)
			{
				_visitedCells[x, y] = true;
				resultCells.Add(cell);

				if (cell.Item.GetMatchType() != MatchType.MatchWithAll && (int)cell.Item.GetMatchType() <= 4)
				{
					var neighbours = cell.Neighbours;
					if (neighbours.Count == 0) return;
		
					for (var i = 0; i < neighbours.Count; i++)
					{
						FindMatches(neighbours[i], matchType, resultCells);
					}
				}
			}
		}
		
		private void FindHintMatches(Cell cell, MatchType matchType, List<Cell> resultCells)
		{
			if (cell == null || cell.Item == null) return;

			int x = cell.X;
			int y = cell.Y;
			if (_visitedCells[x, y]) return;
			
			if (cell.HasItem()&& (cell.Item.GetMatchType() == matchType && cell.Item.GetMatchType() != MatchType.None))
			{
				_visitedCells[x, y] = true;
				resultCells.Add(cell);

				if (cell.Item.GetMatchType() != MatchType.MatchWithAll && (int)cell.Item.GetMatchType() <= 4)
				{
					var neighbours = cell.Neighbours;
					if (neighbours.Count == 0) return;
		
					for (var i = 0; i < neighbours.Count; i++)
					{
						FindHintMatches(neighbours[i], matchType, resultCells);
					}
				}
			}
		}


		private void FindComboMatches(Cell cell, MatchType matchType, List<Cell> resultCells)
		{
			if (cell == null || cell.Item == null) return;

			int x = cell.X;
			int y = cell.Y;
			if (_visitedCells[x, y]) return;
			
			if (cell.HasItem()
			    && (int)cell.Item.GetMatchType() >= 10)
			{
				_visitedCells[x, y] = true;
				resultCells.Add(cell);

				var neighbours = cell.Neighbours;
				if (neighbours.Count == 0) return;
		
				for (var i = 0; i < neighbours.Count; i++)
				{
					FindComboMatches(neighbours[i], matchType, resultCells);
				}
			}
		}
		

		public void ClearVisitedCells() //Basta temizlemek lazim
		{
			for (var x = 0; x < _visitedCells.GetLength(0); x++)
			{
				for (var y = 0; y < _visitedCells.GetLength(1); y++)
				{
					_visitedCells[x, y] = false;
				}
			}
		}
	}
}
