using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class CinematicScreen : World
    {
        public CinematicScreen(Game game)
            :base(game)
        {
            LoadLevel(new Level(@"..\Data\Levels\Level_PreprodBeginning", this, true));
        }
    }
}
