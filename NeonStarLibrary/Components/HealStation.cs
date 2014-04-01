using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class HealStation : EnergyComponent
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _offAnimation = "";

        public string OffAnimation
        {
            get { return _offAnimation; }
            set { _offAnimation = value; }
        }

        private string _lightingAnimation = "";

        public string LightingAnimation
        {
            get { return _lightingAnimation; }
            set { _lightingAnimation = value; }
        }

        private string _onAnimation = "";

        public string OnAnimation
        {
            get { return _onAnimation; }
            set { _onAnimation = value; }
        }
        #endregion

        private Entity _avatarEntity;
        private AvatarCore _avatarComponent;
        private bool _used = false;

        public static List<string> _usedHealStations = new List<string>();

        private SpritesheetManager _baseSpritesheets;
        private SpritesheetManager _pillarSpritesheets;
        private SpritesheetManager _basePillarSpritesheets;
        private FadingSpritesheet _fadingSpritesheet;

        private bool _healing = false;

        public HealStation(Entity entity)
            :base(entity)
        {
            Name = "HealStation";
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatarEntity != null)
                _avatarComponent = _avatarEntity.GetComponent<AvatarCore>();
            if (_usedHealStations.Contains(entity.GameWorld.LevelGroupName + "_" + entity.GameWorld.LevelName + "_" + entity.Name))
                _used = true;

            _fadingSpritesheet = entity.GetComponent<FadingSpritesheet>();

            List<SpritesheetManager> sm = entity.GetComponentsByInheritance<SpritesheetManager>();
            if (sm != null && sm.Count >= 3)
            {
                foreach (SpritesheetManager ssm in sm)
                {
                    if (ssm.SpritesheetList.ContainsKey("BaseOff"))
                        _baseSpritesheets = ssm;
                    if (ssm.SpritesheetList.ContainsKey("PillarOff"))
                        _pillarSpritesheets = ssm;
                    if (ssm.SpritesheetList.ContainsKey("BasePillarOff"))
                        _basePillarSpritesheets = ssm;
                } 
            }

            base.Init();

            if (_baseSpritesheets != null && _pillarSpritesheets != null && _basePillarSpritesheets != null)
            {
                if (_powered)
                {
                    if (_used)
                    {
                        if (_fadingSpritesheet != null)
                            _fadingSpritesheet.Active = false;
                        _baseSpritesheets.ChangeAnimation("BaseOnDisabled", 0, true, false, true);
                        _basePillarSpritesheets.ChangeAnimation("BasePillarOnDisabled", 0, true, false, true);
                        _pillarSpritesheets.ChangeAnimation("PillarOn", 0, true, false, true);
                    }
                    else
                    {
                        if (_fadingSpritesheet != null)
                            _fadingSpritesheet.Active = true;
                        _baseSpritesheets.ChangeAnimation("BaseOn", 0, true, false, true);
                        _basePillarSpritesheets.ChangeAnimation("BasePillarOn", 0, true, false, true);
                        _pillarSpritesheets.ChangeAnimation("PillarOn", 0, true, false, true);

                    }
                }
                else
                {
                    if (_fadingSpritesheet != null)
                        _fadingSpritesheet.Active = false;
                    _baseSpritesheets.ChangeAnimation("BaseOff", 0, true, false, true);
                    _basePillarSpritesheets.ChangeAnimation("BasePillarOff", 0, true, false, true);
                    _pillarSpritesheets.ChangeAnimation("PillarOff", 0, true, false, true);
                }
            }  
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_avatarComponent != null && _healing)
            {
                _avatarComponent.CanAttack = false;
                _avatarComponent.CanMove = false;
                _avatarComponent.CanRoll = false;
                _avatarComponent.CanTurn = false;
                _avatarComponent.CanUseElement = false;
                _avatarEntity.hitboxes[0].SwitchType(HitboxType.Invincible, 1.0f);
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_powered && !_used)
            {
                if (_avatarEntity != null && _avatarComponent != null && _avatarEntity.hitboxes.Count > 0)
                {
                    if (_avatarComponent.CurrentHealthPoints < _avatarComponent.StartingHealthPoints)
                    {
                        if (_fadingSpritesheet != null)
                            _fadingSpritesheet.Active = true;
                    }
                    else
                    {
                        if (_fadingSpritesheet != null)
                            _fadingSpritesheet.Active = false;
                    }

                    if (_avatarEntity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    {
                        if (Neon.Input.Pressed(NeonStarInput.Interact) && _avatarComponent.CurrentHealthPoints < _avatarComponent.StartingHealthPoints)
                        {
                            

                            _healing = true;
                            if (_baseSpritesheets != null)
                            {
                                _baseSpritesheets.ChangeAnimation("BaseUse", 0, true, false, false);
                                _basePillarSpritesheets.ChangeAnimation("BasePillarUse", 0, true, false, false);
                                if (_fadingSpritesheet != null)
                                    _fadingSpritesheet.Active = false;
                            }
                            _used = true;
                        }
                    }
                }
            }
            else
            {
                if (_fadingSpritesheet != null)
                        _fadingSpritesheet.Active = false;
            }

            if (_healing)
            {
                if (_avatarEntity != null)
                    _avatarEntity.transform.Position = new Vector2(MathHelper.Lerp(_avatarEntity.transform.Position.X, this.entity.transform.Position.X, 0.1f), _avatarEntity.transform.Position.Y);
                if (_baseSpritesheets.IsFinished() && _basePillarSpritesheets.IsFinished())
                {
                    _avatarComponent.CurrentHealthPoints = _avatarComponent.StartingHealthPoints;
                    _usedHealStations.Add(entity.GameWorld.LevelGroupName + "_" + entity.GameWorld.LevelName + "_" + entity.Name);
                    _baseSpritesheets.ChangeAnimation("BaseOnDisabled", 0, true, false, true);
                    _basePillarSpritesheets.ChangeAnimation("BasePillarOnDisabled", 0, true, false, true);
                    _healing = false;
                    _used = true;
                }
            }

            if (_baseSpritesheets != null && _pillarSpritesheets != null)
            {              
                if (_powered)
                {
                    if (_used)
                    {
                        if (_baseSpritesheets.CurrentSpritesheetName == "BaseLighting" && _baseSpritesheets.CurrentSpritesheet.currentFrame == 1)
                        _baseSpritesheets.ChangeAnimation("BaseOff", 0, true, false, true);
                    }
                    else
                    { 
                        if (_baseSpritesheets.CurrentSpritesheetName == "BaseLighting" && _baseSpritesheets.CurrentSpritesheet.IsFinished)
                            _baseSpritesheets.ChangeAnimation("BaseOn", 0, true, false, true);
                    }
                    
                    if (_pillarSpritesheets.CurrentSpritesheetName == "PillarLighting" && _pillarSpritesheets.CurrentSpritesheet.IsFinished)
                        _pillarSpritesheets.ChangeAnimation("PillarOn", 0, true, false, true);
                }
                else
                {
                    if (_baseSpritesheets.CurrentSpritesheetName == "BaseLighting" && _baseSpritesheets.CurrentSpritesheet.currentFrame == 1)
                        _baseSpritesheets.ChangeAnimation("BaseOff", 0, true, false, true);
                    else if (_pillarSpritesheets.CurrentSpritesheetName == "PillarLighting" && _pillarSpritesheets.CurrentSpritesheet.currentFrame == 1)
                        _pillarSpritesheets.ChangeAnimation("PillarOff", 0, true, false, true);
                }
                
            }

            
            base.Update(gameTime);
        }

        public override void PowerDevice()
        {
            if (_baseSpritesheets != null && _pillarSpritesheets != null)
            {
                if (!_used)
                {
                    _baseSpritesheets.ChangeAnimation("BaseLighting", 0, true, false, false);
                    _basePillarSpritesheets.ChangeAnimation("BasePillarLighting", true, 0, true, true, false);
                }
                else
                {
                    _baseSpritesheets.ChangeAnimation("BaseDisabledOn", 0, true, true, false);
                    _basePillarSpritesheets.ChangeAnimation("BasePillarDisabledOn", true, 0, true, true, false);
                }

                _pillarSpritesheets.ChangeAnimation("PillarLighting", 0, true, false, false);
                if (_fadingSpritesheet != null)
                {
                    _fadingSpritesheet.Active = true;
                    _fadingSpritesheet.opacity = 0.0f;
                }
            }
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            if (_fadingSpritesheet != null)
            {
                _fadingSpritesheet.Active = false;
            }

            if (_baseSpritesheets != null && _pillarSpritesheets != null)
            {
                if (!_used)
                {
                    _baseSpritesheets.ChangeAnimation("BaseLighting", 0, true, false, false);
                    _baseSpritesheets.CurrentSpritesheet.currentFrame = _baseSpritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1;
                    _baseSpritesheets.CurrentSpritesheet.Reverse = true;
                    _basePillarSpritesheets.ChangeAnimation("BasePillarLighting", true, 0, true, false, false);
                    _basePillarSpritesheets.CurrentSpritesheet.currentFrame = _basePillarSpritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1;
                    _basePillarSpritesheets.CurrentSpritesheet.Reverse = true;
                }
                else
                {
                    _baseSpritesheets.ChangeAnimation("BaseDisabledOn", 0, true, false, false);
                    _baseSpritesheets.CurrentSpritesheet.currentFrame = _baseSpritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1;
                    _baseSpritesheets.CurrentSpritesheet.Reverse = true;
                    _basePillarSpritesheets.ChangeAnimation("BasePillarDisabledOn", 0, true, false, false);
                    _basePillarSpritesheets.CurrentSpritesheet.currentFrame = _basePillarSpritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1;
                    _basePillarSpritesheets.CurrentSpritesheet.Reverse = true;
                }
                
                _pillarSpritesheets.ChangeAnimation("PillarLighting", 0, true, false, false);
                _pillarSpritesheets.CurrentSpritesheet.currentFrame = _pillarSpritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1;
                _pillarSpritesheets.CurrentSpritesheet.Reverse = true;
            }
            base.UnpowerDevice();
        }

    }
}
