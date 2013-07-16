using NeonEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Bullet : Entity
    {

        Vector2 Direction;
        Vector2 InitialPosition;
        float Speed = 15f;

        float EffectTimer = 0f;
        float EffectDelay = 0.5f;
        SpriteSheet currentSpriteSheet;
        string asset;

        public Bullet(Vector2 Position, string asset, Vector2 Direction, World containerWorld)
            :base(containerWorld)
        {
            this.asset = asset;
            hitbox = (Hitbox)AddComponent(new Hitbox(this));
            hitbox.Width = AssetManager.GetSpriteSheet(asset).FrameWidth;
            hitbox.Height = AssetManager.GetSpriteSheet(asset).FrameHeight;
            hitbox.Init();
            currentSpriteSheet = (SpriteSheet)AddComponent(new SpriteSheet(AssetManager.GetSpriteSheet(asset), DrawLayer.Middleground4, this));
            currentSpriteSheet.Init();
            if (Direction.X < 0)
                currentSpriteSheet.spriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            
            //AddComponent(new HitboxRenderer(hitbox, containerWorld));
            this.InitialPosition = Position;
            this.transform.Position = Position;
            this.Direction = Direction;
        }

        public override void Update(GameTime gameTime)
        {
            this.transform.Position += Direction * Speed;
            if (asset == "GunShot")
                for (int i = containerWorld.entities.Count - 1; i >= 0; i--)
                {
                    if(containerWorld.entities[i] is Enemy)
                    {
                        Enemy e = (containerWorld.entities[i] as Enemy);
                        if (hitbox.hitboxRectangle.Intersects(e.hitbox.hitboxRectangle))
                        {
                            e.TakeDamage(5, ElementType.Fire);
                            this.Destroy();
                            return;
                        }
                    }
                }
                    
            if (EffectTimer >= EffectDelay)
            {
                this.containerWorld.entities.Add(new Feedback(asset + "Effect", this.transform.Position + new Vector2(0, this.currentSpriteSheet.spriteSheetInfo.FrameHeight / 2), DrawLayer.Middleground3, Direction.X > 0 ? SideDirection.Right : SideDirection.Left, 0.2f, containerWorld));


                EffectTimer = 0f;
            }
            else
                EffectTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            if (Vector2.Distance(this.InitialPosition, transform.Position) > 1000)
                this.Destroy();
            base.Update(gameTime);
        }


    }
}
