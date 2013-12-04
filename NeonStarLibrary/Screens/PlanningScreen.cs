using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    class PlanningScreen : World
    {
        Entity entityToChase;

        public PlanningScreen(Game game)
            :base(game)
        {
            LoadLevel(new Level(@"..\Data\Levels\Level_PreprodEnding", this, true));

            entityToChase = entities.Where(e => e.Name == "LiOn").First();
            camera.Bounded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!Pause)
            {

                    camera.Chase(entityToChase.transform.Position, gameTime);
            }
        }
    }
}
