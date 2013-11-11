using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Fire : ElementEffect
    {
        private float _charge = 0.0f;
        private float _chargeSpeed = 50.0f;

        private float _maxCharge = 100.0f;

        public Fire(ElementSystem elementSystem, Entity entity, NeonStarInput input, GameScreen world)
            :base(elementSystem, entity, input, world)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch (_state)
            {
                case ElementState.Initialization:
                    _state = ElementState.Charge;
                    break;

                case ElementState.Charge:
                    if (Neon.Input.Check(_input))
                    {
                        if (_charge < _maxCharge)
                        {
                            _charge += (float)gameTime.ElapsedGameTime.TotalSeconds * _chargeSpeed;
                        }
                        else
                        {

                        }
                    }
                    else
                    {

                    }
                    break;

                case ElementState.Effect:
                    break;

                case ElementState.End:
                    break;
            }

            base.Update(gameTime);
        }

        public override void End()
        {
            base.End();
        }
    }
}
