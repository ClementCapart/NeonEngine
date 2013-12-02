using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum Element
    {
        Neutral,
        Fire,
        Thunder
    }

    public class ElementSystem : Component
    {
        #region Properties
        private string _fireLaunchAnimation = "";

        public string FireLaunchAnimation
        {
            get { return _fireLaunchAnimation; }
            set { _fireLaunchAnimation = value; }
        }

        private float _fireCooldown = 0.0f;

        public float FireCooldown
        {
            get { return _fireCooldown; }
            set { _fireCooldown = value; }
        }

        private string _thunderLaunchAnimation = "";

        public string ThunderLaunchAnimation
        {
            get { return _thunderLaunchAnimation; }
            set { _thunderLaunchAnimation = value; }
        }

        private float _thunderCooldown = 0.0f;

        public float ThunderCooldown
        {
            get { return _thunderCooldown; }
            set { _thunderCooldown = value; }
        }

        private Element _leftSlotElement = Element.Neutral;

        public Element LeftSlotElement
        {
            get { return _leftSlotElement; }
            set { _leftSlotElement = value; }
        }

        private float _leftSlotLevel = 1;

        public float LeftSlotLevel
        {
            get { return _leftSlotLevel; }
            set { _leftSlotLevel = value; }
        }

        public float LeftSlotCooldownTimer = 0.0f;

        private Element _rightSlotElement = Element.Neutral;

        public Element RightSlotElement
        {
            get { return _rightSlotElement; }
            set { _rightSlotElement = value; }
        }

        private float _rightSlotLevel = 1;

        public float RightSlotLevel
        {
            get { return _rightSlotLevel; }
            set { _rightSlotLevel = value; }
        }

        public float RightSlotCooldownTimer = 0.0f;      

        private float _maxLevel = 3;

        public float MaxLevel
        {
            get { return _maxLevel; }
            set { _maxLevel = value; }
        }

        private string _thunderGatheringFX = "";

        public string ThunderGatheringFX
        {
            get { return _thunderGatheringFX; }
            set { _thunderGatheringFX = value; }
        }

        private string _fireGatheringFX = "";

        public string FireGatheringFX
        {
            get { return _fireGatheringFX; }
            set { _fireGatheringFX = value; }
        }

        private float _getElementColorDelay = 0.15f;

        public float GetElementColorDelay
        {
            get { return _getElementColorDelay; }
            set { _getElementColorDelay = value; }
        }

        #endregion

        public Avatar AvatarComponent = null;

        public ElementEffect CurrentElementEffect = null;

        private SpriteSheetInfo FrontFireGatheringFX = null;
        private SpriteSheetInfo BackFireGatheringFX = null;

        private SpriteSheetInfo FrontThunderGatheringFX = null;
        private SpriteSheetInfo BackThunderGatheringFX = null;

        private AnimatedSpecialEffect _currentAnimatedSpecialEffect = null;
        private bool _getElementColored = false;
        private float _getElementColorTimer = 0.0f;
        private Color _nextColorToTint;

        public ElementSystem(Entity entity)
            :base(entity, "ElementSystem")
        {
        }

        public override void Init()
        {
            FrontFireGatheringFX = AssetManager.GetSpriteSheet(_fireGatheringFX);
            BackFireGatheringFX = AssetManager.GetSpriteSheet(_fireGatheringFX);
            FrontThunderGatheringFX = AssetManager.GetSpriteSheet(_thunderGatheringFX);
            BackThunderGatheringFX = AssetManager.GetSpriteSheet(_thunderGatheringFX);
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (LeftSlotCooldownTimer > 0.0f)
            {
                LeftSlotCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (LeftSlotCooldownTimer < 0.0f)
                    LeftSlotCooldownTimer = 0.0f;
            }

            if (RightSlotCooldownTimer > 0.0f)
            {
                RightSlotCooldownTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (RightSlotCooldownTimer < 0.0f)
                    RightSlotCooldownTimer = 0.0f;
            }

            if (CurrentElementEffect != null)
            {
                AvatarComponent.State = AvatarState.UsingElement;
                CurrentElementEffect.PreUpdate(gameTime);
                AvatarComponent.CanUseElement = false;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AvatarComponent.CanAttack && AvatarComponent.CanMove && AvatarComponent.CanTurn && AvatarComponent.CanUseElement)
            {
                if (Neon.Input.Pressed(NeonStarInput.UseLeftSlotElement) && LeftSlotCooldownTimer <= 0.0f)
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + LeftSlotElement);
                        UseElement(_leftSlotElement, (int)_leftSlotLevel, NeonStarInput.UseLeftSlotElement);
                    }
                }
                else if (Neon.Input.Pressed(NeonStarInput.UseRightSlotElement) && RightSlotCooldownTimer <= 0.0f)
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + RightSlotElement);
                        UseElement(_rightSlotElement, (int)_rightSlotLevel, NeonStarInput.UseRightSlotElement);
                    }
                }

                if (Neon.Input.Pressed(NeonStarInput.DropLeftSlotElement))
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + LeftSlotElement);
                        _leftSlotElement = Element.Neutral;
                        _leftSlotLevel = 1;
                    }
                }
                if (Neon.Input.Pressed(NeonStarInput.DropRightSlotElement))
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + RightSlotElement);
                        _rightSlotElement = Element.Neutral;
                        _rightSlotLevel = 1;
                    }
                }
            }
            else if (CurrentElementEffect != null)
            {
                CurrentElementEffect.Update(gameTime);
            }
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CurrentElementEffect != null)
            {
                CurrentElementEffect.PostUpdate(gameTime);
                if (CurrentElementEffect.State == ElementState.End)
                {
                    CurrentElementEffect = null;
                    AvatarComponent.State = AvatarState.Idle;
                }
            }

            if (_currentAnimatedSpecialEffect != null)
            {
                _currentAnimatedSpecialEffect.transform.Position = entity.transform.Position + new Vector2((AvatarComponent.CurrentSide == Side.Right ? 10 : - 10), -15);
                if (_currentAnimatedSpecialEffect.spriteSheet.IsFinished)
                {
                    entity.spritesheets.CurrentSpritesheet.MainColor = _nextColorToTint;
                    entity.spritesheets.CurrentSpritesheet.Tint = false;
                    _getElementColored = true;
                    _getElementColorTimer = _getElementColorDelay;
                    _currentAnimatedSpecialEffect = null;
                }
            }

            if (_getElementColored)
            {
                if (_getElementColorTimer > 0.0f)
                    _getElementColorTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                else
                {
                    _getElementColorTimer = 0.0f;
                    entity.spritesheets.CurrentSpritesheet.MainColor = Color.White;
                    entity.spritesheets.Tint = true;
                    _getElementColored = false;
                }
            }
            base.PostUpdate(gameTime);
        }

        public void UseElement(Element element, int level, NeonStarInput input)
        {
            switch(element)
            {
                case Element.Fire:
                    CurrentElementEffect = new Fire(this, level, entity, input, (GameScreen)entity.containerWorld);
                    break;

                case Element.Thunder:
                    CurrentElementEffect = new Thunder(this, level, entity, input, (GameScreen)entity.containerWorld);
                    break;
            }
        }

        public void GetElement(Element element)
        {
            if (_leftSlotElement == element)
            {
                if (_leftSlotLevel < _maxLevel)
                {
                    ElementFeedback(element);
                    _leftSlotLevel++;
                }
                Console.WriteLine("Left Slot Level Up -> " + _leftSlotLevel);
            }
            else if (_rightSlotElement == element)
            {
                if (_rightSlotLevel < _maxLevel)
                {
                    ElementFeedback(element);
                    _rightSlotLevel++;
                }
                Console.WriteLine("Right Slot Level Up -> " + _rightSlotLevel);
            }
            else if (_leftSlotElement == Element.Neutral)
            {
                ElementFeedback(element);
                _leftSlotElement = element;
                Console.WriteLine("Got " + element + " in Left Slot");
            }
            else if (_rightSlotElement == Element.Neutral)
            {
                ElementFeedback(element);
                _rightSlotElement = element;
                Console.WriteLine("Got " + element + " in Right Slot");
            }
            else
            {
                Console.WriteLine("Fizzle");
            }
        }

        private void ElementFeedback(Element element)
        {
            switch(element)
                {
                    case Element.Fire:
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackFireGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontFireGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.Red;
                        break;

                    case Element.Thunder:
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackThunderGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontThunderGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.FromNonPremultiplied(255, 230, 100, 255);
                        break;
                }
        }
    }
}
