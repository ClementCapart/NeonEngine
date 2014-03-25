using NeonEngine;
using NeonEngine.Private;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingScanAlarmScript : ScriptComponent
    {
        private Entity _scanDoor;
        private Entity _exitDoor;
        private Entity _exitDoor2;
        private EnemyCore _importantRobot;
        private EnemyCore _importantTurret;
        private string _scanDoorName = "";
        private string _exitDoorName = "";
        private string _exitDoor2Name = "";
        private string _importantRobotName = "";
        private string _importantTurretName = "";

        public string ExitDoorName2
        {
            get { return _exitDoor2Name; }
            set { _exitDoor2Name = value; }
        }

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

            if (_exitDoorName != "")
                _exitDoor = entity.GameWorld.GetEntityByName(_exitDoorName);

            if (_exitDoor != null)
            {
                _exitDoor.spritesheets.DrawLayer = 0.001f;
            }

            //if (_exitDoor2Name != "")
            //    _exitDoor2 = entity.GameWorld.GetEntityByName(_exitDoor2Name);

            //if (_exitDoor2 != null)
            //    _exitDoor2.spritesheets.ChangeAnimation("Opened2", 0, true, false, false);

            Entity e;

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

            //if (_exitDoor2 != null)
            //    _exitDoor2.spritesheets.ChangeAnimation("Closing2", 0, true, false, false);
            
            /*if (_exitDoor != null)
            {
                _exitDoor.rigidbody.IsGround = true;
                _exitDoor.rigidbody.Init();
            }*/

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            /*if (_importantRobot != null && _importantTurret != null)
            {
                if (_importantRobot.State == EnemyState.Dead && _importantTurret.State == EnemyState.Dead)
                {
                    _exitDoor.spritesheets.ChangeAnimation("Opening", 0, true, false, false);
                    if (_exitDoor != null)
                    {
                        _exitDoor.rigidbody.IsGround = false;
                        _exitDoor.rigidbody.Init();
                    }
                }
            }*/
            base.Update(gameTime);
        }

    }
}
