using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class AnimatedScreen : Component
    {
        #region Properties
        private float _minimumProcTimer = 1.0f;

        public float MinimumProcTimer
        {
            get { return _minimumProcTimer; }
            set { _minimumProcTimer = value; }
        }

        private float _maximumProcTimer = 3.0f;

        public float MaximumProcTimer
        {
            get { return _maximumProcTimer; }
            set { _maximumProcTimer = value; }
        }

        private string _idleAnimation = "";

        public string IdleAnimation
        {
            get { return _idleAnimation; }
            set { _idleAnimation = value; }
        }

        private string _firstAnimation = "";

        public string FirstAnimation
        {
            get { return _firstAnimation; }
            set { _firstAnimation = value; }
        }

        private float _firstAnimationRate = 0.0f;

        public float FirstAnimationRate
        {
            get { return _firstAnimationRate; }
            set { _firstAnimationRate = value; }
        }

        private string _secondAnimation = "";

        public string SecondAnimation
        {
            get { return _secondAnimation; }
            set { _secondAnimation = value; }
        }

        private float _secondAnimationRate = 0.0f;

        public float SecondAnimationRate
        {
            get { return _secondAnimationRate; }
            set { _secondAnimationRate = value; }
        }

        private string _thirdAnimation = "";

        public string ThirdAnimation
        {
            get { return _thirdAnimation; }
            set { _thirdAnimation = value; }
        }

        private float _thirdAnimationRate = 0.0f;

        public float ThirdAnimationRate
        {
            get { return _thirdAnimationRate; }
            set { _thirdAnimationRate = value; }
        }

        private float _fourthAnimationRate = 0.0f;

        public float FourthAnimationRate
        {
            get { return _fourthAnimationRate; }
            set { _fourthAnimationRate = value; }
        }

        private string _fourthAnimation = "";

        public string FourthAnimation
        {
            get { return _fourthAnimation; }
            set { _fourthAnimation = value; }
        }

        private string _fifthAnimation = "";

        public string FifthAnimation
        {
            get { return _fifthAnimation; }
            set { _fifthAnimation = value; }
        }

        private float _fifthAnimationRate = 0.0f;

        public float FifthAnimationRate
        {
            get { return _fifthAnimationRate; }
            set { _fifthAnimationRate = value; }
        }
        #endregion

        private float _timer = 0.0f;
        private float _currentProcDuration = 0.0f;
        private Dictionary<string, float> _animations;

        public AnimatedScreen(Entity entity)
            :base(entity, "AnimatedScreen")
        {
            RequiredComponents = new Type[] { typeof(SpritesheetManager) };
        }

        public override void Init()
        {
            if (entity.spritesheets != null)
                entity.spritesheets.ChangeAnimation(_idleAnimation, 0, true, false, true);
            _currentProcDuration = (float)Neon.Utils.GetRandomNumber(_minimumProcTimer, _maximumProcTimer);

            _animations = new Dictionary<string, float>();

            if (_firstAnimation != "")
                _animations.Add(_firstAnimation, _firstAnimationRate);
            if (_secondAnimation != "")
                _animations.Add(_secondAnimation, _secondAnimationRate);
            if (_thirdAnimation != "")
                _animations.Add(_thirdAnimation, _thirdAnimationRate);
            if (_fourthAnimation != "")
                _animations.Add(_fourthAnimation, _fourthAnimationRate);
            if (_fifthAnimation != "")
                _animations.Add(_fifthAnimation, _fifthAnimationRate);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (entity.spritesheets != null & _currentProcDuration != 0.0f)
            {
                if (entity.spritesheets.CurrentSpritesheetName == _idleAnimation)
                {
                    if (_timer >= _currentProcDuration)
                    {
                        float random = (float)Neon.Utils.GetRandomNumber(0.0f, 100.0f);
                        string selectedAnimation = "";

                        int i = 0;
                        float excludedPart = 0.0f;

                        while (i < _animations.Count)
                        {
                            if (random > excludedPart && random < excludedPart + _animations.ElementAt(i).Value)
                            {
                                selectedAnimation = _animations.ElementAt(i).Key;
                                break;
                            }
                            else
                            {
                                excludedPart += _animations.ElementAt(i).Value;
                                i++;
                            }
                        }

                        entity.spritesheets.ChangeAnimation(selectedAnimation, 0, true, false, false);

                        _timer = 0.0f;
                        _currentProcDuration = (float)Neon.Utils.GetRandomNumber(_minimumProcTimer, _maximumProcTimer);
                    }
                    else
                    {
                        _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    }
                }

                if (entity.spritesheets.CurrentSpritesheet.IsFinished && entity.spritesheets.CurrentSpritesheetName != _idleAnimation)
                {
                    entity.spritesheets.ChangeAnimation(_idleAnimation);
                }
            }
            base.Update(gameTime);
        }
    }
}
