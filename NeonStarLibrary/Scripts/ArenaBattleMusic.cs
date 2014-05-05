using NeonEngine;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Scripts
{
    class ArenaBattleMusic : Component
    {
        #region Properties
        private string _arenaDoorName = "";

        public string ArenaDoorName
        {
            get { return _arenaDoorName; }
            set { _arenaDoorName = value; }
        }
        #endregion

        private QuotaEnergyDoor _arenaDoor;

        public ArenaBattleMusic(Entity entity)
            :base(entity, "ArenaBattleMusic")
        {
        }

        public override void Init()
        {
            Entity e = entity.GameWorld.GetEntityByName(_arenaDoorName);
            if (e != null)
                _arenaDoor = e.GetComponent<QuotaEnergyDoor>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_arenaDoor != null && entity.GameWorld.NextScreen == null)
            {
                if(!_arenaDoor.Closed)
                    SoundManager.MusicLock = false;
            }
            base.Update(gameTime);
        }

        public override void OnChangeLevel()
        {
            SoundManager.MusicLock = false;
            base.OnChangeLevel();
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            SoundManager.MusicLock = true;
            SoundManager.CrossFadeLoopTrack("BattleMainLoop");
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
