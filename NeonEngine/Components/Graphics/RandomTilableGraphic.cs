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
        private bool _verticalTiling = false;

        public bool VerticalTiling
        {
            get { return _verticalTiling; }
            set { _verticalTiling = value; }
        }

        private float _tilingSize = 100.0f;
        public float TilingSize
        {
            get { return _tilingSize; }
            set { _tilingSize = value; }
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

        private Side _closerBasicSide = Side.Right;

        public Side CloserBasicSide
        {
            get { return _closerBasicSide; }
            set { _closerBasicSide = value; }
        }

        private float _closerRotation = 0.0f;

        public float CloserRotation
        {
            get { return _closerRotation; }
            set 
            {
                if (value == 3.14f)
                    _closerRotation = (float)Math.PI;
                else if (value == 1.57f)
                    _closerRotation = (float)Math.PI / 2;
                else if (value == 4.71f)
                    _closerRotation = (float)Math.PI + (float)Math.PI / 2;
                else
                    _closerRotation = value; 
            }
        }

        private string _secondCloserGraphicTag = "";

        public string SecondCloserGraphicTag
        {
            get { return _secondCloserGraphicTag; }
            set { _secondCloserGraphicTag = value; }
        }

        private Side _secondCloserBasicSide = Side.Left;

        public Side SecondCloserBasicSide
        {
            get { return _secondCloserBasicSide; }
            set { _secondCloserBasicSide = value; }
        }

        private float _secondCloserRotation = 0.0f;

        public float SecondCloserRotation
        {
            get { return _secondCloserRotation; }
            set
            {
                if (value == 3.14f)
                    _secondCloserRotation = (float)Math.PI;
                else if (value == 1.57f)
                    _secondCloserRotation = (float)Math.PI / 2;
                else if (value == 4.71f)
                    _secondCloserRotation = (float)Math.PI + (float)Math.PI / 2;
                else
                    _secondCloserRotation = value;
            }
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

        private string _sixthTileGraphicTag = "";

        public string SixthTileGraphicTag
        {
            get { return _sixthTileGraphicTag; }
            set { _sixthTileGraphicTag = value; }
        }

        private string _seventhTileGraphicTag = "";

        public string SeventhTileGraphicTag
        {
            get { return _seventhTileGraphicTag; }
            set { _seventhTileGraphicTag = value; }
        }

        private string _eighthTileGraphicTag = "";

        public string EighthTileGraphicTag
        {
            get { return _eighthTileGraphicTag; }
            set { _eighthTileGraphicTag = value; }
        }

        private string _ninthTileGraphicTag = "";

        public string NinthTileGraphicTag
        {
            get { return _ninthTileGraphicTag; }
            set { _ninthTileGraphicTag = value; }
        }

        private string _tenthTileGraphicTag = "";

        public string TenthTileGraphicTag
        {
            get { return _tenthTileGraphicTag; }
            set { _tenthTileGraphicTag = value; }
        }

        private bool _keepTilingHash = false;

        public bool KeepTilingHash
        {
            get { return _keepTilingHash; }
            set { _keepTilingHash = value; }
        }

        private string _tilingHash = "";

        public string TilingHash
        {
            get { return _tilingHash; }
            set { _tilingHash = value; }
        }

        private Vector2 _closerOffset;

        public Vector2 CloserOffset
        {
            get { return _closerOffset; }
            set { _closerOffset = value; }
        }

        private Vector2 _secondCloserOffset;

        public Vector2 SecondCloserOffset
        {
            get { return _secondCloserOffset; }
            set { _secondCloserOffset = value; }
        }

        #endregion

        private Texture2D _closerTexture;
        private Texture2D _secondCloserTexture;
        private Texture2D _firstTileTexture;
        private Texture2D _secondTileTexture;
        private Texture2D _thirdTileTexture;
        private Texture2D _fourthTileTexture;
        private Texture2D _fifthTileTexture;
        private Texture2D _sixthTileTexture;
        private Texture2D _seventhTileTexture;
        private Texture2D _eighthTileTexture;
        private Texture2D _ninthTileTexture;
        private Texture2D _tenthTileTexture;

        Dictionary<Texture2D, List<Vector2>> _randomResult = new Dictionary<Texture2D, List<Vector2>>();

        public RandomTilableGraphic(Entity entity)
            :base(0.5f, entity, "RandomTilableGraphic")
        {
        }

        public override void Init()
        {
            this._closerTexture = AssetManager.GetTexture(_closerGraphicTag);
            this._secondCloserTexture = AssetManager.GetTexture(_secondCloserGraphicTag);
            this._firstTileTexture = AssetManager.GetTexture(_firstTileGraphicTag);
            this._secondTileTexture = AssetManager.GetTexture(_secondTileGraphicTag);
            this._thirdTileTexture = AssetManager.GetTexture(_thirdTileGraphicTag);
            this._fourthTileTexture = AssetManager.GetTexture(_fourthTileGraphicTag);   
            this._fifthTileTexture = AssetManager.GetTexture(_fifthTileGraphicTag);
            this._sixthTileTexture = AssetManager.GetTexture(_sixthTileGraphicTag);
            this._seventhTileTexture = AssetManager.GetTexture(_seventhTileGraphicTag);
            this._eighthTileTexture = AssetManager.GetTexture(_eighthTileGraphicTag);
            this._ninthTileTexture = AssetManager.GetTexture(_ninthTileGraphicTag);
            this._tenthTileTexture = AssetManager.GetTexture(_tenthTileGraphicTag);

            if (_tilingHash == "" || !_keepTilingHash)
            {
                RandomizeTile();
            }
            else
            {
                LoadFromHash();
            }
            base.Init();
        }

        private void RandomizeTile()
        {
            List<Texture2D> textures = new List<Texture2D>();

            _tilingHash = "";

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
            if (_sixthTileTexture != null)
                textures.Add(_sixthTileTexture);
            if (_seventhTileTexture != null)
                textures.Add(_seventhTileTexture);
            if (_eighthTileTexture != null)
                textures.Add(_eighthTileTexture);
            if (_ninthTileTexture != null)
                textures.Add(_ninthTileTexture);
            if (_tenthTileTexture != null)
                textures.Add(_tenthTileTexture);


            if (textures.Count == 0)
                return;

            float sizeWithoutClosers;
            if (_closerTexture != null) 
                sizeWithoutClosers = _tilingSize / entity.transform.Scale - (_verticalTiling ? _closerTexture.Height * 2 : _closerTexture.Width * 2);
            else if(_secondCloserTexture != null)
                sizeWithoutClosers = _tilingSize / entity.transform.Scale - (_verticalTiling ? _secondCloserTexture.Height * 2 : _secondCloserTexture.Width * 2);           
            else
                sizeWithoutClosers = _tilingSize / entity.transform.Scale;
            float currentPosition = 0.0f;

            Texture2D _shorterTexture = null;
                
            _randomResult = new Dictionary<Texture2D,List<Vector2>>();
            Random r = new Random();
            foreach(Texture2D t in textures)
                if(_shorterTexture == null)
                    _shorterTexture = t;
                else if((_verticalTiling ? t.Height : t.Width ) < (_verticalTiling ? _shorterTexture.Height : _shorterTexture.Width))
                    _shorterTexture = t;

            while (currentPosition < sizeWithoutClosers)
            {
                int tileIndex = r.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + (_verticalTiling ? texture.Height : texture.Width) > sizeWithoutClosers)
                {
                    texture = _shorterTexture;
                }

                if (!_randomResult.ContainsKey(texture))
                    _randomResult.Add(texture, new List<Vector2>());

                _randomResult[texture].Add(_verticalTiling ? new Vector2(0, currentPosition) : new Vector2(currentPosition, 0));
                    
                    

                if(_tilingHash != "")
                    _tilingHash += "_" + textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(_verticalTiling ? new Vector2(0, currentPosition) : new Vector2(currentPosition, 0));                    
                else
                    _tilingHash += textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(_verticalTiling ? new Vector2(0, currentPosition) : new Vector2(currentPosition, 0));

                currentPosition += _verticalTiling ? texture.Height : texture.Width;
            }  
        }

        private void LoadFromHash()
        {
            string[] hashInfo = _tilingHash.Split('_');

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
            if (_sixthTileTexture != null)
                textures.Add(_sixthTileTexture);
            if (_seventhTileTexture != null)
                textures.Add(_seventhTileTexture);
            if (_eighthTileTexture != null)
                textures.Add(_eighthTileTexture);
            if (_ninthTileTexture != null)
                textures.Add(_ninthTileTexture);
            if (_tenthTileTexture != null)
                textures.Add(_tenthTileTexture);

            foreach (string hash in hashInfo)
            {
                string[] values = hash.Split('-');

                if (int.Parse(values[0]) >= textures.Count)
                    continue;
                if (!_randomResult.ContainsKey(textures[int.Parse(values[0])]))
                    _randomResult.Add(textures[int.Parse(values[0])], new List<Vector2>());

                _randomResult[textures[int.Parse(values[0])]].Add(Neon.Utils.ParseVector2(values[1]));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_verticalTiling)
            {
                float basePositionY = entity.transform.Position.Y - _tilingSize / 2;

                foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _randomResult)
                {
                    foreach (Vector2 position in kvp.Value)
                    {
                        spriteBatch.Draw(kvp.Key, new Vector2(entity.transform.Position.X, basePositionY) + Offset + this._parallaxPosition + position * entity.transform.Scale + new Vector2(0, (_closerTexture != null ? _closerTexture.Height : (_secondCloserTexture != null ? _secondCloserTexture.Height : 0)) * entity.transform.Scale), null, Color.White, 0.0f, new Vector2(kvp.Key.Width / 2, 0), entity.transform.Scale, CurrentSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    }
                }

                if (_closerTexture != null || _secondCloserTexture != null)
                {
                    if(_closerTexture != null)
                        spriteBatch.Draw(_closerTexture, new Vector2(entity.transform.Position.X, basePositionY) + Offset + this._parallaxPosition + new Vector2(0, _closerTexture.Height / 2 * entity.transform.Scale), null, Color.White, _closerRotation, new Vector2(_closerTexture.Width / 2, _closerTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                    if(_secondCloserTexture != null)
                        spriteBatch.Draw(_secondCloserTexture, new Vector2(entity.transform.Position.X, basePositionY + _tilingSize - _secondCloserTexture.Height / 2 * entity.transform.Scale - 2) + Offset + this._parallaxPosition, null, Color.White, _secondCloserRotation, new Vector2(_secondCloserTexture.Width / 2, _secondCloserTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipVertically, Layer);
                    else if(_closerTexture != null)
                        spriteBatch.Draw(_closerTexture, new Vector2(entity.transform.Position.X, basePositionY + _tilingSize - _closerTexture.Height / 2 * entity.transform.Scale - 2) + Offset + this._parallaxPosition, null, Color.White, _secondCloserRotation, new Vector2(_closerTexture.Width / 2, _closerTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipVertically, Layer);
                }
            }
            else
            {
                float basePositionX = entity.transform.Position.X - _tilingSize / 2;

                foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _randomResult)
                {
                    foreach (Vector2 position in kvp.Value)
                    {
                        spriteBatch.Draw(kvp.Key, new Vector2(basePositionX, entity.transform.Position.Y) + Offset + this._parallaxPosition + position * entity.transform.Scale + new Vector2((_closerTexture != null ? _closerTexture.Width : (_secondCloserTexture != null ? _secondCloserTexture.Width : 0)) * entity.transform.Scale, 0), null, Color.White, 0.0f, new Vector2(0, kvp.Key.Height / 2), entity.transform.Scale, CurrentSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    }
                }
                if (_closerTexture != null || _secondCloserTexture != null)
                {
                    if(_closerTexture != null)
                        spriteBatch.Draw(_closerTexture, new Vector2(basePositionX, entity.transform.Position.Y) + Offset + _closerOffset + this._parallaxPosition + new Vector2(_closerTexture.Width / 2 * entity.transform.Scale, 0), null, Color.White, _closerRotation, new Vector2(_closerTexture.Width / 2, _closerTexture.Height / 2), entity.transform.Scale, this.CloserBasicSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    if(_secondCloserTexture != null)
                        spriteBatch.Draw(_secondCloserTexture, new Vector2(basePositionX + _tilingSize - _secondCloserTexture.Width / 2 * entity.transform.Scale - 2, entity.transform.Position.Y) + _secondCloserOffset + Offset + this._parallaxPosition, null, Color.White, _secondCloserRotation, new Vector2(_secondCloserTexture.Width / 2, _secondCloserTexture.Height / 2), entity.transform.Scale, this.SecondCloserBasicSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                    else if(_closerTexture != null)
                        spriteBatch.Draw(_closerTexture, new Vector2(basePositionX + _tilingSize - _closerTexture.Width / 2 * entity.transform.Scale - 2, entity.transform.Position.Y) + Offset + _secondCloserOffset + this._parallaxPosition, null, Color.White, _closerRotation, new Vector2(_closerTexture.Width / 2, _closerTexture.Height / 2), entity.transform.Scale, this.SecondCloserBasicSide == Side.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally, Layer);
                }
            }          
            base.Draw(spriteBatch);
        }
    }
}
