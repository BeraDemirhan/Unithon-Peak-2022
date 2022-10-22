using System;
using System.Linq;
using Game.Core.Enums;
using Game.Mechanics;
using UnityEngine;
using System.Collections.Generic;
using Game.Items;
using Game.Managers;

namespace Game.Core.BoardBase
{
	public class Board : MonoBehaviour
	{
		[SerializeField] private Cell cellPrefab;
		
		public const int Rows = 9;
		public const int Cols = 9;
		public Transform cellsParent;
		public Transform itemsParent;
		public Transform particlesParent;
		public readonly Cell[,] Cells = new Cell[Cols, Rows];
		
		private readonly MatchFinder MatchFinder = new MatchFinder();
		private List<Cell> _neighbourCells = new List<Cell>();
		private const int MinimumMatchCount = 2;
	
		public void Prepare()
		{
			CreateCells();
			PrepareCells();
		}
		
		private void CreateCells()
		{
			for (var y = 0; y < Rows; y++)
			{
				for (var x = 0; x < Cols; x++)
				{
					var cell = Instantiate(cellPrefab, Vector3.zero, Quaternion.identity, cellsParent);
					Cells[x, y] = cell;
				}
			}
		}

		private void PrepareCells()
		{
			for (var y = 0; y < Rows; y++)
			{
				for (var x = 0; x < Cols; x++)
				{
					Cells[x, y].Prepare(x, y, this);
				}
			}
		}

		public void CellTapped(Cell cell)
		{
			if (cell == null) return;

			if (!cell.HasItem()) return;
			if(!cell.Item.IsClickable) return;

			_neighbourCells.Clear();
			_neighbourCells = new List<Cell>();

			ExplosionConjunction(cell);
		}

		
		private void ExplosionConjunction(Cell cell)
		{
			if(!cell.HasItem()) return;
			
			if((int)cell.Item.GetMatchType() >= 10 && cell.Item.GetComboTypes() != ComboTypes.None)
			{
				switch(cell.Item.GetComboTypes())
				{
					case ComboTypes.BombandBomb:
						BombandBombExplosion(cell);
						break;
					case ComboTypes.BombandRocket:
						BombandRocketExplosion(cell);
						break;
					case ComboTypes.RocketandRocket:
						RocketandRocketExplosion(cell);
						break;
					case ComboTypes.None:
					default:
						break;
				}
				return;
			}
			
			switch(cell.Item.GetMatchType())
			{
				case MatchType.Bomb:
					ExplodeBomb(cell);
					ExecuteNeighbours();
					break;
				case MatchType.VerticalRocket:
					VerticalRocket(cell);
					break;
				case MatchType.HorizontalRocket:
					HorizontalRocket(cell);
					break;
				default:
					ExplodeMatchingCells(cell);
					break;
			}
		}

		private void BombandBombExplosion(Cell cell)
		{
			int minRow = Math.Clamp(cell.X - 3,0,Rows);
			int maxRow = Math.Clamp(cell.X + 3,0,Rows);
			int minCol = Math.Clamp(cell.Y - 3,0,Cols);
			int maxCol = Math.Clamp(cell.Y + 3,0,Cols);
			
			for(int i = minRow; i<maxRow ; i++)
			{
				for(int j = minCol; j<maxCol; j++)
				{
					if(Cells[i,j].HasItem()) Cells[i,j].Item.TryExecute();
				}
			}
		}

		private void BombandRocketExplosion(Cell cell)
		{
			foreach(Cell neighbour in cell.Neighbours)
			{
				if(cell.HasItem()) 
					cell.Item.TryExecute();
			
				VerticalRocket(neighbour);
				HorizontalRocket(neighbour);
			}
		}

		private void RocketandRocketExplosion(Cell cell)
		{
			foreach(Cell neighbour in cell.Neighbours)
			{
				if(neighbour.HasItem()) neighbour.Item.TryExecute();
			}
			if(cell.HasItem()) 
				cell.Item.TryExecute();
				
			VerticalRocket(cell); 
			HorizontalRocket(cell);
		}



		private void ExplodeMatchingCells(Cell cell)
		{
			if(cell.Item.GetMatchType() == MatchType.Bomb)
			{
				foreach(Cell cellItem in cell.Neighbours)
				{
					cellItem.Item.TryExecute();
				}
				cell.Item.TryExecute();
				return;
			}

			var cells = MatchFinder.FindMatches(cell, cell.Item.GetMatchType());
			List<Cell> specialItemsList = new List<Cell>();
			foreach(Cell i in cells){
				if(i.Item.GetMatchType() == MatchType.MatchWithAll){
					specialItemsList.Add(i);
				}
			}
			if (cells.Count < MinimumMatchCount) return;

			if(CheckColorMatchingCount()) return;
			

			foreach (var item in cells.Select(explodedCell => explodedCell.Item))
			{
				item.TryExecute();
			}

			if (cells.Count >= 7 +  specialItemsList.Count)
			{
				var newItem = ServiceProvider.GetItemFactory.CreateItem(ItemType.Bomb, itemsParent);
				cell.Item = newItem;
				newItem.transform.position = cell.transform.position;
			}

			else if (cells.Count == 5 + specialItemsList.Count|| cells.Count == 6 + specialItemsList.Count)
			{
				int rnd = UnityEngine.Random.RandomRange(0, 2);

				Game.Core.ItemBase.Item newItem;

				if (rnd != 1)
				{
					 newItem = ServiceProvider.GetItemFactory.CreateItem(ItemType.HorizontalRocket, itemsParent);
				}
				else
				{
					 newItem = ServiceProvider.GetItemFactory.CreateItem(ItemType.VerticalRocket, itemsParent);
				}
				
				cell.Item = newItem;
				newItem.transform.position = cell.transform.position;
			}

			bool CheckColorMatchingCount()
			{
				int realMatchCount = 0;

				int temp = cells.Count(cellItem => (int) cellItem.Item.GetMatchType() > 4);

				realMatchCount = cells.Count - temp;

				return realMatchCount < MinimumMatchCount;
			}
		}
		
		private void ExplodeBomb(Cell cell)
		{
			cell.Visited = true;
			_neighbourCells.Add(cell);
			
			foreach(Cell cellItem in cell.GetAllDirectionNeighbours(this))
			{
				if(!cellItem.Visited && cellItem.HasItem() && cellItem.Item.GetMatchType() != MatchType.None)
				{
					if(cellItem.Item.GetMatchType() == MatchType.Bomb)
					{
						ExplodeBomb(cellItem);
					}
					else if(cellItem.Item.GetMatchType() == MatchType.VerticalRocket 
					|| cellItem.Item.GetMatchType() == MatchType.HorizontalRocket)
					{
						ExplosionConjunction(cellItem);
					}
					else
					{
						_neighbourCells.Add(cellItem);
						cellItem.Visited=true;
					}
				}
			}
		}
		
		private void VerticalRocket(Cell cell){
			for (int i = 0; i < Cols; i++)
			{
					
				if(!Cells[cell.X,i].Visited
					&&Cells[cell.X,i].HasItem()
					&& Cells[cell.X,i].Item.GetMatchType()!= MatchType.None
					&& cell.Y!=i)
				{
						Cells[cell.X,i].Visited=true;
						
						if((int)Cells[cell.X,i].Item.GetMatchType()>=10)
						{
							ExplosionConjunction(Cells[cell.X,i]);
						}
						else{
							Cells[cell.X,i].Item.TryExecute();
						}
				}
			}
			
			for (int i = 0; i < Cols; i++)
			{ 
				Cells[cell.X,i].Visited=false;
			} 
			if(cell.HasItem()){ 
				cell.Item.TryExecute();
			}
				
		}

		private void HorizontalRocket(Cell cell)
		{
			for (int i = 0; i < Rows; i++) 
			{ 
				if(!Cells[i,cell.Y].Visited
			     && Cells[i,cell.Y].HasItem()
			     && Cells[i,cell.Y].Item.GetMatchType()!= MatchType.None
			     && cell.X!=i)
				{
				
					Cells[i,cell.Y].Visited=true; 
					if((int)Cells[i,cell.Y].Item.GetMatchType()>=10)
					{ 
						ExplosionConjunction(Cells[i,cell.Y]);
					}
					else
					{ 
						Cells[i,cell.Y].Item.TryExecute();
					}
							
				}
			}
			
			for (int i = 0; i < Cols; i++)
			{ 
				Cells[i,cell.Y].Visited=false;
			} 
			if(cell.HasItem()){ 
				cell.Item.TryExecute();
			}
		}
	
		private void ExecuteNeighbours()
		{
			foreach(Cell  cell in _neighbourCells)
			{
					cell.Visited=false;
					
					if(cell.HasItem())
					{
						cell.Item.TryExecute();
					}
			}
			_neighbourCells=new List<Cell>();
		}

		public Cell GetNeighbourWithDirection(Cell cell, Direction direction)
		{
			var x = cell.X;
			var y = cell.Y;
			switch (direction)
			{
				case Direction.None:
					break;
				case Direction.Up:
					y += 1;
					break;
				case Direction.UpRight:
					y += 1;
					x += 1;
					break;
				case Direction.Right:
					x += 1;
					break;
				case Direction.DownRight:
					y -= 1;
					x += 1;
					break;
				case Direction.Down:
					y -= 1;
					break;
				case Direction.DownLeft:
					y -= 1;
					x -= 1;
					break;
				case Direction.Left:
					x -= 1;
					break;
				case Direction.UpLeft:
					y += 1;
					x -= 1;
					break;
				default:
					throw new ArgumentOutOfRangeException("direction", direction, null);
			}

			if (x >= Cols || x < 0 || y >= Rows || y < 0) return null;

			return Cells[x, y];
		}
	}
}
