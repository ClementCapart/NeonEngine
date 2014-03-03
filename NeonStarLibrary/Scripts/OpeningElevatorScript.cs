using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Private;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.CollisionDetection;
using NeonStarLibrary.Components.GameplayElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class OpeningElevatorScript : ScriptComponent
    {
        #region Properties

        #endregion

        private bool _buttonPressed = false;
        private bool _stopped = false;
        private bool _falling = false;
        private AvatarCore _liOn;
        private MovingGeometry _movingGeometry;
        private Entity _rightWall;
        private Entity _gears;
        private SpriteSheet _gearsSpritesheet;

        private SpriteSheet _firstSpritesheet;
        private SpriteSheet _secondSpritesheet;

        public OpeningElevatorScript(Entity entity)
            :base(entity, "OpeningElevatorScript")
        {
        }

        public override void Init()
        {
            _gears = entity.GameWorld.GetEntityByName("Gears");
            if (_gears != null)
                _gearsSpritesheet = _gears.GetComponent<SpriteSheet>();
            _movingGeometry = entity.GetComponent<MovingGeometry>();
            _liOn = entity.GameWorld.GetEntityByName("LiOn").GetComponent<AvatarCore>();
            _rightWall = entity.GameWorld.GetEntityByName("ElevatorRightWall");
            _rightWall.rigidbody.IsGround = true;
            _rightWall.rigidbody.Init();
            SpriteSheet[] sss = entity.GetComponentsByInheritance<SpriteSheet>().ToArray();
            if (sss.Length == 2)
            {
                _firstSpritesheet = sss[0];
                _firstSpritesheet.isPlaying = false;
                _secondSpritesheet = sss[1];
                _secondSpritesheet.isPlaying = false;
            }
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if(_liOn.CanRoll)
                _liOn.CanRoll = false;
            if (!_movingGeometry.Active && _rightWall != null && _rightWall.rigidbody != null && !_stopped)
            {
                _rightWall.rigidbody.IsGround = false;
                _rightWall.rigidbody.Init();
                if (_gearsSpritesheet != null)
                    _gearsSpritesheet.isPlaying = false;
                _firstSpritesheet.IsLooped = false;
                _secondSpritesheet.IsLooped = false;
                _firstSpritesheet.isPlaying = true;
                _secondSpritesheet.isPlaying = true;
                _stopped = true;
            }
            if(_falling && _liOn != null)
            {
                _liOn.CanAttack = false;
                _liOn.CanMove = false;
                _liOn.CanRoll = false;
                _liOn.CanTurn = false;
                _liOn.CanUseElement = false;
            }
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger.Name == "ElevatorButton")
            {
                _buttonPressed = true;
                if (_firstSpritesheet.SpriteSheetTag == "ElevatorBack")
                    _firstSpritesheet.currentFrame = 0;
                else if (_secondSpritesheet.SpriteSheetTag == "ElevatorBack")
                    _secondSpritesheet.currentFrame = 0;
            }

            if (trigger.Name == "Elevator" && _buttonPressed)
            {
                if (_liOn != null)
                {
                    _liOn.ThirdPersonController.FallSpeedLimit = false;
                    _liOn.CanAttack = false;
                    _liOn.entity.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Kinematic;
                    _liOn.entity.rigidbody.Init();
                    
                    KinematicDistanceJoint kdj = new KinematicDistanceJoint(_liOn.entity);
                    kdj.LinkedEntityName = this.entity.Name;
                    kdj.Init();
                    kdj.Offset = new Vector2(_liOn.entity.transform.Position.X - entity.transform.Position.X, -104);
                    _liOn.entity.AddComponent(kdj);
                    _liOn.entity.spritesheets.SpritesheetList["fallLoop"] = AssetManager.GetSpriteSheet("LiOnFallElevator");
                    _liOn.entity.spritesheets.ChangeAnimation("fallLoop", true, 0, true, true, true);
                    
                }
                if (_gearsSpritesheet != null)
                {
                    _gearsSpritesheet.SpriteSheetTag = "GearsSpeedUp";
                    _gearsSpritesheet.isPlaying = true;
                    SpriteSheet ssTop = new SpriteSheet(_gears);
                    ssTop.SpriteSheetTag = "SparksTop";
                    ssTop.Init();
                    ssTop.Layer = 0.45f;
                    ssTop.Offset = new Vector2(56, -77);
                    _gears.AddComponent(ssTop);

                    SpriteSheet ssBot = new SpriteSheet(_gears);
                    ssBot.SpriteSheetTag = "SparksBot";
                    ssBot.Init();
                    ssBot.Layer = 0.55f;
                    ssBot.Offset = new Vector2(82, 48);
                    _gears.AddComponent(ssBot);
                }

                this.entity.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                this.entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                this.entity.rigidbody.IsGround = false;
                this.entity.rigidbody.Init();
                
                _falling = true;
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
