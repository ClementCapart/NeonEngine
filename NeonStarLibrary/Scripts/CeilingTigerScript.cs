using Microsoft.Xna.Framework;
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

        private string _robotEntityName = "ScriptedRobot";

        public string RobotEntityName
        {
            get { return _robotEntityName; }
            set { _robotEntityName = value; }
        }

        private string _exclamationMarkName = "ExclamationMark";

        public string ExclamationMarkName
        {
            get { return _exclamationMarkName; }
            set { _exclamationMarkName = value; }
        }

        private Vector2 _effectOffset = Vector2.Zero;

        public Vector2 EffectOffset
        {
            get { return _effectOffset; }
            set { _effectOffset = value; }
        }
        #endregion

        private Entity _avatarEntity;
        private AvatarCore _avatarComponent;

        private Entity _tigerEntity;
        private EnemyCore _tigerComponent;

        private Entity _robotEntity;

        private bool _startedSequence = false;
        private bool _startMovingTiger = false;
        private bool _startUsingThunder = false;
        private bool _usedThunder = false;
        private bool _finishedSequence = false;
        private SpriteSheetInfo _exclamationMark;

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

            _robotEntity = entity.GameWorld.GetEntityByName(_robotEntityName);

            _exclamationMark = AssetManager.GetSpriteSheet(_exclamationMarkName);
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_finishedSequence)
            {
                if (Vector2.DistanceSquared(entity.GameWorld.Camera.PreviousPosition, entity.GameWorld.Camera.Position) < 0.1f)
                {
                    entity.GameWorld.Camera.MustStrengthen = true;
                }
            }

            if (_startedSequence && !_finishedSequence)
            {
                if (_tigerEntity != null)
                {
                    entity.GameWorld.Camera.MustStrengthen = false;
                    entity.GameWorld.Camera.ChaseStrength = 0.035f;
                    entity.GameWorld.Camera.Chase(new Vector2(443, -1696), Vector2.Zero, true, gameTime);
                }
                if (_avatarComponent != null)
                {
                    _avatarComponent.State = AvatarState.Idle;
                    _avatarComponent.CanMove = false;
                    _avatarComponent.CanAttack = false;
                    _avatarComponent.CanRoll = false;
                    _avatarComponent.CanTurn = false;
                    _avatarComponent.CanUseElement = false;
                    if(!_startUsingThunder)
                        if(_tigerComponent != null)
                            _tigerComponent.State = EnemyState.ScriptControlled;
                    (entity.GameWorld as GameScreen).MustFollowAvatar = false;                
                }

                if (!_startMovingTiger)
                {
                    _startMovingTiger = true;
                    EffectsManager.GetEffect(_exclamationMark, Side.Right, _avatarEntity.transform.Position, 0.0f, _effectOffset, 2.0f, 1.0f, null, 2.5f, false);
                }
                else if (_startMovingTiger && !_startUsingThunder)
                {
                    if (_tigerComponent != null)
                    {                      
                        _tigerEntity.spritesheets.ChangeAnimation(_tigerComponent.RunAnim);
                        _tigerEntity.rigidbody.body.LinearVelocity = -new Vector2(_tigerComponent.Chase.ChaseSpeed / 2, 0);
                        if (_tigerEntity.transform.Position.X < 820.0f)
                            _startUsingThunder = true;
                    }
                }
                else if (_startUsingThunder && !_usedThunder)
                {
                    if (_tigerComponent != null && _robotEntity != null)
                    {
                        _tigerComponent.State = EnemyState.Attacking;
                        _tigerComponent.Attack.CurrentAttack = AttacksManager.GetAttack("EnemyTigerDash", Side.Left, _tigerEntity, _robotEntity);
                        _usedThunder = true;
                    }
                }
                else if(_usedThunder)
                {
                    if(_robotEntity != null)
                    {
                        if (!_robotEntity.GameWorld.Entities.Contains(_robotEntity))
                        {
                            _tigerComponent.State = EnemyState.Idle;
                            _finishedSequence = true;
                            entity.GameWorld.Camera.ChaseStrength = 0.05f;
                            (entity.GameWorld as GameScreen).MustFollowAvatar = true;
                        }
                    }
                }          
            }
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            _startedSequence = true;
            if (_avatarEntity != null)
                _avatarEntity.rigidbody.body.LinearVelocity = Vector2.Zero;
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
