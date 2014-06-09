using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Scripts
{
    public class WrathOpeningSequence : Component
    {

        private Vector2 _shockImpulse = Vector2.Zero;

        public Vector2 ShockImpulse
        {
            get { return _shockImpulse; }
            set { _shockImpulse = value; }
        }

        private Entity _wrath;
        private FadingSpritesheet _yButton;
        private AvatarCore _avatarCore;
        private bool _sequenceStarted = false;

        public WrathOpeningSequence(Entity entity)
            :base(entity, "WrathOpeningSequence")
        {

        }

        public override void Init()
        {
            _wrath = entity.GameWorld.GetEntityByName("Wrath");
            _yButton = entity.GetComponent<FadingSpritesheet>();
            Entity e = entity.GameWorld.GetEntityByName("LiOn");
            if(e != null)
                _avatarCore = e.GetComponent<AvatarCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_yButton != null && _yButton.Opacity >= 0.5f && Neon.Input.Pressed(NeonStarInput.Interact))
            {
                if (_wrath != null)
                    _wrath.spritesheets.ChangeAnimation("Fall", 0, true, false, false);
            }

            if (_wrath != null)
            {
                if (_wrath.spritesheets.CurrentSpritesheetName == "Fall" && _wrath.spritesheets.CurrentSpritesheet.IsFinished)
                {
                    _sequenceStarted = true;
                    (entity.GameWorld as GameScreen).MustFollowAvatar = false;
                    _wrath.spritesheets.ChangeAnimation("Impact", 0, true, false, false);
                    if(_avatarCore.CurrentHealthPoints == 1.0f)
                        _avatarCore.CurrentHealthPoints = 2.0f;
                    _avatarCore.CurrentSide = Side.Left;
                    _avatarCore.Guard.PerformRoll();
                    
                    
                }

                if (_wrath.spritesheets.CurrentSpritesheetName == "Impact" && _wrath.spritesheets.CurrentSpritesheet.IsFinished)
                    _wrath.spritesheets.ChangeAnimation("Eyes", 0, true, false, false);
            }

            if (_sequenceStarted && _avatarCore.State != AvatarState.Rolling)
            {
                _avatarCore.CanAttack = false;
                _avatarCore.CanRoll = false;
                _avatarCore.CanTurn = false;
                _avatarCore.CanMove = false;
                _avatarCore.CanUseElement = false;
                _avatarCore.CurrentSide = Side.Right;
                (entity.GameWorld as GameScreen).MustFollowAvatar = false;
            }

            if (_sequenceStarted && _wrath.spritesheets.CurrentSpritesheetName == "Eyes" && _wrath.spritesheets.CurrentSpritesheet.IsFinished && entity.GameWorld.NextScreen == null)
            {
                entity.GameWorld.ChangeLevel("00TitleScreen", "02EndScreen", 0);
            }


            base.Update(gameTime);
        }


    }
}
