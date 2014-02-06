using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Menu
{
    public class MenuCamera : Component
    {
        private float _leftBound = 0.0f;

        public float LeftBound
        {
            get { return _leftBound; }
            set { _leftBound = value; }
        }

        private float _rightBound = 2560.0f;

        public float RightBound
        {
            get { return _rightBound; }
            set { _rightBound = value; }
        }

        private float _speed = 10.0f;

        public float Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        private bool _goingRight = true;


        public MenuCamera(Entity entity)
            :base(entity, "MenuScreen")
        {
        }

        public override void Init()
        {
            entity.GameWorld.Camera.Position = Vector2.Zero;
            _goingRight = true;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (entity.GameWorld.Alpha == 0.0f)
            {
                if (_goingRight)
                {
                    entity.GameWorld.Camera.Position = new Vector2(entity.GameWorld.Camera.Position.X + _speed * (float)gameTime.ElapsedGameTime.TotalSeconds, entity.GameWorld.Camera.Position.Y);
                    if (entity.GameWorld.Camera.Position.X > _rightBound)
                    {
                        entity.GameWorld.Camera.Position = new Vector2(_rightBound, entity.GameWorld.Camera.Position.Y);
                        _goingRight = false;
                    }
                }
                else
                {
                    entity.GameWorld.Camera.Position = new Vector2(entity.GameWorld.Camera.Position.X - _speed * (float)gameTime.ElapsedGameTime.TotalSeconds, entity.GameWorld.Camera.Position.Y);
                    if (entity.GameWorld.Camera.Position.X < _leftBound)
                    {
                        entity.GameWorld.Camera.Position = new Vector2(_leftBound, entity.GameWorld.Camera.Position.Y);
                        _goingRight = true;
                    }
                }
            }
            
            base.Update(gameTime);
        }
    }
}
