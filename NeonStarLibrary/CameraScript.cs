using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.Camera;
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

        private string _firstBoundsPrefix = "NoPrefix";

        public string FirstBoundsPrefix
        {
            get { return _firstBoundsPrefix; }
            set { _firstBoundsPrefix = value; }
        }

        private string _secondBoundsPrefix = "NoPrefix";

        public string SecondBoundsPrefix
        {
            get { return _secondBoundsPrefix; }
            set { _secondBoundsPrefix = value; }
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
            foreach (Entity e in entity.GameWorld.Entities)
            {
                if (e.Name.StartsWith(_firstBoundsPrefix))
                {
                    CameraBound cb = e.GetComponent<CameraBound>();
                    if(cb != null)
                        _firstBoundsToActivate.Add(cb);
                }
                else if (e.Name.StartsWith(_secondBoundsPrefix))
                {
                    CameraBound cb = e.GetComponent<CameraBound>();
                    if (cb != null)
                        _secondBoundsToActivate.Add(cb);
                }
            }

            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if((entity.GameWorld as GameScreen).MustFollowAvatar)
                SmoothZoom();

            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            if(trigger.Name == _firstTriggerName)
            {

                foreach (CameraBound cb in _firstBoundsToActivate)
                {
                    cb.BoundStrength = cb.SoftBoundStrength;
                    cb.Enabled = true;                  
                }
            }
            else if(trigger.Name == _secondTriggerName)
            {
                foreach (CameraBound cb in _secondBoundsToActivate)
                {
                    cb.BoundStrength = cb.SoftBoundStrength;
                    cb.Enabled = true;
                }
            }
            else if (trigger.Name == _thirdTriggerName)
            {
                entity.GameWorld.Camera.ChaseStrength = 0.0f;

                for (int i = entity.GameWorld.Camera.CameraBounds.Count - 1; i >= 0; i--)
                {
                    CameraBound cb = entity.GameWorld.Camera.CameraBounds[i];
                    if (cb.entity.Name.StartsWith("03"))
                        cb.Enabled = false;
                }
            }
            else if (trigger.Name == _fourthTriggerName)
            {
                entity.GameWorld.Camera.ChaseStrength = 0.0f;

                for (int i = entity.GameWorld.Camera.CameraBounds.Count - 1; i >= 0; i--)
                {
                    CameraBound cb = entity.GameWorld.Camera.CameraBounds[i];
                    if (cb.entity.Name.StartsWith("04"))
                        cb.Enabled = false;
                }
            }
            else if (trigger.Name == "06Trigger")
            {
                entity.GameWorld.Camera.ChaseStrength = 0.0f;

                for (int i = entity.GameWorld.Camera.CameraBounds.Count - 1; i >= 0; i--)
                {
                    CameraBound cb = entity.GameWorld.Camera.CameraBounds[i];
                    if (cb.entity.Name.StartsWith("06"))
                        cb.Enabled = false;
                }
            }
            base.OnTrigger(trigger, triggeringEntity);
        }

        public void SmoothZoom()
        {
            if (_targetZoom != entity.GameWorld.Camera.Zoom)
            {
                entity.GameWorld.Camera.Zoom = MathHelper.Lerp(_targetZoom, entity.GameWorld.Camera.Zoom, 0.98f);
                if (Math.Sqrt(_targetZoom * _targetZoom + entity.GameWorld.Camera.Zoom * entity.GameWorld.Camera.Zoom) < 0.05f)
                    entity.GameWorld.Camera.Zoom = _targetZoom;
            }
        }
    }
}
