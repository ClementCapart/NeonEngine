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
        private EnemyCore _importantTurret2;
        private AreaAttack _importantRobotRange;
        private AreaAttack _importantTurretRange;
        private AreaAttack _importantTurretRange2;

        private string _scanDoorName = "";
        public string ScanDoorName
        {
            get { return _scanDoorName; }
            set { _scanDoorName = value; }
        }

        private string _exitDoorName = "";
        public string ExitDoorName
        {
            get { return _exitDoorName; }
            set { _exitDoorName = value; }
        }

        private string _importantTurret2Name = "";
        public string ImportantTurret2Name
        {
            get { return _importantTurret2Name; }
            set { _importantTurret2Name = value; }
        }

        private string _importantRobotRangeName = "";
        public string ImportantRobotRangeName
        {
            get { return _importantRobotRangeName; }
            set { _importantRobotRangeName = value; }
        }

        private string _importantTurretRangeName = "";
        public string ImportantTurretRangeName
        {
            get { return _importantTurretRangeName; }
            set { _importantTurretRangeName = value; }
        }

        private string _importantTurretRange2Name = "";
        public string ImportantTurretRange2Name
        {
            get { return _importantTurretRange2Name; }
            set { _importantTurretRange2Name = value; }
        }

        private string _importantRobotName = "";
        public string ImportantRobotName
        {
            get { return _importantRobotName; }
            set { _importantRobotName = value; }
        }

        private string _importantTurretName = "";
        public string ImportantTurretName
        {
            get { return _importantTurretName; }
            set { _importantTurretName = value; }
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

            e = entity.GameWorld.GetEntityByName(_importantTurret2Name);
            if (e != null)
                _importantTurret2 = e.GetComponent<EnemyCore>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantTurretName);
            if (e != null)
                _importantTurret = e.GetComponent<EnemyCore>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantRobotRangeName);
            if (e != null)
                _importantRobotRange = e.GetComponent<AreaAttack>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantTurretRangeName);
            if (e != null)
                _importantTurretRange = e.GetComponent<AreaAttack>();
            e = null;

            e = entity.GameWorld.GetEntityByName(_importantTurretRange2Name);
            if (e != null)
                _importantTurretRange2 = e.GetComponent<AreaAttack>();

            if (_exitDoor != null)
            {
                _exitDoor.ActivateDevice();
            }
            
            if (_importantRobotRange != null)
            {
                _importantRobotRange.RangeForAttackOne = 1;
            }

            if (_importantTurretRange != null)
            {
                _importantTurretRange.RangeForAttackOne = 1;
            }

            if (_importantTurretRange2 != null)
            {
                _importantTurretRange2.RangeForAttackOne = 1;
            }

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

            if (_importantRobotRange != null)
            {
                _importantRobotRange.RangeForAttackOne = 1000;
                _importantRobotRange.Init();
            }

            if (_importantTurretRange != null)
            {
                _importantTurretRange.RangeForAttackOne = 1000;
                _importantTurretRange.Init();
            }

            if (_importantTurretRange2 != null)
            {
                _importantTurretRange2.RangeForAttackOne = 1000;
                _importantTurretRange2.Init();
            }

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }
}
