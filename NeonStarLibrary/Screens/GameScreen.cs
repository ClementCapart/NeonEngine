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
using NeonStarLibrary.Components.Enemies;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.GameplayElements;
using NeonStarLibrary.Components.EnergyObjects;
using NeonStarLibrary.Components.Camera;

namespace NeonStarLibrary
{
    public class GameScreen : World
    {
        public List<EnemyCore> enemies;
        public List<Attack> FreeAttacks = new List<Attack>();
        public List<Entity> Bullets = new List<Entity>();

        public NeonPool<Entity> BulletsPool;
        public bool MustFollowAvatar = true;


        //Sounds obvious but still, I don't know...//
        public Entity avatar;
        public AvatarCore _avatarComponent;

        public CameraFocus CameraFocus;

        //----------------------------------------//

        public static List<XElement> CheckPointsData = new List<XElement>();


        protected XElement _statusToLoad;
        protected int lastSpawnPointIndex;

        public string AvatarName = "LiOn";

        public GameScreen(string groupName, string levelName, int startingSpawnPointIndex, XElement statusToLoad, Game game, bool respawning = false)
            : base(game)
        {
            this._statusToLoad = statusToLoad;
            lastSpawnPointIndex = startingSpawnPointIndex;

            enemies = new List<EnemyCore>();
            
            BulletsPool = new NeonPool<Entity>(() => new Entity(this));

            BulletsManager.LoadBullets();
            AttacksManager.LoadAttacks();
            ElementManager.LoadElementParameters();
            if (!DeviceManager.AlreadyLoaded)
                DeviceManager.LoadDevicesInformation();

            if (statusToLoad != null)
                DeviceManager.LoadDeviceProgression(statusToLoad.Element("Devices"));

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
                if (File.Exists(@"../Data/Prefabs/" + AvatarName + ".prefab"))
                {
                    avatar = DataManager.LoadPrefab(@"../Data/Prefabs/" + AvatarName + ".prefab", this);
                    if (File.Exists(@"../data/Prefabs/HUD.prefab"))
                        DataManager.LoadPrefab(@"../Data/Prefabs/HUD.prefab", this);
                    avatar.transform.Position = currentSpawnPoint.Position;
                    _avatarComponent = avatar.GetComponent<AvatarCore>();
                    CameraFocus = avatar.GetComponent<CameraFocus>();
                    if (_avatarComponent != null)
                    {
                        LoadAvatarStatus(_avatarComponent, respawning);
                        _avatarComponent.CurrentSide = currentSpawnPoint.Side;
                    }
                }         
            }
            else
                Console.WriteLine("Warning : SpawnPoint "+ startingSpawnPointIndex + " not found, Avatar won't be created, please select an existing SpawnPoint");

            if(levelName != "")
                LoadLevel(new Level(groupName, levelName, this, true));

            Camera.Bounded = true;

            if (_statusToLoad != null)
            {
                XElement collectiblesData = _statusToLoad.Element("GatheredCollectibles");
                if (collectiblesData != null)
                    CollectibleManager.InitializeCollectiblesFromCheckpointData(collectiblesData);
            }
            CollectibleManager.InitializeCollectibles(this);

            foreach (Device d in DeviceManager.GetLevelDevices(groupName, levelName))
            {
                Entity e = GetEntityByName(d.DeviceName);
                if (e != null)
                {
                    EnergyDevice ed = e.GetComponent<EnergyDevice>();
                    if(ed != null)
                        ed.State = d.State;

                    ed.Init();
                }
            }
        } 

        public void LoadAvatarStatus(AvatarCore avatarComponent, bool respawning = false)
        {
            if (_statusToLoad != null && avatarComponent != null)
            {
                XElement liOn = _statusToLoad.Element("PlayerStatus").Element("LiOnParameters");
                if (!respawning)
                {
                    avatarComponent.CurrentHealthPoints = float.Parse(liOn.Element("HealthPoints").Value);
                
                    if (avatarComponent.ElementSystem != null)
                    {
                        avatarComponent.ElementSystem.LeftSlotElement = (Element)Enum.Parse(typeof(Element), liOn.Element("LeftElement").Value);
                        avatarComponent.ElementSystem.LeftSlotLevel = float.Parse(liOn.Element("LeftElement").Attribute("Level").Value);
                        avatarComponent.ElementSystem.RightSlotElement = (Element)Enum.Parse(typeof(Element), liOn.Element("RightElement").Value);
                        avatarComponent.ElementSystem.RightSlotLevel = float.Parse(liOn.Element("RightElement").Attribute("Level").Value);
                    }
                }
                
                if (avatarComponent.EnergySystem != null)
                {
                    avatarComponent.EnergySystem.CurrentEnergyStock = float.Parse(liOn.Element("Energy").Value);
                }

                if(respawning)
                    _avatarComponent.State = AvatarState.Respawning;
            }
        }

        public override void PreUpdate(GameTime gameTime)
        {
            if (Alpha != 0.0f && _avatarComponent != null)
            {
                _avatarComponent.CanAttack = false;
                _avatarComponent.CanMove = false;
                _avatarComponent.CanTurn = false;
                _avatarComponent.CanUseElement = false;
                _avatarComponent.CanRoll = false;
            }
            else
            {
                if(avatar != null)
                    if(avatar.spritesheets.CurrentSpritesheetName == _avatarComponent.RespawnAnimation)
                        avatar.spritesheets.CurrentSpritesheet.isPlaying = true;
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {
                if (MustFollowAvatar && avatar != null && _avatarComponent != null && _avatarComponent.CurrentHealthPoints > 0.0f && CameraFocus != null)
                {
                    Camera.Chase(avatar.transform.Position, CameraFocus.FocusDisplacement, CameraFocus.IgnoreSoftBounds, gameTime);
                }
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

        public override void ChangeLevel(string groupName, string levelName, int spawnPointIndex)
        {
            ChangeScreen(new LoadingScreen(Neon.Game, spawnPointIndex, groupName, levelName, SaveStatus()));
        }

        public override void ChangeLevel(XElement savedStatus)
        {
            ChangeScreen(new LoadingScreen(Neon.Game, savedStatus));
        }

        public void Respawn()
        {
            if (CheckPointsData.Count > 0)
            {
                ChangeLevel(CheckPointsData.Last());
            }
            else
            {
                
            }
        }

        public XElement SaveStatus(CheckPoint cp = null)
        {
            XElement playerProgression = new XElement("PlayerProgression");

            XElement currentLevel = new XElement("CurrentLevel");

            XElement currentGroupName = new XElement("GroupName", this.LevelGroupName);
            XElement currentLevelName = new XElement("LevelName", this.LevelName);
            XElement currentSpawnPoint = new XElement("SpawnPoint", cp != null ? cp.SpawnPointIndex.ToString() : "None");
            currentLevel.Add(currentGroupName);
            currentLevel.Add(currentLevelName);
            currentLevel.Add(currentSpawnPoint);

            playerProgression.Add(currentLevel);

            XElement playerStatus = new XElement("PlayerStatus");

            AvatarCore avatarComponent = null;
            if (avatar != null)
                avatarComponent = avatar.GetComponent<AvatarCore>();

            if (avatarComponent != null)
            {
                XElement liOn = new XElement("LiOnParameters");

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

            playerProgression.Add(playerStatus);

            XElement deviceStatus = DeviceManager.SaveDeviceProgression();

            playerProgression.Add(deviceStatus);

            XElement collectibleStatus = CollectibleManager.SaveCollectiblesState();

            playerProgression.Add(collectibleStatus);

            return playerProgression;
        }

        public void SaveProgressionToFile()
        {
            XDocument saveProgression = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
    
            saveProgression.Add(SaveStatus());

            if(!Directory.Exists(@"../Save/"))
                Directory.CreateDirectory(@"../Save/");
            saveProgression.Save(@"../Save/PlayerProgression.xml");
        }

        
    }
}
