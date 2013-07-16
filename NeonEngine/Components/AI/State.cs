using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public class State
    {
        int StateID;
        public int GetStateID
        {
            get { return StateID; }
        }

        string _TagName;
        public string TagName
        {
            get { return _TagName; }
            set 
            { 
                _TagName = value;
            }
        }

        public State ThisState
        {
            get { return this; }
        }

        public bool MustReset = true;
        public List<AICondition> Conditions = new List<AICondition>();
        public List<AIAction> Actions = new List<AIAction>();

        public State(int id)
        {
            StateID = id;
        }

        public void Act(GameTime gameTime)
        {
            foreach (AIAction Action in Actions)
                Action.Act(gameTime);              
        }

        public AICondition Check(GameTime gameTime)
        {
            foreach (AICondition Condition in Conditions)
                if (Condition.Check(gameTime))
                    return Condition;

            return null;
        }

        public void Reset()
        {
            foreach (AIAction Action in Actions)
                Action.Reset();
            foreach (AICondition Condition in Conditions)
                Condition.Reset();
        }
    }
}
