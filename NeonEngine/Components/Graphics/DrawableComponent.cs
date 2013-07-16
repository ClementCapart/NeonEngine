using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public abstract class DrawableComponent : Component
    {
        public DrawLayer DrawType;
        public bool CastShadow;

        public DrawableComponent(DrawLayer DrawType, Entity entity, string Name)
            :base(entity, Name)
        {
            this.DrawType = DrawType;
        }

        public virtual void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
        }

        public void ChangeLayer(DrawLayer drawLayer)
        {
            if (entity.Components.Contains(this))
            {
                switch (this.DrawType)
                {
                    case DrawLayer.Background0:
                        this.entity.containerWorld.backgroundZeroLayer.Remove(this);
                        break;
                    case DrawLayer.Background1:
                        this.entity.containerWorld.backgroundFirstLayer.Remove(this);
                        break;
                    case DrawLayer.Middleground2:
                        this.entity.containerWorld.middlegroundSecondLayer.Remove(this);
                        break;
                    case DrawLayer.Middleground3:
                        this.entity.containerWorld.middlegroundThirdLayer.Remove(this);
                        break;
                    case DrawLayer.Middleground4:
                        this.entity.containerWorld.middlegroundFourthLayer.Remove(this);
                        break;
                    case DrawLayer.Foreground5:
                        this.entity.containerWorld.foregroundFifthLayer.Remove(this);
                        break;
                    case DrawLayer.Foreground6:
                        this.entity.containerWorld.foregroundSixthLayer.Remove(this);
                        break;
                    case DrawLayer.HUD:
                        this.entity.containerWorld.HUDComponents.Remove(this);
                        break;
                }

                switch (drawLayer)
                {
                    case DrawLayer.Background0:
                        this.entity.containerWorld.backgroundZeroLayer.Add(this);
                        break;
                    case DrawLayer.Background1:
                        this.entity.containerWorld.backgroundFirstLayer.Add(this);
                        break;
                    case DrawLayer.Middleground2:
                        this.entity.containerWorld.middlegroundSecondLayer.Add(this);
                        break;
                    case DrawLayer.Middleground3:
                        this.entity.containerWorld.middlegroundThirdLayer.Add(this);
                        break;
                    case DrawLayer.Middleground4:
                        this.entity.containerWorld.middlegroundFourthLayer.Add(this);
                        break;
                    case DrawLayer.Foreground5:
                        this.entity.containerWorld.foregroundFifthLayer.Add(this);
                        break;
                    case DrawLayer.Foreground6:
                        this.entity.containerWorld.foregroundSixthLayer.Add(this);
                        break;
                    case DrawLayer.HUD:
                        this.entity.containerWorld.HUDComponents.Add(this);
                        break;
                }
            }

            DrawType = drawLayer;       
        }

        public override void Remove()
        {        
            switch (DrawType)
            {
                case DrawLayer.Background0:
                    this.entity.containerWorld.backgroundZeroLayer.Remove(this);
                    break;
                case DrawLayer.Background1:
                    this.entity.containerWorld.backgroundFirstLayer.Remove(this);
                    break;
                case DrawLayer.Middleground2:
                    this.entity.containerWorld.middlegroundSecondLayer.Remove(this);
                    break;
                case DrawLayer.Middleground3:
                    this.entity.containerWorld.middlegroundThirdLayer.Remove(this);
                    break;
                case DrawLayer.Middleground4:
                    this.entity.containerWorld.middlegroundFourthLayer.Remove(this);
                    break;
                case DrawLayer.Foreground5:
                    this.entity.containerWorld.foregroundFifthLayer.Remove(this);
                    break;
                case DrawLayer.Foreground6:
                    this.entity.containerWorld.foregroundSixthLayer.Remove(this);
                    break;
                case DrawLayer.HUD:
                    this.entity.containerWorld.HUDComponents.Remove(this);
                    break;
            }
            base.Remove();
        }
    }
}
