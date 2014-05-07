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
    public class DamagingArea : Component
    {
        #region Properties
        private bool _hitEnemies = true;

        public bool HitEnemies
        {
            get { return _hitEnemies; }
            set { _hitEnemies = value; }
        }

        private bool _hitAvatar = true;

        public bool HitAvatar
        {
            get { return _hitAvatar; }
            set { _hitAvatar = value; }
        }

        private string _hitAvatarAttackName = "";

        public string HitAvatarAttackName
        {
            get { return _hitAvatarAttackName; }
            set { _hitAvatarAttackName = value; }
        }

        private string _hitEnemyAttackName = "";

        public string HitEnemyAttackName
        {
            get { return _hitEnemyAttackName; }
            set { _hitEnemyAttackName = value; }
        }
        #endregion

        private Attack _damagingAvatar;
        private Attack _damagingEnemies;

        public DamagingArea(Entity entity)
            :base(entity, "DamagingArea")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!ComponentEnabled)
                return;

            if(!_hitEnemies && !_hitAvatar)
                return;

            foreach (Entity e in entity.GameWorld.Entities)
            {
                if (e.hitboxes.Count > 0 && e.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                {
                    if (_hitEnemies)
                    {
                        EnemyCore ec = e.GetComponent<EnemyCore>();
                        if (ec != null)
                        {
                            if (_damagingEnemies == null)
                            {
                                _damagingEnemies = AttacksManager.StartFreeAttack(_hitEnemyAttackName, Side.Right, entity.transform.Position, false);
                                if(_damagingEnemies != null)
                                    _damagingEnemies.Launcher = entity.GameWorld.Avatar;
                            }
                        }
                    }

                    if (_hitAvatar)
                    {
                        AvatarCore ac = e.GetComponent<AvatarCore>();
                        if (ac != null)
                        {
                            if(_damagingAvatar == null)
                                _damagingAvatar = AttacksManager.StartFreeAttack(_hitAvatarAttackName, Side.Right, entity.transform.Position, true);
                        }
                    }
                }
            }

            if (_damagingAvatar != null && _damagingAvatar.CooldownFinished)
                _damagingAvatar = null;
            if (_damagingEnemies != null && _damagingEnemies.CooldownFinished)
                _damagingEnemies = null;

            base.Update(gameTime);
        }
    }
}
