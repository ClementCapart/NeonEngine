using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

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

        private KeyboardState ks, _ks;
        private MouseState ms, _ms;
        private GamePadState gps, _gps;

        private Dictionary<string, Dictionary<string, string>> CustomInputs;
        Type EnumType;

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

        private Keys[] keysPressed;
        public Keys[] KeysPressed
        {
            get { return keysPressed; }
        }

        private float MouseWheelDelta;

        private Input()
        {
            _ks = Keyboard.GetState();
            _ms = Mouse.GetState();
            _gps = GamePad.GetState(PlayerIndex.One);

            mousePosition = new Vector2();
        }

        public void AssignCustomControls(Type enumType)
        {
            if(!enumType.IsEnum)
            {
                Console.WriteLine("You must provide an enum type to AssignCustomControls()");
                return;
            }

            if (File.Exists(@"../Data/Config/Input.xml"))
            {
                EnumType = enumType;
                CustomInputs = new Dictionary<string, Dictionary<string, string>>();

                Stream stream = File.OpenRead(@"../Data/Config/Input.xml");
                XDocument document = XDocument.Load(stream);

                XElement neonInputs = document.Element("NeonInputs");

                foreach (XElement input in neonInputs.Elements("Input"))
                {
                    CustomInputs[input.Attribute("Name").Value] = new Dictionary<string, string>();

                    foreach (XElement inputMethod in input.Elements("InputMethod"))
                    {
                        CustomInputs[input.Attribute("Name").Value].Add(inputMethod.Attribute("Name").Value, inputMethod.Value);
                    }
                }

                Console.WriteLine("");
            }
            else
            {
                string[] enumValues = Enum.GetNames(enumType);
                XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                XElement neonInputs = new XElement("NeonInputs");
               
                for (int i = 0; i < enumValues.Length - 1; i++)
                {
                    XElement input = new XElement("Input", new XAttribute("Name", enumValues[i]));
                    XElement inputMethod = new XElement("InputMethod", new XAttribute("Name", "Keyboard"));
                    inputMethod.Value = "None";
                    input.Add(inputMethod);
                    inputMethod = new XElement("InputMethod", new XAttribute("Name", "XboxController"));
                    inputMethod.Value = "None";
                    input.Add(inputMethod);

                    neonInputs.Add(input);
                }             

                document.Add(neonInputs);

                document.Save(@"../Data/Config/Input.xml");

                Console.WriteLine("Input.xml created with no input assigned.");
            }
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
                MouseWheelDelta = ms.ScrollWheelValue - _ms.ScrollWheelValue;

            keysPressed = ks.GetPressedKeys();
        }

        public void LastFrameState()
        {
            _ks = ks;
            _ms = ms;
            _gps = gps;
            MouseWheelDelta = 0;
        }

        #region Custom input functions
        public bool Pressed<T>(T NeonCustomInput)
        {
            if (NeonCustomInput.GetType() != EnumType)
                return false;
            else
                if (CustomInputs[NeonCustomInput.ToString()] != null)
                    foreach (KeyValuePair<string, string> kvp in CustomInputs[NeonCustomInput.ToString()])
                    {
                        if (kvp.Value == "None")
                            continue;

                        if (kvp.Key == "Keyboard")
                        {
                            if (this.Pressed((Keys)Enum.Parse(typeof(Keys), kvp.Value)))
                                return true;
                        }
                        else if (kvp.Key == "XboxController")
                        {
                            if (this.Pressed((Buttons)Enum.Parse(typeof(Buttons), kvp.Value)))
                                return true;
                        }
                    }

            return false;
        }
        public bool Check<T>(T NeonCustomInput)
        {
            if (NeonCustomInput.GetType() != EnumType)
                return false;
            else
                if (CustomInputs[NeonCustomInput.ToString()] != null)
                    foreach (KeyValuePair<string, string> kvp in CustomInputs[NeonCustomInput.ToString()])
                    {
                        if (kvp.Value == "None")
                            continue;

                        if (kvp.Key == "Keyboard")
                        {
                            if (this.Check((Keys)Enum.Parse(typeof(Keys), kvp.Value)))
                                return true;
                        }
                        else if (kvp.Key == "XboxController")
                        {
                            if (this.Check((Buttons)Enum.Parse(typeof(Buttons), kvp.Value)))
                                return true;
                        }
                    }
                       
            return false;
        }

        
        public bool Released<T>(T NeonCustomInput)
        {
            if (NeonCustomInput.GetType() != EnumType)
                return false;
            else
                if (CustomInputs[NeonCustomInput.ToString()] != null)
                    foreach (KeyValuePair<string, string> kvp in CustomInputs[NeonCustomInput.ToString()])
                    {
                        if (kvp.Value == "None")
                            continue;

                        if (kvp.Key == "Keyboard")
                        {
                            if (this.Released((Keys)Enum.Parse(typeof(Keys), kvp.Value)))
                                return true;
                        }
                        else if (kvp.Key == "XboxController")
                        {
                            if (this.Released((Buttons)Enum.Parse(typeof(Buttons), kvp.Value)))
                                return true;
                        }
                    }

            return false;
        }
        #endregion

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

        public float MouseWheel()
        {
            return MouseWheelDelta;
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
