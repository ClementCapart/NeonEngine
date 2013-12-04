using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class PipelineScript : Component
    {
        private int _currentState = 0;
        private bool _inTrigger = false;

        SpriteSheetInfo fx;
        Entity _screen;

        public PipelineScript(Entity entity)
            :base(entity, "PipelineScript")
        {
        }

        public override void Init()
        {
            fx = AssetManager.GetSpriteSheet("AnimationJury");
            _screen = Neon.world.GetEntityByName("05PipelineScreen");
            _screen.spritesheets.ChangeAnimation("BlankScreen");
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_inTrigger)
            {
                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.Y))
                {
                    _currentState++;
                    switch (_currentState)
                    {
                        case 1:
                            entity.spritesheets.ChangeAnimation("BluePrintModel", 0, true, false, false);
                            _screen.spritesheets.ChangeAnimation("BehaviourConceptScreen", 0, true, false, false);
                            break;

                        case 2:
                            entity.spritesheets.ChangeAnimation("SkeletonModel", 0, true, false, false);
                            _screen.spritesheets.ChangeAnimation("BehaviourProdScreen", 0, true, false, false);
                            break;

                        case 3:
                            entity.spritesheets.ChangeAnimation("ConstructedModel", 0, true, false, false);
                            _screen.spritesheets.ChangeAnimation("ArtScreen", 0, true, false, false);
                            break;

                        case 4:
                            EffectsManager.GetEffect(fx, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, 0.9f);
                            entity.spritesheets.ChangeAnimation("IdleModel");
                            _screen.spritesheets.ChangeAnimation("GlobalTweakScreen", 0, true, false, false);
                            break;

                        case 5:
                            EffectsManager.GetEffect(fx, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, 0.9f);
                            entity.spritesheets.ChangeAnimation("WalkModel");
                            _screen.spritesheets.ChangeAnimation("SituationalTweak", 0, true, false, false);
                            break;

                        case 6:
                            _screen.spritesheets.ChangeAnimation("BlankScreen", 0, true, false, false);
                            break;
                    }
                }
            }
            base.Update(gameTime);
        }

        public override void FinalUpdate(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _inTrigger = false;
            base.FinalUpdate(gameTime);
        }

        public override void OnTrigger(Entity trigger, Entity triggeringEntity, object[] parameters = null)
        {
            _inTrigger = true;
            
            base.OnTrigger(trigger, triggeringEntity, parameters);
        }
    }
}
