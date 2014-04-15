using NeonEngine.Components.Camera;
using NeonEngine.Components.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Camera
{
    public class ChangeBoundState : Component
    {
        #region Properties
        private string _boundName;

        public string BoundName
        {
            get { return _boundName; }
            set { _boundName = value; }
        }

        private bool _switchBoundsOnly = false;

        public bool SwitchBoundsOnly
        {
            get { return _switchBoundsOnly; }
            set { _switchBoundsOnly = value; }
        }

        private bool _enableBounds = false;

        public bool EnableBounds
        {
            get { return _enableBounds; }
            set { _enableBounds = value; }
        }
        #endregion

        private CameraBound _bound;

        public ChangeBoundState(Entity entity)
            :base(entity, "ChangeBoundState")
        {
            this.RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            Entity e = entity.GameWorld.GetEntityByName(_boundName);
            if (e != null) _bound = e.GetComponent<CameraBound>();
            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if (_bound != null)
            {
                if (_bound.SoftBound)
                {
                    if (_switchBoundsOnly)
                        _bound.ReverseBound = !_bound.ReverseBound;
                    else
                        _bound.ReverseBound = !EnableBounds;
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

    }
}
