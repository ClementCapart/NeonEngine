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


        public MusicScript(Entity entity)
            :base(entity, "MusicScript")
        {
        }

        public override void Init()
        {
            this.entity.spritesheets.ChangeAnimation("Idle");
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.FinalUpdate(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
