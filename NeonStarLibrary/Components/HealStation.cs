using NeonEngine;
using NeonEngine.Components.CollisionDetection;
using NeonStarLibrary.Components.Avatar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.EnergyObjects
{
    public class HealStation : EnergyComponent
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private string _offAnimation = "";

        public string OffAnimation
        {
            get { return _offAnimation; }
            set { _offAnimation = value; }
        }

        private string _lightingAnimation = "";

        public string LightingAnimation
        {
            get { return _lightingAnimation; }
            set { _lightingAnimation = value; }
        }

        private string _onAnimation = "";

        public string OnAnimation
        {
            get { return _onAnimation; }
            set { _onAnimation = value; }
        }
        #endregion

        private Entity _avatarEntity;
        private AvatarCore _avatarComponent;
        private bool _used = false;

        public static List<string> _usedHealStations = new List<string>();

        public HealStation(Entity entity)
            :base(entity)
        {
            Name = "HealStation";
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _avatarEntity = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatarEntity != null)
                _avatarComponent = _avatarEntity.GetComponent<AvatarCore>();
            if (_usedHealStations.Contains(entity.GameWorld.LevelGroupName + "_" + entity.GameWorld.LevelName + "_" + entity.Name))
                _used = true;


            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_powered && !_used)
            {
                if (_avatarEntity != null && _avatarComponent != null && _avatarEntity.hitboxes.Count > 0)
                {
                    if (_avatarEntity.hitboxes[0].hitboxRectangle.Intersects(entity.hitboxes[0].hitboxRectangle))
                    {
                        if (Neon.Input.Pressed(NeonStarInput.Interact) && _avatarComponent.CurrentHealthPoints < _avatarComponent.StartingHealthPoints)
                        {
                            _avatarComponent.CurrentHealthPoints = _avatarComponent.StartingHealthPoints;
                            _usedHealStations.Add(entity.GameWorld.LevelGroupName + "_" + entity.GameWorld.LevelName + "_" + entity.Name);
                            _used = true;
                        }
                    }
                }
            }
            base.Update(gameTime);
        }


    }
}
