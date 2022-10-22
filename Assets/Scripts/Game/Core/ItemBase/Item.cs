using Game.Core.BoardBase;
using Game.Core.Enums;
using Game.Mechanics;
using UnityEngine;
using System;
using Game.Items;
using Game.Managers;

namespace Game.Core.ItemBase
{
    public abstract class Item : MonoBehaviour
    {
        public bool IsClickable { get; private set; }
        public Cell Cell
        {
            get => _cell;
            set
            {
                if (_cell == value) return;

                var oldCell = _cell;
                _cell = value;

                if (oldCell != null && oldCell.Item == this)
                {
                    oldCell.Item = null;
                }

                if (value != null)
                {
                    value.Item = this;
                    gameObject.name = _cell.gameObject.name + " " + GetType().Name;
                }

            }
        }

        protected SpriteRenderer SpriteRenderer { get; private set; }
        
        private FallAnimation FallAnimation { get; set; }
        private Cell _cell;
        private Sprite _defaultSprite;
        private ParticleSystem _particle;
        private int _childSpriteOrder;
        private const int BaseSortingOrder = 10;
        private bool _canFall;
        private int _currentHealth;
        private bool _isParticlePlaying;

        protected void Prepare(ItemBase itemBase, Sprite sprite, bool isClickable = true, bool canFall = true, int health = 1)     
        {
            SpriteRenderer = AddSprite(sprite);
            _defaultSprite = sprite;
            FallAnimation = itemBase.FallAnimation;
            FallAnimation.Item = this;
            _canFall = canFall;
            _currentHealth = health;
            IsClickable = isClickable;
        }

        private SpriteRenderer AddSprite(Sprite sprite)
        {
            var spriteRenderer = new GameObject("Sprite_" + _childSpriteOrder).AddComponent<SpriteRenderer>();
            spriteRenderer.transform.SetParent(transform);
            spriteRenderer.sprite = sprite;
            spriteRenderer.sortingLayerID = SortingLayer.NameToID("Item");
            spriteRenderer.sortingOrder = BaseSortingOrder + _childSpriteOrder++;

            return spriteRenderer;
        }

        public void PlayParticle()
        {
            if(_isParticlePlaying) return;
            _isParticlePlaying = true;

            _particle = ServiceProvider.GetParticleManager.PlayComboParticleOnItem(this);
        }

        public void StopParticle()
        {
            if(!_isParticlePlaying) return;
            _isParticlePlaying = false;
            
            if(_particle != null) ServiceProvider.GetParticleManager.StopParticle(_particle);
        }

        public void RemoveSprite(SpriteRenderer spriteRenderer)
        {
            if (spriteRenderer == SpriteRenderer)
            {
                SpriteRenderer = null;
            }

            Destroy(spriteRenderer.gameObject);
        }

        protected void ChangeSprite(Sprite newSprite)
        {
            SpriteRenderer.sprite = newSprite;
        }

        public virtual void SetHintSprite(MatchType matchType)
        {
        }

        public void SetDefaultSprite()
        {
            SpriteRenderer.sprite = _defaultSprite;
        }

        public bool IsFalling()
        {
            return FallAnimation.IsFalling;
        }

        public void Fall()
        {
            if(!_canFall) return;
            FallAnimation.FallTo(Cell.GetFallTarget());				
        }
        
        public virtual void TryExecute()
        {
            _cell.Item.TakeDamage(1);
            if (TryGetComponent(out ISpriteChange spriteChange))
            {
                spriteChange.ChangeSprite();
            }
            if(!_cell.Item.CheckAlive())
            {
                RemoveItem();
            }
        }

        private void RemoveItem()
        {
            Cell.Item = null;
            Cell = null;

            Destroy(gameObject);
        }

        private void TakeDamage(int damage)
        {
            int newHealth = _currentHealth - damage;
            _currentHealth = Math.Clamp(newHealth, 0, Int32.MaxValue);
        }

        private bool CheckAlive()
        {
            return _currentHealth > 0;
        }

        public override string ToString()
        {
            return gameObject.name;
        }
        
        public virtual MatchType GetMatchType()
        {
            return MatchType.None;
        }

        public virtual ComboTypes GetComboTypes()
        {
            return ComboTypes.None;
        }

        public virtual void SetComboType(ComboTypes newComboType)
        {

        }

    }
}