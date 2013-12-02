using System;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary;
using Microsoft.Xna.Framework;
using FarseerPhysics.Dynamics;

namespace NeonScripts
{
    public class PreprodBeginning : ScriptComponent
    {
        private string _avatarName = "";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }
		
		private string _enemyName = "";

        public string EnemyName
        {
            get { return _enemyName; }
            set { _enemyName = value; }
        }

        private string _platformName = "";

        public string PlatformName
        {
            get { return _platformName; }
            set { _platformName = value; }
        }

        private float _delay = 0.0f;

        public float Delay
        {
            get { return _delay; }
            set { _delay = value; }
        }

        private float _impulseFrame = 0;

        public float ImpulseFrame
        {
            get { return _impulseFrame; }
            set { _impulseFrame = value; }
        }

        private float _xImpulse = 0.0f;

        public float XImpulse
        {
            get { return _xImpulse; }
            set { _xImpulse = value; }
        }

        private float _yImpulse = 0.0f;

        public float YImpulse
        {
            get { return _yImpulse; }
            set { _yImpulse = value; }
        }

        private Entity _platform = null;
        private Entity _blacksmith = null;
        private Entity _liOn = null;
		
		private float _timer = 0.0f;

        private bool _blacksmithAttacked = false;

        public PreprodBeginning(Entity entity)
            :base(entity, "PreprodBeginning")
        {
        }

        public override void Init()
        {
			_timer = _delay;
            if (_enemyName != "")
                _blacksmith = Neon.world.GetEntityByName(_enemyName);
            if (_blacksmith != null)
                _blacksmith.spritesheets.ChangeAnimation("Idle");
            if (_avatarName != "")
                _liOn = Neon.world.GetEntityByName(_avatarName);
            if (_liOn != null)
                _liOn.spritesheets.ChangeAnimation("idle");
            Neon.world.camera.Position = new Vector2(60, -200);
        }

        public override void Update(GameTime gameTime)
        {
            if (_timer >= 0.0f)
                _timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timer < 0.0f && _blacksmithAttacked == false)
            {
                _blacksmith.spritesheets.ChangeAnimation("Attack");
                _blacksmithAttacked = true;
            }
            if (_blacksmithAttacked == true && _blacksmith.spritesheets.CurrentSpritesheetName == "Attack" && _blacksmith.spritesheets.CurrentSpritesheet.currentFrame == _blacksmith.spritesheets.CurrentSpritesheet.spriteSheetInfo.FrameCount - 1)
				_blacksmith.spritesheets.ChangeAnimation("Idle");
            if (_avatarName != "" && _blacksmith.spritesheets.CurrentSpritesheetName == "Attack" && _blacksmith.spritesheets.CurrentSpritesheet.currentFrame == _impulseFrame)
            {
                _liOn.rigidbody.body.ApplyLinearImpulse(new Vector2(_xImpulse, _yImpulse));
                _liOn.spritesheets.ChangeAnimation("Hit", 0, true, false, false);
            }
            if (_liOn.rigidbody.body.LinearVelocity.Y > 0.5f && _liOn.spritesheets.CurrentSpritesheetName == "Hit")
                _liOn.spritesheets.ChangeAnimation("startFall",0,true,false,false);
            if (_liOn.spritesheets.CurrentSpritesheetName == "startFall" && _liOn.spritesheets.IsFinished())
                _liOn.spritesheets.ChangeAnimation("fallLoop");
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            Neon.world.ChangeScreen(new GameScreen(Neon.game));
        }
    }
}
