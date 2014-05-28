using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class TilableSpritesheet : SpriteSheet
    {
        #region Properties
        private bool _useTextureWidth = false;

        public bool UseTextureWidth
        {
            get { return _useTextureWidth; }
            set { _useTextureWidth = value; }
        }

        private float _tilingWidth = 100.0f;

        public float TilingWidth
        {
            get { return _tilingWidth; }
            set { _tilingWidth = value; }
        }

        private bool _useTextureHeight = false;

        public bool UseTextureHeight
        {
            get { return _useTextureHeight; }
            set { _useTextureHeight = value; }
        }

        private float _tilingHeight = 100.0f;

        public float TilingHeight
        {
            get { return _tilingHeight; }
            set { _tilingHeight = value; }
        }
        #endregion

        public TilableSpritesheet(Entity entity)
            :base(entity)
        {
            Name = "TilableSpritesheet";
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritebatch)
        {
            if (spriteSheetInfo != null && spriteSheetInfo.Frames != null && Active)
            {
                Vector2 CurrentOffset = Vector2.Transform(spriteSheetInfo.Offset, Matrix.CreateRotationZ(RotationOffset));
                if (particle == null)
                {
                    if (currentFrame >= 0 && currentFrame <= spriteSheetInfo.Frames.Length - 1)
                    {
                        CurrentEffect.CurrentTechnique.Passes[0].Apply();
                        spritebatch.Draw(spriteSheetInfo.Frames[currentFrame], new Vector2((int)(entity.transform.Position.X + ((CurrentSide == Side.Right ? (int)CurrentOffset.X + (int)Offset.X : -(int)CurrentOffset.X - (int)Offset.X) * entity.transform.Scale)), (int)(entity.transform.Position.Y + (((int)CurrentOffset.Y + Offset.Y) * entity.transform.Scale))) + _parallaxPosition, new Rectangle(0, 0, (int)((_useTextureWidth ? (int)spriteSheetInfo.FrameWidth : (int)_tilingWidth / entity.transform.Scale)), (int)((_useTextureHeight ? (int)spriteSheetInfo.FrameHeight : (int)_tilingHeight / entity.transform.Scale))),
                            Color.Lerp(Color.Transparent, Tint ? Color.Lerp(Color.White, TintColor, 0.5f) : MainColor, Opacity), RotationOffset, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2) + RotationCenter, entity.transform.Scale, CurrentSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    }
                }
                else
                {
                    spritebatch.Draw(spriteSheetInfo.Frames[currentFrame], particle.Position, null,
                        TintColor, particle.Angle, new Vector2(spriteSheetInfo.FrameWidth / 2, spriteSheetInfo.FrameHeight / 2), scale, spriteEffects, Layer);
                }
            }
        }
    }
}
