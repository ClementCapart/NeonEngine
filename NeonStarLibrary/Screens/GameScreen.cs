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
using System.Xml.Linq;
using System.IO;

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


        protected XElement _statusToLoad;
        protected int lastSpawnPointIndex;

        public string AvatarName = "LiOn";

        public GameScreen(string groupName, string levelName, int startingSpawnPointIndex, XElement statusToLoad, Game game)
            : base(game)
        {
            this._statusToLoad = statusToLoad;
            lastSpawnPointIndex = startingSpawnPointIndex;

            enemies = new List<Enemy>();
            
            BulletsPool = new NeonPool<Entity>(() => new Entity(this));


            BulletsManager.LoadBullets();
            AttacksManager.LoadAttacks();
            ElementManager.LoadElementParameters();

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
                {
                    LoadAvatarStatus(avatarComponent);
                    avatarComponent.CurrentSide = currentSpawnPoint.Side;
                }
            }
            else
                Console.WriteLine("Warning : SpawnPoint "+ startingSpawnPointIndex + " not found, Avatar won't be created, please select an existing SpawnPoint");

            if(levelName != "")
                LoadLevel(new Level(groupName, levelName, this, true));

            Camera.Bounded = true;
        } 

        public void LoadAvatarStatus(Avatar avatarComponent)
        {
            if (_statusToLoad != null && avatarComponent != null)
            {
                XElement liOn = _statusToLoad.Element("LiOnParameters");
                avatarComponent.CurrentHealthPoints = float.Parse(liOn.Element("HealthPoints").Value);
                if(avatarComponent.ElementSystem != null)
                {
                    avatarComponent.ElementSystem.LeftSlotElement = (Element)Enum.Parse(typeof(Element), liOn.Element("LeftElement").Value);
                    avatarComponent.ElementSystem.LeftSlotLevel = float.Parse(liOn.Element("LeftElement").Attribute("Level").Value);

                    avatarComponent.ElementSystem.RightSlotElement = (Element)Enum.Parse(typeof(Element), liOn.Element("RightElement").Value);
                    avatarComponent.ElementSystem.RightSlotLevel = float.Parse(liOn.Element("RightElement").Attribute("Level").Value);
                }
                if (avatarComponent.EnergySystem != null)
                {
                    avatarComponent.EnergySystem.CurrentEnergyStock = float.Parse(liOn.Element("Energy").Value);
                }
            }
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
                    avatar = this.GetEntityByName("LiOn");
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
            ChangeScreen(new GameScreen(this.LevelMap.Group, this.LevelMap.Name, lastSpawnPointIndex, _statusToLoad, game));
        }

        public void ChangeLevel(string groupName, string levelName, int spawnPointIndex)
        {
            ChangeScreen(new LoadingScreen(Neon.Game, spawnPointIndex, groupName, levelName, SaveStatus()));
        }

        public XElement SaveStatus()
        {
            XElement playerStatus = new XElement("PlayerStatus");

            Avatar avatarComponent = null;
            if (avatar != null)
                avatarComponent = avatar.GetComponent<Avatar>();

            if (avatarComponent != null)
            {
                XElement liOn = new XElement("LiOnParameters");

                XElement currentPosition = new XElement("Position", Neon.Utils.Vector2ToString(avatar.transform.Position));
                liOn.Add(currentPosition);
                XElement currentSide = new XElement("Side", avatarComponent.CurrentSide.ToString());
                liOn.Add(currentSide);
                XElement currentHealthPoints = new XElement("HealthPoints", avatarComponent.CurrentHealthPoints);
                liOn.Add(currentHealthPoints);
                if (avatarComponent.EnergySystem != null)
                {
                    XElement currentEnergyStock = new XElement("Energy", avatarComponent.EnergySystem.CurrentEnergyStock.ToString());
                    liOn.Add(currentEnergyStock);
                }

                if (avatarComponent.ElementSystem != null)
                {
                    XElement currentLeftElement = new XElement("LeftElement", new XAttribute("Level", avatarComponent.ElementSystem.LeftSlotLevel), avatarComponent.ElementSystem.LeftSlotElement.ToString());
                    XElement currentRightElement = new XElement("RightElement", new XAttribute("Level", avatarComponent.ElementSystem.RightSlotLevel), avatarComponent.ElementSystem.RightSlotElement.ToString());
                    liOn.Add(currentLeftElement);
                    liOn.Add(currentRightElement);
                }

                playerStatus.Add(liOn);
            }
            return playerStatus;
        }

        public void SaveProgressionToFile()
        {
            XDocument saveProgression = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

            XElement playerProgression = new XElement("PlayerProgression");

            XElement currentLevel = new XElement("CurrentLevel");

            XElement currentGroupName = new XElement("GroupName", this.LevelGroupName);
            XElement currentLevelName = new XElement("LevelName", this.LevelName);

            currentLevel.Add(currentGroupName);
            currentLevel.Add(currentLevelName);

            playerProgression.Add(currentLevel);

            XElement playerStatus = SaveStatus();
            playerProgression.Add(playerStatus);
            
            saveProgression.Add(playerProgression);

            if(!Directory.Exists(@"../Save/"))
                Directory.CreateDirectory(@"../Save/");
            saveProgression.Save(@"../Save/PlayerProgression.xml");
        }

        
    }
}
