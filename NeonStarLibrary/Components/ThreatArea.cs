﻿using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class ThreatArea : Component
    {
        public Enemy EnemyComponent;

        private float _threatRange = 300f;

        public float ThreatRange
        {
            get { return _threatRange; }
            set { _threatRange = value; }
        }

        private string _entityToSearchFor= "";

        public string EntityToSearchFor
        {
            get { return _entityToSearchFor; }
            set { _entityToSearchFor = value; }
        }

        public ThreatArea(Entity entity)
            :base(entity, "ThreatArea")
        {
        }

        public override void Init()
        {
            EnemyComponent = entity.GetComponent<Enemy>();
            if (EnemyComponent != null)
                EnemyComponent._threatArea = this;
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (Entity ent in entity.containerWorld.entities.Where(e => e.Name == _entityToSearchFor))
            {
                if (Vector2.Distance(ent.transform.Position, entity.transform.Position) < ThreatRange)
                {
                    EnemyComponent.State = EnemyState.Chase;
                }
                else if (EnemyComponent.State == EnemyState.Chase)
                {
                    EnemyComponent.State = EnemyState.Idle;
                }
            }
            base.Update(gameTime);
        }

        public void SwitchMode()
        {

        }

    }
}
