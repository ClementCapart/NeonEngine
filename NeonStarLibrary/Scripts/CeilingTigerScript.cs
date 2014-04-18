using NeonEngine;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Scripts
{
    public class CeilingTigerScript : Component
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _tigerName = "ScriptedTiger";

        public string TigerName
        {
            get { return _tigerName; }
            set { _tigerName = value; }
        }
        #endregion

        private Entity _avatarEntity;
        private AvatarCore _avatarComponent;

        private Entity _tigerEntity;
        private EnemyCore _tigerComponent;

        private bool _startedSequence = false;

        public CeilingTigerScript(Entity entity)
            :base(entity, "CeilingTigerScript")
        {
        }

        public override void Init()
        {
            _avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatarEntity != null)
                _avatarComponent = _avatarEntity.GetComponent<AvatarCore>();

            _tigerEntity = entity.GameWorld.GetEntityByName(_tigerName);
            if (_tigerEntity != null)
                _tigerComponent = _tigerEntity.GetComponent<EnemyCore>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_startedSequence)
            {
                if (_avatarComponent != null)
                {
                    _avatarComponent.State = AvatarState.Idle;
                    _avatarComponent.CanMove = false;
                    _avatarComponent.CanAttack = false;
                    _avatarComponent.CanRoll = false;
                    _avatarComponent.CanTurn = false;
                    _avatarComponent.CanUseElement = false;
                }
            }
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            _startedSequence = true;
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
