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
    public class CityTrainStationEntranceScanScript : ScriptComponent
    {
        private string _scanDoorName = "";
        public string ScanDoorName
        {
            get { return _scanDoorName; }
            set { _scanDoorName = value; }
        }

        private Entity _scanDoor;

        public CityTrainStationEntranceScanScript(Entity entity)
            : base(entity, "CityTrainStationEntranceScanScript")
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

            base.Update(gameTime);
        }

    }
}