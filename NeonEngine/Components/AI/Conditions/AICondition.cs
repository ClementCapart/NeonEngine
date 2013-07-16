using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public abstract class AICondition
    {
        private string _tagName;
        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        public int ConditionID;

        public AICondition(string TagName)
        {
            this.TagName = TagName;
        }

        public virtual bool Check(GameTime gameTime)
        {
            return false;
        }

        public virtual void Reset()
        {

        }
    }
}
