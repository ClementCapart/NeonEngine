using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;
using Microsoft.Xna.Framework;

namespace NeonStarLibrary.Entities
{
    public class Cosmetic : Entity
    {
        public SpriteSheet spriteSheet;

        public float drawLayer
        {
            get { return spriteSheet.DrawLayer; }
            set { spritesheet.DrawLayer = value; }
        }

        private string _spriteSheetTag;
        public string spriteSheetTag
        {
            get { return _spriteSheetTag; }
            set { _spriteSheetTag = value; }
        }

        public Cosmetic(Vector2 Position, string SpriteSheetTag, float DrawLayer, World containerWorld)
            : base(containerWorld)
        {
            this.transform.Position = Position;
            this.spriteSheetTag = SpriteSheetTag;
            spriteSheet = (SpriteSheet)AddComponent(new SpriteSheet(AssetManager.GetSpriteSheet(spriteSheetTag), DrawLayer, this));
            spriteSheet.Init();
        }

        public Cosmetic(string SpriteSheetTag, float DrawLayer, World containerWorld)
            :this(Vector2.Zero, SpriteSheetTag, DrawLayer, containerWorld)
        {

        }
    }
}
