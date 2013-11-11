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

        private int _leftSlotLevel = 1;

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

        private int _rightSlotLevel = 1;

        public int RightSlotLevel
        {
            get { return _rightSlotLevel; }
            set { _rightSlotLevel = value; }
        }

        private int _maxLevel = 3;

        public int MaxLevel
        {
            get { return _maxLevel; }
            set { _maxLevel = value; }
        }

        public ElementEffect CurrentElementEffect = null;
        public bool CanUseElement = true;

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
            if (AvatarComponent.meleeFight.CanAttack && AvatarComponent.thirdPersonController.CanMove && AvatarComponent.thirdPersonController.CanTurn && CanUseElement)
            {
                if (Neon.Input.Pressed(NeonStarInput.UseLeftSlotElement))
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + LeftSlotElement);
                        UseElement(_leftSlotElement, NeonStarInput.UseLeftSlotElement);
                    }
                }
                else if (Neon.Input.Pressed(NeonStarInput.UseRightSlotElement))
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Use Element -> " + RightSlotElement);
                        UseElement(_rightSlotElement, NeonStarInput.UseRightSlotElement);
                    }
                }

                if (Neon.Input.Pressed(NeonStarInput.DropLeftSlotElement))
                {
                    if (_leftSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + LeftSlotElement);
                        _leftSlotElement = Element.Neutral;
                        _leftSlotLevel = 1;
                    }
                }
                if (Neon.Input.Pressed(NeonStarInput.DropRightSlotElement))
                {
                    if (_rightSlotElement != Element.Neutral)
                    {
                        Console.WriteLine("Drop Element -> " + RightSlotElement);
                        _rightSlotElement = Element.Neutral;
                        _rightSlotLevel = 1;
                    }
                }
            }
            else if (CurrentElementEffect != null)
            {
                CurrentElementEffect.Update(gameTime);
            }
        }

        public void UseElement(Element element, NeonStarInput input)
        {
            switch(element)
            {
                case Element.Fire:
                    CurrentElementEffect = new Fire(this, entity, input, (GameScreen)entity.containerWorld);
                    CanUseElement = false;
                    break;

                case Element.Thunder:
                    break;
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
