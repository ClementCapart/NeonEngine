using System;
using NeonEngine;
using NeonStarLibrary;
using NeonEngine.Private;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Components.Avatar;

namespace NeonStarLibrary.Components.Scripts
{
    public class TrainingEntranceScript : ScriptComponent
    {
        private Entity _airWave;

        private SpriteSheetInfo _fadeEffect;

        public TrainingEntranceScript(Entity entity)
            :base(entity, "TrainingEntranceScript")
        {
        }
		
		public override void Init()
		{
            _fadeEffect = AssetManager.GetSpriteSheet("WindAnimFrontFade");
            _airWave = entity.GameWorld.GetEntityByName("AirWave");
		}

        public override void Update(GameTime gameTime)
        {
            if(_airWave != null && _airWave.transform.Position.X <= 0)
            {
                if (_fadeEffect != null)
                {
                    EffectsManager.GetEffect(_fadeEffect, Side.Right, entity.transform.Position, 0.0f, new Vector2(0, 0), 2.0f, 0.45f);
                }
                _airWave.transform.Position += new Vector2(1650,0);
                
            }
        }
    }
}
