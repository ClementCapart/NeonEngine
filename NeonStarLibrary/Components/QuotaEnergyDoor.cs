using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class QuotaEnergyDoor : EnergyDoor
    {
        #region Properties
        private string _doorTopNickname = "";

        public string DoorTopNickName
        {
            get { return _doorTopNickname; }
            set { _doorTopNickname = value; }
        }

        private string _doorLockNickname = "";

        public string DoorLockNickname
        {
            get { return _doorLockNickname; }
            set { _doorLockNickname = value; }
        }

        private string _gaugeBackName = "";

        public string GaugeBackName
        {
            get { return _gaugeBackName; }
            set { _gaugeBackName = value; }
        }

        private string _gaugeGridName = "";

        public string GaugeGridName
        {
            get { return _gaugeGridName; }
            set { _gaugeGridName = value; }
        }
        #endregion

        private SpritesheetManager _doorTop;

        private Graphic _gaugeBack;
        private Graphic _gaugeGrid;
        private TilableGraphic _gauge;
        public float _totalEnemiesToKill = 0.0f;
        public float _currentEnemiesKilled = 0.0f;

        List<EnemyPoweredDevice> _enemyPoweredDevices;

        public QuotaEnergyDoor(Entity e)
            :base(e)
        {
            Name = "QuotaEnergyDoor";
        }

        public override void Init()
        {
            entity.spritesheets = entity.GetComponentsByInheritance<SpritesheetManager>().Where(n => n.NickName != _doorTopNickname).First() ;
            _totalEnemiesToKill = 0.0f;
            _currentEnemiesKilled = 0.0f;

            _enemyPoweredDevices = new List<EnemyPoweredDevice>();
            _gauge = entity.GetComponent<TilableGraphic>();

            List<Graphic> gs = entity.GetComponentsByInheritance<Graphic>();
            foreach (Graphic g in gs)
            {
                if (g.NickName == _gaugeBackName)
                {
                    _gaugeBack = g;
                    continue;
                }

                if (g.NickName == _gaugeGridName)
                    _gaugeGrid = g;
            }

            _doorTop = entity.GetComponentByNickname(_doorTopNickname) as SpritesheetManager;

            base.Init();

            if (_powered)
            {
                this.entity.spritesheets.Layer = 0.35f;

                if (_gauge != null)
                    _gauge.Active = false;

                if (_gaugeGrid != null)
                    _gaugeGrid.Active = false;
                if (_gaugeBack != null)
                    _gaugeBack.Active = false;

                if (_doorTop != null)
                {
                    _doorTop.ChangeAnimation("Opened");
                }
            }
            else
            {
                if (_doorTop != null)
                {
                    _doorTop.ChangeAnimation("Closed");
                }
                this.entity.spritesheets.Layer = 0.6f;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void PowerDevice()
        {
            this.entity.spritesheets.Layer = 0.35f;
            if (_doorTop != null)
            {
                _doorTop.ChangeAnimation("Opening", 0, true, false, false);
            }

            if (_gauge != null)
                _gauge.Active = false;

            if (_gaugeGrid != null)
                _gaugeGrid.Active = false;
            if (_gaugeBack != null)
                _gaugeBack.Active = false;
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            this.entity.spritesheets.Layer = 0.6f;
            base.UnpowerDevice();
        }
    }
}
