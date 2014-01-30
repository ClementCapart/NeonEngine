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

        #endregion

        private Entity _avatar = null;

        private ElementSystem _elementSystem = null;
        private Element _lastFrameElementLeft;
        private Element _lastFrameElementRight;

        private int _lastFrameLevelLeft = 0;
        private int _lastFrameLevelRight = 0;

        private Texture2D leftCharacterHUD = null;
        private Texture2D rightCharacterHUD = null;

        private Texture2D emptySlotHUD = null;

        private Texture2D fireElementHUD = null;
        private Texture2D thunderElementHUD = null;

        private Texture2D CurrentLeftElement = null;
        private Texture2D CurrentRightElement = null;

        public ElementHUD(Entity entity)
            :base(0.99f, entity, "ElementHUD")
        {
            IsHUD = true;
        }

        public override void Init()
        {
            leftCharacterHUD = AssetManager.GetTexture("HUDLeftCharacter");
            rightCharacterHUD = AssetManager.GetTexture("HUDRightCharacter");

            emptySlotHUD = AssetManager.GetTexture("HUDElementNeutral");

            fireElementHUD = AssetManager.GetTexture("HUDElementFire");
            thunderElementHUD = AssetManager.GetTexture("HUDElementThunder");

            _avatar = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatar != null)
            {
                _elementSystem = _avatar.GetComponent<ElementSystem>();
                if (_elementSystem != null)
                {
                    _lastFrameElementLeft = _elementSystem.LeftSlotElement;
                    _lastFrameElementRight = _elementSystem.RightSlotElement;
                    _lastFrameLevelLeft = (int)_elementSystem.LeftSlotLevel;
                    _lastFrameLevelRight = (int)_elementSystem.RightSlotLevel;

                    switch(_elementSystem.LeftSlotElement)
                    {
                        case Element.Neutral:
                            CurrentLeftElement = emptySlotHUD;
                            break;

                        case Element.Fire:
                            CurrentLeftElement = fireElementHUD;
                            break;

                        case Element.Thunder:
                            CurrentLeftElement = thunderElementHUD;
                            break;
                    }

                    switch (_elementSystem.RightSlotElement)
                    {
                        case Element.Neutral:
                            CurrentRightElement = emptySlotHUD;
                            break;

                        case Element.Fire:
                            CurrentRightElement = fireElementHUD;
                            break;

                        case Element.Thunder:
                            CurrentRightElement = thunderElementHUD;
                            break;
                    }
                }            
            }

            

            
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.PreUpdate(gameTime);
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_elementSystem != null)
            {
                if (_lastFrameElementLeft != _elementSystem.LeftSlotElement)
                {
                    if (_elementSystem.LeftSlotElement == Element.Neutral)
                        RemoveElement(Side.Left);
                    else
                        AddElement(_elementSystem.LeftSlotElement, Side.Left);
                }
                if (_lastFrameElementRight != _elementSystem.RightSlotElement)
                {
                    if (_elementSystem.RightSlotElement == Element.Neutral)
                        RemoveElement(Side.Right);
                    else
                        AddElement(_elementSystem.RightSlotElement, Side.Right);
                }

                if (_lastFrameLevelLeft != (int)_elementSystem.LeftSlotLevel)
                {
                    if (_elementSystem.LeftSlotElement != Element.Neutral)
                        ChangeElementLevel(Side.Left);
                }

                if (_lastFrameLevelRight != (int)_elementSystem.RightSlotLevel)
                {
                    if (_elementSystem.LeftSlotElement != Element.Neutral)
                        ChangeElementLevel(Side.Right);
                }



                _lastFrameElementLeft = _elementSystem.LeftSlotElement;
                _lastFrameElementRight = _elementSystem.RightSlotElement;

                _lastFrameLevelLeft = (int)_elementSystem.LeftSlotLevel;
                _lastFrameLevelRight = (int)_elementSystem.RightSlotLevel;
            }
            base.FinalUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void RemoveElement(Side side)
        {
            if (side == Side.Left)
            {
                CurrentLeftElement = null;
                CurrentLeftElement = emptySlotHUD;
            }
            else
            {
                CurrentRightElement = null;
                CurrentRightElement = emptySlotHUD;
            }
        }

        private void AddElement(Element element, Side side)
        {
            if (side == Side.Left)
            {
                switch(element)
                {
                    case Element.Fire:
                        CurrentLeftElement = null;
                        CurrentLeftElement = fireElementHUD;
                        break;

                    case Element.Thunder:
                        CurrentLeftElement = null;
                        CurrentLeftElement = thunderElementHUD;
                        break;
                }
            }
            else
            {
                switch(element)
                {
                    case Element.Fire:
                        CurrentRightElement = null;
                        CurrentRightElement = fireElementHUD;
                        break;

                    case Element.Thunder:
                        CurrentRightElement = null;
                        CurrentRightElement = thunderElementHUD;
                        break;
                }
            }
        }

        private void ChangeElementLevel(Side side)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_elementSystem != null)
            {
                if(CurrentLeftElement != null)
                    for (int i = 0; i < _elementSystem.LeftSlotLevel; i++)
                        spriteBatch.Draw(CurrentLeftElement, entity.transform.Position + Offset + new Vector2(0, i * (CurrentLeftElement.Height * 2) + i * 4), null, CurrentLeftElement != emptySlotHUD ? Color.Lerp(Color.Black, Color.White, 0.3f) : Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                if(CurrentRightElement != null)
                    for (int i = 0; i < _elementSystem.RightSlotLevel; i++)
                        spriteBatch.Draw(CurrentRightElement, entity.transform.Position + Offset + new Vector2(CurrentLeftElement.Width * entity.transform.Scale + 10, i * (CurrentRightElement.Height * 2) + i * 4), null, CurrentRightElement != emptySlotHUD ? Color.Lerp(Color.Black, Color.White, 0.3f) : Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);

                float ratio = 1.0f;

                if (CurrentLeftElement != emptySlotHUD)
                {
                    switch (_elementSystem.LeftSlotElement)
                    {
                        case Element.Fire:
                            ratio = Math.Abs(_elementSystem.LeftSlotEnergy / 100f);
                            break;

                        case Element.Thunder:
                            ratio = Math.Abs(_elementSystem.LeftSlotEnergy / 100f);
                            break;
                    }
                }

                if(CurrentLeftElement != null)
                    for (int i = 0; i < _elementSystem.LeftSlotLevel; i++)
                        spriteBatch.Draw(CurrentLeftElement, entity.transform.Position + Offset + new Vector2(0, i * (CurrentLeftElement.Height * 2) + i * 4), new Rectangle(0, 0, (int)Math.Round(CurrentLeftElement.Width * ratio), CurrentLeftElement.Height), Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer + 0.001f);

                ratio = 1.0f;
                if (CurrentRightElement != emptySlotHUD)
                {
                    switch (_elementSystem.RightSlotElement)
                    {
                        case Element.Fire:
                            ratio = Math.Abs(_elementSystem.RightSlotEnergy / 100f);
                            break;

                        case Element.Thunder:
                            ratio = Math.Abs(_elementSystem.RightSlotEnergy / 100f);
                            break;
                    }
                }

                if(CurrentRightElement != null)
                    for (int i = 0; i < _elementSystem.RightSlotLevel; i++)
                        spriteBatch.Draw(CurrentRightElement, entity.transform.Position + Offset + new Vector2(CurrentLeftElement.Width * entity.transform.Scale + 10, i * (CurrentRightElement.Height * 2) + i * 4), new Rectangle(0, 0, (int)Math.Round(CurrentRightElement.Width * ratio), CurrentLeftElement.Height), Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer + 0.001f);

                if(leftCharacterHUD != null)
                    spriteBatch.Draw(leftCharacterHUD, entity.transform.Position + Offset + new Vector2(CurrentLeftElement.Width / 2 * entity.transform.Scale - leftCharacterHUD.Width, _elementSystem.LeftSlotLevel * (CurrentLeftElement.Height * 2) + _elementSystem.LeftSlotLevel * 4), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                if(rightCharacterHUD != null)
                    spriteBatch.Draw(rightCharacterHUD, entity.transform.Position + Offset + new Vector2(CurrentLeftElement.Width * entity.transform.Scale + CurrentRightElement.Width / 2 * entity.transform.Scale + 10 - rightCharacterHUD.Width, _elementSystem.RightSlotLevel * (CurrentRightElement.Height * 2) + _elementSystem.RightSlotLevel * 4), null, Color.White, entity.transform.rotation, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
            }
           
            
            base.Draw(spriteBatch);
        }
    }
}
