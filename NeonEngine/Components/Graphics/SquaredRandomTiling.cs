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

        private Vector2 _centerOffset = new Vector2(50, 50);

        public Vector2 CenterOffset
        {
            get { return _centerOffset; }
            set { _centerOffset = value; }
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

        private string _centerTileGraphicTag = "";

        public string CenterTileGraphicTag
        {
            get { return _centerTileGraphicTag; }
            set { _centerTileGraphicTag = value; }
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

        private Texture2D _centerTexture;

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

            _centerTexture = AssetManager.GetTexture(_centerTileGraphicTag);

            SetCorners();

            if (_tilingHash == "" || !_keepTilingHash)
            {
                _tilingHash = "";
                RandomizeTop();
                RandomizeBottom();
                RandomizeWalls();
            }
            else
            {
                LoadFromHash();
            }          

            base.Init();
        }

        private void LoadFromHash()
        {
            _topRandomResult.Clear();
            string topHash = _tilingHash.Substring(4, _tilingHash.IndexOf("_BOT") - 4);
            string[] hashInfo = topHash.Split('_');

            List<Texture2D> textures = new List<Texture2D>();

            if (_firstTopTileTexture != null)
                textures.Add(_firstTopTileTexture);
            if (_secondTopTileTexture != null)
                textures.Add(_secondTopTileTexture);
            if (_thirdTopTileTexture != null)
                textures.Add(_thirdTopTileTexture);

            foreach (string hash in hashInfo)
            {
                string[] values = hash.Split('-');

                if (int.Parse(values[0]) >= textures.Count)
                    continue;
                if (!_topRandomResult.ContainsKey(textures[int.Parse(values[0])]))
                    _topRandomResult.Add(textures[int.Parse(values[0])], new List<Vector2>());

                Vector2 v = Neon.Utils.ParseVector2(values[1]);
                v += new Vector2((_topCornerTexture != null ? _topCornerTexture.Width * entity.transform.Scale : 0) + GlobalOffset, GlobalOffset);

                _topRandomResult[textures[int.Parse(values[0])]].Add(v);
            }

            int i = _tilingHash.IndexOf("_BOT_");
            string botHash = _tilingHash.Substring(i + 5, _tilingHash.IndexOf("_LEFT_") - i - 5);

            hashInfo = botHash.Split('_');

            textures = new List<Texture2D>();

            if (_firstBottomTileTexture != null)
                textures.Add(_firstBottomTileTexture);
            if (_secondBottomTileTexture != null)
                textures.Add(_secondBottomTileTexture);
            if (_thirdBottomTileTexture != null)
                textures.Add(_thirdBottomTileTexture);

            foreach (string hash in hashInfo)
            {
                string[] values = hash.Split('-');

                if (int.Parse(values[0]) >= textures.Count)
                    continue;
                if (!_bottomRandomResult.ContainsKey(textures[int.Parse(values[0])]))
                    _bottomRandomResult.Add(textures[int.Parse(values[0])], new List<Vector2>());

                Vector2 v = Neon.Utils.ParseVector2(values[1]);
                v += new Vector2((_bottomCornerTexture != null ? _bottomCornerTexture.Width * entity.transform.Scale : 0) + GlobalOffset, entity.hitboxes[0].Height - textures[int.Parse(values[0])].Height * entity.transform.Scale - GlobalOffset);

                _bottomRandomResult[textures[int.Parse(values[0])]].Add(v);
            }

            i = _tilingHash.IndexOf("_LEFT_");
            string leftHash = _tilingHash.Substring(i + 6, _tilingHash.IndexOf("_RIGHT_") - i - 6);

            hashInfo = leftHash.Split('_');

            textures = new List<Texture2D>();

            if (_firstWallTileTexture != null)
                textures.Add(_firstWallTileTexture);
            if (_secondWallTileTexture != null)
                textures.Add(_secondWallTileTexture);
            if (_thirdWallTileTexture != null)
                textures.Add(_thirdWallTileTexture);

            foreach (string hash in hashInfo)
            {
                string[] values = hash.Split('-');

                if (int.Parse(values[0]) >= textures.Count)
                    continue;
                if (!_leftWallRandomResult.ContainsKey(textures[int.Parse(values[0])]))
                    _leftWallRandomResult.Add(textures[int.Parse(values[0])], new List<Vector2>());

                Vector2 v = Neon.Utils.ParseVector2(values[1]);
                v += new Vector2(GlobalOffset, (_topCornerTexture != null ? _topCornerTexture.Height * entity.transform.Scale : 0) + GlobalOffset);

                _leftWallRandomResult[textures[int.Parse(values[0])]].Add(v);
            }

            i = _tilingHash.IndexOf("_RIGHT_");
            string rightHash = _tilingHash.Substring(i + 7, _tilingHash.Length - 1 - (i + 7));

            hashInfo = rightHash.Split('_');

            foreach (string hash in hashInfo)
            {
                string[] values = hash.Split('-');

                if (int.Parse(values[0]) >= textures.Count)
                    continue;

                if (!_rightWallRandomResult.ContainsKey(textures[int.Parse(values[0])]))
                    _rightWallRandomResult.Add(textures[int.Parse(values[0])], new List<Vector2>());

                Vector2 v = Neon.Utils.ParseVector2(values[1]);
                v += new Vector2(entity.hitboxes[0].Width - textures[int.Parse(values[0])].Width * entity.transform.Scale - GlobalOffset, (_topCornerTexture != null ? _topCornerTexture.Height * entity.transform.Scale : 0) + GlobalOffset);

                _rightWallRandomResult[textures[int.Parse(values[0])]].Add(v);
            }

        }

        private void SetCorners()
        {
            _cornerPositions = new Vector2[4];
            if (_topCornerTexture != null)
            {
                float displacement = 0.0f;
                if (_topCornerTexture.Width < 50)
                    displacement = 1.0f;
                _cornerPositions[0] = Vector2.Zero + new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2) * entity.transform.Scale + new Vector2(GlobalOffset, GlobalOffset);
                _cornerPositions[1] = new Vector2(entity.hitboxes[0].Width - displacement * entity.transform.Scale, 0) + new Vector2(-_topCornerTexture.Width / 2, _topCornerTexture.Height / 2) * entity.transform.Scale + new Vector2(-GlobalOffset, GlobalOffset);
            }

            if (_bottomCornerTexture != null)
            {
                float displacement = 0.0f;
                if (_topCornerTexture.Width < 50)
                    displacement = 1.0f;
                _cornerPositions[2] = new Vector2(entity.hitboxes[0].Width - displacement * entity.transform.Scale, entity.hitboxes[0].Height - displacement * entity.transform.Scale) + new Vector2(-_bottomCornerTexture.Width / 2, -_bottomCornerTexture.Height / 2) * entity.transform.Scale + new Vector2(-GlobalOffset, -GlobalOffset);
                _cornerPositions[3] = new Vector2(0, entity.hitboxes[0].Height - displacement * entity.transform.Scale) + new Vector2(_bottomCornerTexture.Width / 2, -_bottomCornerTexture.Height / 2) * entity.transform.Scale + new Vector2(GlobalOffset, -GlobalOffset);
            }
        }

        private void RandomizeTop()
        {
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstTopTileTexture != null)
                textures.Add(_firstTopTileTexture);
            if (_secondTopTileTexture != null)
                textures.Add(_secondTopTileTexture);
            if (_thirdTopTileTexture != null)
                textures.Add(_thirdTopTileTexture);

            if (textures.Count == 0)
                return;

            float widthToFill = entity.hitboxes[0].Width - (_topCornerTexture != null ? _topCornerTexture.Width * entity.transform.Scale * 2 : 0) - GlobalOffset * 2;

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
                int tileIndex = Neon.Utils.CommonRandom.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Width * entity.transform.Scale > widthToFill)
                    texture = _shorterTexture;

                if (!_topRandomResult.ContainsKey(texture))
                    _topRandomResult.Add(texture, new List<Vector2>());

                _topRandomResult[texture].Add(new Vector2(currentPosition + (_topCornerTexture != null ? _topCornerTexture.Width * entity.transform.Scale : 0) + GlobalOffset, GlobalOffset));

                if (_tilingHash != "")
                    _tilingHash += "_" + textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(new Vector2(currentPosition, 0));
                else
                    _tilingHash += "TOP_"+ textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(new Vector2(currentPosition, 0));

                currentPosition += texture.Width * entity.transform.Scale;             
            }

            _tilingHash += "_BOT";

        }

        private void RandomizeBottom()
        {
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstBottomTileTexture != null)
                textures.Add(_firstBottomTileTexture);
            if (_secondBottomTileTexture != null)
                textures.Add(_secondBottomTileTexture);
            if (_thirdBottomTileTexture != null)
                textures.Add(_thirdBottomTileTexture);

            if (textures.Count == 0)
                return;

            float widthToFill = entity.hitboxes[0].Width - (_bottomCornerTexture != null ? _bottomCornerTexture.Width * entity.transform.Scale * 2 : 0) - GlobalOffset * 2;

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
                int tileIndex = Neon.Utils.CommonRandom.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Width * entity.transform.Scale > widthToFill)
                    texture = _shorterTexture;

                if (!_bottomRandomResult.ContainsKey(texture))
                    _bottomRandomResult.Add(texture, new List<Vector2>());

                _bottomRandomResult[texture].Add(new Vector2(currentPosition + (_bottomCornerTexture != null ? _bottomCornerTexture.Width * entity.transform.Scale : 0) + GlobalOffset, entity.hitboxes[0].Height - texture.Height * entity.transform.Scale - GlobalOffset));

                if (_tilingHash != "")
                    _tilingHash += "_" + textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(new Vector2(currentPosition, 0));

                currentPosition += texture.Width * entity.transform.Scale;
            }

            _tilingHash += "_LEFT";
        }

        private void RandomizeWalls()
        {
            List<Texture2D> textures = new List<Texture2D>();

            if (_firstWallTileTexture != null)
                textures.Add(_firstWallTileTexture);
            if (_secondWallTileTexture != null)
                textures.Add(_secondWallTileTexture);
            if (_thirdWallTileTexture != null)
                textures.Add(_thirdWallTileTexture);

            if (textures.Count == 0)
                return;

            float heightToFill = entity.hitboxes[0] .Height - (_bottomCornerTexture != null ? _bottomCornerTexture.Height * entity.transform.Scale : 0) - (_topCornerTexture != null ? _topCornerTexture.Height * entity.transform.Scale : 0) - GlobalOffset * 2;

            Texture2D _shorterTexture = null;
            foreach (Texture2D t in textures)
                if (_shorterTexture == null)
                    _shorterTexture = t;
                else if (t.Width < _shorterTexture.Height)
                    _shorterTexture = t;

            _leftWallRandomResult = new Dictionary<Texture2D, List<Vector2>>();

            float currentPosition = 0.0f;

            while (currentPosition < heightToFill)
            {
                int tileIndex = Neon.Utils.CommonRandom.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Height * entity.transform.Scale > heightToFill)
                    texture = _shorterTexture;

                if (!_leftWallRandomResult.ContainsKey(texture))
                    _leftWallRandomResult.Add(texture, new List<Vector2>());

                _leftWallRandomResult[texture].Add(new Vector2(GlobalOffset, currentPosition + (_topCornerTexture != null ? _topCornerTexture.Height * entity.transform.Scale : 0) + GlobalOffset));

                if (_tilingHash != "")
                    _tilingHash += "_" + textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(new Vector2(0, currentPosition));

                currentPosition += texture.Height * entity.transform.Scale;
            }

            _tilingHash += "_RIGHT";

            _rightWallRandomResult = new Dictionary<Texture2D, List<Vector2>>();

            currentPosition = 0.0f;

            while (currentPosition < heightToFill)
            {
                int tileIndex = Neon.Utils.CommonRandom.Next(textures.Count);
                Texture2D texture = textures[tileIndex];

                if (currentPosition + texture.Height * entity.transform.Scale > heightToFill)
                    texture = _shorterTexture;

                if (!_rightWallRandomResult.ContainsKey(texture))
                    _rightWallRandomResult.Add(texture, new List<Vector2>());

                _rightWallRandomResult[texture].Add(new Vector2(entity.hitboxes[0].Width - texture.Width * entity.transform.Scale - GlobalOffset, currentPosition + (_topCornerTexture != null ? _topCornerTexture.Height * entity.transform.Scale : 0) + GlobalOffset));

                if (_tilingHash != "")
                    _tilingHash += "_" + textures.IndexOf(texture) + "-" + Neon.Utils.Vector2ToString(new Vector2(0, currentPosition));

                currentPosition += texture.Height * entity.transform.Scale;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (_topCornerTexture != null)
            {
                spriteBatch.Draw(_topCornerTexture, _cornerPositions[0] + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                Neon.SpriteBatch.Draw(_topCornerTexture, _cornerPositions[1] + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, new Vector2(_topCornerTexture.Width / 2, _topCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipHorizontally, Layer);
            }
            if (_bottomCornerTexture != null)
            {
                spriteBatch.Draw(_bottomCornerTexture, _cornerPositions[3] + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, new Vector2(_bottomCornerTexture.Width / 2, _bottomCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.None, Layer);
                spriteBatch.Draw(_bottomCornerTexture, _cornerPositions[2] + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, new Vector2(_bottomCornerTexture.Width / 2, _bottomCornerTexture.Height / 2), entity.transform.Scale, SpriteEffects.FlipHorizontally, Layer);
            }

            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _topRandomResult)
                foreach (Vector2 position in kvp.Value)
                    spriteBatch.Draw(kvp.Key, position + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);

            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _bottomRandomResult)
                foreach (Vector2 position in kvp.Value)
                    spriteBatch.Draw(kvp.Key, position + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);

            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _leftWallRandomResult)
                foreach (Vector2 position in kvp.Value)
                    spriteBatch.Draw(kvp.Key, position + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.None, Layer);

            foreach (KeyValuePair<Texture2D, List<Vector2>> kvp in _rightWallRandomResult)
                foreach (Vector2 position in kvp.Value)
                    spriteBatch.Draw(kvp.Key, position + _parallaxPosition + entity.transform.Position - new Vector2(entity.hitboxes[0].Width / 2, entity.hitboxes[0].Height / 2), null, MainColor, 0.0f, Vector2.Zero, entity.transform.Scale, SpriteEffects.FlipHorizontally, Layer);    
            
            if (_centerTexture != null)
            {
                Vector2 size = (new Vector2((entity.hitboxes[0].Width), (entity.hitboxes[0].Height)) - new Vector2(GlobalOffset * 2) - _centerOffset * 2) / entity.transform.Scale;
                spriteBatch.Draw(_centerTexture, entity.transform.Position + _parallaxPosition,
                    new Rectangle(0, 0, (int)(size.X), (int)(size.Y)), MainColor, 0.0f, size / 2, entity.transform.Scale, SpriteEffects.None, Layer - 0.001f);
            }
            base.Draw(spriteBatch);
        }

    }
}
