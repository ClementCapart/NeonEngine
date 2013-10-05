﻿using Microsoft.Xna.Framework.Graphics;
using NeonEngine;
using Microsoft.Xna.Framework;
using NeonStarLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    class HitboxRenderer : DrawableComponent
    {
        Fight FightComponent;
        Hitbox hitbox;

        public HitboxRenderer(Fight fightComponent)
            :base(0, fightComponent.entity, "HitboxRenderer")
        {
            FightComponent = fightComponent;
        }

        public HitboxRenderer(Hitbox hitbox)
            : base(0, hitbox.entity, "HitboxRenderer")
        {
            this.hitbox = hitbox;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (FightComponent != null)
            {
                /*FightComponent.MainHitbox.Draw(spriteBatch, Color.Red);
                foreach (Hitbox h in FightComponent.AttackHitboxes)
                    h.Draw(spriteBatch, Color.Green);*/
            }
            //else
                //hitbox.Draw(spriteBatch, Color.Yellow);
            base.Draw(spriteBatch);
        }
    }
}
