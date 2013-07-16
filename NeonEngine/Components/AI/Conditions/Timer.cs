using NeonEngine.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public class Timer : AICondition
    {
        float _TargetTimer = 0f;
        float _CurrentTimer = 0f;

        public float TargetTimer 
        {
            get { return _TargetTimer; }
            set { _TargetTimer = value; }
        }

        public float StartingTimer
        {
            get { return _CurrentTimer; }
            set { _CurrentTimer = value; }
        }
        
        public Timer(float TargetTimer, float StartingTimer)
            :this()
        {
            this.StartingTimer = StartingTimer;
            this.TargetTimer = TargetTimer;
        }

        public Timer()
            :base("Timer")
        {

        }

        public override bool Check(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _CurrentTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_CurrentTimer >= _TargetTimer)
            {
                _CurrentTimer = 0;
                return true;
            }
            
            return false;
        }

        public override void Reset()
        {
            _CurrentTimer = StartingTimer;
            base.Reset();
        }
    }
}
