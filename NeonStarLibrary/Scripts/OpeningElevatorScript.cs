using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary.Components.Avatar;
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
        private AvatarCore _liOn;
        private MovingGeometry _movingGeometry;
        private Entity _rightWall;

        public OpeningElevatorScript(Entity entity)
            :base(entity, "OpeningElevatorScript")
        {
            _movingGeometry = entity.GetComponent<MovingGeometry>();
            _liOn = entity.GameWorld.GetEntityByName("LiOn").GetComponent<AvatarCore>();
            _rightWall = entity.GameWorld.GetEntityByName("ElevatorRightWall");
        }

        public override void Update(GameTime gameTime)
        {
            if (!_movingGeometry.Active && _rightWall != null && _rightWall.rigidbody != null)
            {
                _rightWall.rigidbody.IsGround = false;
                _rightWall.rigidbody.Init();
            }
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (trigger.Name == "ElevatorButton")
            {
                _buttonPressed = true;
            }

            if (trigger.Name == "Elevator" && _buttonPressed)
            {
                if (_liOn != null)
                    _liOn.ThirdPersonController.FallSpeedLimit = false;
                this.entity.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                this.entity.rigidbody.Init();
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
