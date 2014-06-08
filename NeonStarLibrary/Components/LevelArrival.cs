using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components
{
    public class LevelArrival : Component
    {
        private float _loopTime = 1.0f;

        public float LoopTime
        {
            get { return _loopTime; }
            set { _loopTime = value; }
        }

        private SpritesheetManager _frontierArrival;
        private SpritesheetManager _lowerCityArrival;

        private bool _displayingName = false;
        private SpritesheetManager _currentDisplayer = null;
        private float _loopTimer = 0.0f;
        

        public static string NextLevelToDisplay = "";
        public static bool WaitForNextLevel = true;

        public LevelArrival(Entity entity)
            :base(entity, "LevelArrival")
        {
        }

        public override void Init()
        {
            WaitForNextLevel = false;
            _frontierArrival = entity.GetComponentByNickname("Frontier") as SpritesheetManager;
            _lowerCityArrival = entity.GetComponentByNickname("LowerCity") as SpritesheetManager;

            _lowerCityArrival.Opacity = 0.0f;
            _frontierArrival.Opacity = 0.0f;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.GameWorld.Alpha <= 0 && NextLevelToDisplay != "" && !WaitForNextLevel)
            {
                DisplayLevelName(NextLevelToDisplay);
                NextLevelToDisplay = "";
            }

            if (_currentDisplayer != null)
            {
                if (_currentDisplayer.CurrentSpritesheetName == "FadeIn" && _currentDisplayer.CurrentSpritesheet.IsFinished)
                    _currentDisplayer.ChangeAnimation("Loop");

                if (_currentDisplayer.CurrentSpritesheetName == "Loop")
                {
                    if (_loopTimer < _loopTime)
                    {
                        _loopTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                    else
                    {
                        _loopTimer = 0.0f;
                        _currentDisplayer.ChangeAnimation("FadeOut", 0, true, false, false);
                    }
                }

                if (_currentDisplayer.CurrentSpritesheetName == "FadeOut" && _currentDisplayer.CurrentSpritesheet.IsFinished)
                {
                    _currentDisplayer = null;
                    _displayingName = false;
                }
            }

            if (_displayingName)
            {
                _lowerCityArrival.Opacity = 1.0f;
                _frontierArrival.Opacity = 1.0f;
            }
            else
            {
                _lowerCityArrival.Opacity = 0.0f;
                _frontierArrival.Opacity = 0.0f;
            }
            base.Update(gameTime);
        }

        public void DisplayLevelName(string levelName)
        {
            switch(levelName)
            {
                case "Frontier":
                    _currentDisplayer = _frontierArrival;
                    break;

                case "LowerCity":
                    _currentDisplayer = _lowerCityArrival;
                    break;
            }

            if (_currentDisplayer != null)
            {
                _displayingName = true;
                _currentDisplayer.ChangeAnimation("FadeIn", 0, true, false, false);
            }
        }
    }
}
