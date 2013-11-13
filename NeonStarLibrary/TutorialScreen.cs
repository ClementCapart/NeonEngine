using NeonEngine;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class TutorialScreen : ScriptComponent
    {
        private string _walkTriggerName = "";

        public string WalkTriggerName
        {
            get { return _walkTriggerName; }
            set { _walkTriggerName = value; }
        }

        private string _jumpTriggerName = "";

        public string JumpTriggerName
        {
            get { return _jumpTriggerName; }
            set { _jumpTriggerName = value; }
        }

        private string _fallTriggerName = "";

        public string FallTriggerName
        {
            get { return _fallTriggerName; }
            set { _fallTriggerName = value; }
        }

        private string _walkTutorialAnimation = "";

        public string WalkTutorialAnimation
        {
            get { return _walkTutorialAnimation; }
            set { _walkTutorialAnimation = value; }
        }

        private string _jumpTutorialAnimation = "";

        public string JumpTutorialAnimation
        {
            get { return _jumpTutorialAnimation; }
            set { _jumpTutorialAnimation = value; }
        }

        private string _fallTutorialAnimation = "";

        public string FallTutorialAnimation
        {
            get { return _fallTutorialAnimation; }
            set { _fallTutorialAnimation = value; }
        }

        private string _hitTutorialAnimation = "";

        public string HitTutorialAnimation
        {
            get { return _hitTutorialAnimation; }
            set { _hitTutorialAnimation = value; }
        }

        private string _comboTutorialAnimation = "";

        public string ComboTutorialAnimation
        {
            get { return _comboTutorialAnimation; }
            set { _comboTutorialAnimation = value; }
        }

        private string _uppercutTutorialAnimation = "";

        public string UppercutTutorialAnimation
        {
            get { return _uppercutTutorialAnimation; }
            set { _uppercutTutorialAnimation = value; }
        }

        private string _dodgeTutorialAnimation = "";

        public string DodgeTutorialAnimation
        {
            get { return _dodgeTutorialAnimation; }
            set { _dodgeTutorialAnimation = value; }
        }


        public TutorialScreen(Entity entity)
            :base(entity, "TutorialScreen")
        {
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity)
        {
            if (trigger.Name == _walkTriggerName)
            {
                entity.spritesheets.ChangeAnimation(_jumpTutorialAnimation);
            }
            else if (trigger.Name == _jumpTriggerName)
            {
                entity.spritesheets.ChangeAnimation(_fallTutorialAnimation);
            }
            else if (trigger.Name == _fallTriggerName)
            {
                entity.spritesheets.ChangeAnimation(_hitTutorialAnimation);
            }
            base.OnTrigger(trigger, triggeringEntity);
        }
    }
}
