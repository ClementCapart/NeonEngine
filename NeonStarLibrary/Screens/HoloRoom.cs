using Microsoft.Xna.Framework.Input;
using NeonEngine;
using Microsoft.Xna.Framework;
using NeonStarLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class HoloRoom : World
    {
        Teleport tp;
        public Avatar avatar;

        public HoloRoom(Game game)
            :base(game)
        {
            LoadLevel(new Level(@"..\Data\Levels\Level_0-1", this, true));
            avatar = new Avatar(this, new Vector2(-561, 200));
            AddEntity(avatar);

            List<string> FirstSlides = new List<string>();
            FirstSlides.Add("Slide01-01");
            FirstSlides.Add("Slide01-02");
            FirstSlides.Add("Slide01-03");
            FirstSlides.Add("Slide01-04");

            List<string> SecondSlides = new List<string>();
            SecondSlides.Add("Slide02-01");
            SecondSlides.Add("Slide02-02");
            SecondSlides.Add("Slide02-03");
            SecondSlides.Add("Slide02-04");


            List<string> ThirdSlides = new List<string>();
            ThirdSlides.Add("Slide03-01");
            ThirdSlides.Add("Slide03-02");
            ThirdSlides.Add("Slide03-03");

            List<string> FourthSlides = new List<string>();
            FourthSlides.Add("Slide04-01");

            AddEntity((new ParallaxPlan(this, Vector2.Zero, "HoloRoomBackground", ScrollingType.FreeScroll, 1f, 1f)));
            AddEntity(new Holo(new Vector2(-451, 148), FirstSlides, this, avatar));
            AddEntity(new Holo(new Vector2(-221, 148), SecondSlides, this, avatar));
            AddEntity(new Holo(new Vector2(221, 148), ThirdSlides, this, avatar));
            AddEntity(new Holo(new Vector2(451, 148), FourthSlides, this, avatar));

            tp = new Teleport(new Vector2(11, 152), this, avatar);
            AddEntity(tp);
        }

        public override void Update(GameTime gameTime)
        {
            if (Neon.Input.Pressed(Buttons.DPadUp))
                tp.IsActive = !tp.IsActive;

            base.Update(gameTime);
        } 
    }
}
