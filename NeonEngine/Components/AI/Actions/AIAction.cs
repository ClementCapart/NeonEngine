using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public class AIAction
    {
        private string _tagName;
        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        public AIAction(string TagName)
        {
            this.TagName = TagName;
        }

        public virtual void Act(GameTime gameTime)
        {
        }

        public virtual void Reset()
        {

        }
    }
}
