using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class RandomTilableGraphic : DrawableComponent
    {
        #region Properties
        private float _tilingWidth = 100.0f;
        public float TilingWidth
        {
            get { return _tilingWidth; }
            set { _tilingWidth = value; }
        }

        private bool _useTextureHeight = true;

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

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        private string _closerGraphicTag = "";

        public string CloserGraphicTag
        {
            get { return _closerGraphicTag; }
            set { _closerGraphicTag = value; }
        }

        private string _firstTileGraphicTag = "";

        public string FirstTileGraphicTag
        {
            get { return _firstTileGraphicTag; }
            set { _firstTileGraphicTag = value; }
        }

        private string _secondTileGraphicTag = "";

        public string SecondTileGraphicTag
        {
            get { return _secondTileGraphicTag; }
            set { _secondTileGraphicTag = value; }
        }

        private string _thirdTileGraphicTag = "";

        public string ThirdTileGraphicTag
        {
            get { return _thirdTileGraphicTag; }
            set { _thirdTileGraphicTag = value; }
        }

        private string _fourthTileGraphicTag = "";

        public string FourthTileGraphicTag
        {
            get { return _fourthTileGraphicTag; }
            set { _fourthTileGraphicTag = value; }
        }

        private string _fifthTileGraphicTag = "";

        public string FifthTileGraphicTag
        {
            get { return _fifthTileGraphicTag; }
            set { _fifthTileGraphicTag = value; }
        }

        private string _tilingHash = "";

        public string TilingHash
        {
            get { return _tilingHash; }
            set { _tilingHash = value; }
        }
        #endregion

        private Texture2D _closerTexture;
        private Texture2D _firstTileTexture;
        private Texture2D _secondTileTexture;
        private Texture2D _thirdTileTexture;
        private Texture2D _fourthTileTexture;
        private Texture2D _fifthTileTexture;

        Dictionary<Texture2D, List<Vector2>> _randomResult = new Dictionary<Texture2D, List<Vector2>>();

        private int _numberOfDifferentTiles = 0;

        public RandomTilableGraphic(Entity entity)
            :base(1.0f, entity, "RandomTilableGraphic")
        {
        }

        public override void Init()
        {
            this._closerTexture = AssetManager.GetTexture(_closerGraphicTag);
            this._firstTileTexture = AssetManager.GetTexture(_firstTileGraphicTag);
            if (_firstTileTexture != null)
                _numberOfDifferentTiles++;
            this._secondTileTexture = AssetManager.GetTexture(_secondTileGraphicTag);
            if (_secondTileTexture != null)
                _numberOfDifferentTiles++;
            this._thirdTileTexture = AssetManager.GetTexture(_thirdTileGraphicTag);
            if (_thirdTileTexture != null)
                _numberOfDifferentTiles++;
            this._fourthTileTexture = AssetManager.GetTexture(_fourthTileGraphicTag);
            if (_fourthTileTexture != null)
                _numberOfDifferentTiles++;
            this._fifthTileTexture = AssetManager.GetTexture(_fifthTileGraphicTag);
            if (_fifthTileTexture != null)
                _numberOfDifferentTiles++;

            if (_tilingHash == "")
            {
                RandomizeTile();
               
            }
            else
            {

            }
            base.Init();
        }

        private void RandomizeTile()
        {
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstTileTexture != null)
                textures.Add(_firstTileTexture);
            if (_secondTileTexture != null)
                textures.Add(_secondTileTexture);
            if (_thirdTileTexture != null)
                textures.Add(_thirdTileTexture);
            if (_fourthTileTexture != null)
                textures.Add(_fourthTileTexture);
            if (_fifthTileTexture != null)
                textures.Add(_fifthTileTexture);

            float widthWithoutClosers = _tilingWidth - _closerTexture.Width * 2;

            Random r = new Random();
            int firstTile = r.Next(textures.Count);
            Console.WriteLine(firstTile.ToString());
            
        }
    }
}
