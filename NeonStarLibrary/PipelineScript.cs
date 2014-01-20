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
        SpriteSheetInfo tpFX;
        AnimatedSpecialEffect tpEffect;
        Entity _screen;

        public PipelineScript(Entity entity)
            :base(entity, "PipelineScript")
        {
        }

        public override void Init()
        {
            fx = AssetManager.GetSpriteSheet("AnimationJury");
            tpFX = AssetManager.GetSpriteSheet("AnimationJuryTeleportDOWN");
            _screen = entity.containerWorld.GetEntityByName("05PipelineScreen");
            _screen.spritesheets.ChangeAnimation("BlankScreen");
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (_inTrigger && ((entity.spritesheets.CurrentSpritesheet.IsFinished && !entity.spritesheets.CurrentSpritesheet.IsLooped) || entity.spritesheets.CurrentSpritesheet.IsLooped))
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
                            EffectsManager.GetEffect(fx, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, 2.0f, 0.9f);
                            entity.spritesheets.ChangeAnimation("IdleModel");
                            _screen.spritesheets.ChangeAnimation("GlobalTweakScreen", 0, true, false, false);
                            break;

                        case 5:
                            EffectsManager.GetEffect(fx, Side.Right, entity.transform.Position, 0.0f, Vector2.Zero, 2.0f ,0.9f);
                            entity.spritesheets.ChangeAnimation("WalkModel");
                            _screen.spritesheets.ChangeAnimation("SituationalTweak", 0, true, false, false);
                            break;

                        case 6:
                            _screen.spritesheets.ChangeAnimation("BlankScreen", 0, true, false, false);
                            entity.spritesheets.ChangeAnimation("Teleport", 0, true, false, false);
                            break;
                    }
                }
            }
            
            if (entity.spritesheets.CurrentSpritesheetName == "Teleport" && entity.spritesheets.IsFinished())
            {
                Vector2 fxPosition = new Vector2(6000f, -565.50f);
                tpEffect = EffectsManager.GetEffect(tpFX, Side.Left, fxPosition, 0.0f, Vector2.Zero, 2.0f, 0.3f);
                entity.spritesheets.CurrentSpritesheetName = "";
            }

            if (tpEffect != null && tpEffect.spriteSheet.currentFrame == tpEffect.spriteSheet.spriteSheetInfo.FrameCount - 2)
            {
                DataManager.LoadPrefab(@"../Data/Prefabs/EnemyTiger.prefab", entity.containerWorld);
                entity.containerWorld.Entities[entity.containerWorld.Entities.Count - 1].transform.Position = new Vector2(6000f, -565.50f);
                entity.containerWorld.Entities[entity.containerWorld.Entities.Count - 1].GetComponent<Enemy>().CurrentSide = Side.Left;
                tpEffect = null;
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
