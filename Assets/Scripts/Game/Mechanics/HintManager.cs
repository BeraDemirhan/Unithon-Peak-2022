using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Managers;
using UnityEngine;

namespace Game.Mechanics
{	
	public class HintManager : MonoBehaviour 
	{
		[SerializeField] private Board board;
		[SerializeField] private FallAndFillManager fallAndFillManager;

		private MatchFinder _matchFinder = new MatchFinder();

		private void Update ()
		{
			CheckForHint();
			_matchFinder.ClearVisitedCells();
		}

		private void CheckForHint()
		{
			for (int i = 0; i < Board.Rows; i++)
			{
				for (int j = 0; j < Board.Cols; j++)
				{
					if (board.Cells[i, j].HasItem() && board.Cells[i, j].Item.GetMatchType() != MatchType.None)
					{
						var matching = _matchFinder.FindHintMatches(board.Cells[i, j], board.Cells[i, j].Item.GetMatchType());

						if (matching.Count >= 7)
						{
							board.Cells[i, j].Item.SetHintSprite(MatchType.Bomb);
						}
						else if (matching.Count == 5 || matching.Count == 6)
						{
							board.Cells[i, j].Item.SetHintSprite(MatchType.HorizontalRocket);
						}
						else
						{
							if ((int) board.Cells[i, j].Item.GetMatchType() < 5)
							{
								board.Cells[i, j].Item.SetDefaultSprite();
							}
							
						}
						
						_matchFinder.ClearVisitedCells();
						
						var findComboMatches = _matchFinder.FindComboMatches(board.Cells[i, j], board.Cells[i, j].Item.GetMatchType());
						
						if (findComboMatches.Count >= 2)
						{
							board.Cells[i, j].Item.PlayParticle();
						}
						else
						{
							board.Cells[i, j].Item.StopParticle();
						}
					}
				}
			}
		}
	}
}
