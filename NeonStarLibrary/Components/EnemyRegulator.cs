using Microsoft.Xna.Framework;
using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
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

        private float _elementLevelToCheck = 1.0f;

        public float ElementLevelToCheck
        {
            get { return _elementLevelToCheck; }
            set { _elementLevelToCheck = value; }
        }

        private Vector2 _spawnPoint;

        public Vector2 SpawnPoint
        {
            get { return _spawnPoint; }
            set { _spawnPoint = value; }
        }

        private float _minimumIntervalBetweenSpawns = 2.0f;

        public float MinimumIntervalBetweenSpawns
        {
            get { return _minimumIntervalBetweenSpawns; }
            set { _minimumIntervalBetweenSpawns = value; }
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
            if (_avatarComponent != null && _avatarComponent.ElementSystem != null && (_avatarComponent.ElementSystem.RightSlotElement != _elementToCheck && _avatarComponent.ElementSystem.LeftSlotElement != _elementToCheck))
                _haveToSpawnEnemy = true;
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
            
            base.Update(gameTime);
        }

        public void SpawnEnemy()
        {
            if (_enemyToSpawn != "")
                DataManager.LoadPrefab(@"../Data/Prefabs/" + EnemyToSpawn + ".prefab", entity.GameWorld).transform.Position = _spawnPoint;
            _spawnTimer = _minimumIntervalBetweenSpawns;
        }




    }
}
