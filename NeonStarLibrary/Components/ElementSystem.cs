using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public enum Element
    {
        Neutral,
        Fire,
        Thunder
    }

    public class ElementSystem : Component
    {
        public Avatar AvatarComponent;

        private Element _leftSlotElement = Element.Neutral;

        public Element LeftSlotElement
        {
            get { return _leftSlotElement; }
            set { _leftSlotElement = value; }
        }

        private int _leftSlotLevel = 0;

        public int LeftSlotLevel
        {
            get { return _leftSlotLevel; }
            set { _leftSlotLevel = value; }
        }
        
        private Element _rightSlotElement = Element.Neutral;

        public Element RightSlotElement
        {
            get { return _rightSlotElement; }
            set { _rightSlotElement = value; }
        }

        private int _rightSlotLevel = 0;

        private int _maxLevel = 3;

        public int MaxLevel
        {
            get { return _maxLevel; }
            set { _maxLevel = value; }
        }

        public int RightSlotLevel
        {
            get { return _rightSlotLevel; }
            set { _rightSlotLevel = value; }
        }

        public ElementSystem(Entity entity)
            :base(entity, "ElementSystem")
        {
        }

        public override void Init()
        {
            AvatarComponent = entity.GetComponent<Avatar>();
            base.Init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (AvatarComponent.meleeFight.CanAttack && AvatarComponent.thirdPersonController.CanMove && AvatarComponent.thirdPersonController.CanTurn)
            {
                if (Neon.Input.Pressed(NeonStarInput.UseLeftSlotElement))
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + LeftSlotElement);
                    }
                }
                else if (Neon.Input.Pressed(NeonStarInput.UseRightSlotElement))
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + RightSlotElement);
                    }
                }

                if (Neon.Input.Pressed(NeonStarInput.DropLeftSlotElement))
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + LeftSlotElement);
                        _leftSlotElement = Element.Neutral;
                    }
                }
                if (Neon.Input.Pressed(NeonStarInput.DropRightSlotElement))
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + RightSlotElement);
                        _rightSlotElement = Element.Neutral;
                    }
                }
            }          
        }

        public void GetElement(Element element)
        {
            if (_leftSlotElement == element)
            {
                if (_leftSlotLevel < _maxLevel)
                    _leftSlotLevel++;
                Console.WriteLine("Left Slot Level Up -> " + _leftSlotLevel);
            }
            else if (_rightSlotElement == element)
            {
                if (_rightSlotLevel < _maxLevel)
                    _rightSlotLevel++;
                Console.WriteLine("Right Slot Level Up -> " + _rightSlotLevel);
            }
            else if (_leftSlotElement == Element.Neutral)
            {
                _leftSlotElement = element;
                Console.WriteLine("Got " + element + " in Left Slot");
            }
            else if (_rightSlotElement == Element.Neutral)
            {
                _rightSlotElement = element;
                Console.WriteLine("Got " + element + " in Right Slot");
            }
            else
            {
                Console.WriteLine("Fizzle");
            }
        }
    }
}
