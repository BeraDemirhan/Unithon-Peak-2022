using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.ItemBase;
using Game.Core.Enums;
using Game.Managers;

namespace Game.Items
{
    public class RocketItem : Item
    {
        private MatchType _matchType;
        private ComboTypes _comboTypes;

        public void PrepareRocketItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;

            Sprite rocketSprite;

            if (_matchType == MatchType.HorizontalRocket)
            {
                rocketSprite = ServiceProvider.GetImageLibrary.RocketHorizontal;
            }
            else
            {
                rocketSprite = ServiceProvider.GetImageLibrary.RocketVertical;
            }

            Prepare(itemBase, rocketSprite);
        }
            
        public override MatchType GetMatchType()
        {
            return _matchType;
        }

        public override ComboTypes GetComboTypes()
        {
            return _comboTypes;
        }

        public override void SetComboType(ComboTypes newComboType)
        {
            base.SetComboType(newComboType);
            _comboTypes = newComboType;
        }

    }
}