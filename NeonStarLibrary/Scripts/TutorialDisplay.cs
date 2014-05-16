using NeonEngine;
using NeonEngine.Components.Graphics2D;
using NeonStarLibrary.Components.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Scripts
{
    public class TutorialDisplay : Component
    {
        #region Properties
        private string _enemyToCheck = "";

        public string EnemyToCheck
        {
            get { return _enemyToCheck; }
            set { _enemyToCheck = value; }
        }

        private string _tutorialPanel = "";

        public string TutorialPanel
        {
            get { return _tutorialPanel; }
            set { _tutorialPanel = value; }
        }
        #endregion

        private Entity _enemy;
        private EnemyCore _enemyComponent;
        private List<DrawableComponent> _tutorialComponents;
        private bool _tutorialDisplayed = false;

        public TutorialDisplay(Entity entity)
            :base(entity, "TutorialDisplay")
        {

        }

        public override void Init()
        {
            _enemy = entity.GameWorld.GetEntityByName(_enemyToCheck);
            if (_enemy != null)
                _enemyComponent = _enemy.GetComponent<EnemyCore>();
            Entity e = entity.GameWorld.GetEntityByName(_tutorialPanel);
            if(e != null)
                _tutorialComponents = e.GetComponentsByInheritance<DrawableComponent>();

            if(_tutorialComponents != null)
                foreach (DrawableComponent dc in _tutorialComponents)
                {
                    dc.Opacity = 0.0f;
                }
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!_tutorialDisplayed && _enemyComponent != null && (_enemyComponent.State == EnemyState.Dying || _enemyComponent.State == EnemyState.Dead))
            {
                if (_tutorialComponents != null)
                    foreach (DrawableComponent dc in _tutorialComponents)
                    {
                        dc.Opacity += 1.0f * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        if (dc.Opacity >= 1.0f)
                        {
                            dc.Opacity = 1.0f;
                            _tutorialDisplayed = true;
                        }
                    }
            }
            base.Update(gameTime);
        }
    }
}
