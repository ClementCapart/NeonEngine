using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class QuotaEnergyDoor : EnergyDoor
    {
        #region Properties
        private string _doorTopNickname = "";

        public string DoorTopNickName
        {
            get { return _doorTopNickname; }
            set { _doorTopNickname = value; }
        }

        private string _doorLockNickname = "";

        public string DoorLockNickname
        {
            get { return _doorLockNickname; }
            set { _doorLockNickname = value; }
        }
        #endregion

        private SpriteSheet _doorTop;
        private SpriteSheet _doorLock;

        public QuotaEnergyDoor(Entity e)
            :base(e)
        {
            Name = "QuotaEnergyDoor";
        }

        public override void Init()
        {
            List<SpriteSheet> sss = entity.GetComponentsByInheritance<SpriteSheet>();
            foreach (SpriteSheet ss in sss)
            {
                if (ss.NickName == _doorTopNickname)
                {
                    _doorTop = ss;
                    continue;
                }

                if (ss.NickName == _doorLockNickname)
                    _doorLock = ss;
            }

            if (_doorTop != null)
            {
                _doorTop.IsLooped = false;
                _doorTop.isPlaying = false;
                _doorTop.currentFrame = 0;
            }

            if (_doorLock != null)
                _doorLock.isPlaying = false;
            base.Init();

            if (_powered)
                this.entity.spritesheets.Layer = 0.3f;
            else
                this.entity.spritesheets.Layer = 0.6f;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void PowerDevice()
        {
            this.entity.spritesheets.Layer = 0.3f;
            if (_doorTop != null)
            {
                _doorTop.currentFrame = 0;
                _doorTop.isPlaying = true;
            }
            base.PowerDevice();
        }

        public override void UnpowerDevice()
        {
            this.entity.spritesheets.Layer = 0.6f;
            base.UnpowerDevice();
        }
    }
}
