using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Managers;

namespace Game.Mechanics
{
    public class ComboManager : MonoBehaviour
    {
        [SerializeField] private Board board;

        private MatchFinder _matchFinder = new MatchFinder();
        
       
        // Update is called once per frame
        void Update()
        {
            SetComboTypes();
            _matchFinder.ClearVisitedCells();
        }

        private void SetComboTypes()
        {
            for (int i = 0; i < Board.Rows; i++)
            {
                for (int j = 0; j < Board.Cols; j++)
                {
                    if (board.Cells[i, j].HasItem() && board.Cells[i, j].Item.GetMatchType() != MatchType.None)   
                    { 
                        var findComboMatches = _matchFinder.FindComboMatches(board.Cells[i, j], board.Cells[i, j].Item.GetMatchType());
                        if (findComboMatches.Count >= 2)
                        {
                            board.Cells[i, j].Item.SetComboType(DecideComboType(findComboMatches));
                        }
                        else
                        {
                            board.Cells[i, j].Item.SetComboType(ComboTypes.None);
                        }

                    }       

                }
            }
        }

        private ComboTypes DecideComboType(List<Cell> cells)
        {
            int rocketCount = 0;
            int bombCount = 0;

            for (int i = 0; i < cells.Count; i++)
            {
                if (cells[i].Item.GetMatchType() == MatchType.Bomb) bombCount++;
                else if (cells[i].Item.GetMatchType() == MatchType.HorizontalRocket || cells[i].Item.GetMatchType() == MatchType.VerticalRocket) rocketCount++; 
            }

            if (bombCount > 1)
            {
                return ComboTypes.BombandBomb;
            }
            else if (bombCount >= 1 && rocketCount >= 1)
            {
                return ComboTypes.BombandRocket;
            }
            else
            {
                return ComboTypes.RocketandRocket;
            }
        }

    }
}