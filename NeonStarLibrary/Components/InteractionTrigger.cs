using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Triggers
{
    public class InteractionTrigger : Component
    {

        #region Properties

        private string _triggeringEntityName = "";

        public string TriggeringEntityName
        {
            get { return _triggeringEntityName; }
            set { _triggeringEntityName = value; }
        }

        private string _triggeredEntityName = "";

        public string TriggeredEntityName
        {
            get { return _triggeredEntityName; }
            set { _triggeredEntityName = value; }
        }

        private string _triggeredComponentName = "";

        public string TriggeredComponentName
        {
            get { return _triggeredComponentName; }
            set { _triggeredComponentName = value; }
        }

        private string _initialAnimation = "";

        public string InitialAnimation
        {
            get { return _initialAnimation; }
            set { _initialAnimation = value; }
        }

        private string _activationAnimation = "";

        public string ActivationAnimation
        {
            get { return _activationAnimation; }
            set { _activationAnimation = value; }
        }

        private string _activatedAnimation = "";

        public string ActivatedAnimation
        {
            get { return _activatedAnimation; }
            set { _activatedAnimation = value; }
        }
        #endregion

        private Entity _triggeringEntity;
        private Component _triggeredComponent;
        private Entity _triggeredEntity;
        private bool _activated = false;

        public InteractionTrigger(Entity entity)
            :base(entity, "InteractionTrigger")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _triggeringEntity = entity.GameWorld.GetEntityByName(_triggeringEntityName);
            _triggeredEntity = entity.GameWorld.GetEntityByName(_triggeredEntityName);
            if (_triggeredEntity != null)
                _triggeredComponent = _triggeredEntity.GetComponentByName(_triggeredComponentName);
            _activated = false;
            if(entity.spritesheets != null)
            {
                entity.spritesheets.ChangeAnimation(_initialAnimation);
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_triggeringEntity != null)
            {
                if (!_activated && entity.hitboxes.Count > 0 && _triggeringEntity.hitboxes.Count > 0 && entity.hitboxes[0].hitboxRectangle.Intersects(_triggeringEntity.hitboxes[0].hitboxRectangle))
                {
                    if (Neon.Input.Check(NeonStarInput.Interact))
                    {
                        _activated = true;
                        if (_triggeredComponent != null)
                            _triggeredComponent.OnTrigger(entity, _triggeringEntity);
                        if (entity.spritesheets != null)
                            entity.spritesheets.ChangeAnimation(_activationAnimation, 0, true, false, false);
                    }
                }
            }
            

            if (entity.spritesheets != null && entity.spritesheets.CurrentSpritesheetName == _activationAnimation && entity.spritesheets.CurrentSpritesheet.IsFinished)
                entity.spritesheets.ChangeAnimation(_activatedAnimation);
            base.Update(gameTime);
        }


    }
}
