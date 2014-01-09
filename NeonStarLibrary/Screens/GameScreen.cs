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


        //Sounds obvious but still, I don't know...//
        public Entity avatar;
        //----------------------------------------//

        protected int lastSpawnPointIndex;

        public string AvatarName = "LiOn";

        public GameScreen(string groupName, string levelName, int startingSpawnPointIndex, Game game)
            : base(game)
        {
            lastSpawnPointIndex = startingSpawnPointIndex;

            enemies = new List<Enemy>();
            
            BulletsPool = new NeonPool<Entity>(() => new Entity(this));

            BulletsManager.LoadBullets();
            AttacksManager.LoadAttacks();

            DataManager.LoadLevelInfo(groupName, levelName, this);

            SpawnPoint currentSpawnPoint = null;

            foreach (SpawnPoint sp in SpawnPoints)
                if (sp.Index == startingSpawnPointIndex)
                    currentSpawnPoint = sp;
            if (SpawnPoints.Count == 0)
            {
                Console.WriteLine("Warning : No Spawn Point found, Avatar won't be created");
            }
            else if (currentSpawnPoint != null)
            {
                avatar = DataManager.LoadPrefab(@"../Data/Prefabs/" + AvatarName + ".prefab", this);

                avatar.transform.Position = currentSpawnPoint.Position;
                Avatar avatarComponent = avatar.GetComponent<Avatar>();
                if (avatarComponent != null)
                    avatarComponent.CurrentSide = currentSpawnPoint.Side;
            }
            else
                Console.WriteLine("Warning : SpawnPoint "+ startingSpawnPointIndex + " not found, Avatar won't be created, please select an existing SpawnPoint");

            if(levelName != "")
                LoadLevel(new Level(groupName, levelName, this, true));

            Camera.Bounded = true;
        } 

        public override void PreUpdate(GameTime gameTime)
        {
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {
                if (MustFollowAvatar && avatar != null)
                    Camera.Chase(avatar.transform.Position, gameTime);
                else if (avatar == null)
                {
                    avatar = Neon.World.GetEntityByName("LiOn");
                }
                for (int i = FreeAttacks.Count - 1; i >= 0; i--)
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
            }

            if (Neon.Input.Pressed(Buttons.Start))
                Pause = !Pause;

            base.Update(gameTime);
        }

        public virtual void ReloadLevel()
        {
            ChangeScreen(new GameScreen(this.LevelMap.Group, this.LevelMap.Name, lastSpawnPointIndex, game));
        }

        public override void ManualDrawBackHUD(SpriteBatch sb)
        {
            base.ManualDrawBackHUD(sb);
        }
    }
}
