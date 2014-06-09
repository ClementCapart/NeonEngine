using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonEngine.Components.Utils;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using NeonStarLibrary.Components.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Menu
{
    public enum MenuState
    {
        PressStart,
        ModeSelect,
        ChapterSelect,
        StartingGame,
    }

    class MainMenu : Component
    {
        
        private MenuState _menuState = MenuState.PressStart;

        private Graphic _pressStart = null;
        private Graphic _newGame = null;
        private Graphic _chapterSelection = null;
        private Graphic _chapterSelect = null;
        private SpritesheetManager _chapterManager;

        private Graphic _title;
        private Graphic _titleShadow;
        private Graphic _titleGlow;

        private Vector2 _cameraTarget;

        private string _levelToGo = "";

        public Vector2 CameraTarget
        {
            get { return _cameraTarget; }
            set { _cameraTarget = value; }
        }

        private Vector2 _zoomTarget = Vector2.Zero;

        public Vector2 ZoomTarget
        {
            get { return _zoomTarget; }
            set { _zoomTarget = value; }
        }

        private ChangeOpacity _opacityComponent;
        private FollowOpacity _titleFollowOpacity;
        private bool _chapterSelectInitFinished = false;
        private bool _modeSelectInitFinished = false;

        private bool _firstUpdateDone = false;
        private int _currentSelection = 0;

        public MainMenu(Entity entity)
            :base(entity, "MainMenu")
        {
        }

        public override void Init()
        {
            List<Graphic> _graphicList = entity.GetComponentsByInheritance<Graphic>();
            if (_graphicList != null)
            {
                _pressStart = _graphicList.Where(g => g.NickName == "PressStart").FirstOrDefault();
                _newGame = _graphicList.Where(g => g.NickName == "NewGame").FirstOrDefault();
                _chapterSelect = _graphicList.Where(g => g.NickName == "ChapterSelect").FirstOrDefault();
                _chapterSelection = _graphicList.Where(g => g.NickName == "ChapterSelection").FirstOrDefault();
            }
            _opacityComponent = entity.GetComponent<ChangeOpacity>();
            _chapterManager = entity.spritesheets;
            ChangeState(MenuState.PressStart);
            Entity e = entity.GameWorld.GetEntityByName("Title");
            if (e != null)
            {
                _title = e.GetComponentByNickname("Logo") as Graphic;
                _titleShadow = e.GetComponentByNickname("Shadow") as Graphic;
            }
            e = entity.GameWorld.GetEntityByName("TitleGlow");
            if (e != null)
            {
                _titleGlow = e.GetComponent<Graphic>();
                _titleFollowOpacity = e.GetComponent<FollowOpacity>();
            }

            AvatarCore.TimeSinceLastCompletion = 0.0f;

            DeviceManager.AlreadyLoaded = false;
            DeviceManager.LoadDevicesInformation();
            GameScreen.CheckPointsData.Clear();
            CollectibleManager.ResetCollectibles();
            CollectibleManager.InitializeCollectibles(entity.GameWorld as GameScreen);
            (entity.GameWorld as GameScreen).PauseAllowed = false;
 	        base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!_firstUpdateDone)
            {
                DeviceManager.AlreadyLoaded = false;
                DeviceManager.LoadDevicesInformation();
                GameScreen.CheckPointsData.Clear();
                HealStation._usedHealStations.Clear();
                CollectibleManager.ResetCollectibles();
                CollectibleManager.InitializeCollectibles(entity.GameWorld as GameScreen);
                _firstUpdateDone = true;
            }
            switch(_menuState)
            {
                case MenuState.PressStart:                
                    if (Neon.Input.Pressed(NeonStarInput.Start))
                        ChangeState(MenuState.ModeSelect);
                    break;    

                case MenuState.ModeSelect:
                    if (!_modeSelectInitFinished)
                    {
                        if (Vector2.DistanceSquared(entity.GameWorld.Camera.Position, Vector2.Zero) < 5)
                        {
                            entity.GameWorld.Camera.Position = Vector2.Zero;
                            _modeSelectInitFinished = true;
                            _currentSelection = 0;
                            if (_newGame != null)
                            {
                                _newGame.Active = true;
                                _newGame.GraphicTag = "NewMainTitleNewGameOn";
                                _newGame.Opacity = 1.0f;
                            }
                            if (_chapterSelect != null)
                            {
                                _chapterSelect.Active = true;
                                _chapterSelect.GraphicTag = "NewMainTitleChaptersOff";
                                _chapterSelect.Opacity = 1.0f;
                            }
                            entity.transform.Scale = 1.0f;
                        }
                        else
                            entity.GameWorld.Camera.Position = Vector2.Lerp(entity.GameWorld.Camera.Position, Vector2.Zero, 0.05f);
                    }


                    if (Neon.Input.Pressed(NeonStarInput.MoveDown))
                    {
                        _currentSelection = (_currentSelection + 1) % 2;
                    }
                    else if (Neon.Input.Pressed(NeonStarInput.MoveUp))
                    {
                        _currentSelection = Math.Abs((_currentSelection - 1)) % 2;
                    }

                    if (_currentSelection == 0)
                    {
                        _newGame.GraphicTag = "NewMainTitleNewGameOn";
                        _chapterSelect.GraphicTag = "NewMainTitleChaptersOff";
                    }
                    else
                    {
                        _newGame.GraphicTag = "NewMainTitleNewGameOff";
                        _chapterSelect.GraphicTag = "NewMainTitleChaptersOn";
                    }
                    if (Neon.Input.Pressed(NeonStarInput.Guard))
                    {
                        ChangeState(MenuState.PressStart);
                    }


                    if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        if (_currentSelection == 0)
                            StartLevel("Start");
                        else
                            ChangeState(MenuState.ChapterSelect);
                    }

                    if (_title != null && _titleGlow != null && _titleShadow != null)
                    {
                        if (_title.Opacity < 1.0f)
                        {
                            _title.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                            _titleGlow.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                            _titleShadow.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                        }
                        else
                        {
                            _title.Opacity = 1.0f;
                            _titleGlow.Opacity = 1.0f;
                            _titleShadow.Opacity = 1.0f;
                        }
                    }

                    if (_chapterManager != null && _chapterSelection != null)
                    {
                        if (_chapterManager.Opacity > 0.0f && _chapterManager.Active)
                        {
                            _chapterManager.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                            _chapterSelection.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                        }
                        else
                        {
                            _chapterSelection.Active = false;
                            _chapterManager.Active = false;
                            _chapterSelection.Opacity = 1.0f;
                            _chapterManager.Opacity = 1.0f;
                        }
                    }

                    break;

                case MenuState.ChapterSelect:
                    if (!_chapterSelectInitFinished)
                    {
                        if (Vector2.DistanceSquared(entity.GameWorld.Camera.Position, _cameraTarget) < 5)
                        {
                            entity.GameWorld.Camera.Position = _cameraTarget;
                            _chapterManager.Active = true;
                            _chapterManager.ChangeAnimation("Fade", 0, true, false, false);
                            _chapterSelectInitFinished = true;
                            _currentSelection = 0;

                        }
                        else
                            entity.GameWorld.Camera.Position = Vector2.Lerp(entity.GameWorld.Camera.Position, _cameraTarget, 0.05f);
                    }
                    else
                    {
                        if (_chapterManager.CurrentSpritesheetName == "Fade" && _chapterManager.CurrentSpritesheet.IsFinished)
                            _chapterManager.ChangeAnimation("Frontier", 0, true, false, true);

                        if (_chapterManager.CurrentSpritesheet.IsLooped)
                        {
                            if (Neon.Input.Pressed(NeonStarInput.MoveLeft))
                            {
                                _currentSelection = Math.Abs((_currentSelection - 1)) % 2;
                            }
                            else if (Neon.Input.Pressed(NeonStarInput.MoveRight))
                            {
                                _currentSelection = (_currentSelection + 1) % 2;
                            }

                            switch (_currentSelection)
                            {
                                case 0:
                                    if (_chapterManager.CurrentSpritesheetName != "Frontier")
                                        _chapterManager.ChangeAnimation("Frontier", 0, true, false, true);
                                    break;

                                case 1:
                                    if (_chapterManager.CurrentSpritesheetName != "LowerCity")
                                        _chapterManager.ChangeAnimation("LowerCity", 0, true, false, true);
                                    break;
                            }

                            if (Neon.Input.Pressed(NeonStarInput.Jump))
                            {
                                switch (_currentSelection)
                                {
                                    case 0:
                                        StartLevel("Frontier");
                                        break;

                                    case 1:
                                        StartLevel("LowerCity");
                                        break;
                                }
                            }

                            if (Neon.Input.Pressed(NeonStarInput.Guard))
                            {
                                ChangeState(MenuState.ModeSelect);
                            }
                        }
                        if (_chapterSelection.Opacity < 1.0f)
                        {
                            _chapterSelection.Opacity += (float)gameTime.ElapsedGameTime.TotalSeconds * 1.0f;
                        }
                        else
                        {
                            _chapterSelection.Opacity = 1.0f;
                        }
                    }

                    

                    

                    if (_title != null && _titleGlow != null && _titleShadow != null)
                    {
                        if (_title.Opacity > 0.0f)
                        {
                            _title.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                            _titleGlow.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                            _titleShadow.Opacity -= (float)gameTime.ElapsedGameTime.TotalSeconds * 2.0f;
                        }
                        else
                        {
                            _title.Opacity = 0.0f;
                            _titleGlow.Opacity = 0.0f;
                            _titleShadow.Opacity = 0.0f;
                        }
                    }

                    
                    
                    break;

                case MenuState.StartingGame:
                    if (_levelToGo != "")
                    {
                        entity.GameWorld.Camera.Zoom += (float)gameTime.ElapsedGameTime.TotalSeconds * 9;
                        entity.GameWorld.Camera.Position = Vector2.Lerp(entity.GameWorld.Camera.Position, _zoomTarget, 0.1f);
                        entity.GameWorld.Camera.Rotate((float)gameTime.ElapsedGameTime.TotalSeconds * 0.4f);
                        if (entity.GameWorld.Camera.Zoom >= 8.0f && entity.GameWorld.NextScreen == null)
                        {
                            switch(_levelToGo)
                            {
                                case "Start":
                                    entity.GameWorld.ChangeLevel("01TrainingLevel", "00TrainingOpening", 0);
                                    break;

                                case "Frontier":
                                    entity.GameWorld.ChangeLevel("01TrainingLevel", "01TrainingEntrance", 0);
                                    break;

                                case "LowerCity":
                                    entity.GameWorld.ChangeLevel("02CityLevel", "01CityTrainStationEntrance", 0);
                                    break;
                            }
                        }
                    }
                    break;
            }

            
            base.Update(gameTime);
        }

        private void StartLevel(string level)
        {
            switch(level)
            {
                case "Start":
                    AvatarCore.StartedNewGame = true;
                    _levelToGo = "Start";
                    break;

                case "Frontier":
                    AvatarCore.StartedNewGame = false;
                    LevelArrival.NextLevelToDisplay = "Frontier";
                    _levelToGo = "Frontier";
                    break;

                case "LowerCity":
                    AvatarCore.StartedNewGame = false;
                    LevelArrival.NextLevelToDisplay = "LowerCity";
                    _levelToGo = "LowerCity";
                    break;
            }
            ChangeState(MenuState.StartingGame);
        }

        public void ChangeState(MenuState newState)
        {
            _menuState = newState;

            switch(newState)
            {
                case MenuState.PressStart:
                    if (_titleFollowOpacity != null)
                        _titleFollowOpacity.Active = true;
                    if (_opacityComponent != null)
                        _opacityComponent.Active = true;
                    if (_pressStart != null)
                        _pressStart.Active = true;
                    if (_newGame != null)
                        _newGame.Active = false;
                    if (_chapterSelect != null)
                        _chapterSelect.Active = false;
                    if (_chapterManager != null)
                        _chapterManager.Active = false;
                    if (_chapterSelection != null)
                        _chapterSelection.Active = false;
                    entity.transform.Scale = 1.0f;
                    break;

                case MenuState.ModeSelect:
                    _modeSelectInitFinished = false;
                    _currentSelection = 0;
                    if (_titleFollowOpacity != null)
                        _titleFollowOpacity.Active = true;
                    if (_opacityComponent != null)
                        _opacityComponent.Active = false;
                    if (_pressStart != null)
                        _pressStart.Active = false;
                   
                    break;

                case MenuState.ChapterSelect:
                    _currentSelection = 0;
                    _chapterSelectInitFinished = false;
                    if (_titleFollowOpacity != null)
                        _titleFollowOpacity.Active = false;
                        _currentSelection = 0;
                        if (_opacityComponent != null)
                            _opacityComponent.Active = false;
                        if (_pressStart != null)
                            _pressStart.Active = false;
                        if (_newGame != null)
                            _newGame.Active = false;
                        if (_chapterSelect != null)
                            _chapterSelect.Active = false;
                        if (_chapterSelection != null)
                        {
                            _chapterSelection.Active = true;
                            _chapterSelection.Opacity = 0.0f;
                        }

                        entity.transform.Scale = 2.0f;
                    break;

                case MenuState.StartingGame:
                    if (_titleFollowOpacity != null)
                        _titleFollowOpacity.Active = false;
                    if (_opacityComponent != null)
                        _opacityComponent.Active = false;
                    if (_pressStart != null)
                        _pressStart.Active = false;
                    if (_newGame != null)
                        _newGame.Active = false;
                    if (_chapterSelect != null)
                        _chapterSelect.Active = false;
                    if (_chapterManager != null)
                        _chapterManager.Active = false;
                    if (_chapterSelection != null)
                        _chapterSelection.Active = false;
                    if (_title != null)
                        _title.Active = false;
                    if (_titleGlow != null)
                        _titleGlow.Active = false;
                    entity.transform.Scale = 1.0f;
                    break;

            }     
        }
       

    }
}
