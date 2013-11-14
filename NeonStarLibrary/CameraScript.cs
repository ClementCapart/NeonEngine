using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class CameraScript : ScriptComponent
    {
        private string _permanentBoundsPrefix = "NoPrefix";

        public string PermanentBoundsPrefix
        {
            get { return _permanentBoundsPrefix; }
            set { _permanentBoundsPrefix = value; }
        }
        
        private string _firstTriggerName = "";

        public string FirstTriggerName
        {
            get { return _firstTriggerName; }
            set { _firstTriggerName = value; }
        }

        private float _firstTriggerZoom = 1.0f;

        public float FirstTriggerZoom
        {
            get { return _firstTriggerZoom; }
            set { _firstTriggerZoom = value; }
        }

        private string _firstBoundsPrefix = "NoPrefix";

        public string FirstBoundsPrefix
        {
            get { return _firstBoundsPrefix; }
            set { _firstBoundsPrefix = value; }
        }

        private string _secondTriggerName = "";

        public string SecondTriggerName
        {
            get { return _secondTriggerName; }
            set { _secondTriggerName = value; }
        }

        private float _secondTriggerZoom = 1.0f;

        public float SecondTriggerZoom
        {
            get { return _secondTriggerZoom; }
            set { _secondTriggerZoom = value; }
        }

        private string _secondBoundsPrefix = "NoPrefix";

        public string SecondBoundsPrefix
        {
            get { return _secondBoundsPrefix; }
            set { _secondBoundsPrefix = value; }
        }

        private string _thirdTriggerName = "";

        public string ThirdTriggerName
        {
            get { return _thirdTriggerName; }
            set { _thirdTriggerName = value; }
        }

        private float _thirdTriggerZoom = 1.0f;

        public float ThirdTriggerZoom
        {
            get { return _thirdTriggerZoom; }
            set { _thirdTriggerZoom = value; }
        }

        private string _thirdBoundsPrefix = "NoPrefix";

        public string ThirdBoundsPrefix
        {
            get { return _thirdBoundsPrefix; }
            set { _thirdBoundsPrefix = value; }
        }

        private string _fourthTriggerName = "";

        public string FourthTriggerName
        {
            get { return _fourthTriggerName; }
            set { _fourthTriggerName = value; }
        }

        private float _fourthTriggerZoom = 1.0f;

        public float FourthTriggerZoom
        {
            get { return _fourthTriggerZoom; }
            set { _fourthTriggerZoom = value; }
        }

        private string _fourthBoundsPrefix = "NoPrefix";

        public string FourthBoundsPrefix
        {
            get { return _fourthBoundsPrefix; }
            set { _fourthBoundsPrefix = value; }
        }

        private float _targetZoom = 1.0f;

        private List<CameraBound> _firstBoundsToActivate = new List<CameraBound>();
        private List<CameraBound> _secondBoundsToActivate = new List<CameraBound>();
        private List<CameraBound> _thirdBoundsToActivate = new List<CameraBound>();
        private List<CameraBound> _fourthBoundsToActivate = new List<CameraBound>();

        public CameraScript(Entity entity)
            :base(entity, "CameraScript")
        {
        }

        public override void Init()
        {
            foreach (Entity e in Neon.world.entities)
            {
                if (e.Name.StartsWith(_firstBoundsPrefix))
                {
                    _firstBoundsToActivate.Add(e.GetComponent<CameraBound>());
                }
                else if (e.Name.StartsWith(_secondBoundsPrefix))
                {
                    _secondBoundsToActivate.Add(e.GetComponent<CameraBound>());
                }
            }
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if((entity.containerWorld as GameScreen).MustFollowAvatar)
                SmoothZoom();
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity)
        {
            if(trigger.Name == _firstTriggerName)
            {
                _targetZoom = _firstTriggerZoom;
                for (int i = Neon.world.camera.CameraBounds.Count - 1; i >= 0; i--)
                {
                    CameraBound cb = Neon.world.camera.CameraBounds[i];
                    if (!cb.entity.Name.StartsWith(_permanentBoundsPrefix))
                        cb.Enabled = false;
                }
            }
            else if(trigger.Name == _secondTriggerName)
            {
                _targetZoom = _secondTriggerZoom;

                foreach (CameraBound cb in _firstBoundsToActivate)
                    cb.Enabled = true;
            }
            else if (trigger.Name == _thirdTriggerName)
            {
                _targetZoom = _thirdTriggerZoom;

                for (int i = Neon.world.camera.CameraBounds.Count - 1; i >= 0; i--)
                {
                    CameraBound cb = Neon.world.camera.CameraBounds[i];
                    if (!cb.entity.Name.StartsWith(_permanentBoundsPrefix))
                        cb.Enabled = false;
                }
            }
            else if (trigger.Name == _fourthTriggerName)
            {
                _targetZoom = _fourthTriggerZoom;

                foreach (CameraBound cb in _secondBoundsToActivate)
                    cb.Enabled = true;
            }
            base.OnTrigger(trigger, triggeringEntity);
        }

        public void SmoothZoom()
        {
            if (_targetZoom != Neon.world.camera.Zoom)
            {
                Neon.world.camera.Zoom = MathHelper.Lerp(_targetZoom, Neon.world.camera.Zoom, 0.95f);
            }
        }
    }
}
