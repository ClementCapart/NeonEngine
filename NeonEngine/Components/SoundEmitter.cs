using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class SoundEmitter : Component
    {
        #region Properties
        private float _zDistance = 0.0f;

        public float ZDistance
        {
            get { return _zDistance; }
            set { _zDistance = value; }
        }

        public float DistanceScale
        {
            get { return SoundEffect.DistanceScale; }
            set { SoundEffect.DistanceScale = value; }
        }
        #endregion

        public AudioEmitter AudioEmitter;

        public List<SoundEffectInstance> SoundInstances;
        private SoundEffectInstance sei;

        public SoundEmitter(Entity entity)
            :base(entity, "SoundEmitter")
        {   
        }

        public override void Init()
        {
            SoundInstances = new List<SoundEffectInstance>();
            sei = SoundManager.GetSound("DoubleJump").CreateInstance();
            sei.IsLooped = true;
            
            SoundInstances.Add(sei);
            if (AudioEmitter == null)
                AudioEmitter = new AudioEmitter();

            sei.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
            sei.Play();
            entity.GameWorld.AudioEmitters.Add(this);
            
            base.Init();
        }

        public override void Remove()
        {
            if (AudioEmitter != null)
                entity.GameWorld.AudioEmitters.Remove(this);
            base.Remove();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AudioEmitter != null)
            {
                AudioEmitter.Position = new Microsoft.Xna.Framework.Vector3(entity.transform.Position, _zDistance);
            }
            base.Update(gameTime);
        }
    }
}
