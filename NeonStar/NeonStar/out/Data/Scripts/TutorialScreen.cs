using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary;
using System;
using System.Collections.Generic;
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

        private string _enemyName = "";

        public string EnemyName
        {
            get { return _enemyName; }
            set { _enemyName = value; }
        }

        private string _avatarName = "";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _firstAttackName = "";

        public string FirstAttackName
        {
            get { return _firstAttackName; }
            set { _firstAttackName = value; }
        }

        private string _secondAttackName = "";

        public string SecondAttackName
        {
            get { return _secondAttackName; }
            set { _secondAttackName = value; }
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

        private string _doorToOpenName = "";

        public string DoorToOpenName
        {
            get { return _doorToOpenName; }
            set { _doorToOpenName = value; }
        }

        private string _dodgeTutorialAnimation = "";

        public string DodgeTutorialAnimation
        {
            get { return _dodgeTutorialAnimation; }
            set { _dodgeTutorialAnimation = value; }
        }

        private Entity _doorToOpen = null;

        private int _currentTutorialState = 1;
        private Avatar _avatar;

        public TutorialScreen(Entity entity)
            :base(entity, "TutorialScreen")
        {
        }

        public override void Init()
        {
            if(_avatarName != "")
                _avatar = Neon.world.GetEntityByName(_avatarName).GetComponent<Avatar>();
            entity.spritesheets.ChangeAnimation(_walkTutorialAnimation);

           if(_doorToOpenName != "")
                _doorToOpen = Neon.world.GetEntityByName(_doorToOpenName);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_doorToOpenName != "")
                if (_doorToOpen == null)
                    _doorToOpen = Neon.world.GetEntityByName(_doorToOpenName);
            base.Update(gameTime);
        }

        public override void PostUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.PostUpdate(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity)
        {
            if (trigger.Name == _walkTriggerName && _currentTutorialState == 1)
            {
                _currentTutorialState++;
                entity.spritesheets.ChangeAnimation(_jumpTutorialAnimation);
            }
            else if (trigger.Name == _jumpTriggerName && _currentTutorialState == 2)
            {
                _currentTutorialState++;
                entity.spritesheets.ChangeAnimation(_fallTutorialAnimation);
            }
            else if (trigger.Name == _fallTriggerName && _currentTutorialState == 3)
            {
                _currentTutorialState++;
                entity.spritesheets.ChangeAnimation(_hitTutorialAnimation);
            }
            else if (trigger.Name == _enemyName && _currentTutorialState == 4)
            {
                if (_avatar != null && _avatar.meleeFight.CurrentAttack.Name == _firstAttackName)
                {
                    _currentTutorialState++;
                    entity.spritesheets.ChangeAnimation(_comboTutorialAnimation);
                }
            }
            else if (trigger.Name == _enemyName && _currentTutorialState == 5)
            {
                if (_avatar != null && _avatar.meleeFight.CurrentAttack.Name == _firstAttackName + "Finish")
                {
                    _currentTutorialState++;
                    entity.spritesheets.ChangeAnimation(_uppercutTutorialAnimation);
                }
            }
            else if (trigger.Name == _enemyName && _currentTutorialState == 6)
            {
                if (_avatar != null && _avatar.meleeFight.CurrentAttack.Name == _secondAttackName)
                {
                    _currentTutorialState++;
                    entity.spritesheets.ChangeAnimation(_dodgeTutorialAnimation);
                    if (_doorToOpen != null)
                        _doorToOpen.Destroy();
                }
            }

            base.OnTrigger(trigger, triggeringEntity);
        }
    }
}
