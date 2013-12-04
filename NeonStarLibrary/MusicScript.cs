using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class MusicScript : Component
    {
        private bool _onTrigger = false;

        private Song _battleTheme;

        public MusicScript(Entity entity)
            :base(entity, "MusicScript")
        {
        }

        public override void Init()
        {
            _battleTheme = SoundManager.GetSong("BattleTheme");
            this.entity.spritesheets.ChangeAnimation("Idle");
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_onTrigger && Neon.Input.Pressed(Buttons.Y))
            {
                MediaPlayer.Play(_battleTheme);
                this.entity.spritesheets.ChangeAnimation("Playing");
            }

            if(MediaPlayer.State == MediaState.Stopped && entity.spritesheets.CurrentSpritesheetName == "Playing")
                this.entity.spritesheets.ChangeAnimation("Idle");
            base.Update(gameTime);
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _onTrigger = false;
            base.FinalUpdate(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            _onTrigger = true;
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
