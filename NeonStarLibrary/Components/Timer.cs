using Microsoft.Xna.Framework;
using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarLibrary
{
    public class Timer : Component
    {
        private TextDisplay _display = null;

        public TextDisplay Display
        {
            get { return _display; }
            set { _display = value; }
        }

        private string _displayFormat = "";

        public string DisplayFormat
        {
            get { return _displayFormat; }
            set { _displayFormat = value; }
        }

        private TimeSpan _timer = new TimeSpan();

        public Timer(Entity entity)
            :base(entity, "Timer")
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _timer += gameTime.ElapsedGameTime;
            if (_display != null)
            {
                _display.Text = _timer.ToString(_displayFormat);
            }
            base.Update(gameTime);
        }
    }
}
