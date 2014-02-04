using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class EnemyRegulator : Component
    {
        #region Properties
        private string _avatarName = "";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _enemyToSpawn = "";

        public string EnemyToSpawn
        {
            get { return _enemyToSpawn; }
            set { _enemyToSpawn = value; }
        }

        private Element _elementToCheck = Element.Neutral;

        public Element ElementToCheck
        {
            get { return _elementToCheck; }
            set { _elementToCheck = value; }
        }

        private float _elementLevelNeeded = 1.0f;

        public float ElementLevelNeeded
        {
            get { return _elementLevelNeeded; }
            set { _elementLevelNeeded = value; }
        }

        private Vector2 _spawnPoint;

        public Vector2 SpawnPoint
        {
            get { return _spawnPoint; }
            set { _spawnPoint = value; }
        }

        private float _timeBeforeSpawn = 1.0f;

        public float TimeBeforeSpawn
        {
            get { return _timeBeforeSpawn; }
            set { _timeBeforeSpawn = value; }
        }
        #endregion

        private AvatarCore _avatarComponent;
        private float _spawnTimer = 0.0f;
        private bool _haveToSpawnEnemy = false;

        public EnemyRegulator(Entity entity)
            :base(entity, "EnemyRegulator")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            Entity avatar = entity.GameWorld.GetEntityByName(_avatarName);
            if (avatar != null) 
                _avatarComponent = avatar.GetComponent<AvatarCore>();
            if(_spawnPoint == Vector2.Zero)
                _spawnPoint = entity.transform.Position;
            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (entity.hitboxes.Count > 0 && _avatarComponent != null && _avatarComponent.entity.hitboxes.Count > 0
                && entity.hitboxes[0].hitboxRectangle.Intersects(_avatarComponent.entity.hitboxes[0].hitboxRectangle))
            {
                int elementCount = 0;
                foreach (Entity e in entity.GameWorld.Entities)
                {
                    if (e.hitboxes.Count > 0 && e.hitboxes[0].hitboxRectangle.Intersects(this.entity.hitboxes[0].hitboxRectangle))
                    {
                        EnemyCore ec;
                        ec = e.GetComponent<EnemyCore>();
                        if (ec != null)
                        {
                            if (ec.CoreElement == _elementToCheck)
                                elementCount++;
                        }
                    }
                }

                if (_avatarComponent.ElementSystem != null)
                {
                    if (_avatarComponent.ElementSystem.RightSlotElement == _elementToCheck)
                        elementCount += (int)_avatarComponent.ElementSystem.RightSlotLevel;
                    if (_avatarComponent.ElementSystem.LeftSlotElement == _elementToCheck)
                        elementCount += (int)_avatarComponent.ElementSystem.LeftSlotLevel;
                }

                if (_elementLevelNeeded > elementCount)
                {
                    if (!_haveToSpawnEnemy)
                    {
                        _haveToSpawnEnemy = true;
                        _spawnTimer = _timeBeforeSpawn;
                    }
                }
                else
                    _haveToSpawnEnemy = false;

                if (_haveToSpawnEnemy)
                {
                    if (_spawnTimer > 0.0f)
                        _spawnTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_spawnTimer <= 0.0f)
                    {
                        SpawnEnemy();
                        _haveToSpawnEnemy = false;
                    }

                }
            }
            else
                _haveToSpawnEnemy = false;         
            
            base.Update(gameTime);
        }

        public void SpawnEnemy()
        {
            if (_enemyToSpawn != "")
                DataManager.LoadPrefab(@"../Data/Prefabs/" + EnemyToSpawn + ".prefab", entity.GameWorld).transform.Position = _spawnPoint;
        }




    }
}
