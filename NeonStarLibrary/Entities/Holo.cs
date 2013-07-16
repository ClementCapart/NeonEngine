using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NeonEngine;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Entities;

namespace NeonStarLibrary
{
    class Holo : Cosmetic
    {
        SpriteSheet PressButtonAnim;
        Avatar avatar;

        float ActivationRange = 100f;
        bool Active;
        bool LastActive;
        bool Displaying = false;
        int CurrentIndex;
        Graphic CurrentSlide;
        List<string> SlideTags;
        int ChangingSlide = 0;

        public Holo(Vector2 Position, List<string> SlideTags, World containerWorld, Avatar avatar)
            : base(Position, "HoloScreen" ,DrawLayer.Middleground2, containerWorld)
        {
            this.avatar = avatar;
            this.SlideTags = SlideTags;
            PressButtonAnim = new SpriteSheet(AssetManager.GetSpriteSheet("PressButton"), DrawLayer.Middleground2, this);          
        }

        public override void Update(GameTime gameTime)
        {
            LastActive = Active;

            if (Vector2.Distance(avatar.transform.Position, this.transform.Position) < ActivationRange)
                Active = true;
            else
                Active = false;

            if (Active && !LastActive)
                AddComponent(PressButtonAnim);
            else if (!Active && LastActive)
                PressButtonAnim.Remove();

            if (Displaying && Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                RemoveSlide();
                avatar.NoControl = false;
            }



            if (Displaying)
            {
                if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.DPadLeft))
                    PreviousSlide();
                else if (Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.DPadRight))
                    NextSlide();

                if (ChangingSlide == 1)
                {
                    if (CurrentSlide.opacity > 0)
                        CurrentSlide.opacity -= 0.05f;
                    else
                    {
                        CurrentSlide.Remove();
                        Displaying = false;
                        this.DisplaySlide();
                        ChangingSlide = 0;
                    }
                }
                else
                {
                    if (CurrentSlide.opacity <= 1f)
                        CurrentSlide.opacity += 0.05f;
                }
            }

            if (Active && Neon.Input.Pressed(Microsoft.Xna.Framework.Input.Buttons.X))
            {
                if (Displaying)
                    Displaying = false;
                else
                {
                    DisplaySlide();
                    avatar.NoControl = true;
                }
            }

            base.Update(gameTime);
        }

        private void RemoveSlide()
        {
            CurrentSlide.Remove();
        }

        private void DisplaySlide()
        {
            if (!Displaying)
            {
                Displaying = true;
                CurrentSlide = new Graphic(AssetManager.GetTexture(SlideTags[CurrentIndex]), DrawLayer.HUD, this);
                CurrentSlide.opacity = 0f;
                AddComponent(CurrentSlide);
            }           
        }

        private void NextSlide()
        {
            if (CurrentIndex != SlideTags.Count - 1)
            {
                ChangingSlide = 1;
                CurrentIndex++;              
            }
        }

        private void PreviousSlide()
        {
            if (CurrentIndex != 0)
            {
                ChangingSlide = 1;             
                CurrentIndex--;
            }
         
        }
    }
}
