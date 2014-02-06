using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NeonEngine;
using NeonEngine.Components.Graphics2D;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary.Components.Menu
{
    public enum MenuState
    {
        PressStart,
        ModeSelect,
        ChapterSelect
    }

    class MainMenu : Graphic
    {
        #region Properties
        private float _opacityChangeValue = 0.5f;

        public float OpacityChangeValue
        {
            get { return _opacityChangeValue; }
            set { _opacityChangeValue = value; }
        }
        #endregion
        
        private bool _opacityGoingDown = true;

        private MenuState _menuState = MenuState.PressStart;
        List<DrawableComponent> _drawableComponents = new List<DrawableComponent>();

        private Graphic _selectBar = null;

        private Graphic _newGameGraphic = null;
        private Graphic _chapterGraphic = null;

        private int _currentSelection = 0;

        public MainMenu(Entity entity)
            :base(entity)
        {
            Name = "GraphicOpacity";
        }

        public override void Init()
        {
            ChangeState(MenuState.PressStart);
            this.opacity = 1.0f;
 	        base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            switch(_menuState)
            {
                case MenuState.PressStart:
                    
                    if (_opacityGoingDown)
                    {
                        if (opacity > 0)
                        {
                            opacity -= _opacityChangeValue * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            _opacityGoingDown = false;
                            opacity = 0.0f;
                        }
                    }
                    else
                    {
                        if (opacity < 1)
                        {
                            opacity += _opacityChangeValue * (float)gameTime.ElapsedGameTime.TotalSeconds;
                        }
                        else
                        {
                            _opacityGoingDown = true;
                            opacity = 1.0f;
                        }
                    }

                    foreach (DrawableComponent dc in _drawableComponents)
                        (dc as Graphic).opacity = opacity;                 

                    if (Neon.Input.Pressed(Buttons.Start))
                        ChangeState(MenuState.ModeSelect);
                    break;    

                case MenuState.ModeSelect:
                    if (_selectBar != null)
                    {
                        if (_opacityGoingDown)
                        {
                            if (this._selectBar.opacity > 0)
                            {
                                this._selectBar.opacity -= _opacityChangeValue * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            else
                            {
                                _opacityGoingDown = false;
                                this._selectBar.opacity = 0.0f;
                            }
                        }
                        else
                        {
                            if (this._selectBar.opacity < 1)
                            {
                                this._selectBar.opacity += _opacityChangeValue * (float)gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            else
                            {
                                _opacityGoingDown = true;
                                this._selectBar.opacity = 1.0f;
                            }
                        }
                    }

                    if (Neon.Input.Pressed(NeonStarInput.MoveDown))
                    {
                        _currentSelection = (_currentSelection + 1) % 2;
                    }
                    else if (Neon.Input.Pressed(NeonStarInput.MoveUp))
                    {
                        _currentSelection = Math.Abs((_currentSelection - 1)) % 2;
                    }

                    if (_currentSelection == 0)
                    {
                        _newGameGraphic.GraphicTag = "NewGameOn";
                        _chapterGraphic.GraphicTag = "ChaptersOff";
                        _selectBar.Offset = Vector2.Zero;
                    }
                    else
                    {
                        _newGameGraphic.GraphicTag = "NewGameOff";
                        _chapterGraphic.GraphicTag = "ChaptersOn";
                        _selectBar.Offset = new Vector2(0, 40);
                    }
                    if (Neon.Input.Pressed(NeonStarInput.Guard))
                    {
                        ChangeState(MenuState.PressStart);
                    }


                    if (Neon.Input.Pressed(NeonStarInput.Jump))
                    {
                        if (_currentSelection == 0)
                            entity.GameWorld.ChangeLevel("SequenceOne", "TrainingOne", 0);
                        else
                            ChangeState(MenuState.ChapterSelect);
                    }
                    break;
            }
            
            base.Update(gameTime);
        }

        public void ChangeState(MenuState newState)
        {
            _menuState = newState;
            for (int i = _drawableComponents.Count - 1; i >= 0; i--)
            {
                DrawableComponent dc = _drawableComponents[i];
                dc.Remove();
            }
            _drawableComponents.Clear();

            switch(newState)
            {
                case MenuState.PressStart:
                    Graphic g = new Graphic(this.entity);
                    g.GraphicTag = "PressStart";
                    g.ParallaxForce = Vector2.One;
                    entity.AddComponent(g);
                    g.Layer = 0.7f;
                    _drawableComponents.Add(g);
                    g = new Graphic(this.entity);
                    g.GraphicTag = "SelectBar";
                    g.ParallaxForce = Vector2.One;
                    g.Layer = 0.7f;
                    entity.AddComponent(g);
                    _drawableComponents.Add(g);
                    break;

                case MenuState.ModeSelect:
                    _selectBar = new Graphic(this.entity);
                    _selectBar.GraphicTag = "SelectBar";
                    _selectBar.ParallaxForce = Vector2.One;
                    _selectBar.Layer = 0.7f;
                    entity.AddComponent(_selectBar);
                    _drawableComponents.Add(_selectBar);
                    _newGameGraphic = new Graphic(this.entity);
                    _newGameGraphic.GraphicTag = "NewGameOn";
                    _newGameGraphic.ParallaxForce = Vector2.One;
                    _newGameGraphic.Layer = 0.7f;
                    entity.AddComponent(_newGameGraphic);
                    _drawableComponents.Add(_newGameGraphic);
                    _chapterGraphic = new Graphic(this.entity);
                    _chapterGraphic.GraphicTag = "ChaptersOff";
                    _chapterGraphic.ParallaxForce = Vector2.One;
                    _chapterGraphic.Layer = 0.7f;
                    _chapterGraphic.Offset = new Vector2(0, 40);
                    entity.AddComponent(_chapterGraphic);
                    _drawableComponents.Add(_chapterGraphic);
                    break;

                case MenuState.ChapterSelect:
                    break;
            }
        
        }
       

    }
}
