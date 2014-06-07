using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Menu
{
    public class EndMenu : Component
    {
        private Vector2 _cameraTarget = Vector2.Zero;

        public Vector2 CameraTarget
        {
            get { return _cameraTarget; }
            set { _cameraTarget = value; }
        }

        private bool _initFinished = false;
        private bool _moveFinished = false;
        private Graphic _textGraphic;

        public EndMenu(Entity entity)
            :base(entity, "EndMenu")
        {

        }

        public override void Init()
        {
            (entity.GameWorld as GameScreen).PauseAllowed = false;
            entity.GameWorld.Camera.Zoom = 10.0f;
            entity.GameWorld.Camera.Position = new Vector2(-15, 10);
            _textGraphic = entity.GetComponentByNickname("Text") as Graphic;
            if (_textGraphic != null)
                _textGraphic.Opacity = 0.0f;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!_initFinished)
            {
                if (entity.GameWorld.Camera.Zoom > 1.0f)
                    entity.GameWorld.Camera.Zoom -= (float)gameTime.ElapsedGameTime.TotalSeconds * 9;
                else
                {
                    entity.GameWorld.Camera.Zoom = 1.0f;
                    _initFinished = true;
                }
            }

            if (_initFinished && !_moveFinished)
            {
                if (Vector2.DistanceSquared(entity.GameWorld.Camera.Position, _cameraTarget) < 5)
                {
                    entity.GameWorld.Camera.Position = _cameraTarget;
                    _moveFinished = true;
                }
                else
                    entity.GameWorld.Camera.Position = Vector2.Lerp(entity.GameWorld.Camera.Position, _cameraTarget, 0.05f);
            }

            if (_initFinished && _moveFinished)
            {
                if (entity.spritesheets != null)
                {
                    if (entity.spritesheets.CurrentSpritesheetName != "Idle")
                        entity.spritesheets.ChangeAnimation("Fade", 0, true, false, false);
                    if (entity.spritesheets.CurrentSpritesheetName == "Fade" && entity.spritesheets.CurrentSpritesheet.IsFinished)
                        entity.spritesheets.ChangeAnimation("Idle");
                }
                if (_textGraphic.Opacity < 1.0f)
                    _textGraphic.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                else
                    _textGraphic.Opacity = 1.0f;

                if (Neon.Input.Pressed(NeonStarInput.Start) || Neon.Input.Pressed(NeonStarInput.Jump) || Neon.Input.Pressed(NeonStarInput.Guard))
                    entity.GameWorld.ChangeLevel("00TitleScreen", "01TitleScreenMain", 0);
            }
            base.Update(gameTime);
        }
    }
}
