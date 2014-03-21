using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary.Components.Avatar;

namespace NeonStarLibrary.Components.Scripts
{
    public class EntranceScanScript : ScriptComponent
    {
        private float _scanningDuration;

        public float ScanningDuration
        {
            get { return _scanningDuration; }
            set { _scanningDuration = value; }
        }

        private string _scannerName = "";

        public string ScannerName
        {
            get { return _scannerName; }
            set { _scannerName = value; }
        }
        private string _scannerCommandName = "";

        public string ScannerCommandName
        {
            get { return _scannerCommandName; }
            set { _scannerCommandName = value; }
        }

        private string _brokenTurretName = "";

        public string BrokenTurretName
        {
            get { return _brokenTurretName; }
            set { _brokenTurretName = value; }
        }

        private string _turretPlatformName = "";

        public string TurretPlatformName
        {
            get { return _turretPlatformName; }
            set { _turretPlatformName = value; }
        }

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private Entity _scannerCommand;
        private Entity _scanner;
        private Entity _brokenTurret;
        private Entity _turretPlatform;
        private float _scanningTimer;
        private float _sparksTimer1;
        private float _sparksTimer2;
        private float _smokeTimer;
        private bool _isScanning;
        private bool _hasScanned;
        private bool _platformMoved;
        private bool _platformRigidbodyReinited;
        private AvatarCore _avatar;

        public EntranceScanScript(Entity entity)
            : base(entity, "EntranceScanScript")
        {
        }

        public override void Init()
        {
            _isScanning = false;
            _hasScanned = false;
            _sparksTimer1 = 1f;
            _sparksTimer2 = 1.5f;
            _smokeTimer = 5f;
            
            if (_scannerCommandName != "")
                _scannerCommand = entity.GameWorld.GetEntityByName(_scannerCommandName);
            if (_scannerName != "")
                _scanner = entity.GameWorld.GetEntityByName(_scannerName);
            if (_brokenTurretName != "")
                _brokenTurret = entity.GameWorld.GetEntityByName(_brokenTurretName);
            if (_turretPlatformName != "")
                _turretPlatform = entity.GameWorld.GetEntityByName(_turretPlatformName);

            Entity e = entity.GameWorld.GetEntityByName(_avatarName);
            if (e != null)
                _avatar = e.GetComponent<AvatarCore>();
            
            if (_scanner != null)
                _scanner.spritesheets.ChangeAnimation("Opening", 0, false, false, false);
            if (_scannerCommand != null)
                _scannerCommand.spritesheets.ChangeAnimation("Waiting", 0, true, false, false);
            if (_brokenTurret != null)
                _brokenTurret.spritesheets.ChangeAnimation("Off", 0, true, false, false);
            if (_turretPlatform != null)
                _turretPlatform.transform.Position = _brokenTurret.transform.Position + new Vector2(0, 125);
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger.Name == _scannerCommandName)
            {
                if (_scanner != null)
                {
                    _scanner.spritesheets.CurrentSpritesheet.isPlaying = true;
                }
            }
            if (trigger.Name == _scannerName && _scanner != null && _scannerCommand != null)
            {
                if (_hasScanned == false && _scanner.spritesheets.CurrentSpritesheetName == "Opening" && _scanner.spritesheets.CurrentSpritesheet.IsFinished)
                {
                    _scanner.spritesheets.ChangeAnimation("ScanStart", 0, true, false, false);
                    _scannerCommand.spritesheets.ChangeAnimation("Scanning", 0, true, false, true);
                    _isScanning = true;
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_scanner != null && _scannerCommand != null && _brokenTurret != null)
            {
                if (_scanner.spritesheets.CurrentSpritesheetName == "ScanStart" && _scanner.spritesheets.CurrentSpritesheet.IsFinished)
                    _scanner.spritesheets.ChangeAnimation("Scanning", 0, true, false, true);
                if (_isScanning)
                {
                    if (_scanningTimer < _scanningDuration)
                    {
                        _scanningTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (_avatar != null)
                        {
                            _avatar.CanMove = false;
                            _avatar.CanAttack = false;
                            _avatar.CanRoll = false;
                            _avatar.CanTurn = false;
                            _avatar.CanUseElement = false;
                            _avatar.State = AvatarState.Idle;
                        }
                        if (_scanningTimer >= _scanningDuration)
                        {
                            _isScanning = false;
                            _hasScanned = true;
                            _scanner.spritesheets.ChangeAnimation("Error", 0, true, false, false);
                            _scannerCommand.spritesheets.ChangeAnimation("Denied", 0, true, false, false);
                        }
                    }
                }
                if (_hasScanned && _scanner.spritesheets.CurrentSpritesheetName == "Error" && _scanner.spritesheets.CurrentSpritesheet.IsFinished)
                {
                    _brokenTurret.spritesheets.ChangeAnimation("Opening", 0, true, false, false);
                    _scanner.spritesheets.ChangeAnimation("Opening", 0, true, false, false);
                    _scanner.spritesheets.CurrentSpritesheet.Reverse = true;
                    _scanner.spritesheets.CurrentSpritesheet.currentFrame = 9;
                    _turretPlatform.rigidbody.IsGround = false;
                    _turretPlatform.rigidbody.Init();
                }

                if (_turretPlatform != null && _platformMoved && _platformRigidbodyReinited == false)
                {
                    _turretPlatform.rigidbody.Init();
                    _platformRigidbodyReinited = true;
                }

                if (_brokenTurret.spritesheets.CurrentSpritesheetName == "Opening" && _brokenTurret.spritesheets.CurrentSpritesheet.IsFinished)
                {
                    if (_turretPlatform != null && _platformMoved == false)
                    {
                        _turretPlatform.transform.Position = _brokenTurret.transform.Position;
                        _turretPlatform.rigidbody.IsGround = true;
                        _platformMoved = true;
                    }
                    _smokeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_smokeTimer >= 5.0f)
                    {
                        EffectsManager.GetEffect(AssetManager.GetSpriteSheet("ScannerTurretSmoke"), Side.Left, _brokenTurret.transform.Position, 0, new Vector2(50, 20), 2, 0.420f);
                        _smokeTimer = 0;
                    }
                    _sparksTimer1 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_sparksTimer1 >= 2.0f)
                    {
                        EffectsManager.GetEffect(AssetManager.GetSpriteSheet("ScannerTurretSparks"), Side.Right, _brokenTurret.transform.Position, 0, new Vector2(-20,100), 2, 0.450f);
                        _sparksTimer1 = 0;
                    }
                    _sparksTimer2 += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_sparksTimer2 >= 2.0f)
                    {
                        EffectsManager.GetEffect(AssetManager.GetSpriteSheet("ScannerTurretSparks"), Side.Right, _brokenTurret.transform.Position, 0, new Vector2(20, 0), 2, 0.450f);
                        _sparksTimer2 = 0;
                    }

                }
            }
            base.Update(gameTime);
        }
    }
}
