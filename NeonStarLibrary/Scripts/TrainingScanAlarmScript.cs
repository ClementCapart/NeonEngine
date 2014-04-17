using Microsoft.Xna.Framework;
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
        private string _scanDoorName = "";
        public string ScanDoorName
        {
            get { return _scanDoorName; }
            set { _scanDoorName = value; }
        }

        private string _importantRobotName = "ImportantRobot";
        public string ImportantRobotName
        {
            get { return _importantRobotName; }
            set { _importantRobotName = value; }
        }

        private string _secondImportantRobotName = "ImportantRobot2";

        public string SecondImportantRobotName
        {
            get { return _secondImportantRobotName; }
            set { _secondImportantRobotName = value; }
        }

        private string _thirdImportantRobotName = "ImportantRobot3";

        public string ThirdImportantRobotName
        {
            get { return _thirdImportantRobotName; }
            set { _thirdImportantRobotName = value; }
        }

        private Entity _scanDoor;
        private Entity _importantRobot;
        private Entity _secondImportantRobot;
        private Entity _thirdImportantRobot;
        private EnemyCore _importantRobotEnemyCore;

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

            _importantRobot = entity.GameWorld.GetEntityByName(_importantRobotName);
            if (_importantRobot != null)
                _importantRobotEnemyCore = _importantRobot.GetComponent<EnemyCore>();
            _importantRobot = null;

            _secondImportantRobot = entity.GameWorld.GetEntityByName(_secondImportantRobotName);
            _thirdImportantRobot = entity.GameWorld.GetEntityByName(_thirdImportantRobotName);

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

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_importantRobotEnemyCore != null && _importantRobotEnemyCore.State == EnemyState.Dying)
            {
                if (_secondImportantRobot != null && _thirdImportantRobot != null)
                {
                    _secondImportantRobot.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                    _secondImportantRobot.rigidbody.Mass = 400.0f;
                    _secondImportantRobot.rigidbody.Init();
                    _thirdImportantRobot.rigidbody.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                    _thirdImportantRobot.rigidbody.Mass = 400.0f;
                    _thirdImportantRobot.rigidbody.Init();
                }
            }

            base.Update(gameTime);
        }

    }
}
