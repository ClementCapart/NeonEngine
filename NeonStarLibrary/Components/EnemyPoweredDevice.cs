using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class EnemyPoweredDevice : EnergyDevice
    {
        #region Properties
        private string _firstEnemyToKill = "";

        public string FirstEnemyToKill
        {
            get { return _firstEnemyToKill; }
            set { _firstEnemyToKill = value; }
        }

        private string _secondEnemyToKill = "";

        public string SecondEnemyToKill
        {
            get { return _secondEnemyToKill; }
            set { _secondEnemyToKill = value; }
        }

        private string _thirdEnemyToKill = "";

        public string ThirdEnemyToKill
        {
            get { return _thirdEnemyToKill; }
            set { _thirdEnemyToKill = value; }
        }

        private string _fourthEnemyToKill = "";

        public string FourthEnemyToKill
        {
            get { return _fourthEnemyToKill; }
            set { _fourthEnemyToKill = value; }
        }

        private string _fifthEnemyToKill = "";

        public string FifthEnemyToKill
        {
            get { return _fifthEnemyToKill; }
            set { _fifthEnemyToKill = value; }
        }

        private bool _addSignOnTargets = true;

        public bool AddSignOnTargets
        {
            get { return _addSignOnTargets; }
            set { _addSignOnTargets = value; }
        }
        #endregion

        private List<EnemyCore> _enemiesToKill;

        public EnemyPoweredDevice(Entity entity)
            :base(entity)
        {
            Name = "EnemyPoweredDevice";
        }

        public override void Init()
        {
            State = DeviceManager.GetDeviceState(entity.GameWorld.LevelGroupName, entity.GameWorld.LevelName, entity.Name);

            _enemiesToKill = new List<EnemyCore>();

            AddEnemyCore(_firstEnemyToKill);           
            AddEnemyCore(_secondEnemyToKill);
            AddEnemyCore(_thirdEnemyToKill);
            AddEnemyCore(_fourthEnemyToKill);
            AddEnemyCore(_fifthEnemyToKill);

            if (_addSignOnTargets)
            {
                if (this.State == DeviceState.Deactivated)
                {
                    foreach (EnemyCore ec in _enemiesToKill)
                    {
                        Graphic graphic = new Graphic(ec.entity);
                        graphic.GraphicTag = "EnemyToKillSign";
                        graphic.Layer = 0.499f;
                        graphic.HasToBeSaved = false;
                        ec.entity.AddComponent(graphic);
                        graphic.Init();
                    }
                }
            }
            
            base.Init();
        }

        private void AddEnemyCore(string s)
        {
            Entity e = entity.GameWorld.GetEntityByName(s);
            if (e != null)
            {
                EnemyCore ec = e.GetComponent<EnemyCore>();
                if (ec != null) _enemiesToKill.Add(ec);   
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for(int i = _enemiesToKill.Count - 1; i >= 0; i--)
            {
                EnemyCore ec = _enemiesToKill[i];
                if (ec.State == EnemyState.Dead)
                {
                    _enemiesToKill.Remove(ec);
                }
            }

            if (_enemiesToKill.Count <= 0)
            {
                if(State == DeviceState.Deactivated)
                    this.ActivateDevice();
            }
            base.Update(gameTime);
        }
    }
}
