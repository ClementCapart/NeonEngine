using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class ButtonsScript : Component
    {
        Entity _buttonDive;
        Entity _buttonUppercut;

        private float _timer = 0.0f;
        private float _buttonDelay = 1f;
        private AvatarCore _avatar;

        private Entity _buttonPushed = null;
        private bool _activated = false;

        private Door _3rdDoor;

        public ButtonsScript(Entity entity)
            :base(entity, "ButtonsScript")
        {

        }

        public override void Init()
        {
            _buttonDive = entity.GameWorld.GetEntityByName("ButtonDive");
            _buttonDive.spritesheets.ChangeAnimation("ButtonIdle");
            _buttonUppercut = entity.GameWorld.GetEntityByName("ButtonUppercut");
            _buttonUppercut.spritesheets.ChangeAnimation("ButtonIdle");
            _avatar = entity.GameWorld.GetEntityByName("LiOn").GetComponent<AvatarCore>();
            _3rdDoor = entity.GameWorld.GetEntityByName("03Door").GetComponent<Door>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        
            if (!_activated)
            {
                if (_buttonPushed != null)
                {
                    if (_timer >= _buttonDelay)
                    {
                        _timer = 0.0f;
                        _buttonPushed.spritesheets.ChangeAnimation("ButtonDeactivation", true, 0, true, false, false);
                        _buttonPushed = null;
                    }
                    _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                }

                if (_buttonUppercut.spritesheets.CurrentSpritesheetName == "ButtonDeactivation" && _buttonUppercut.spritesheets.IsFinished())
                {
                    _buttonUppercut.spritesheets.ChangeAnimation("ButtonIdle");
                }

                if (_buttonDive.spritesheets.CurrentSpritesheetName == "ButtonDeactivation" && _buttonDive.spritesheets.IsFinished())
                {
                    _buttonDive.spritesheets.ChangeAnimation("ButtonIdle");
                }
            }
            
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (!_activated)
            {
                if (triggeringEntity.Name == "ButtonDive")
                {
                    if (_avatar.MeleeFight.CurrentAttack != null && _avatar.MeleeFight.CurrentAttack.Name == "LiOnDiveAttack")
                    {
                        if (_buttonPushed == null)
                        {
                            _buttonPushed = triggeringEntity;
                            _buttonPushed.spritesheets.ChangeAnimation("ButtonActivation", 0, true, false, false);
                        }
                        else if(_buttonPushed != _buttonDive)
                        {
                            _buttonPushed = null;
                            triggeringEntity.spritesheets.ChangeAnimation("ButtonActivation", 0, true, false, false);
                            _activated = true;
                            _3rdDoor.OpenDoor();
                        }
                    }
                }
                else if (triggeringEntity.Name == "ButtonUppercut")
                {
                    if (_avatar.MeleeFight.CurrentAttack != null && _avatar.MeleeFight.CurrentAttack.Name == "LiOnUppercut")
                    {
                        if (_buttonPushed == null)
                        {
                            _buttonPushed = triggeringEntity;
                            _buttonPushed.spritesheets.ChangeAnimation("ButtonActivation", 0, true, false, false);
                        }
                        else if (_buttonPushed != _buttonUppercut)
                        {
                            _buttonPushed = null;
                            triggeringEntity.spritesheets.ChangeAnimation("ButtonActivation", 0, true, false, false);
                            _activated = true;
                            _3rdDoor.OpenDoor();
                        }
                    }
                }
            }
            
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
