using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components
{
    public class AutoDestruction : Component
    {
        #region Properties
        private float _destructionTimer = 0.0f;

        public float DestructionTimer
        {
            get { return _destructionTimer; }
            set { _destructionTimer = value; }
        }
        #endregion

        private float _currentTimer = 0.0f;

        public AutoDestruction(Entity e)
            :base(e, "AutoDestruction")
        {
        }

        public override void Init()
        {
            _currentTimer = _destructionTimer;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _currentTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_currentTimer <= 0.0f)
                entity.Destroy();
            base.Update(gameTime);
        }
    }
}
