using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Core.ItemBase;
using Game.Core.Enums;
using Game.Managers;

namespace Game.Items
{   public class BombItem : Item
    {
        private MatchType _matchType;
        private ComboTypes _comboTypes;

        public void PrepareBombItem(ItemBase itemBase, MatchType matchType)
        {
            _matchType = matchType;
            var bombSprite = ServiceProvider.GetImageLibrary.BombSprite;
            Prepare(itemBase, bombSprite);
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