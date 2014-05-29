using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class TilableSpritesheetManager : SpritesheetManager
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

        public TilableSpritesheetManager(Entity entity)
            : base(entity)
        {
            Name = "TilableSpritesheetManager";
            CurrentSpritesheet = new TilableSpritesheet(entity);
        }

        public override void ChangeAnimation(string spriteSheetName, bool forceSwitch = false, int priority = 0, bool IsPlaying = true, bool Reset = false, bool Loop = true, int StartingFrame = -1)
        {
            base.ChangeAnimation(spriteSheetName, forceSwitch, priority, IsPlaying, Reset, Loop, StartingFrame);
            (CurrentSpritesheet as TilableSpritesheet).UseTextureHeight = UseTextureHeight;
            (CurrentSpritesheet as TilableSpritesheet).UseTextureWidth = UseTextureWidth;
            (CurrentSpritesheet as TilableSpritesheet).TilingHeight = TilingHeight;
            (CurrentSpritesheet as TilableSpritesheet).TilingWidth = TilingWidth;
        }

    }
}
