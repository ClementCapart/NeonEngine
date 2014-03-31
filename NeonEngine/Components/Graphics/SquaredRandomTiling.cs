using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NeonEngine.Components.CollisionDetection;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.Components.Graphics2D
{
    public class SquaredRandomTiling : DrawableComponent
    {
        #region Properties
        private float _globalOffset = 0.0f;

        public float GlobalOffset
        {
            get { return _globalOffset; }
            set { _globalOffset = value; }
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

        public float DrawLayer
        {
            get { return Layer; }
            set { Layer = value; }
        }

        private string _topCornerGraphicTag = "";

        public string TopCornerGraphicTag
        {
            get { return _topCornerGraphicTag; }
            set 
            {
                _topCornerGraphicTag = value;
            }
        }

        private string _bottomCornerGraphicTag = "";

        public string BottomCornerGraphicTag
        {
            get { return _bottomCornerGraphicTag; }
            set
            {
                _bottomCornerGraphicTag = value;
            }
        }

        private string _firstBottomTileGraphicTag = "";

        public string FirstBottomTileGraphicTag
        {
            get { return _firstBottomTileGraphicTag; }
            set 
            { 
                _firstBottomTileGraphicTag = value; 
            }
        }

        private string _secondBottomTileGraphicTag = "";

        public string SecondBottomTileGraphicTag
        {
            get { return _secondBottomTileGraphicTag; }
            set { _secondBottomTileGraphicTag = value; }
        }

        private string _thirdBottomTileGraphicTag = "";

        public string ThirdBottomTileGraphicTag
        {
            get { return _thirdBottomTileGraphicTag; }
            set { _thirdBottomTileGraphicTag = value; }
        }

        private string _firstTopTileGraphicTag = "";

        public string FirstTopTileGraphicTag
        {
            get { return _firstTopTileGraphicTag; }
            set { _firstTopTileGraphicTag = value; }
        }

        private string _secondTopTileGraphicTag = "";

        public string SecondTopTileGraphicTag
        {
            get { return _secondTopTileGraphicTag; }
            set { _secondTopTileGraphicTag = value; }
        }

        private string _thirdTopTileGraphicTag = "";

        public string ThirdTopTileGraphicTag
        {
            get { return _thirdTopTileGraphicTag; }
            set { _thirdTopTileGraphicTag = value; }
        }

        private string _firstWallTileGraphicTag = "";

        public string FirstWallTileGraphicTag
        {
            get { return _firstWallTileGraphicTag; }
            set { _firstWallTileGraphicTag = value; }
        }

        private string _secondWallTileGraphicTag = "";

        public string SecondWallTileGraphicTag
        {
            get { return _secondWallTileGraphicTag; }
            set { _secondWallTileGraphicTag = value; }
        }

        private string _thirdWallTileGraphicTag = "";

        public string ThirdWallTileGraphicTag
        {
            get { return _thirdWallTileGraphicTag; }
            set { _thirdWallTileGraphicTag = value; }
        }

        
        #endregion

        private Texture2D _topCornerTexture;
        private Texture2D _bottomCornerTexture;

        private Texture2D _firstTopTileTexture;
        private Texture2D _secondTopTileTexture;
        private Texture2D _thirdTopTileTexture;

        private Texture2D _firstBottomTileTexture;
        private Texture2D _secondBottomTileTexture;
        private Texture2D _thirdBottomTileTexture;

        private Texture2D _firstWallTileTexture;
        private Texture2D _secondWallTileTexture;
        private Texture2D _thirdWallTileTexture;

        private Vector2[] _cornerPositions;
        RenderTarget2D _finalTexture;

        Dictionary<Texture2D, List<Vector2>> _topRandomResult = new Dictionary<Texture2D, List<Vector2>>();
        Dictionary<Texture2D, List<Vector2>> _bottomRandomResult = new Dictionary<Texture2D, List<Vector2>>();
        Dictionary<Texture2D, List<Vector2>> _leftWallRandomResult = new Dictionary<Texture2D, List<Vector2>>();
        Dictionary<Texture2D, List<Vector2>> _rightWallRandomResult = new Dictionary<Texture2D, List<Vector2>>();
        

        public SquaredRandomTiling(Entity entity)
            :base(0.5f, entity, "SquaredRandomTiling")
        {
            RequiredComponents = new Type[] { typeof(Hitbox) };
        }

        public override void Init()
        {
            _finalTexture = new RenderTarget2D(Neon.GraphicsDevice, (int)entity.hitboxes[0].Width, (int)entity.hitboxes[0].Height);
            _topCornerTexture = AssetManager.GetTexture(_topCornerGraphicTag);
            _bottomCornerTexture = AssetManager.GetTexture(_bottomCornerGraphicTag);

            _firstBottomTileTexture = AssetManager.GetTexture(_firstBottomTileGraphicTag);
            _secondBottomTileTexture = AssetManager.GetTexture(_secondBottomTileGraphicTag);
            _thirdBottomTileTexture = AssetManager.GetTexture(_thirdBottomTileGraphicTag);

            _firstTopTileTexture = AssetManager.GetTexture(_firstTopTileGraphicTag);
            _secondTopTileTexture = AssetManager.GetTexture(_secondTopTileGraphicTag);
            _thirdTopTileTexture = AssetManager.GetTexture(_thirdTopTileGraphicTag);

            _firstWallTileTexture = AssetManager.GetTexture(_firstWallTileGraphicTag);
            _secondWallTileTexture = AssetManager.GetTexture(_secondWallTileGraphicTag);
            _thirdWallTileTexture = AssetManager.GetTexture(_thirdWallTileGraphicTag);

            SetCorners();

            if (_tilingHash == "" || !_keepTilingHash)
            {
                _tilingHash = "";
                RandomizeTop();
                RandomizeBottom();
            }

            Neon.SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            Neon.GraphicsDevice.SetRenderTarget(_finalTexture);
            Neon.GraphicsDevice.Clear(Color.Transparent);
            if (_topCornerTexture != null)
            {
                Neon.SpriteBatch.Draw(_topCornerTexture, _cornerPositions[0], null, Color.White, 0.0f, new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                Neon.SpriteBatch.Draw(_topCornerTexture, _cornerPositions[1], null, Color.White, 0.0f, new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipHorizontally, Layer);
            }
            if(_bottomCornerTexture != null)
            {                
                Neon.SpriteBatch.Draw(_bottomCornerTexture, _cornerPositions[3], null, Color.White, 0.0f, new Vector2(_bottomCornerTexture.Width / 2, _bottomCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                Neon.SpriteBatch.Draw(_bottomCornerTexture, _cornerPositions[2], null, Color.White, 0.0f, new Vector2(_bottomCornerTexture.Width / 2, _bottomCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipHorizontally, Layer);
            }
            
            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _topRandomResult)
            {
                foreach (Vector2 position in kvp.Value)
                {
                    Neon.SpriteBatch.Draw(kvp.Key, position, null, Color.White, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                }
            }

            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _bottomRandomResult)
            {
                foreach (Vector2 position in kvp.Value)
                {
                    Neon.SpriteBatch.Draw(kvp.Key, position, null, Color.White, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);
                }
            }
           
            Neon.SpriteBatch.End();
            Neon.GraphicsDevice.SetRenderTarget(null);
            
            base.Init();
        }

        private void SetCorners()
        {
            _cornerPositions = new Vector2[4];
            if (_topCornerTexture != null)
            {
                _cornerPositions[0] = Vector2.Zero + new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2) * entity.transform.Scale /*+ new Vector2(Offset ;*/;
                _cornerPositions[1] = new Vector2(entity.hitboxes[0].Width - 1, 0) + new Vector2(-_topCornerTexture.Width / 2, _topCornerTexture.Height / 2) * entity.transform.Scale - Offset;
            }

            if (_bottomCornerTexture != null)
            {
                _cornerPositions[2] = new Vector2(entity.hitboxes[0].Width - 1, entity.hitboxes[0].Height - 1) + new Vector2(-_bottomCornerTexture.Width / 2, -_bottomCornerTexture.Height / 2) * entity.transform.Scale;
                _cornerPositions[3] = new Vector2(0, entity.hitboxes[0].Height - 1) + new Vector2(_bottomCornerTexture.Width / 2, -_bottomCornerTexture.Height / 2) * entity.transform.Scale;
            }
        }

        private void RandomizeTop()
        {
            Random r = new Random();
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstTopTileTexture != null)
                textures.Add(_firstTopTileTexture);
            if (_secondTopTileTexture != null)
                textures.Add(_secondTopTileTexture);
            if (_thirdTopTileTexture != null)
                textures.Add(_thirdTopTileTexture);

            if (textures.Count == 0)
                return;

            float widthToFill = entity.hitboxes[0].Width - (_topCornerTexture != null ? _topCornerTexture.Width * entity.transform.Scale * 2 : 0);

            Texture2D _shorterTexture = null;
            foreach (Texture2D t in textures)
                if (_shorterTexture == null)
                    _shorterTexture = t;
                else if (t.Width < _shorterTexture.Width)
                    _shorterTexture = t;

            _topRandomResult = new Dictionary<Texture2D, List<Vector2>>();

            float currentPosition = 0.0f;

            while (currentPosition < widthToFill)
            {
                int tileIndex = r.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Width * entity.transform.Scale > widthToFill)
                    texture = _shorterTexture;

                if (!_topRandomResult.ContainsKey(texture))
                    _topRandomResult.Add(texture, new List<Vector2>());

                _topRandomResult[texture].Add(new Vector2(currentPosition + (_topCornerTexture != null ? _topCornerTexture.Width * entity.transform.Scale : 0), 0));

                currentPosition += texture.Width * entity.transform.Scale;
            }

        }

        private void RandomizeBottom()
        {
            Random r = new Random();
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstBottomTileTexture != null)
                textures.Add(_firstBottomTileTexture);
            if (_secondBottomTileTexture != null)
                textures.Add(_secondBottomTileTexture);
            if (_thirdBottomTileTexture != null)
                textures.Add(_thirdBottomTileTexture);

            if (textures.Count == 0)
                return;

            float widthToFill = entity.hitboxes[0].Width - (_bottomCornerTexture != null ? _bottomCornerTexture.Width * entity.transform.Scale * 2 : 0);

            Texture2D _shorterTexture = null;
            foreach (Texture2D t in textures)
                if (_shorterTexture == null)
                    _shorterTexture = t;
                else if (t.Width < _shorterTexture.Width)
                    _shorterTexture = t;

            _bottomRandomResult = new Dictionary<Texture2D, List<Vector2>>();

            float currentPosition = 0.0f;

            while (currentPosition < widthToFill)
            {
                int tileIndex = r.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Width * entity.transform.Scale > widthToFill)
                    texture = _shorterTexture;

                if (!_bottomRandomResult.ContainsKey(texture))
                    _bottomRandomResult.Add(texture, new List<Vector2>());

                _bottomRandomResult[texture].Add(new Vector2(currentPosition + (_bottomCornerTexture != null ? _bottomCornerTexture.Width * entity.transform.Scale : 0), entity.hitboxes[0].Height - texture.Height));

                currentPosition += texture.Width * entity.transform.Scale;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_finalTexture, entity.transform.Position, null, Color.White, 0.0f, new Vector2(_finalTexture.Width / 2, _finalTexture.Height / 2), 1.0f, SpriteEffects.None, Layer);
            base.Draw(spriteBatch);
        }

    }
}
