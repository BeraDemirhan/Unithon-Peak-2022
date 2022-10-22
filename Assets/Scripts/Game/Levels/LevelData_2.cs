using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Core.LevelBase;
using UnityEngine;

namespace Game.Levels
{
    public class LevelData_2 : LevelData
    {
        private int _balloonFillRate = 10;

        public override ItemType GetNextFillItemType()
        {
            return GetRandomBalloonItemWithRate(_balloonFillRate);
        }

        public override void Initialize()
        {
            GridData = new ItemType[Board.Rows, Board.Cols];

            for (var y = 0; y < Board.Rows; y++)
            {
                for (var x = 0; x < Board.Cols; x++)
                {
                    if (GridData[x, y] != ItemType.None) continue;
                    if (x == y || x + y == 8)
                    {
                        GridData[x, y] = ItemType.Balloon;
                    }
                    else
                    {
                        GridData[x, y] = GetRandomCubeItemType();
                    }
                }
            }
        }
    }
}