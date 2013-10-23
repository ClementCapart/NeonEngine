using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    class FollowPlatformNodes : FollowNodes
    {
        public LinkedToPath CurrentPlatform;
        private Rigidbody _lastRigidBody;
        private Entity _lastEntityHit;

        public FollowPlatformNodes(Entity entity)
            :base(entity)
        {
            
        }

        public override void Init()
        {
            if (entity.rigidbody.beacon != null)
            {
                _lastRigidBody = entity.rigidbody.beacon.CheckGround();
                if (_lastRigidBody != null)
                {
                    _lastEntityHit = _lastRigidBody.entity;
                    CurrentPlatform = _lastEntityHit.GetComponent<LinkedToPath>();
                }
            }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (CurrentPlatform != null && this.CurrentNodeList != CurrentPlatform.LinkedPathNodeList)
                this.CurrentNodeList = CurrentPlatform.LinkedPathNodeList;
            
            Rigidbody rg = entity.rigidbody.beacon.CheckGround();
            if (rg != null && rg != _lastRigidBody)
            {
                SearchForNewPath(rg);
            }
            base.Update(gameTime);
        }

        private void SearchForNewPath(Rigidbody rg)
        {
            LinkedToPath linkedToPath = rg.entity.GetComponent<LinkedToPath>();
            if (linkedToPath != null)
            {
                CurrentPlatform = linkedToPath;
                _lastRigidBody = rg;
                _lastEntityHit = rg.entity;
            }
            else
                CurrentNodeList = null;
        }
    }
}
