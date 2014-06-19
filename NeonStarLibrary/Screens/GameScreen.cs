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
using NeonStarLibrary.Components;

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
        public AvatarCore _avatarComponent;

        public CameraFocus CameraFocus;

        public Texture2D _pauseMenuTexture;

        public bool PauseAllowed = true;

        //----------------------------------------//

        public static List<XElement> CheckPointsData = new List<XElement>();
        public Vector2 InstantRespawnPoint = new Vector2(float.MaxValue, float.MaxValue);

        protected XElement _statusToLoad;
        protected int lastSpawnPointIndex;

        public static MapScreen Map;

        public string AvatarName = "LiOn";

        public GameScreen(string groupName, string levelName, int startingSpawnPointIndex, XElement statusToLoad, Game game, bool respawning = false)
            : base(game)
        {
            LevelGroupName = groupName;
            LevelName = levelName;
            
            if (Map == null)
                Map = new MapScreen(game);
          
            Map.CurrentGameScreen = this;
            if (groupName == "00TitleScreen")
                statusToLoad = null;
            Map.InitializeMapData(statusToLoad);
            Map.AddLevelToMap(groupName, levelName);

            _pauseMenuTexture = AssetManager.GetTexture("PauseMenu");
            this._statusToLoad = statusToLoad;
            lastSpawnPointIndex = startingSpawnPointIndex;

            enemies = new List<EnemyCore>();
            
            BulletsPool = new NeonPool<Entity>(() => new Entity(this));

            BulletsManager.LoadBullets();
            AttacksManager.LoadAttacks();
            ElementManager.LoadElementParameters();
            if (!DeviceManager.AlreadyLoaded)
                DeviceManager.LoadDevicesInformation();

            if (statusToLoad != null && respawning)
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
                    Avatar = DataManager.LoadPrefab(@"../Data/Prefabs/" + AvatarName + ".prefab", this);
                    if (File.Exists(@"../data/Prefabs/HUD.prefab"))
                        DataManager.LoadPrefab(@"../Data/Prefabs/HUD.prefab", this);
                    Avatar.transform.Position = currentSpawnPoint.Position;
                    
                }         
            }
            else
                Console.WriteLine("Warning : SpawnPoint "+ startingSpawnPointIndex + " not found, Avatar won't be created, please select an existing SpawnPoint");


            if(levelName != "")
                LoadLevel(new Level(groupName, levelName, this, true));
            if (Avatar != null)
            {
                _avatarComponent = Avatar.GetComponent<AvatarCore>();
                CameraFocus = Avatar.GetComponent<CameraFocus>();
                if (_avatarComponent != null)
                {
                    LoadAvatarStatus(_avatarComponent, respawning);
                    _avatarComponent.CurrentSide = currentSpawnPoint.Side;
                }
                if(!respawning)
                    Avatar.transform.Position = currentSpawnPoint.Position;
            }
            Camera.Bounded = true;

            if (_statusToLoad != null)
            {
                XElement collectiblesData = _statusToLoad.Element("GatheredCollectibles");
                if (collectiblesData != null)
                    CollectibleManager.InitializeCollectiblesFromCheckpointData(collectiblesData);
                CollectibleManager.InitializeCollectibles(this);
            }
            else
            {
                CollectibleManager.ResetCollectibles();
                CollectibleManager.InitializeCollectibles(this);
            }
            

            foreach (Device d in DeviceManager.GetLevelDevices(groupName, levelName))
            {
                Entity e = GetEntityByName(d.DeviceName);
                if (e != null)
                {
                    EnergyDevice ed = e.GetComponent<EnergyDevice>();
                    if(ed != null)
                        ed.State = d.State;
                    if(ed != null)
                        ed.Init();
                }
            }

            if (LevelName == "01CityTrainStationEntrance" && (CheckPointsData.Count == 0 || CheckPointsData.Last().Element("CurrentLevel").Element("GroupName").Value == "01TrainingLevel"))
            {
                CheckPointsData.Add(SaveStatus());
            }

            if (CheckPointsData.Count == 0 && _avatarComponent != null)
                GameScreen.CheckPointsData.Add(SaveStatus());
        } 

        public void LoadAvatarStatus(AvatarCore avatarComponent, bool respawning = false)
        {
            if (_statusToLoad != null && avatarComponent != null)
            {
                XElement liOn = _statusToLoad.Element("PlayerStatus").Element("LiOnParameters");
                if (liOn == null)
                    return;
                if (!respawning)
                {
                    avatarComponent.CurrentHealthPoints = float.Parse(liOn.Element("HealthPoints").Value);
                }

                if (avatarComponent.ElementSystem != null)
                {
                    avatarComponent.ElementSystem.MaxLevel = float.Parse(liOn.Element("MaxLevel").Value);

                    avatarComponent.ElementSystem.LeftSlotElementFirst = (Element)Enum.Parse(typeof(Element), liOn.Element("FirstLeftElement").Value);
                    avatarComponent.ElementSystem.RightSlotElementFirst = (Element)Enum.Parse(typeof(Element), liOn.Element("FirstRightElement").Value);

                    if (avatarComponent.ElementSystem.MaxLevel > 1)
                    {
                        avatarComponent.ElementSystem.LeftSlotElementSecond = (Element)Enum.Parse(typeof(Element), liOn.Element("SecondLeftElement").Value);
                        avatarComponent.ElementSystem.RightSlotElementSecond = (Element)Enum.Parse(typeof(Element), liOn.Element("SecondRightElement").Value);
                    }

                    if (avatarComponent.ElementSystem.MaxLevel > 2)
                    {
                        avatarComponent.ElementSystem.LeftSlotElementThird = (Element)Enum.Parse(typeof(Element), liOn.Element("ThirdLeftElement").Value);
                        avatarComponent.ElementSystem.RightSlotElementThird = (Element)Enum.Parse(typeof(Element), liOn.Element("ThirdRightElement").Value);
                    }
                }

                if (avatarComponent.EnergySystem != null)
                {
                    avatarComponent.EnergySystem.CurrentEnergyStock = float.Parse(liOn.Element("Energy").Value);
                }

                if(respawning)
                {
                    _avatarComponent.State = AvatarState.FastRespawning;

                    Entity e = GetEntityByName("SavePoint");
                    if (e != null)
                    {
                        _avatarComponent.entity.transform.Position = e.transform.Position;
                        SaveRoom saveRoom = e.GetComponent<SaveRoom>();
                        if (saveRoom != null)
                        {
                            saveRoom.Respawn();
                        }
                    }

                }

                if (Avatar != null)
                {
                    if (Avatar.spritesheets.CurrentSpritesheetName == _avatarComponent.RespawnAnimation)
                        Avatar.spritesheets.CurrentSpritesheet.isPlaying = true;
                    Camera.Position = Avatar.transform.Position;
                }
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
                if (Avatar != null)
                {
                    if (Avatar.spritesheets.CurrentSpritesheetName == _avatarComponent.RespawnAnimation)
                        Avatar.spritesheets.CurrentSpritesheet.isPlaying = true;
                }
            }
            base.PreUpdate(gameTime);
        }

        public override void Update(GameTime gameTime)
        {


            if (!Pause && !ForcedPause)
            {
                if (MustFollowAvatar && Avatar != null && _avatarComponent != null && _avatarComponent.CurrentHealthPoints > 0.0f && CameraFocus != null && this.NextScreen == null)
                {
                    Camera.Chase(Avatar.transform.Position, CameraFocus.FocusDisplacement, CameraFocus.IgnoreSoftBounds, gameTime);
                }
                else if (Avatar == null)
                {
                    Avatar = this.GetEntityByName("LiOn");
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


            if (Neon.Input.Pressed(NeonStarInput.Start) && PauseAllowed)
            {
                Pause = !Pause;
                if (Pause)
                {
                    if (SoundManager.CurrentTrack != null)
                        SoundManager.CurrentTrack.Pause();
                }
                else
                {
                    if (SoundManager.CurrentTrack != null)
                        SoundManager.CurrentTrack.Resume();
                }

            }

            if (Neon.Input.Pressed(NeonStarInput.Map) && !Pause && LevelGroupName == "02CityLevel")
            {
                Map.RefreshMapData();
                Neon.World = Map;              
            }

            if (Pause)
            {
                if (Neon.Input.Pressed(NeonStarInput.Map))
                    this.ChangeLevel("00TitleScreen", "01TitleScreenMain", 0);
            }

            base.Update(gameTime);
        }

        public virtual void ReloadLevel()
        {
            ChangeScreen(new GameScreen(this.LevelMap.Group, this.LevelMap.Name, lastSpawnPointIndex, _statusToLoad, game));
        }

        public override void ChangeLevel(string groupName, string levelName, int spawnPointIndex)
        {
            ChangeScreen(new LoadingScreen(Neon.Game, LevelGroupName == groupName ? true : false, spawnPointIndex, groupName, levelName, SaveStatus()));
        }

        public override void ChangeLevel(XElement savedStatus)
        {
            ChangeScreen(new LoadingScreen(Neon.Game, savedStatus));
        }

        public void Respawn(bool instantRespawn = false)
        {
            if (CheckPointsData.Count > 0)
            {
                switch(CheckPointsData.Last().Element("CurrentLevel").Element("GroupName").Value)
                {
                    case "01TrainingLevel":
                        LevelArrival.NextLevelToDisplay = "Frontier";
                        break;
                    case "02CityLevel":
                        LevelArrival.NextLevelToDisplay = "LowerCity";
                        break;
                }

                ChangeLevel(CheckPointsData.Last());
            }
            else
            {
                
            }
        }

        public XElement SaveStatus(SaveRoom cp = null)
        {
            XElement playerProgression = new XElement("PlayerProgression");

            XElement currentLevel = new XElement("CurrentLevel");

            XElement currentGroupName = new XElement("GroupName", this.LevelGroupName);
            XElement currentLevelName = new XElement("LevelName", this.LevelName);
            XElement currentSpawnPoint = new XElement("SpawnPoint", cp != null ? cp.SpawnPointIndex.ToString() : "0");
            currentLevel.Add(currentGroupName);
            currentLevel.Add(currentLevelName);
            currentLevel.Add(currentSpawnPoint);

            playerProgression.Add(currentLevel);

            if (Map != null)
                playerProgression.Add(Map.SaveMapStatus());

            XElement playerStatus = new XElement("PlayerStatus");

            AvatarCore avatarComponent = null;
            if (Avatar != null)
                avatarComponent = Avatar.GetComponent<AvatarCore>();
                
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
                    XElement currentFirstLeftElement = new XElement("FirstLeftElement", avatarComponent.ElementSystem.LeftSlotElementFirst.ToString());
                    XElement currentFirstRightElement = new XElement("FirstRightElement", avatarComponent.ElementSystem.RightSlotElementFirst.ToString());
                    XElement currentSecondLeftElement = new XElement("SecondLeftElement", avatarComponent.ElementSystem.LeftSlotElementSecond.ToString());
                    XElement currentSecondRightElement = new XElement("SecondRightElement", avatarComponent.ElementSystem.RightSlotElementSecond.ToString());
                    XElement currentThirdLeftElement = new XElement("ThirdLeftElement", avatarComponent.ElementSystem.LeftSlotElementThird.ToString());
                    XElement currentThirdRightElement = new XElement("ThirdRightElement", avatarComponent.ElementSystem.RightSlotElementThird.ToString());                   
                    
                    XElement maxLevel = new XElement("MaxLevel", avatarComponent.ElementSystem.MaxLevel.ToString());
                    liOn.Add(currentFirstLeftElement);
                    liOn.Add(currentFirstRightElement);
                    liOn.Add(currentSecondLeftElement);
                    liOn.Add(currentSecondRightElement);
                    liOn.Add(currentThirdLeftElement);
                    liOn.Add(currentThirdRightElement);
                    liOn.Add(maxLevel);
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

        public override void ManualDrawHUD(SpriteBatch sb)
        {
            if (Pause)
            {
                if(_pauseMenuTexture != null)
                    sb.Draw(_pauseMenuTexture, Vector2.Zero, Color.White);
            }
            base.ManualDrawHUD(sb);
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
