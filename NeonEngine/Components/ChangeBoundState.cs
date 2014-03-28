using NeonEngine.Components.Camera;
using NeonEngine.Components.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Camera2D
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
                    _bound.ReverseBound = !_bound.ReverseBound;
                }
            }
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

    }
}
