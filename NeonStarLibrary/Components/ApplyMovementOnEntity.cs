using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.CollisionDetection
{
   

    public class ApplyMovementOnEntity : Component
    {
        #region Properties
        private string _entityNameToMove = "LiOn";

        public string EntityNameToMove
        {
            get { return _entityNameToMove; }
            set { _entityNameToMove = value; }
        }

        private bool _checkIfGrounded = true;

        public bool CheckIfGrounded
        {
            get { return _checkIfGrounded; }
            set { _checkIfGrounded = value; }
        }

        private Vector2 _checkGroundOffset;

        public Vector2 CheckGroundOffset
        {
            get { return _checkGroundOffset; }
            set { _checkGroundOffset = value; }
        }

        private bool _verticalMovingPlatform = false;

        public bool VerticalMovingPlatform
        {
            get { return _verticalMovingPlatform; }
            set { _verticalMovingPlatform = value; }
        }
        #endregion

        private Entity _entityToMove;
        private Vector2 _lastFramePosition;

        public ApplyMovementOnEntity(Entity entity)
            :base(entity, "ApplyMovemenentOnEntity")
        {
            RequiredComponents = new Type[] { typeof(Rigidbody) };
        }

        public override void Init()
        {
            _lastFramePosition = entity.transform.Position;
            _entityToMove = entity.GameWorld.GetEntityByName(_entityNameToMove);
            base.Init();
        }

        public override void FinalUpdate(GameTime gameTime)
        {
            if (_checkIfGrounded)
            {
                if (_entityToMove != null && _entityToMove.rigidbody != null && _entityToMove.rigidbody.beacon != null)
                {
                    if (entity.rigidbody != null && _entityToMove.rigidbody.beacon.CheckGround(_checkGroundOffset) == entity.rigidbody)
                    {
                        Vector2 deltaMove = entity.transform.Position - _lastFramePosition;
                        if(!_verticalMovingPlatform)
                            _entityToMove.transform.Position = _entityToMove.transform.Position + deltaMove;
                        if(deltaMove.Y > 0)
                            _entityToMove.rigidbody.beacon.GroundOffset = _checkGroundOffset;                  
                    }
                }
            }
            else if (_entityToMove != null)
            {
                _entityToMove.transform.Position = _entityToMove.transform.Position + (entity.transform.Position - _lastFramePosition);
            }

            _lastFramePosition = entity.transform.Position;  
            base.FinalUpdate(gameTime);
        }



    }
}
