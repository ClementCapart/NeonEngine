using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Triggers;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using NeonStarLibrary.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class SaveRoom : Component
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private float _spawnPointIndex = 0.0f;

        public float SpawnPointIndex
        {
            get { return _spawnPointIndex; }
            set { _spawnPointIndex = value; }
        }

        private string _leftColumnName = "LeftColumnToLight";

        public string LeftColumnName
        {
            get { return _leftColumnName; }
            set { _leftColumnName = value; }
        }

        private string _rightColumnName = "RightColumnToLight";

        public string RightColumnName
        {
            get { return _rightColumnName; }
            set { _rightColumnName = value; }
        }

        private string _backgroundName = "BackgroundToLight";

        public string BackgroundName
        {
            get { return _backgroundName; }
            set { _backgroundName = value; }
        }

        private string _cabinName = "SaveCabin";

        public string CabinName
        {
            get { return _cabinName; }
            set { _cabinName = value; }
        }

        private string _leftLampName = "LeftLamp";

        public string LeftLampName
        {
            get { return _leftLampName; }
            set { _leftLampName = value; }
        }

        private string _rightLampName = "RightLamp";

        public string RightLampName
        {
            get { return _rightLampName; }
            set { _rightLampName = value; }
        }

        private string _groundName = "Ground";

        public string GroundName
        {
            get { return _groundName; }
            set { _groundName = value; }
        }

        private string _pipesName = "Pipes";

        public string PipesName
        {
            get { return _pipesName; }
            set { _pipesName = value; }
        }

        private string _lightingName = "RoomLighting";

        public string LightingName
        {
            get { return _lightingName; }
            set { _lightingName = value; }
        }

        private string _leftScreensName = "ScreensLeft";

        public string LeftScreensName
        {
            get { return _leftScreensName; }
            set { _leftScreensName = value; }
        }

        private string _rightScreensName = "ScreensRight";

        public string RightScreensName
        {
            get { return _rightScreensName; }
            set { _rightScreensName = value; }
        }

        private string _ceilingFade = "CeilingFade";

        public string CeilingFade
        {
            get { return _ceilingFade; }
            set { _ceilingFade = value; }
        }

        private float _waitAfterActivationDuration = 1.0f;

        public float WaitAfterActivationDuration
        {
            get { return _waitAfterActivationDuration; }
            set { _waitAfterActivationDuration = value; }
        }

        private float _saveDuration = 2.0f;

        public float SaveDuration
        {
            get { return _saveDuration; }
            set { _saveDuration = value; }
        }


        #endregion

        public bool Active = true;
        private Entity _avatar;
        private AvatarCore _avatarComponent;

        private Entity _leftColumn;
        private Entity _rightColumn;

        private Entity _background;
        private Entity _cabin;

        private Entity _leftLamp;
        private Entity _rightLamp;

        private Entity _pipes;

        private Entity _ground;

        private SpriteSheet _roomLighting;

        private Entity _leftScreens;
        private Entity _rightScreens;

        private SpriteSheet _ceiling;

        private bool _startSave = false;
        private bool _finishSave = false;
        private bool _finishingSave = false;
        private bool _startActualSave = false;
        private bool _finishedSave = false;
        private float _activationTimer = 0.0f;
        private float _saveTimer = 0.0f;

        private FadingSpritesheet _yButton;

        public SaveRoom(Entity entity)
            :base(entity, "CheckPoint")
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            _yButton = entity.GetComponent<FadingSpritesheet>();
            _avatar = entity.GameWorld.GetEntityByName(_avatarName);

            Entity e = entity.GameWorld.GetEntityByName(_lightingName);
            if (e != null)
                _roomLighting = e.GetComponent<SpriteSheet>();
            _leftScreens = entity.GameWorld.GetEntityByName(_leftScreensName);
            _rightScreens = entity.GameWorld.GetEntityByName(_rightScreensName);

            e = null;
            e = entity.GameWorld.GetEntityByName(_ceilingFade);
            if (e != null)
                _ceiling = e.GetComponent<SpriteSheet>();
            if (_ceiling != null)
            {
                _ceiling.Active = false;
                _ceiling.IsLooped = false;
                _ceiling.isPlaying = false;
            }

            if (_roomLighting != null)
                _roomLighting.Active = false;
            if (_avatar != null)
                _avatarComponent = _avatar.GetComponent<AvatarCore>();

            _leftColumn = entity.GameWorld.GetEntityByName(_leftColumnName);
            if (_leftColumn != null && _leftColumn.spritesheets != null)
                _leftColumn.spritesheets.ChangeAnimation("Off");
            _rightColumn = entity.GameWorld.GetEntityByName(_rightColumnName);
            if (_rightColumn != null && _rightColumn.spritesheets != null)
                _rightColumn.spritesheets.ChangeAnimation("Off");
            _background = entity.GameWorld.GetEntityByName(_backgroundName);
            if (_background != null && _background.spritesheets != null)
                _background.spritesheets.ChangeAnimation("Off");
            _cabin = entity.GameWorld.GetEntityByName(_cabinName);
            if (_cabin != null && _cabin.spritesheets != null)
                _cabin.spritesheets.ChangeAnimation("Off");

            _leftLamp = entity.GameWorld.GetEntityByName(_leftLampName);
            if (_leftLamp != null && _leftLamp.spritesheets != null)
                _leftLamp.spritesheets.ChangeAnimation("Off");

            _rightLamp = entity.GameWorld.GetEntityByName(_rightLampName);
            if (_rightLamp != null && _rightLamp.spritesheets != null)
                _rightLamp.spritesheets.ChangeAnimation("Off");

            _ground = entity.GameWorld.GetEntityByName(_groundName);
            if (_ground != null && _ground.spritesheets != null)
                _ground.spritesheets.ChangeAnimation("Off");

            _pipes = entity.GameWorld.GetEntityByName(_pipesName);
            if (_pipes != null && _pipes.spritesheets != null)
                _pipes.spritesheets.ChangeAnimation("Off");

            base.Init();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {

            if (_leftLamp != null && _leftLamp.spritesheets.CurrentSpritesheetName == "Lighting" && _leftLamp.spritesheets.IsFinished())
            {
                _leftLamp.spritesheets.ChangeAnimation("On");
                _leftLamp.spritesheets.CurrentSpritesheet.DelayBeforeLoop = true;
                _leftLamp.spritesheets.CurrentSpritesheet.InvisibleDuringDelay = false;
                _leftLamp.spritesheets.CurrentSpritesheet.DelayBeforeLoopAgain = 0.5f;
            }
            if (_rightLamp != null && _rightLamp.spritesheets.CurrentSpritesheetName == "Lighting" && _rightLamp.spritesheets.IsFinished())
            {
                
                _rightLamp.spritesheets.ChangeAnimation("On");
                _rightLamp.spritesheets.CurrentSpritesheet.InvisibleDuringDelay = false;
                _rightLamp.spritesheets.CurrentSpritesheet.DelayBeforeLoop = true;
                _rightLamp.spritesheets.CurrentSpritesheet.DelayBeforeLoopAgain = 0.5f;
            }
            if (!_finishedSave)
            {
                if (_startSave)
                {
                    if (_avatarComponent != null)
                    {
                        _avatarComponent.CanMove = false;
                        _avatarComponent.CanTurn = false;
                        _avatarComponent.CanRoll = false;
                        _avatarComponent.CanAttack = false;
                        _avatarComponent.CanUseElement = false;
                    }
                }
                if (_startSave && !_startActualSave && !_finishSave)
                {

                    if (_background.spritesheets.CurrentSpritesheet.currentFrame == 11)
                    {
                        if (_leftScreens != null)
                        {
                            _leftScreens.spritesheets.ChangeAnimation("Fade", 0, true, false, false);
                        }
                        if (_rightScreens != null)
                        {
                            _rightScreens.spritesheets.ChangeAnimation("Fade", 0, true, false, false);
                        }

                        if (_ceiling != null)
                        {
                            _ceiling.Active = true;
                            _ceiling.isPlaying = true;
                        }
                    }

                    if (_activationTimer >= _waitAfterActivationDuration)
                    {
                        if (_cabin != null && _cabin.spritesheets != null && _cabin.spritesheets.CurrentSpritesheetName != "StartOpening")
                            _cabin.spritesheets.ChangeAnimation("StartOpening", 0, true, false, false);

                        
                        else if (_cabin != null && _cabin.spritesheets != null && _cabin.spritesheets.CurrentSpritesheetName == "StartOpening" && _cabin.spritesheets.IsFinished())
                        {
                            _startActualSave = true;
                        }
                    }
                    else
                    {
                        _activationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (_startActualSave && !_finishSave)
                {
                    _avatarComponent.State = AvatarState.Saving;
                    if (_avatar != null)
                        _avatar.transform.Position = new Vector2(MathHelper.Lerp(_avatar.transform.Position.X, this.entity.transform.Position.X, 0.1f), _avatar.transform.Position.Y);

                    if (_avatar != null && _avatar.spritesheets != null && _avatar.spritesheets.CurrentSpritesheet.opacity <= 0.0f)
                    {
                        SaveCheckPoint();
                        if (_cabin != null && _cabin.spritesheets != null)
                            _cabin.spritesheets.ChangeAnimation("StartClosing", 0, true, false, false);
                    }
                    if (_cabin != null && _cabin.spritesheets != null)
                    {
                        if (_cabin.spritesheets.CurrentSpritesheetName == "StartClosing" && _cabin.spritesheets.IsFinished())
                        {
                            _cabin.spritesheets.ChangeAnimation("Scanning", 0, true, false, false);
                            if (_pipes != null && _pipes.spritesheets != null)
                                _pipes.spritesheets.ChangeAnimation("Lighting", 0, true, false, true);

                            if (_leftScreens != null)
                            {
                                _leftScreens.spritesheets.ChangeAnimation("Load", 0, true, false, true);
                            }
                            if (_rightScreens != null)
                            {
                                _rightScreens.spritesheets.ChangeAnimation("Load", 0, true, false, true);
                            }
                            _finishSave = true;
                        }
                    }
                }

                if (_finishSave && !_finishingSave)
                {
                    if (_cabin != null && _cabin.spritesheets != null && _cabin.spritesheets.CurrentSpritesheetName == "Scanning" && _cabin.spritesheets.CurrentSpritesheet.IsFinished)
                    {
                        if (_cabin != null && _cabin.spritesheets != null)
                            _cabin.spritesheets.ChangeAnimation("EndOpening", 0, true, false, false);

                        if (_leftScreens != null)
                        {
                            _leftScreens.spritesheets.ChangeAnimation("Fade", 0, true, false, true);
                            _leftScreens.spritesheets.CurrentSpritesheet.spriteSheetInfo.Fps = 8;
                            _leftScreens.spritesheets.CurrentSpritesheet.Init();
                            _leftScreens.spritesheets.CurrentSpritesheet.ReverseLoop = true;
                        }
                        if (_rightScreens != null)
                        {
                            _rightScreens.spritesheets.ChangeAnimation("Fade", 0, true, false, true);
                            _rightScreens.spritesheets.CurrentSpritesheet.spriteSheetInfo.Fps = 8;
                            _rightScreens.spritesheets.CurrentSpritesheet.Init();
                            _rightScreens.spritesheets.CurrentSpritesheet.ReverseLoop = true;
                        }
                            

                        if (_pipes != null && _pipes.spritesheets != null)
                            _pipes.spritesheets.ChangeAnimation("Off", 0, true, false, true);
                        _finishingSave = true;
                    }
                    else
                    {
                        _saveTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (_finishingSave)
                {
                    if (_cabin != null && _cabin.spritesheets != null)
                    {
                        if (_cabin.spritesheets.CurrentSpritesheetName == "EndOpening" && _cabin.spritesheets.IsFinished())
                        {
                            if (_avatarComponent != null && _avatarComponent.State != AvatarState.FinishSaving)
                            {
                                _avatar.spritesheets.CurrentSpritesheet.opacity = 0.0f;
                                _avatarComponent.State = AvatarState.FinishSaving;
                            }

                        }
                    }

                    if (_avatarComponent != null && _avatarComponent.State == AvatarState.FinishSaving && _avatar != null && _avatar.spritesheets.CurrentSpritesheet.opacity >= 1.0f)
                    {
                        _avatar.spritesheets.CurrentSpritesheet.opacity = 1.0f;
                        if (_cabin != null && _cabin.spritesheets != null)
                            _cabin.spritesheets.ChangeAnimation("EndClosing", 0, true, false, false);
                        if (_roomLighting != null)
                            _roomLighting.Active = true;
                        _finishedSave = true;
                        _avatarComponent.State = AvatarState.Idle;

                    }
                }
            }
            
            base.PreUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!_finishedSave && !_startSave)
            {
                if (_avatar.hitboxes.Count > 0 && this.entity.hitboxes.Count > 0)
                {
                    if (_avatar.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    {
                        if (Neon.Input.Pressed(NeonStarInput.Interact) && _avatar.rigidbody != null && _avatar.rigidbody.isGrounded)
                        {
                            if (_leftColumn != null && _leftColumn.spritesheets != null)
                                _leftColumn.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
                            if (_rightColumn != null && _rightColumn.spritesheets != null)
                                _rightColumn.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
                            if (_background != null && _background.spritesheets != null)
                                _background.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);

                            if (_leftLamp != null && _leftLamp.spritesheets != null)
                                _leftLamp.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
                            if (_rightLamp != null && _rightLamp.spritesheets != null)
                                _rightLamp.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
                            if (_ground != null && _ground.spritesheets != null)
                                _ground.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
                            if (_yButton != null)
                                _yButton.Active = false;

                            

                            _avatarComponent.State = AvatarState.Idle;
                            _avatarComponent.CanMove = false;
                            _avatarComponent.CanTurn = false;
                            _avatarComponent.CanRoll = false;
                            _avatarComponent.CanAttack = false;
                            _avatarComponent.CanUseElement = false;

                            _startSave = true;
                        }
                    }
                }
            }           
                
            base.Update(gameTime);
        }

        private void SaveCheckPoint()
        {
            GameScreen.CheckPointsData.Add((entity.GameWorld as GameScreen).SaveStatus(this));
            if (_avatarComponent != null)
                _avatarComponent.CurrentHealthPoints = _avatarComponent.StartingHealthPoints;
            HealStation._usedHealStations.Clear();        
        }




    }
}
