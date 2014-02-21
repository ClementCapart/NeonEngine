using NeonEngine;
using NeonEngine.Components.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class InstantRespawnPoint : Component
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

        public InstantRespawnPoint(Entity entity)
            :base(entity, "InstantRespawnPoint")
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            _avatar = entity.GameWorld.GetEntityByName(_avatarName);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            (entity.GameWorld as GameScreen).InstantRespawnPoint = (entity.GameWorld as GameScreen).SpawnPoints.ElementAt((int)_spawnPointIndex).Position;
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
