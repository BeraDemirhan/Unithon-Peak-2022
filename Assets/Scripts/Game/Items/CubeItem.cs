using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class CubeItem : Item
    {
        private MatchType _matchType;

        public void PrepareCubeItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            Prepare(itemBase, GetSpritesForMatchType());
        }

        private Sprite GetSpritesForMatchType()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;
            
            switch (_matchType)
            {
                case MatchType.Green:
                    return imageLibrary.GreenCubeSprite;
                case MatchType.Yellow:
                    return imageLibrary.YellowCubeSprite;
                case MatchType.Blue:
                    return imageLibrary.BlueCubeSprite;
                case MatchType.Red:
                    return imageLibrary.RedCubeSprite;
            }

            return null;
        }

        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override void TryExecute()
        {
            ServiceProvider.GetParticleManager.PlayCubeParticle(this);
            
            base.TryExecute();
        }

        public override void SetHintSprite(MatchType matchType)
        {
            base.SetHintSprite(matchType);

            if (matchType == MatchType.Bomb)
            {
                switch (GetMatchType())
                {

                    case MatchType.Green:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.GreenCubeBombHintSprite;
                        break;
                    case MatchType.Yellow:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.YellowCubeBombHintSprite;
                        break;
                    case MatchType.Blue:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.BlueCubeBombHintSprite;
                        break;
                    case MatchType.Red:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.RedCubeBombHintSprite;
                        break;
                }
            }
            else if (matchType == MatchType.HorizontalRocket)
            {
                switch (GetMatchType())
                {

                    case MatchType.Green:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.GreenCubeRocketHintSprite;
                        break;
                    case MatchType.Yellow:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.YellowCubeRocketHintSprite;
                        break;
                    case MatchType.Blue:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.BlueCubeRocketHintSprite;
                        break;
                    case MatchType.Red:
                        SpriteRenderer.sprite = ServiceProvider.GetImageLibrary.RedCubeRocketHintSprite;
                        break;
                }
            }
        }
    }
}
