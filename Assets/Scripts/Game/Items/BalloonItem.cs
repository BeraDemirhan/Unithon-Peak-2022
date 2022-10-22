using System.Collections;
using System.Collections.Generic;
using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using UnityEngine;

namespace Game.Items
{
    public class BalloonItem : Item
    {
        private MatchType _matchType;
        
        public void PrepareBalloonItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            var balloonSprite = ServiceProvider.GetImageLibrary.BalloonSprite;
            Prepare(itemBase, balloonSprite, isClickable : false);
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
