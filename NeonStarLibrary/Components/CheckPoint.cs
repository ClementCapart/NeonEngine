using NeonEngine;
using NeonEngine.Components.Triggers;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class SaveRoom : Component
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private float _spawnPointIndex = 0.0f;

        public float SpawnPointIndex
        {
            get { return _spawnPointIndex; }
            set { _spawnPointIndex = value; }
        }

        #endregion

        public bool Active = true;
        private Entity _avatar;
        private AvatarCore _avatarComponent;

        public SaveRoom(Entity entity)
            :base(entity, "CheckPoint")
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            _avatar = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatar != null)
                _avatarComponent = _avatar.GetComponent<AvatarCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            SaveCheckPoint();
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        private void SaveCheckPoint()
        {
            GameScreen.CheckPointsData.Add((entity.GameWorld as GameScreen).SaveStatus(this));
            if (_avatarComponent != null)
                _avatarComponent.CurrentHealthPoints = _avatarComponent.StartingHealthPoints;
            HealStation._usedHealStations.Clear();        
        }




    }
}
