using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{

    public class Fight : Component
    {
        public bool isHitting;

        public SideDirection currentAttackSide;

        public Hitbox MainHitbox;
        public List<Hitbox> AttackHitboxes;

        private Hitbox LastHitTaken;
        private Rectangle AttackSize;

        public Fight(Hitbox hitbox, Rectangle AttackSize, Entity entity)
            : base(entity, "Fight")
        {
            this.AttackSize = AttackSize;
            this.MainHitbox = hitbox;
            this.AttackHitboxes = new List<Hitbox>();
        }

        public void StartHit(SideDirection attackSide)
        {
            if(!isHitting)
            {
                this.currentAttackSide = attackSide;
                isHitting = true;

               /* switch (attackSide)
                {
                    case SideDirection.Left:
                        AttackHitboxes.Add(new Hitbox(AttackSize.Width,AttackSize.Height, HitboxType.Hit, entity, new Vector2(-AttackSize.Y, 20)));
                        break;
                    case SideDirection.Right:
                        AttackHitboxes.Add(new Hitbox(AttackSize.Width, AttackSize.Height, HitboxType.Hit, entity, new Vector2(AttackSize.Y, 20)));
                        break;
                }*/
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (isHitting)
            {
                for(int i = AttackHitboxes.Count - 1; i >= 0; i--)
                {
                    Hitbox AttackHitbox = AttackHitboxes[i];

                    switch (currentAttackSide)
                    {
                        case SideDirection.Left:
                            AttackHitbox.OffsetX -= (100 * (float)Neon.elapsedTime / 1000);
                            if (((entity as Avatar).Spritesheets.CurrentSpritesheetName == "Kick01" || (entity as Avatar).Spritesheets.CurrentSpritesheetName == "Kick02") && (entity as Avatar).Spritesheets.IsFinished())
                            {
                                AttackHitboxes.Remove(AttackHitbox);
                                isHitting = false;
                            }
                            break;
                        case SideDirection.Right:
                            AttackHitbox.OffsetX += (100 * (float)Neon.elapsedTime / 1000);
                            if (((entity as Avatar).Spritesheets.CurrentSpritesheetName == "Kick01" || (entity as Avatar).Spritesheets.CurrentSpritesheetName == "Kick02") && (entity as Avatar).Spritesheets.IsFinished())
                            {
                                AttackHitboxes.Remove(AttackHitbox);
                                isHitting = false;
                            }
                            break;
                    }
                    AttackHitbox.Center = MainHitbox.Center;
                }
            }
            base.Update(gameTime);
        }

        public bool Hit(Hitbox hitbox)
        {           
            foreach(Hitbox hb in AttackHitboxes)
                if(hitbox.hitboxRectangle.Intersects(hb.hitboxRectangle))
                    return true;
            return false;
        }

        public bool TakeHit(Fight fight)
        {
            foreach(Hitbox hb in fight.AttackHitboxes)
                if(hb != LastHitTaken)
                    if(hb.hitboxRectangle.Intersects(MainHitbox.hitboxRectangle))
                    {
                        LastHitTaken = hb;
                        return true;
                    }

            return false;
        }
    }
}
