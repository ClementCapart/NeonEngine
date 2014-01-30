using NeonEngine;
using NeonEngine.Components.Triggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class CheckPoint : Component
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

        public CheckPoint(Entity entity)
            :base(entity, "CheckPoint")
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
            SaveCheckPoint();
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        private void SaveCheckPoint()
        {
            if ((entity.GameWorld as GameScreen).CheckPointsData.Count > 0)
            {
                XElement progression = (entity.GameWorld as GameScreen).CheckPointsData.Last();
                string levelName = progression.Element("CurrentLevel").Element("LevelName").Value;
                string groupName = progression.Element("CurrentLevel").Element("GroupName").Value;
                string indexString = progression.Element("CurrentLevel").Element("SpawnPoint").Value;
                int index = int.MaxValue;
                if (indexString != "None")
                    index = int.Parse(indexString);

                if (entity.GameWorld.LevelGroupName != groupName || entity.GameWorld.LevelName != levelName || index != SpawnPointIndex)
                    (entity.GameWorld as GameScreen).CheckPointsData.Add((entity.GameWorld as GameScreen).SaveStatus(this));
            }
            else
            {
                (entity.GameWorld as GameScreen).CheckPointsData.Add((entity.GameWorld as GameScreen).SaveStatus(this));
            }          
        }




    }
}
