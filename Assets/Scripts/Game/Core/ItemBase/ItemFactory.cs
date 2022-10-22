using Game.Core.Enums;
using Game.Items;
using Game.Managers;
using UnityEngine;

namespace Game.Core.ItemBase
{
    public class ItemFactory : MonoBehaviour, IProvidable
    {
        [SerializeField] private ItemBase ItemBasePrefab;

        private void Awake()
        {
            ServiceProvider.Register(this);
        }

        public Item CreateItem(ItemType itemType, Transform parent)
        {
            if (itemType == ItemType.None)
            {
                return null;
            }

            var itemBase = Instantiate(ItemBasePrefab, Vector3.zero, Quaternion.identity, parent);

            Item item = null;
            switch (itemType)
            {
                case ItemType.GreenCube:
                    item = CreateCubeItem(itemBase, MatchType.Green);
                    break;
                case ItemType.YellowCube:
                    item = CreateCubeItem(itemBase, MatchType.Yellow);
                    break;
                case ItemType.BlueCube:
                    item = CreateCubeItem(itemBase, MatchType.Blue);
                    break;
                case ItemType.RedCube:
                    item = CreateCubeItem(itemBase, MatchType.Red);
                    break;
                case ItemType.Crate:
                    item = CreateCrateItem(itemBase, MatchType.MatchWithAll);
                    break;
                case ItemType.Balloon:
                    item = CreateBalloonItem(itemBase, MatchType.MatchWithAll);
                    break;
                case ItemType.BlueBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.BlueBalloon);
                    break;
                case ItemType.GreenBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.GreenBalloon);
                    break;
                case ItemType.RedBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.RedBalloon);
                    break;
                case ItemType.YellowBalloon:
                    item = CreateColorBalloonItem(itemBase, MatchType.YellowBalloon);
                    break;
                case ItemType.Bomb:
                    item = CreateBombItem(itemBase, MatchType.Bomb);
                    break;
                case ItemType.HorizontalRocket:
                    item = CreateRocketItem(itemBase, MatchType.HorizontalRocket);
                    break;
                case ItemType.VerticalRocket:
                    item = CreateRocketItem(itemBase, MatchType.VerticalRocket);
                    break;
                default:
                    Debug.LogWarning("Can not create item: " + itemType);
                    break;
            }

            return item;
        }

        private Item CreateCubeItem(ItemBase itemBase, MatchType matchType)
        {
            var cubeItem = itemBase.gameObject.AddComponent<CubeItem>();
            cubeItem.PrepareCubeItem(itemBase, matchType);

            return cubeItem;
        }

        private Item CreateCrateItem(ItemBase itemBase, MatchType matchType)
        {
            var crateItem = itemBase.gameObject.AddComponent<CrateItem>();
            crateItem.PrepareCrateItem(itemBase, matchType);
            return crateItem;
        }
        
        private Item CreateBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            var balloonItem = itemBase.gameObject.AddComponent<BalloonItem>();
            balloonItem.PrepareBalloonItem(itemBase, matchType);
            return balloonItem;
        }
        private Item CreateColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            var colorBalloonItem = itemBase.gameObject.AddComponent<ColorBalloonItem>();
            colorBalloonItem.PrepareColorBalloonItem(itemBase, matchType);
            return colorBalloonItem;
        }
        private Item CreateBombItem(ItemBase itemBase, MatchType matchType)
        {
            var bombItem = itemBase.gameObject.AddComponent<BombItem>();
            bombItem.PrepareBombItem(itemBase, matchType);
            return bombItem;
        }

        private Item CreateRocketItem(ItemBase itemBase, MatchType matchType)
        {
            var rocketItem = itemBase.gameObject.AddComponent<RocketItem>();
            rocketItem.PrepareRocketItem(itemBase, matchType);
            return rocketItem;
        }
    }
}