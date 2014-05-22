using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnergyDoor : EnergyComponent
    {
        #region Properties
        private bool _openWhenAnimationEnd = false;

        public bool OpenWhenAnimationEnd
        {
            get { return _openWhenAnimationEnd; }
            set { _openWhenAnimationEnd = value; }
        }

        private string _closedIdleAnimation = "";

        public string ClosedIdleAnimation
        {
            get { return _closedIdleAnimation; }
            set { _closedIdleAnimation = value; }
        }

        private string _openedIdleAnimation = "";

        public string OpenedIdleAnimation
        {
            get { return _openedIdleAnimation; }
            set { _openedIdleAnimation = value; }
        }

        private string _doorOpeningAnimation = "";

        public string DoorOpeningAnimation
        {
            get { return _doorOpeningAnimation; }
            set { _doorOpeningAnimation = value; }
        }

        private bool _closingIsReverseOpening = true;

        public bool ClosingIsReverseOpening
        {
            get { return _closingIsReverseOpening; }
            set { _closingIsReverseOpening = value; }
        }

        private string _doorClosingAnimation = "";

        public string DoorClosingAnimation
        {
            get { return _doorClosingAnimation; }
            set { _doorClosingAnimation = value; }
        }

        #endregion

        private bool _opening = false;
        private bool _closing = false;
        public bool Closed = false;

        public EnergyDoor(Entity entity)
            :base(entity)
        {
            Name = "EnergyDoor";
        }

        public override void Init()
        {
            base.Init();

            if (_powered)
            {
                if (entity.spritesheets != null)
                {
                    entity.spritesheets.ChangeAnimation(_openedIdleAnimation, true);
                    Closed = false;
                    _opening = false;
                    if (entity.rigidbody != null)
                    {
                        entity.rigidbody.IsGround = false;
                        entity.rigidbody.Init();
                    }
                }           
            }
            else
            {
                if (entity.spritesheets != null)
                {
                    entity.spritesheets.ChangeAnimation(_closedIdleAnimation, true, 0, true, false, true);
                    _closing = false;
                    if (entity.rigidbody != null)
                    {
                        entity.rigidbody.IsGround = true;
                        entity.rigidbody.Init();
                    }
                    Closed = true;
                }
            }
         
        }

        public override void PowerDevice()
        {
            OpenDoor();
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            CloseDoor();
            base.UnpowerDevice();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Closed && !_opening && !_closing)
            {
                if (entity.spritesheets != null)
                {
                    entity.spritesheets.ChangeAnimation(_closedIdleAnimation, true, 0, true, false, true);
                }
            }
            
            if (entity.spritesheets != null)
            {
                if (_closing)
                {
                    if (_closingIsReverseOpening)
                    {
                        if (entity.spritesheets.CurrentSpritesheetName == _doorOpeningAnimation && entity.spritesheets.CurrentSpritesheet.currentFrame == 0)
                        {
                            _closing = false;
                        }
                    }
                    else
                    {
                        if (entity.spritesheets.IsFinished())
                        {
                            _closing = false;
                        }
                    }

                }
                else if (_opening)
                {
                    if (entity.spritesheets.IsFinished())
                    {
                        entity.spritesheets.ChangeAnimation(_openedIdleAnimation, true);

                        if (_openWhenAnimationEnd)
                        {
                            if (entity.hitboxes.Count > 0)
                                entity.hitboxes[0].Type = NeonEngine.Components.CollisionDetection.HitboxType.OneWay;
                            Closed = false;
                            _opening = false;
                            if (entity.rigidbody != null)
                            {
                                Vector2 position = entity.transform.Position;
                                entity.rigidbody.IsGround = false;
                                entity.rigidbody.Init();
                                entity.transform.Position = position;
                            }
                        }    
                        else
                            _opening = false;
                    }
                }
            }

            base.Update(gameTime);
        }

        public void CloseDoor()
        {
            if (!_closing)
            {
                if (entity.hitboxes.Count > 0)
                    entity.hitboxes[0].Type = NeonEngine.Components.CollisionDetection.HitboxType.Solid;
                _closing = true;
                Closed = true;
                if (entity.rigidbody != null)
                {
                    entity.rigidbody.IsGround = true;
                    entity.rigidbody.Init();
                }
                if (entity.spritesheets != null)
                {
                    if (_closingIsReverseOpening)
                    {
                        if(entity.spritesheets.SpritesheetList.ContainsKey(_doorOpeningAnimation))
                            entity.spritesheets.ChangeAnimation(_doorOpeningAnimation, 0, true, false, false, entity.spritesheets.SpritesheetList[_doorOpeningAnimation].FrameCount - 1);
                        entity.spritesheets.CurrentSpritesheet.ReverseLoop = true;
                    }
                    else
                    {
                        entity.spritesheets.ChangeAnimation(_doorClosingAnimation, 0, true, false, false);
                    }
                }
            }
        }

        public void OpenDoor()
        {
            if (!_opening)
            {
                
                _opening = true;
                if (entity.spritesheets != null)
                {
                    entity.spritesheets.ChangeAnimation(_doorOpeningAnimation, true, 0, true, false, false);
                    if (!_openWhenAnimationEnd)
                    {
                        Closed = false;
                        if (entity.hitboxes.Count > 0)
                            entity.hitboxes[0].Type = NeonEngine.Components.CollisionDetection.HitboxType.OneWay;
                        if (entity.rigidbody != null)
                        {
                            Vector2 position = entity.transform.Position;
                            entity.rigidbody.IsGround = false;
                            entity.rigidbody.Init();
                            entity.transform.Position = position;
                        }
                    }
                }
            }
        }

        public void Switch()
        {
            if (Closed)
                OpenDoor();
            else
                CloseDoor();
        }


    }
}
