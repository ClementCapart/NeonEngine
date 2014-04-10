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

        private string _attackName = "";

        public string AttackName
        {
            get { return _attackName; }
            set { _attackName = value; }
        }
        #endregion

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
                            ec.TakeDamage(AttacksManager.StartFreeAttack(_attackName, Side.Right, entity.transform.Position, false));

                        }
                    }

                    if (_hitAvatar)
                    {
                        AvatarCore ac = e.GetComponent<AvatarCore>();
                        if (ac != null)
                        {
                            ac.TakeDamage(AttacksManager.StartFreeAttack(_attackName, Side.Right, entity.transform.Position, true));
                        }
                    }
                }
            }
            base.Update(gameTime);
        }
    }
}
