using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary.Components.Enemies;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingScanAlarmScript : ScriptComponent
    {
        private Entity _scanDoor;
        private EnemyPoweredDevice _exitDoor;
        private EnemyCore _importantRobot;
        private EnemyCore _importantTurret;
        private string _scanDoorName = "";
        private string _exitDoorName = "";
        private string _importantRobotName = "";
        private string _importantTurretName = "";

        public string ImportantTurretName
        {
            get { return _importantTurretName; }
            set { _importantTurretName = value; }
        }

        public string ImportantRobotName
        {
            get { return _importantRobotName; }
            set { _importantRobotName = value; }
        }

        public string ExitDoorName
        {
            get { return _exitDoorName; }
            set { _exitDoorName = value; }
        }

        public string ScanDoorName
        {
            get { return _scanDoorName; }
            set { _scanDoorName = value; }
        }

        public TrainingScanAlarmScript(Entity entity)
            : base(entity, "TrainingScanAlarmScript")
        {
        }

        public override void Init()
        {
            this.entity.spritesheets.ChangeAnimation("Opened", 0, true, false, false);

            if (_scanDoorName != "")
                _scanDoor = entity.GameWorld.GetEntityByName(_scanDoorName);
            if (_scanDoor != null)
            {
                _scanDoor.rigidbody.IsGround = false;
                _scanDoor.rigidbody.Init();
            }

            Entity e;

            e = entity.GameWorld.GetEntityByName(_exitDoorName);
            if (e != null)
                _exitDoor = e.GetComponent<EnemyPoweredDevice>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantRobotName);
            if (e != null)
                _importantRobot = e.GetComponent<EnemyCore>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantTurretName);
            if (e != null)
                _importantTurret = e.GetComponent<EnemyCore>();

            base.Init();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            this.entity.spritesheets.ChangeAnimation("Closing", 0, true, false, false);
            
            if (_scanDoor != null)
            {
                _scanDoor.rigidbody.IsGround = true;
                _scanDoor.rigidbody.Init();
            }
            
            if (_exitDoor != null)
            {
                _exitDoor.DeactivateDevice();
            }

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
