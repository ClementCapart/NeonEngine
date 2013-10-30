using System;
using System.Collections.Generic;
using System.Linq;
using NeonEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace NeonStarLibrary
{
    public class GameScreen : World
    {
        public List<Enemy> enemies;
        public List<Attack> FreeAttacks = new List<Attack>();
        public List<Entity> Bullets = new List<Entity>();

        public NeonPool<Entity> BulletsPool;
        public bool MustFollowAvatar = true;

        public GameScreen(Game game)
            : base(game)
        {
            enemies = new List<Enemy>();
            
            BulletsPool = new NeonPool<Entity>(() => new Entity(this));

            BulletsManager.LoadBullets();
            AttacksManager.LoadAttacks();          

            LoadLevel(new Level(@"..\Data\Levels\Level_0-0", this, true));

            camera.Bounded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if(MustFollowAvatar)
                camera.Chase(entities.Where(e => e.Name == "LiOn").First().transform.Position, gameTime);

            for(int i = FreeAttacks.Count - 1; i >= 0; i--)
            {
                Attack attack = FreeAttacks[i];
                attack.Update(gameTime);
                if (attack.CooldownFinished)
                {
                    FreeAttacks.Remove(attack);
                    attack._entity.Destroy();
                    attack.CancelAttack();
                }
            }


            for (int i = Bullets.Count - 1; i >= 0; i--)
                Bullets[i].Update(gameTime);

            if (Neon.Input.Pressed(Buttons.Start))
                    Pause = !Pause;
            base.Update(gameTime);
        }

        public virtual void ReloadLevel()
        {
            ChangeScreen(new GameScreen(game));
        }

        public override void ManualDrawBackHUD(SpriteBatch sb)
        {
            base.ManualDrawBackHUD(sb);
        }
    }
}
