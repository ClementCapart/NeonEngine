using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.HUD
{
    public class ElementHUD : DrawableComponent
    {
        #region Properties
        private string _avatarName = "";

        public string AvatarName
        {
          get { return _avatarName; }
          set { _avatarName = value; }
        }

        private string _elementCooldownFinishedSpriteSheetTag = "";

        public string ElementCooldownFinishedSpriteSheetTag
        {
            get { return _elementCooldownFinishedSpriteSheetTag; }
            set { _elementCooldownFinishedSpriteSheetTag = value; }
        }

        private string _elementLossSpriteSheetTag = "";

        public string ElementLossSpriteSheetTag
        {
            get { return _elementLossSpriteSheetTag; }
            set { _elementLossSpriteSheetTag = value; }
        }

        private string _crystalReadySpriteSheetTag = "";

        public string CrystalReadySpriteSheetTag
        {
            get { return _crystalReadySpriteSheetTag; }
            set { _crystalReadySpriteSheetTag = value; }
        }

        private string _thunderReadySpriteSheetTag = "";

        public string ThunderReadySpriteSheetTag
        {
            get { return _thunderReadySpriteSheetTag; }
            set { _thunderReadySpriteSheetTag = value; }
        }

        private string _increaseLevelSpriteSheetTag = "";

        public string IncreaseLevelSpriteSheetTag
        {
            get { return _increaseLevelSpriteSheetTag; }
            set { _increaseLevelSpriteSheetTag = value; }
        }

        #endregion

        private Entity _avatar = null;

        private ElementSystem _elementSystem = null;

        private Texture2D _leftCharacterHUD = null;
        private Texture2D _rightCharacterHUD = null;

        private Texture2D _fireElementHUD = null;
        private Texture2D _thunderElementHUD = null;

        private SpriteSheetInfo _elementCooldownFinished;
        private SpriteSheetInfo _elementLoss;

        private SpriteSheet _leftSlotFullAnimation;
        private SpriteSheet _rightSlotFullAnimation;

        private Texture2D _leftAddedSlot = null;
        private Texture2D _rightAddedSlot = null;

        private SpriteSheetInfo _increaseElementLevel = null;

        private Vector2 _leftBaseOffset = new Vector2(-208, -22);
        private Vector2 _rightBaseOffset = new Vector2(-88, -22);

        public ElementHUD(Entity entity)
            :base(0.99f, entity, "ElementHUD")
        {
            IsHUD = true;
        }

        public override void Init()
        {
            _leftCharacterHUD = AssetManager.GetTexture("HUDLeftCharacter");
            _rightCharacterHUD = AssetManager.GetTexture("HUDRightCharacter");

            _fireElementHUD = AssetManager.GetTexture("HUDElementCrystalGauge");
            _thunderElementHUD = AssetManager.GetTexture("HUDElementThunderGauge");

            _leftAddedSlot = AssetManager.GetTexture("HUDLeftAddedSlot");
            _rightAddedSlot = AssetManager.GetTexture("HUDRightAddedSlot");

            _avatar = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatar != null)
                _elementSystem = _avatar.GetComponent<ElementSystem>();
            _elementCooldownFinished = AssetManager.GetSpriteSheet(_elementCooldownFinishedSpriteSheetTag);
            _elementLoss = AssetManager.GetSpriteSheet(_elementLossSpriteSheetTag);

            _leftSlotFullAnimation = new SpriteSheet(entity);
            _rightSlotFullAnimation = new SpriteSheet(entity);
            _leftSlotFullAnimation.Offset = _leftBaseOffset;
            _rightSlotFullAnimation.Offset = _rightBaseOffset;

            _increaseElementLevel = AssetManager.GetSpriteSheet(_increaseLevelSpriteSheetTag);

            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            _leftSlotFullAnimation.Update(gameTime);
            _rightSlotFullAnimation.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_elementSystem != null)
            {
                Texture2D leftTextureToUse = null;
                Texture2D rightTextureToUse = null;
                for (int i = 0; i < _elementSystem.ElementSlots[0].Length; i++)
                {
                    switch(_elementSystem.ElementSlots[0][i].Type)
                    {
                        case Element.Thunder:
                            if (_leftSlotFullAnimation.SpriteSheetTag != _thunderReadySpriteSheetTag)
                            {
                                _leftSlotFullAnimation.SpriteSheetTag = _thunderReadySpriteSheetTag;
                                _leftSlotFullAnimation.Init();
                            }
                            leftTextureToUse = _thunderElementHUD;
                            break;

                        case Element.Fire:
                            if (_leftSlotFullAnimation.SpriteSheetTag != _crystalReadySpriteSheetTag)
                            {
                                _leftSlotFullAnimation.SpriteSheetTag = _crystalReadySpriteSheetTag;
                                _leftSlotFullAnimation.Init();
                            }
                            leftTextureToUse = _fireElementHUD;
                            break;
                    }

                    if (i > 0 && _leftAddedSlot != null)
                        spriteBatch.Draw(_leftAddedSlot, entity.transform.Position + Offset + new Vector2(-1, i * (_leftAddedSlot.Height) - 5 + i * 11), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                    if (_elementSystem.ElementSlots[0][i].Type != Element.Neutral)
                    {
                        if(_elementSystem.ElementSlots[0][i].Cooldown <= 0.0f && _leftSlotFullAnimation.spriteSheetInfo != null && _leftSlotFullAnimation.spriteSheetInfo.Frames != null)
                            spriteBatch.Draw(_leftSlotFullAnimation.spriteSheetInfo.Frames[_leftSlotFullAnimation.currentFrame], entity.transform.Position + _leftSlotFullAnimation.Offset + new Vector2(0, i * 22), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, 1.0f);
                        else
                            spriteBatch.Draw(leftTextureToUse, entity.transform.Position + Offset + new Vector2(0, i * (leftTextureToUse.Height * 2) + i * 12) + new Vector2(1, 5), new Rectangle(0, 0, (int)Math.Round(leftTextureToUse.Width * (1 - _elementSystem.ElementSlots[0][i].Cooldown / Math.Max(_elementSystem.ElementSlotCooldownDuration, 0.1f))), leftTextureToUse.Height), Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                    }
                }

                for (int i = 0; i < _elementSystem.ElementSlots[1].Length; i++)
                {
                        switch (_elementSystem.ElementSlots[1][i].Type)
                        {
                            case Element.Thunder:
                                if (_rightSlotFullAnimation.SpriteSheetTag != _thunderReadySpriteSheetTag)
                                {
                                    _rightSlotFullAnimation.SpriteSheetTag = _thunderReadySpriteSheetTag;
                                    _rightSlotFullAnimation.Init();
                                }
                                rightTextureToUse = _thunderElementHUD;
                                break;

                            case Element.Fire:
                                if (_rightSlotFullAnimation.SpriteSheetTag != _crystalReadySpriteSheetTag)
                                {
                                    _rightSlotFullAnimation.SpriteSheetTag = _crystalReadySpriteSheetTag;
                                    _rightSlotFullAnimation.Init();
                                }
                                rightTextureToUse = _fireElementHUD;
                                break;
                        }

                        if (i > 0 && _rightAddedSlot != null)
                            spriteBatch.Draw(_rightAddedSlot, entity.transform.Position + Offset + new Vector2(_rightAddedSlot.Width + 73, i * (_leftAddedSlot.Height) - 5 + i * 11) , null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                        if (_elementSystem.ElementSlots[1][i].Type != Element.Neutral)
                        {
                            if (_elementSystem.ElementSlots[1][i].Cooldown <= 0.0f && _rightSlotFullAnimation.spriteSheetInfo != null && _rightSlotFullAnimation.spriteSheetInfo.Frames != null)
                                spriteBatch.Draw(_rightSlotFullAnimation.spriteSheetInfo.Frames[_rightSlotFullAnimation.currentFrame], entity.transform.Position + _rightSlotFullAnimation.Offset + new Vector2(0, i * 22), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, 1.0f);
                            else
                                spriteBatch.Draw(rightTextureToUse, entity.transform.Position + Offset + new Vector2(_fireElementHUD.Width * entity.transform.Scale + 10, i * (rightTextureToUse.Height * 2) + i * 12) + new Vector2(23, 5), new Rectangle(0, 0, (int)Math.Round(_fireElementHUD.Width * (1 - _elementSystem.ElementSlots[1][i].Cooldown / Math.Max(_elementSystem.ElementSlotCooldownDuration, 0.1f))), rightTextureToUse.Height), Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                        }
                }

                if (_leftCharacterHUD != null)
                {
                    if(_elementSystem.MaxLevel == 1)
                        spriteBatch.Draw(_leftCharacterHUD, entity.transform.Position + Offset + new Vector2(_fireElementHUD.Width / 2 * entity.transform.Scale - _leftCharacterHUD.Width + 2, (_elementSystem.MaxLevel) * (_leftAddedSlot.Height * 2)), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                    else
                        spriteBatch.Draw(_leftCharacterHUD, entity.transform.Position + Offset + new Vector2(_fireElementHUD.Width / 2 * entity.transform.Scale - _leftCharacterHUD.Width + 2, (_elementSystem.MaxLevel) * (_leftAddedSlot.Height * 2) - 1), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                }
                if (_rightCharacterHUD != null)
                {
                    if (_elementSystem.MaxLevel == 1)
                        spriteBatch.Draw(_rightCharacterHUD, entity.transform.Position + Offset + new Vector2(_fireElementHUD.Width * entity.transform.Scale + _fireElementHUD.Width / 2 * entity.transform.Scale + 35 - _rightCharacterHUD.Width, (_elementSystem.MaxLevel) * (_leftAddedSlot.Height * 2)), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                    else
                        spriteBatch.Draw(_rightCharacterHUD, entity.transform.Position + Offset + new Vector2(_fireElementHUD.Width * entity.transform.Scale + _fireElementHUD.Width / 2 * entity.transform.Scale + 35 - _rightCharacterHUD.Width, (_elementSystem.MaxLevel) * (_leftAddedSlot.Height * 2) - 1), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                }
            }
            base.Draw(spriteBatch);
        }

        public void CooldownFinished(int row, int column)
        {
            if (_elementCooldownFinished != null)
            {
                if(row > 0)
                    EffectsManager.GetEffect(_elementCooldownFinished, Side.Right, entity.transform.Position + Offset + new Vector2(_elementCooldownFinished.FrameWidth / 2, (row * _fireElementHUD.Height * 2) + row * 12) + new Vector2(22, 7) + new Vector2(column * 120, 0), 0.0f, Vector2.Zero, 2.0f, 1.0f, null, 0.0f, false, true);
                else
                    EffectsManager.GetEffect(_elementCooldownFinished, Side.Right, entity.transform.Position + Offset + new Vector2(_elementCooldownFinished.FrameWidth / 2, 0 * (_fireElementHUD.Height * 2)) + new Vector2(22, 7) + new Vector2(column * 120, 0), 0.0f, Vector2.Zero, 2.0f, 1.0f, null, 0.0f, false, true);
            }
        }

        public void LoseElement(int row, int column)
        {
            if (_elementLoss != null)
            {
                if (row > 0)
                    EffectsManager.GetEffect(_elementLoss, Side.Right, entity.transform.Position + Offset + new Vector2(_elementLoss.FrameWidth / 2, (row * _fireElementHUD.Height * 2) + row * 12) + new Vector2(22, 9) + new Vector2(column * 120, 0), 0.0f, Vector2.Zero, 2.0f, 1.0f, null, 0.0f, false, true);
                else
                    EffectsManager.GetEffect(_elementLoss, Side.Right, entity.transform.Position + Offset + new Vector2(_elementLoss.FrameWidth / 2, 0 * (_fireElementHUD.Height * 2)) + new Vector2(22, 9) + new Vector2(column * 120, 0), 0.0f, Vector2.Zero, 2.0f, 1.0f, null, 0.0f, false, true);
            }
        }

        public void UpgradeMaxLevel(int level)
        {
            if(level > 1)
                if(_increaseElementLevel != null)
                    EffectsManager.GetEffect(_increaseElementLevel, Side.Right, entity.transform.Position + Offset + new Vector2(105, 25), 0.0f, Vector2.Zero, 2.0f, 1.0f, null, 0.0f, false, true);
        }
    }
}
