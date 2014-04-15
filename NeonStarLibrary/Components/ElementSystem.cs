using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
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

        public float LeftSlotEnergy = 100.0f;

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

        public float RightSlotEnergy = 100.0f;      

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

        private float _energyRegenerationRate = 20.0f;
        public float EnergyRegenerationRate
        {
            get { return _energyRegenerationRate; }
            set { _energyRegenerationRate = value; }
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
                    if (LeftSlotEnergy < 100.0f)
                    {
                        LeftSlotEnergy += (float)gameTime.ElapsedGameTime.TotalSeconds * EnergyRegenerationRate;
                        if (LeftSlotEnergy > 100.0f)
                            LeftSlotEnergy = 100.0f;
                    }

                    if (RightSlotEnergy < 100.0f)
                    {
                        RightSlotEnergy += (float)gameTime.ElapsedGameTime.TotalSeconds * EnergyRegenerationRate;
                        if (RightSlotEnergy > 100.0f)
                            RightSlotEnergy = 100.0f;
                    }
                }
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
                        if (_leftSlotElement != Element.Neutral)
                        {
                            Console.WriteLine("Use Element -> " + LeftSlotElement);
                            UseElement(_leftSlotElement, (int)_leftSlotLevel, NeonStarInput.UseLeftSlotElement);
                        }
                    }
                    else if (Neon.Input.Pressed(NeonStarInput.UseRightSlotElement))
                    {
                        if (_rightSlotElement != Element.Neutral)
                        {
                            Console.WriteLine("Use Element -> " + RightSlotElement);
                            UseElement(_rightSlotElement, (int)_rightSlotLevel, NeonStarInput.UseRightSlotElement);
                        }
                    }

                   /* if (Neon.Input.Pressed(NeonStarInput.DropLeftSlotElement))
                    {
                        if (_leftSlotElement != Element.Neutral)
                            DropElement(Side.Left);
                    }
                    if (Neon.Input.Pressed(NeonStarInput.DropRightSlotElement))
                    {
                        if (_rightSlotElement != Element.Neutral)
                            DropElement(Side.Right);
                    }*/
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

        public void UseElement(Element element, int level, NeonStarInput input)
        {
            switch (element)
            {
                case Element.Fire:
                    float fireGaugeCost = 0.0f;
                    switch (level)
                    {
                        case 1:
                            fireGaugeCost = (float)ElementManager.FireParameters[1][0];
                            break;

                        case 2:
                            fireGaugeCost = (float)ElementManager.FireParameters[2][0];
                            break;

                        case 3:
                            fireGaugeCost = (float)ElementManager.FireParameters[3][0];
                            break;
                    }

                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            if (fireGaugeCost <= LeftSlotEnergy)
                                CurrentElementEffect = new Fire(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            if (fireGaugeCost <= RightSlotEnergy)
                                CurrentElementEffect = new Fire(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;
                    }

                    break;

                case Element.Thunder:
                    float thunderGaugeCost = 0.0f;
                    switch (level)
                    {
                        case 1:
                            thunderGaugeCost = (float)ElementManager.ThunderParameters[0][0];
                            break;

                        case 2:
                            thunderGaugeCost = (float)ElementManager.ThunderParameters[1][0];
                            break;

                        case 3:
                            thunderGaugeCost = (float)ElementManager.ThunderParameters[2][0];
                            break;
                    }

                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            if (thunderGaugeCost <= LeftSlotEnergy)
                                CurrentElementEffect = new Thunder(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            if(thunderGaugeCost <= RightSlotEnergy)
                                CurrentElementEffect = new Thunder(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;
                    }

                    break;

                case Element.Wind:
                    float windGaugeCost = 30.0f;                  
                    switch (input)
                    {
                        case NeonStarInput.UseLeftSlotElement:
                            if (windGaugeCost <= LeftSlotEnergy)
                                CurrentElementEffect = new Wind(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;

                        case NeonStarInput.UseRightSlotElement:
                            if(windGaugeCost <= RightSlotEnergy)
                                CurrentElementEffect = new Wind(this, level, entity, input, (GameScreen)entity.GameWorld);
                            else
                                Console.WriteLine("Not enough energy");
                            break;
                    }
                    break;
            }
        }

        public void DropElement(Side side)
        {
            if (side == Side.Left)
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
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackFireGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), 2.0f, entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontFireGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.Red;
                        break;

                    case Element.Thunder:
                        _currentAnimatedSpecialEffect = EffectsManager.GetEffect(BackThunderGatheringFX, AvatarComponent.CurrentSide, entity.transform.Position, 0.0f, new Vector2(10, -15), 2.0f, entity.spritesheets.Layer + 0.01f);
                        //EffectsManager.GetEffect(FrontThunderGatheringFX, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, entity.spritesheets.Layer + 0.01f);
                        _nextColorToTint = Color.FromNonPremultiplied(255, 230, 100, 255);
                        break;
                }
        }
    }
}
