using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items{
    
    public class ColorBalloonItem : Item
    {
        
        private MatchType _matchType;
        
        public void PrepareColorBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            var colorBalloonSprite = GetSpritesForMatchType();
            Prepare(itemBase, colorBalloonSprite, isClickable : false);
        }
        
        private Sprite GetSpritesForMatchType()
        {
            var imageLibrary = ServiceProvider.GetImageLibrary;
            
            switch (_matchType)
            {
                case MatchType.YellowBalloon:
                    return imageLibrary.YellowBalloonSprite;
                case MatchType.BlueBalloon:
                    return imageLibrary.BlueBalloonSprite;
                case MatchType.RedBalloon:
                    return imageLibrary.RedBalloonSprite;
                case MatchType.GreenBalloon:
                    return imageLibrary.GreenBalloonSprite;
            }

            return null;
        }
        
        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override void TryExecute()
        {
            ServiceProvider.GetParticleManager.PlayCubeParticle(this); //TODO: particle yapilcak
            
            base.TryExecute();
        }
    }

    
}

