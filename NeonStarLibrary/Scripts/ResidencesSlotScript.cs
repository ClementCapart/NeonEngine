using Microsoft.Xna.Framework;
using NeonEngine;
using NeonStarLibrary.Components.Enemies;
using NeonStarLibrary.Components.Private;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components
{
    public class ResidencesSlotScript : Component
    {
        #region Properties

        private string _lanternName = "";

        public string LanternName
        {
            get { return _lanternName; }
            set { _lanternName = value; }
        }

        private Vector2 _lanternSpawnPosition;

        public Vector2 LanternSpawnPosition
        {
            get { return _lanternSpawnPosition; }
            set { _lanternSpawnPosition = value; }
        }

        private string _firstRobotName = "";

        public string FirstRobotName
        {
            get { return _firstRobotName; }
            set { _firstRobotName = value; }
        }

        private Vector2 _firstRobotSpawnPosition;

        public Vector2 FirstRobotSpawnPosition
        {
            get { return _firstRobotSpawnPosition; }
            set { _firstRobotSpawnPosition = value; }
        }

        private string _secondRobotName = "";

        public string SecondRobotName
        {
            get { return _secondRobotName; }
            set { _secondRobotName = value; }
        }

        private Vector2 _secondRobotSpawnPosition;

        public Vector2 SecondRobotSpawnPosition
        {
            get { return _secondRobotSpawnPosition; }
            set { _secondRobotSpawnPosition = value; }
        }

        private string _thirdRobotName = "";

        public string ThirdRobotName
        {
            get { return _thirdRobotName; }
            set { _thirdRobotName = value; }
        }

        private Vector2 _thirdRobotSpawnPosition;

        public Vector2 ThirdRobotSpawnPosition
        {
            get { return _thirdRobotSpawnPosition; }
            set { _thirdRobotSpawnPosition = value; }
        }

        private string _fourthRobotName = "";

        public string FourthRobotName
        {
            get { return _fourthRobotName; }
            set { _fourthRobotName = value; }
        }

        private Vector2 _fourthRobotSpawnPosition;

        public Vector2 FourthRobotSpawnPosition
        {
            get { return _fourthRobotSpawnPosition; }
            set { _fourthRobotSpawnPosition = value; }
        }

        private string _fifthRobotName = "";

        public string FifthRobotName
        {
            get { return _fifthRobotName; }
            set { _fifthRobotName = value; }
        }

        private Vector2 _fifthRobotSpawnPosition;

        public Vector2 FifthRobotSpawnPosition
        {
            get { return _fifthRobotSpawnPosition; }
            set { _fifthRobotSpawnPosition = value; }
        }

        private string _sixthRobotName = "";

        public string SixthRobotName
        {
            get { return _sixthRobotName; }
            set { _sixthRobotName = value; }
        }

        private Vector2 _sixthRobotSpawnPosition;

        public Vector2 SixthRobotSpawnPosition
        {
            get { return _sixthRobotSpawnPosition; }
            set { _sixthRobotSpawnPosition = value; }
        }

        private string _collectibleToCheckName = "";

        public string CollectibleToCheckName
        {
            get { return _collectibleToCheckName; }
            set { _collectibleToCheckName = value; }
        }
        #endregion

        private Entity _lanternToSpawn;
        private EnemyCore _lanternCore;

        private Entity _firstRobotToSpawn;
        private Entity _secondRobotToSpawn;
        private Entity _thirdRobotToSpawn;
        private Entity _fourthRobotToSpawn;
        private Entity _fifthRobotToSpawn;
        private Entity _sixthRobotToSpawn;

        private Entity _collectibleToCheck;
        private Collectible _collectible;

        private CollectibleState _lastCollectibleState;

        private bool _spawnedLantern = false;
        private bool _enemySpawned = false;

        public ResidencesSlotScript(Entity entity)
            :base(entity, "ResidencesSlotScript")
        {
        }

        public override void Init()
        {
            _lanternToSpawn = entity.GameWorld.GetEntityByName(_lanternName);
            if (_lanternToSpawn != null)
                _lanternCore = _lanternToSpawn.GetComponent<EnemyCore>();

            _firstRobotToSpawn = entity.GameWorld.GetEntityByName(_firstRobotName);
            _secondRobotToSpawn = entity.GameWorld.GetEntityByName(_secondRobotName);
            _thirdRobotToSpawn = entity.GameWorld.GetEntityByName(_thirdRobotName);
            _fourthRobotToSpawn = entity.GameWorld.GetEntityByName(_fourthRobotName);
            _fifthRobotToSpawn = entity.GameWorld.GetEntityByName(_fifthRobotName);
            _sixthRobotToSpawn = entity.GameWorld.GetEntityByName(_sixthRobotName);

            _collectibleToCheck = entity.GameWorld.GetEntityByName(_collectibleToCheckName);
            if (_collectibleToCheck != null)
                _collectible = _collectibleToCheck.GetComponent<Collectible>();

            base.Init();
        }

        public override void Update(GameTime gameTime)
        {
            if (entity.GameWorld.FirstUpdateWorld)
            {
                if (_collectible != null)
                    _lastCollectibleState = _collectible.State;
            }

            if (_collectible != null)
            {
                if (_collectible.State == CollectibleState.Used && _collectible.State != _lastCollectibleState)
                {
                    if (_lanternToSpawn != null)
                    {
                        _spawnedLantern = true;
                        _lanternToSpawn.transform.Position = _lanternSpawnPosition;
                    }
                }

                _lastCollectibleState = _collectible.State;
            }

            if (!_enemySpawned && _lanternToSpawn != null && _spawnedLantern && _lanternCore != null)
            {
                if (_lanternCore.State == EnemyState.Dead)
                {
                    if (_firstRobotToSpawn != null)
                        _firstRobotToSpawn.transform.Position = _firstRobotSpawnPosition;
                    if (_secondRobotToSpawn != null)
                        _secondRobotToSpawn.transform.Position = _secondRobotSpawnPosition;
                    if (_thirdRobotToSpawn != null)
                        _thirdRobotToSpawn.transform.Position = _thirdRobotSpawnPosition;
                    if (_fourthRobotToSpawn != null)
                        _fourthRobotToSpawn.transform.Position = _fourthRobotSpawnPosition;
                    if (_fifthRobotToSpawn != null)
                        _fifthRobotToSpawn.transform.Position = _fifthRobotSpawnPosition;
                    if (_sixthRobotToSpawn != null)
                        _sixthRobotToSpawn.transform.Position = _sixthRobotSpawnPosition;

                    _enemySpawned = true;
                }

            }

            base.Update(gameTime);
        }
    }
}
