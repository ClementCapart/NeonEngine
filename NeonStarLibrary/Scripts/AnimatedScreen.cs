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

        public AnimatedScreen(Entity entity)
            :base(entity, "AnimatedScreen")
        {
            RequiredComponents = new Type[] { typeof(SpritesheetManager) };
        }

        public override void Init()
        {
            base.Init();
        }
    }
}
