using NeonEngine;
using NeonEngine.Components.Triggers;
using NeonStarLibrary.Components.Avatar;
using NeonStarLibrary.Components.EnergyObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarLibrary.Components.GameplayElements
{
    public class SaveRoom : Component
    {
        #region Properties
        private string _avatarName = "LiOn";

        public string AvatarName
        {
            get { return _avatarName; }
            set { _avatarName = value; }
        }

        private float _spawnPointIndex = 0.0f;

        public float SpawnPointIndex
        {
            get { return _spawnPointIndex; }
            set { _spawnPointIndex = value; }
        }

        private string _leftColumnName = "LeftColumnToLight";

        public string LeftColumnName
        {
            get { return _leftColumnName; }
            set { _leftColumnName = value; }
        }

        private string _rightColumnName = "RightColumnToLight";

        public string RightColumnName
        {
            get { return _rightColumnName; }
            set { _rightColumnName = value; }
        }

        private string _backgroundName = "BackgroundToLight";

        public string BackgroundName
        {
            get { return _backgroundName; }
            set { _backgroundName = value; }
        }

        private string _cabinName = "SaveCabin";

        public string CabinName
        {
            get { return _cabinName; }
            set { _cabinName = value; }
        }

        private string _leftLampName = "LeftLamp";

        public string LeftLampName
        {
            get { return _leftLampName; }
            set { _leftLampName = value; }
        }

        private string _rightLampName = "RightLamp";

        public string RightLampName
        {
            get { return _rightLampName; }
            set { _rightLampName = value; }
        }
        #endregion

        public bool Active = true;
        private Entity _avatar;
        private AvatarCore _avatarComponent;

        private Entity _leftColumn;
        private Entity _rightColumn;

        private Entity _background;
        private Entity _cabin;

        private Entity _leftLamp;
        private Entity _rightLamp;

        public SaveRoom(Entity entity)
            :base(entity, "CheckPoint")
        {
            RequiredComponents = new Type[] { typeof(HitboxTrigger) };
        }

        public override void Init()
        {
            _avatar = entity.GameWorld.GetEntityByName(_avatarName);
            if (_avatar != null)
                _avatarComponent = _avatar.GetComponent<AvatarCore>();

            _leftColumn = entity.GameWorld.GetEntityByName(_leftColumnName);
            if (_leftColumn != null && _leftColumn.spritesheets != null)
                _leftColumn.spritesheets.ChangeAnimation("Off");
            _rightColumn = entity.GameWorld.GetEntityByName(_rightColumnName);
            if (_rightColumn != null && _rightColumn.spritesheets != null)
                _rightColumn.spritesheets.ChangeAnimation("Off");
            _background = entity.GameWorld.GetEntityByName(_backgroundName);
            if (_background != null && _background.spritesheets != null)
                _background.spritesheets.ChangeAnimation("Off");
            _cabin = entity.GameWorld.GetEntityByName(_cabinName);
            if (_cabin != null && _cabin.spritesheets != null)
                _cabin.spritesheets.ChangeAnimation("Off");

            _leftLamp = entity.GameWorld.GetEntityByName(_leftLampName);
            if (_leftLamp != null && _leftLamp.spritesheets != null)
                _leftLamp.spritesheets.ChangeAnimation("Off");

            _rightLamp = entity.GameWorld.GetEntityByName(_rightColumnName);
            if (_rightLamp != null && _rightLamp.spritesheets != null)
                _rightLamp.spritesheets.ChangeAnimation("Off");

            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            SaveCheckPoint();
            if (_leftColumn != null && _leftColumn.spritesheets != null)
                _leftColumn.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
            if (_rightColumn != null && _rightColumn.spritesheets != null)
                _rightColumn.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
            if (_background != null && _background.spritesheets != null)
                _background.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
            if (_cabin != null && _cabin.spritesheets != null)
                _cabin.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
            if (_leftLamp != null && _leftLamp.spritesheets != null)
                _leftLamp.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);
            if (_rightLamp != null && _rightLamp.spritesheets != null)
                _rightLamp.spritesheets.ChangeAnimation("Lighting", 0, true, false, false);

            base.OnTrigger(trigger, triggeringEntity, parameters);
        }

        private void SaveCheckPoint()
        {
            GameScreen.CheckPointsData.Add((entity.GameWorld as GameScreen).SaveStatus(this));
            if (_avatarComponent != null)
                _avatarComponent.CurrentHealthPoints = _avatarComponent.StartingHealthPoints;
            HealStation._usedHealStations.Clear();        
        }




    }
}
