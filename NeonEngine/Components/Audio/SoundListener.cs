using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Audio
{
    public class SoundListener : Component
    {
        #region Properties
        private bool _attachedOnCamera = false;

        public bool AttachedOnCamera
        {
            get { return _attachedOnCamera; }
            set { _attachedOnCamera = value; }
        }

        private float _zDistance = 1.0f;

        public float ZDistance
        {
            get { return _zDistance; }
            set { _zDistance = value; }
        }
        #endregion

        private AudioListener _audioListener;

        public SoundListener(Entity entity)
            :base(entity, "SoundListener")
        {   
        }

        public override void Init()
        {
            if(_audioListener == null)
                _audioListener = new AudioListener();

            if (_audioListener != null)
            {
                if (_attachedOnCamera)
                    _audioListener.Position = new Microsoft.Xna.Framework.Vector3(entity.GameWorld.Camera.Position, _zDistance);
                else
                    _audioListener.Position = new Microsoft.Xna.Framework.Vector3(entity.transform.Position, _zDistance);
            }

            entity.GameWorld.AudioListeners.Add(_audioListener);
            
            base.Init();
        }

        public override void Remove()
        {
            if (_audioListener != null)
                entity.GameWorld.AudioListeners.Remove(_audioListener);
            base.Remove();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_audioListener != null)
            {
                if (_attachedOnCamera)
                    _audioListener.Position = new Microsoft.Xna.Framework.Vector3(entity.GameWorld.Camera.Position, _zDistance);
                else
                    _audioListener.Position = new Microsoft.Xna.Framework.Vector3(entity.transform.Position, _zDistance);
            }
            base.Update(gameTime);
        }
    }
}
