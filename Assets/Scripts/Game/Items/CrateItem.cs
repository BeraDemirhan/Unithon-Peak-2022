using Game.Core.Enums;
using Game.Core.ItemBase;
using Game.Managers;
using System;
using UnityEngine;
namespace Game.Items
{
    public class CrateItem : Item, ISpriteChange
    {   
        private MatchType _matchType; 
        
        public void PrepareCrateItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            var crateLayer2Sprite = ServiceProvider.GetImageLibrary.CrateLayer2Sprite;
            Prepare(itemBase, crateLayer2Sprite, isClickable:false ,false, 2);
        }

        public override MatchType GetMatchType()
        {
            return _matchType;
        }

		public void ChangeSprite()
		{
            ChangeSprite(ServiceProvider.GetImageLibrary.CrateLayer1Sprite);
		}
	}
}
