using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Lightning : ElementEffect
    {
        private float _impulseForce = 700.0f;
        private float _dashDuration = 0.3f;

        public Lightning(ElementSystem elementSystem, int elementLevel, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, elementLevel, entity, input, world)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch(State)
            {
                case ElementState.Initialization:
                    State = ElementState.Charge;
                    break;

                case ElementState.Charge:
                    _entity.rigidbody.GravityScale = 0.0f;
                    _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                    _entity.rigidbody.body.ApplyLinearImpulse(new Vector2(_impulseForce * (_entity.spritesheets.CurrentSide == Side.Right ? 1 : -1), 0));
                    State = ElementState.Effect;
                    break;

                case ElementState.Effect:
                    if (_dashDuration > 0.0f)
                    {
                        _entity.rigidbody.GravityScale = 0.0f;
                        _dashDuration -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _entity.spritesheets.ChangeAnimation("AirDash", true, 1);
                    }
                    else
                    {
                        _dashDuration = 0.0f;
                        _entity.rigidbody.body.LinearVelocity = Vector2.Zero;
                        State = ElementState.End;
                    }
                                 
                    
                    break;

                case ElementState.End:
                    break;
            }
            base.Update(gameTime);
        }
    }
}
