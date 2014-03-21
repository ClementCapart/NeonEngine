using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private Entity _scannerCommand;
        private Entity _scanner;
        private float _scanningTimer;
        private bool _isScanning;
        private bool _hasScanned;
        private AvatarCore _avatar;

        public EntranceScanScript(Entity entity)
            : base(entity, "EntranceScanScript")
        {
        }

        public override void Init()
        {
            _isScanning = false;
            _hasScanned = false;
            if (_scannerCommandName != "")
                _scannerCommand = entity.GameWorld.GetEntityByName(_scannerCommandName);
            if (_scannerName != "")
                _scanner = entity.GameWorld.GetEntityByName(_scannerName);
            Entity e = entity.GameWorld.GetEntityByName(_avatarName);
            if (e != null)
                _avatar = e.GetComponent<AvatarCore>();
            if (_scanner != null)
                _scanner.spritesheets.ChangeAnimation("Opening", 0, false, false, false);
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
            if (trigger.Name == _scannerName)
            {
                if (_hasScanned == false && _scanner != null && _scanner.spritesheets.CurrentSpritesheetName == "Opening" && _scanner.spritesheets.CurrentSpritesheet.IsFinished)
                {
                    _scanner.spritesheets.ChangeAnimation("Scanning", 0, true, false, true);
                    _isScanning = true;
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
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
                    }
                }
            }
            if (_hasScanned && _scanner.spritesheets.CurrentSpritesheetName == "Error" && _scanner.spritesheets.CurrentSpritesheet.IsFinished)
            {
                _scanner.spritesheets.ChangeAnimation("Opening", 0, true, false, false);
                _scanner.spritesheets.CurrentSpritesheet.Reverse = true;
                _scanner.spritesheets.CurrentSpritesheet.currentFrame = 9;
            }
            base.Update(gameTime);
        }
    }
}
