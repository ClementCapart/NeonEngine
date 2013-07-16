using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class Input
    {
        #region Singleton
        private static readonly Input instance = new Input();

        public static Input Instance
        {
            get
            {
                return instance;
            }

        }
        #endregion

        public delegate void OnScroll(object sender, EventArgs e, float delta);

        public event OnScroll OnScrollUp;
        public event OnScroll OnScrollDown;
        public event OnScroll Scrolled;

        private KeyboardState ks, _ks;
        private MouseState ms, _ms;
        private GamePadState gps, _gps;

        private Vector2 mousePosition;
        public Vector2 MousePosition
        {
            get { return mousePosition; }
        }

        private Vector2 screenMousePosition;
        public Vector2 ScreenMousePosition
        {
            get { return screenMousePosition; }
        }

        private Vector2 deltaMouse;
        public Vector2 DeltaMouse
        {
            get { return deltaMouse; }
        }

        private Input()
        {
            _ks = Keyboard.GetState();
            _ms = Mouse.GetState();
            _gps = GamePad.GetState(PlayerIndex.One);

            mousePosition = new Vector2();
        }

        public void Update(Camera2D camera)
        {
            ks = Keyboard.GetState();
            ms = Mouse.GetState();
            gps = GamePad.GetState(PlayerIndex.One);

            screenMousePosition.X = ms.X;
            screenMousePosition.Y = ms.Y;

            deltaMouse = new Vector2(ms.X - _ms.X, ms.Y - _ms.Y);

            mousePosition.X = (ms.X + camera.Position.X - Neon.HalfScreen.X) * camera.Zoom;
            mousePosition.Y = (ms.Y + camera.Position.Y - Neon.HalfScreen.Y) * camera.Zoom;
            if (ms.ScrollWheelValue != _ms.ScrollWheelValue)
            {
                if (Scrolled != null) Scrolled(this, null, (ms.ScrollWheelValue - _ms.ScrollWheelValue));
                if (ms.ScrollWheelValue > _ms.ScrollWheelValue)
                {
                    if (OnScrollUp != null) OnScrollUp(this, null, (ms.ScrollWheelValue - _ms.ScrollWheelValue));
                }
                else if (ms.ScrollWheelValue < _ms.ScrollWheelValue)
                    if (OnScrollDown != null) OnScrollDown(this, null, (ms.ScrollWheelValue - _ms.ScrollWheelValue));
            }
        }

        public void LastFrameState()
        {
            _ks = ks;
            _ms = ms;
            _gps = gps;
        }

        #region Keyboard functions
        public bool Pressed(Keys key)
        {
            if (ks.IsKeyDown(key) && _ks.IsKeyUp(key))
                return true;

            return false;
        }

        public bool Check(Keys key)
        {
            if (ks.IsKeyDown(key))
                return true;

            return false;
        }

        public bool Released(Keys key)
        {
            if (ks.IsKeyUp(key) && _ks.IsKeyDown(key))
                return true;

            return false;
        }
        #endregion

        #region Mouse functions
        public bool MousePressed(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    if (ms.LeftButton == ButtonState.Pressed && _ms.LeftButton == ButtonState.Released)
                        return true;
                    else
                        return false;
                case MouseButton.RightButton:
                    if (ms.RightButton == ButtonState.Pressed && _ms.RightButton == ButtonState.Released)
                        return true;
                    else
                        return false;
                case MouseButton.MiddleButton:
                    if (ms.MiddleButton == ButtonState.Pressed && _ms.MiddleButton == ButtonState.Released)
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public bool MouseCheck(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    if (ms.LeftButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
                case MouseButton.RightButton:
                    if (ms.RightButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
                case MouseButton.MiddleButton:
                    if (ms.MiddleButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
            }
            return false;
        }

        public bool MouseReleased(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.LeftButton:
                    if (ms.LeftButton == ButtonState.Released && _ms.LeftButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
                case MouseButton.RightButton:
                    if (ms.RightButton == ButtonState.Released && _ms.RightButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
                case MouseButton.MiddleButton:
                    if (ms.MiddleButton == ButtonState.Released && _ms.MiddleButton == ButtonState.Pressed)
                        return true;
                    else
                        return false;
            }
            return false;
        }
        #endregion       

        #region Xbox360 controller functions
        public bool Pressed(Buttons button)
        {
            if (gps.IsConnected)
                if (gps.IsButtonDown(button) && _gps.IsButtonUp(button))
                    return true;

            return false;
        }

        public bool Check(Buttons button)
        {
            if (gps.IsConnected)
                if (gps.IsButtonDown(button))
                    return true;

            return false;
        }

        public bool Released(Buttons button)
        {
            if (gps.IsConnected)
                if (gps.IsButtonUp(button) && _gps.IsButtonDown(button))
                    return true;

            return false;
        }
        #endregion
    }
}
