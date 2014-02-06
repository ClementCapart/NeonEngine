using NeonEngine.Components.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class TriggeredAnimation : SpriteSheet
    {
        #region Properties
        private bool _startPlaying = false;

        public bool StartPlaying
        {
            get { return _startPlaying; }
            set { _startPlaying = value; }
        }

        private bool _playReverseOnSecondTrigger = false;

        public bool PlayReverseOnSecondTrigger
        {
            get { return _playReverseOnSecondTrigger; }
            set { _playReverseOnSecondTrigger = value; }
        }

        private bool _loop;
         
        public bool Loop
        {
            get { return _loop; }
            set { _loop = value; }
        }
        #endregion

        public TriggeredAnimation(Entity entity)
            :base(entity)
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
            Name = "TriggeredAnimation";
        }

        public override void Init()
        {
            this.isPlaying = _startPlaying;

            this.IsLooped = _loop;
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (_playReverseOnSecondTrigger && IsFinished)
            {
                this.Reverse = !Reverse;
            }
            else
                currentFrame = 0;

            isPlaying = !isPlaying;

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
