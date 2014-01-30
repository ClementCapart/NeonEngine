using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class ElevatorScript : Component
    {
        #region Properties
        private float _elevatorTravelDistance = 350.0f;

        public float ElevatorTravelDistance
        {
            get { return _elevatorTravelDistance; }
            set { _elevatorTravelDistance = value; }
        }

        private string _upTriggerName = "";

        public string UpTriggerName
        {
            get { return _upTriggerName; }
            set { _upTriggerName = value; }
        }

        private string _downTriggerName = "";

        public string DownTriggerName
        {
            get { return _downTriggerName; }
            set { _downTriggerName = value; }
        }

        #endregion

        private bool _isInUpTrigger = false;
        private bool _isInDownTrigger = false;

        private bool _movingUp = false;
        private bool _movingDown = false;

        private Entity _avatar = null;

        public ElevatorScript(Entity entity)
            :base(entity, "ElevatorScript")
        {

        }

        public override void Init()
        {
            if (_avatar == null)
                _avatar = entity.GameWorld.GetEntityByName("LiOn");
            base.Init();
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _isInUpTrigger = false;
            _isInDownTrigger = false;
            base.FinalUpdate(gameTime);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_isInUpTrigger && Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Y) && !_movingUp && !_movingDown)
                _movingUp = true;
            else if (_isInDownTrigger && Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Y) && !_movingUp && !_movingDown)
                _movingDown = true;

            if (_movingUp)
                this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0.0f, -2.0f);
            else if (_movingDown)
                this.entity.rigidbody.body.LinearVelocity = new Microsoft.Xna.Framework.Vector2(0.0f, 2.0f);
            else
                this.entity.rigidbody.body.LinearVelocity = Vector2.Lerp(this.entity.rigidbody.body.LinearVelocity, Vector2.Zero, 0.1f);

            if (this.entity.rigidbody.body.LinearVelocity.Y < 0.2f && this.entity.rigidbody.body.LinearVelocity.Y > -0.2f)
                this.entity.rigidbody.body.LinearVelocity = Vector2.Zero;

            if (Math.Abs(this.entity.transform.Position.Y - this.entity.transform.InitialPosition.Y) >= _elevatorTravelDistance)
                _movingUp = false;
            if (Math.Abs(this.entity.transform.Position.Y - this.entity.transform.InitialPosition.Y) < 10.0f)
                _movingDown = false;
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger.Name == _upTriggerName)
            {
                _isInUpTrigger = true;
            }
            else if (trigger.Name == _downTriggerName)
            {
                _isInDownTrigger = true;
            }

            if (trigger.Name == "003ElevatorLevelTrigger")
            {
                entity.GameWorld.ChangeScreen(new LoadingScreen(Neon.Game, 0, @"../Data/Levels/PreprodPresentation/Ending.xml"));
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }



    }
}
