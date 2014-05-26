using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.HUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Avatar
{
    public enum Element
    {
        Neutral,
        Fire,
        Thunder,
        Wind
    }

    public class ElementSlot
    {
        public Element Type;
        public float Cooldown;
    }

    public class ElementSystem : Component
    {
        #region Properties
        private string _fireLaunchAnimationLevelOne = "";

        public string FireLaunchAnimationLevelOne
        {
            get { return _fireLaunchAnimationLevelOne; }
            set { _fireLaunchAnimationLevelOne = value; }
        }

        private string _fireLaunchLoopAnimationLevelOne = "";

        public string FireLaunchLoopAnimationLevelOne
        {
            get { return _fireLaunchLoopAnimationLevelOne; }
            set { _fireLaunchLoopAnimationLevelOne = value; }
        }

        private string _fireReleaseAnimationLevelOne = "";

        public string FireReleaseAnimationLevelOne
        {
            get { return _fireReleaseAnimationLevelOne; }
            set { _fireReleaseAnimationLevelOne = value; }
        }

        private string _fireLaunchAnimationLevelTwo = "";

        public string FireLaunchAnimationLevelTwo
        {
            get { return _fireLaunchAnimationLevelTwo; }
            set { _fireLaunchAnimationLevelTwo = value; }
        }

        private string _fireLaunchLoopAnimationLevelTwo = "";

        public string FireLaunchLoopAnimationLevelTwo
        {
            get { return _fireLaunchLoopAnimationLevelTwo; }
            set { _fireLaunchLoopAnimationLevelTwo = value; }
        }

        private string _fireReleaseAnimationLevelTwo = "";

        public string FireReleaseAnimationLevelTwo
        {
            get { return _fireReleaseAnimationLevelTwo; }
            set { _fireReleaseAnimationLevelTwo = value; }
        }

        private string _fireLaunchAnimationLevelThree = "";

        public string FireLaunchAnimationLevelThree
        {
            get { return _fireLaunchAnimationLevelThree; }
            set { _fireLaunchAnimationLevelThree = value; }
        }

        private string _fireLaunchLoopAnimationLevelThree = "";

        public string FireLaunchLoopAnimationLevelThree
        {
            get { return _fireLaunchLoopAnimationLevelThree; }
            set { _fireLaunchLoopAnimationLevelThree = value; }
        }

        private string _fireReleaseAnimationLevelThree = "";

        public string FireReleaseAnimationLevelThree
        {
            get { return _fireReleaseAnimationLevelThree; }
            set { _fireReleaseAnimationLevelThree = value; }
        }

        private string _thunderLaunchAnimation = "";

        public string ThunderLaunchAnimation
        {
            get { return _thunderLaunchAnimation; }
            set { _thunderLaunchAnimation = value; }
        }

        private string _windStartAnimation = "";

        public string WindStartAnimation
        {
            get { return _windStartAnimation; }
            set { _windStartAnimation = value; }
        }

        private string _windImpulseAnimation = "";

        public string WindImpulseAnimation
        {
            get { return _windImpulseAnimation; }
            set { _windImpulseAnimation = value; }
        }

        public Element LeftSlotElementFirst
        {
            get 
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 1)
                        return ElementSlots[0][0].Type;
                }
                return Element.Neutral;
            }
            set 
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 1)
                        ElementSlots[0][0].Type = value;
                } 
            }
        }

        public Element LeftSlotElementSecond
        {
            get
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 2)
                        return ElementSlots[0][1].Type;
                }
                return Element.Neutral;
            }
            set
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 2)
                        ElementSlots[0][1].Type = value;
                }
            }
        }

        public Element LeftSlotElementThird
        {
            get
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 3)
                        return ElementSlots[0][2].Type;
                }
                return Element.Neutral;
            }
            set
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[0].Length >= 3)
                        ElementSlots[0][2].Type = value;
                }
            }
        }

        public Element RightSlotElementFirst
        {
            get
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 1)
                        return ElementSlots[1][0].Type;
                }
                return Element.Neutral;
            }
            set
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 1)
                        ElementSlots[1][0].Type = value;
                }
            }
        }

        public Element RightSlotElementSecond
        {
            get
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 2)
                        return ElementSlots[1][1].Type;
                }
                return Element.Neutral;
            }
            set
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 2)
                        ElementSlots[1][1].Type = value;
                }
            }
        }

        public Element RightSlotElementThird
        {
            get
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 3)
                        return ElementSlots[1][2].Type;
                }
                return Element.Neutral;
            }
            set
            {
                if (ElementSlots != null && ElementSlots.Length == 2)
                {
                    if (ElementSlots[1].Length >= 3)
                        ElementSlots[1][2].Type = value;
                }
            }
        } 

        private float _maxLevel = 1.0f;

        public float MaxLevel
        {
            get { return _maxLevel; }
            set 
            { 
                ChangeMaxLevel((int)value);
            }
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

        private float _elementSlotCooldownDuration;

        public float ElementSlotCooldownDuration
        {
            get { return _elementSlotCooldownDuration; }
            set { _elementSlotCooldownDuration = value; }
        }

        private float _idleCooldownThreshold = 1.0f;

        public float IdleCooldownThreshold
        {
            get { return _idleCooldownThreshold; }
            set { _idleCooldownThreshold = value; }
        }

        private float _idleCooldownRatio = 3.0f;

        public float IdleCooldownRatio
        {
            get { return _idleCooldownRatio; }
            set { _idleCooldownRatio = value; }
        }
        #endregion     

        public AvatarCore AvatarComponent = null;

        public ElementEffect CurrentElementEffect = null;

        private SpriteSheetInfo FrontFireGatheringFX = null;
        private SpriteSheetInfo BackFireGatheringFX = null;

        private SpriteSheetInfo FrontThunderGatheringFX = null;
        private SpriteSheetInfo BackThunderGatheringFX = null;

        private AnimatedSpecialEffect _currentAnimatedSpecialEffect = null;
        private bool _getElementColored = false;
        private float _getElementColorTimer = 0.0f;
        private Color _nextColorToTint;

        public ElementSlot[][] ElementSlots;

        private ElementHUD _hud;

        public ElementSystem(Entity entity)
            :base(entity, "ElementSystem")
        {
        }

        public override void Init()
        {
            Fire.FirePlatforms.Clear();
            FrontFireGatheringFX = AssetManager.GetSpriteSheet(_fireGatheringFX);
            BackFireGatheringFX = AssetManager.GetSpriteSheet(_fireGatheringFX);
            FrontThunderGatheringFX = AssetManager.GetSpriteSheet(_thunderGatheringFX);
            BackThunderGatheringFX = AssetManager.GetSpriteSheet(_thunderGatheringFX);
            AvatarComponent = entity.GetComponent<AvatarCore>();
            ChangeMaxLevel((int)_maxLevel);

            Entity e = entity.GameWorld.GetEntityByName("HUD");
            if (e != null)
                _hud = e.GetComponent<ElementHUD>();
            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = Fire.FirePlatforms.Count - 1; i >= 0; i--)
            {
                Rigidbody rg = Fire.FirePlatforms[i];
                if (!entity.GameWorld.PhysicWorld.BodyList.Contains(rg.body))
                    Fire.FirePlatforms.Remove(rg);
            }
                
            if (AvatarComponent.State != AvatarState.Dying && AvatarComponent.State != AvatarState.FastRespawning)
            {
                if (CurrentElementEffect != null)
                {
                    AvatarComponent.State = AvatarState.UsingElement;
                    CurrentElementEffect.PreUpdate(gameTime);
                    AvatarComponent.CanUseElement = false;
                }
                else
                {
                    for (int i = 0; i < ElementSlots.Length; i++)
                    {
                        ElementSlot higherCooldown = null;
                        
                        int highestCooldownRow = int.MinValue;

                        for (int j = 0; j < ElementSlots[i].Length; j++)
                        {
                            if (ElementSlots[i][j].Type == Element.Neutral)
                                ElementSlots[i][j].Cooldown = 0.0f;
                            else if (ElementSlots[i][j].Cooldown > 0.0f)
                            {
                                if(higherCooldown == null || higherCooldown.Cooldown > ElementSlots[i][j].Cooldown)
                                {
                                    higherCooldown = ElementSlots[i][j];
                                    highestCooldownRow = j;
                                }
                                if (ElementSlots[i][j].Cooldown < 0.0f)
                                {
                                    ElementSlots[i][j].Cooldown = 0.0f;
                                }
                            }
                        }
                        if (higherCooldown != null)
                        {

                            higherCooldown.Cooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds * (AvatarComponent.IdleTimer >= _idleCooldownThreshold ? _idleCooldownRatio : 1);
                            if (higherCooldown.Cooldown <= 0.0f)
                            {
                                higherCooldown.Cooldown = 0.0f;
                                _hud.CooldownFinished(highestCooldownRow, i);
                            }
                        }



                    }

                }

                ElementSlots[0] = ElementSlots[0].OrderBy(e => e.Cooldown).ToArray();
                ElementSlots[0] = ElementSlots[0].OrderByDescending(e => e.Type).ToArray();

                ElementSlots[1] = ElementSlots[1].OrderBy(e => e.Cooldown).ToArray();
                ElementSlots[1] = ElementSlots[1].OrderByDescending(e => e.Type).ToArray();
            }             
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AvatarComponent.State != AvatarState.Dying && AvatarComponent.State != AvatarState.FastRespawning)
            {
                if (AvatarComponent.CanAttack && AvatarComponent.CanMove && AvatarComponent.CanTurn && AvatarComponent.CanUseElement)
                {
                    if (Neon.Input.Pressed(NeonStarInput.UseLeftSlotElement))
                    {
                        for (int i = ElementSlots[0].Length - 1; i >= 0; i--)
                        {
                            if (ElementSlots[0][i].Type != Element.Neutral && ElementSlots[0][i].Cooldown <= 0.0f)
                            {
                                Console.WriteLine("Use Element -> " + ElementSlots[0][i].Type.ToString() + " at position: " + i + ".");
                                UseElement(ElementSlots[0][i], NeonStarInput.UseLeftSlotElement);
                                break;
                            }
                        }
                    }
                    else if (Neon.Input.Pressed(NeonStarInput.UseRightSlotElement))
                    {
                        for(int i = ElementSlots[1].Length - 1; i >= 0; i --)
                        {
                            if(ElementSlots[1][i].Type != Element.Neutral && ElementSlots[1][i].Cooldown <= 0.0f)
                            {
                                Console.WriteLine("Use Element -> " + ElementSlots[1][i].Type.ToString() + " at position: " + i + ".");
                                UseElement(ElementSlots[1][i], NeonStarInput.UseRightSlotElement);
                                break;
                            }
                        }
                    }
                }
                else if (CurrentElementEffect != null)
                {
                   CurrentElementEffect.Update(gameTime);
                }
            }          
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AvatarComponent.State != AvatarState.Dying && AvatarComponent.State != AvatarState.FastRespawning)
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
                    _currentAnimatedSpecialEffect.transform.Position = entity.transform.Position + new Vector2((AvatarComponent.CurrentSide == Side.Right ? 10 : -10), -15);
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
            }
            
            base.PostUpdate(gameTime);
        }

        public void UseElement(ElementSlot elementSlot, NeonStarInput input)
        {
            switch (elementSlot.Type)
            {
                case Element.Fire:
                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                                CurrentElementEffect = new Fire(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;

                        case NeonStarInput.UseRightSlotElement:
                                CurrentElementEffect = new Fire(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;
                    }

                    break;

                case Element.Thunder:
                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                                CurrentElementEffect = new Thunder(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            CurrentElementEffect = new Thunder(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;
                    }

                    break;

                case Element.Wind:                 
                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                                CurrentElementEffect = new Wind(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;

                        case NeonStarInput.UseRightSlotElement:
                                CurrentElementEffect = new Wind(this, elementSlot, entity, input, (GameScreen)entity.GameWorld);
                            break;
                    }
                    break;
            }
        }

        public void DropElement(Side side)
        {
            /*if (side == Side.Left)
            {
                if (_leftSlotElement != Element.Neutral)
                {
                    Console.WriteLine("Drop Element -> " + LeftSlotElement);
                    switch (_leftSlotElement)
                    {
                        case Element.Thunder:
                            switch (_leftSlotLevel.ToString())
                            {
                                case "1":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[3][1] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[3][2] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    break;

                                case "2":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[3][1] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[3][2] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    break;

                                case "3":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[3][1] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[3][2] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    break;
                            }
                            break;

                        case Element.Fire:
                            switch (_leftSlotLevel.ToString())
                            {
                                case "1":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][1]);
                                    break;

                                case "2":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][2]);
                                    break;

                                case "3":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][3]);
                                    break;
                            }
                            break;
                    }

                    _leftSlotElement = Element.Neutral;
                    _leftSlotLevel = 1;
                }
            }
            else
            {
                if (_rightSlotElement != Element.Neutral)
                {
                    Console.WriteLine("Drop Element -> " + RightSlotElement);
                    switch (_rightSlotElement)
                    {
                        case Element.Thunder:
                            switch (_rightSlotLevel.ToString())
                            {
                                case "1":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[3][1] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[3][2] / 100f, (float)ElementManager.ThunderParameters[3][0]);
                                    break;

                                case "2":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[4][1] / 100f, (float)ElementManager.ThunderParameters[4][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[4][2] / 100f, (float)ElementManager.ThunderParameters[4][0]);
                                    break;

                                case "3":
                                    AvatarComponent.ThirdPersonController.BoostMovementSpeed((float)ElementManager.ThunderParameters[5][1] / 100f, (float)ElementManager.ThunderParameters[5][0]);
                                    AvatarComponent.MeleeFight.BoostAttackSpeed((float)ElementManager.ThunderParameters[5][2] / 100f, (float)ElementManager.ThunderParameters[5][0]);
                                    break;
                            }
                            break;

                        case Element.Fire:
                            switch (_rightSlotLevel.ToString())
                            {
                                case "1":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][1]);
                                    break;

                                case "2":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][2]);
                                    break;

                                case "3":
                                    AvatarComponent.MeleeFight.BoostDamage((float)ElementManager.FireParameters[4][0] / 100f, (float)ElementManager.FireParameters[4][3]);
                                    break;
                            }
                            break;
                    }
                    _rightSlotElement = Element.Neutral;
                    _rightSlotLevel = 1;
                }
            }*/
            
        }

        public void GetElement(Element element)
        {
            bool checkRightSlot = false;

            for (int i = 0; i < ElementSlots[0].Length; i++)
            {
                if (ElementSlots[0][i].Type == Element.Neutral)
                {
                    ElementSlots[0][i].Type = element;
                    ElementSlots[0][i].Cooldown = 0.0f;
                    ElementFeedback(element);
                    if (_hud != null)
                        _hud.CooldownFinished(i, 0);
                    return;
                }
                else if (ElementSlots[0][i].Type != element)
                {
                    checkRightSlot = true;
                    break;
                }
            }

            if (checkRightSlot)
            {
                for (int i = 0; i < ElementSlots[1].Length; i++)
                {
                    if (ElementSlots[1][i].Type == Element.Neutral)
                    {
                        ElementSlots[1][i].Type = element;
                        ElementSlots[1][i].Cooldown = 0.0f;
                        ElementFeedback(element);
                        if (_hud != null)
                            _hud.CooldownFinished(i, 1);
                        return;
                    }
                    else if (ElementSlots[1][i].Type != element)
                        break;
                }
            }           
        }

        private void ElementFeedback(Element element)
        {
            switch(element)
                {
                    case Element.Fire:
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackFireGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), 2.0f, entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontFireGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.FromNonPremultiplied(220,30,255,255);
                        break;

                    case Element.Thunder:
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackThunderGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), 2.0f, entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontThunderGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.FromNonPremultiplied(255, 230, 100, 255);
                        break;
                }
        }

        public void ChangeMaxLevel(int newMaxLevel)
        {
            _maxLevel = newMaxLevel;

            ElementSlot[] leftSlotElements = null;
            ElementSlot[] rightSlotElements = null;

            if (ElementSlots != null && ElementSlots.Length == 2)
            {
                leftSlotElements = ElementSlots[0];
                rightSlotElements = ElementSlots[1];
            }
            ElementSlots = new ElementSlot[2][];
            ElementSlots[0] = new ElementSlot[(int)MaxLevel];
            ElementSlots[1] = new ElementSlot[(int)MaxLevel];
            for (int i = 0; i < MaxLevel; i++)
            {
                ElementSlots[0][i] = new ElementSlot();
                ElementSlots[1][i] = new ElementSlot();
            }

            if (leftSlotElements != null)
            {
                for (int i = 0; i < leftSlotElements.Length; i++)
                {
                    if (ElementSlots[0].Length > i)
                    {
                        ElementSlots[0][i] = leftSlotElements[i];
                        ElementSlots[1][i] = rightSlotElements[i];
                    }
                }
            }

            if(_hud != null)
                _hud.UpgradeMaxLevel((int)MaxLevel);
        }
    }
}
