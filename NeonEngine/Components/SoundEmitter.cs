using Microsoft.Xna.Framework;
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
        private bool _debug = true;

        public bool Debug
        {
            get { return _debug; }
            set { _debug = value; }
        }

        private float _zDistance = 0.0f;

        public float ZDistance
        {
            get { return _zDistance; }
            set { _zDistance = value; }
        }

        private string _playingSoundTag = "";

        public string PlayingSoundTag
        {
            get { return _playingSoundTag; }
            set 
            { 
                _playingSoundTag = value;
                _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
                if (_currentSoundInstance != null)
                    _currentSoundInstance.Stop();
                if (_playingSoundEffect != null && AudioEmitter != null)
                {
                    _currentSoundInstance = _playingSoundEffect.CreateInstance();
                    _currentSoundInstance.IsLooped = _isLooped;
                    if (_is3DSound) _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                    _currentSoundInstance.Volume = 0.0f;
                    _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                    _currentSoundInstance.Play();
                    SoundInstances.Add(_currentSoundInstance);

                }
            }
        }

        private bool _is3DSound = true;

        public bool Is3DSound
        {
            get { return _is3DSound; }
            set 
            { 
                _is3DSound = value;
                if (_is3DSound && _currentSoundInstance != null && AudioEmitter != null)
                {
                    _currentSoundInstance.Stop();
                    SoundInstances.Remove(_currentSoundInstance);
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance);
                    }
                }
                else if (!_is3DSound)
                {
                    if (_currentSoundInstance != null)
                    {
                        _currentSoundInstance.Stop();
                        SoundInstances.Remove(_currentSoundInstance);
                    }
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance);
                    }
                }
            }
        }

        private bool _isLooped = true;

        public bool IsLooped
        {
            get { return _isLooped; }
            set
            { 
                _isLooped = value;
                if (_currentSoundInstance != null)
                {
                    _currentSoundInstance.Stop();
                    SoundInstances.Remove(_currentSoundInstance);
                    if (_playingSoundEffect != null && AudioEmitter != null)
                    {
                        _currentSoundInstance = _playingSoundEffect.CreateInstance();
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                        _currentSoundInstance.IsLooped = _isLooped;
                        _currentSoundInstance.Volume = 0.0f;
                        _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                        _currentSoundInstance.Play();
                        SoundInstances.Add(_currentSoundInstance);
                    }
                }
            }
        }

        private float _volume = 1.0f;

        public float Volume
        {
            get { return _volume; }
            set 
            { 
                _volume = value;
            }
        }

        private float _pitch = 0.0f;

        public float Pitch
        {
            get { return _pitch; }
            set 
            { 
                _pitch = value;
                if (_currentSoundInstance != null)
                    _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
            }
        }

        private float _maxDistance = 0.0f;

        public float MaxDistance
        {
            get { return _maxDistance; }
            set { _maxDistance = value; }
        }
        #endregion

        public AudioEmitter AudioEmitter;

        public List<SoundEffectInstance> SoundInstances;
        protected SoundEffect _playingSoundEffect;
        protected SoundEffectInstance _currentSoundInstance;
        protected float _currentVolume = 0.0f;

        public SoundEmitter(Entity entity)
            :base(entity, "SoundEmitter")
        {   
        }

        public override void Init()
        {
            SoundInstances = new List<SoundEffectInstance>();
            if (_currentSoundInstance != null)
                _currentSoundInstance.Stop();
            _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
            if (_playingSoundEffect != null)
                _currentSoundInstance = _playingSoundEffect.CreateInstance();

            if (AudioEmitter == null)
                AudioEmitter = new AudioEmitter();

            if (AudioEmitter != null)
            {
                AudioEmitter.Position = new Microsoft.Xna.Framework.Vector3(entity.transform.Position, _zDistance);
            }

            if (_currentSoundInstance != null)
            {
                _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);
                _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                _currentSoundInstance.IsLooped = _isLooped;
                if(_is3DSound)
                    _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                _currentSoundInstance.Play();
            }

            SoundInstances.Add(_currentSoundInstance);
            entity.GameWorld.AudioEmitters.Add(this);


                base.Init();
        }

        public override void Remove()
        {
            if (AudioEmitter != null)
                entity.GameWorld.AudioEmitters.Remove(this);
            if (_currentSoundInstance != null)
                _currentSoundInstance.Stop();
            base.Remove();
        }

        public override void PreUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AudioEmitter != null && _is3DSound)
            {
                AudioEmitter.Position = new Microsoft.Xna.Framework.Vector3(entity.transform.Position, _zDistance);
            }
            if (MaxDistance != 0.0f)
            {
                foreach (AudioListener al in entity.GameWorld.AudioListeners)
                {
                    float distance = Vector2.Distance(new Vector2(al.Position.X, al.Position.Y), entity.transform.Position);
                    if (distance <= MaxDistance)
                    {
                        _currentVolume = _volume * (float)Math.Cos(distance / MaxDistance * Math.PI / 2);
                    }
                    else
                    {
                        _currentVolume = 0.0f;
                    }
                }
            }
            else
                _currentVolume = _volume;

            if (_currentSoundInstance != null && !_currentSoundInstance.IsDisposed)
                _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);

            if (_playingSoundEffect != null && _playingSoundEffect.IsDisposed)
            {
                _playingSoundEffect = SoundManager.GetSound(_playingSoundTag);
                if (_playingSoundEffect != null)
                    _currentSoundInstance = _playingSoundEffect.CreateInstance();

                if (_currentSoundInstance != null)
                {
                    _currentSoundInstance.Volume = MathHelper.Clamp(_currentVolume, 0.0f, 1.0f);
                    _currentSoundInstance.Pitch = MathHelper.Clamp(_pitch, -1.0f, 1.0f);
                    _currentSoundInstance.IsLooped = _isLooped;
                    if (_is3DSound)
                        _currentSoundInstance.Apply3D(entity.GameWorld.AudioListeners.ToArray(), AudioEmitter);
                    _currentSoundInstance.Play();
                }
            }
            
            base.Update(gameTime);
        }

        public override void OnChangeLevel()
        {
            if (_currentSoundInstance != null && !_currentSoundInstance.IsDisposed)
                _currentSoundInstance.Stop();
            base.OnChangeLevel();
        }
    }
}
