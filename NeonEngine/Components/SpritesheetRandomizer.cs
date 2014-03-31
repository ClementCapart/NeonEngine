using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class SpritesheetRandomizer : SpritesheetManager
    {

        #region Properties
        private float _delayBetweenAnimation = 1.0f;

        public float DelayBetweenAnimation
        {
            get { return _delayBetweenAnimation; }
            set { _delayBetweenAnimation = value; }
        }

        private bool _canPlayTheSameTwice = false;

        public bool CanPlayTheSameTwice
        {
            get { return _canPlayTheSameTwice; }
            set { _canPlayTheSameTwice = value; }
        }
        #endregion

        private Random _random;
        private bool _startChanging = false;
        private float _timer;

        public SpritesheetRandomizer(Entity entity)
            :base(entity)
        {
            Name = "SpritesheetRandomizer";
        }

        public override void Init()
        {
            _random = Neon.Utils.CommonRandom;
            if (SpritesheetList.Count > 0)
            {
                int random = _random.Next(0, SpritesheetList.Count);
                ChangeAnimation(SpritesheetList.ElementAt(random).Key, 0, true, false, false);
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if ((CurrentSpritesheet.IsFinished || CurrentSpritesheet.spriteSheetInfo.Frames == null) && SpritesheetList.Count > 0 && !_startChanging)
            {
                _startChanging = true;
                _timer = 0.0f;
            }
            if (_startChanging)
            {
                if (_timer < _delayBetweenAnimation)
                {
                    _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _startChanging = false;
                    if (SpritesheetList.Count > 0)
                    {
                        int random = _random.Next(0, SpritesheetList.Count);
                        if (!_canPlayTheSameTwice && SpritesheetList.Count > 1)
                            while (SpritesheetList.ElementAt(random).Key == CurrentSpritesheetName)
                                random = _random.Next(0, SpritesheetList.Count);
                        ChangeAnimation(SpritesheetList.ElementAt(random).Key, true, 0, true, true, false);
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
