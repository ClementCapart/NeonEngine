using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine.AI
{
    public class AI : Component
    {
        int AIID;
        public int GetAIID
        {
            get { return AIID; }
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
        
        public List<State> StatesList = new List<State>();
        public Dictionary<AICondition, State> StateSwitch = new Dictionary<AICondition, State>();
        State CurrentState;
        Rigidbody AIBody;

        public AI(int ID, Rigidbody AIBody, Entity entity)
            :base(entity, "AI")
        {
            this.AIID = ID;
            this.AIBody = AIBody;
        }

        public AI(int ID)
            : this(ID, null, null)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            CurrentState.Act(gameTime);
            AICondition FilledCondition = CurrentState.Check(gameTime);
            if (FilledCondition != null)
                ChangeState(FilledCondition);
            base.Update(gameTime);
        }

        public void ChangeState(AICondition condition)
        {
            if (CurrentState.MustReset)
                CurrentState.Reset();
            CurrentState = StateSwitch[condition];           
        }

        public AICondition GetConditionByID(int ID)
        {
            foreach (State s in StatesList)
                foreach (AICondition c in s.Conditions)
                    if (c.ConditionID == ID)
                        return c;

            return null;
        }

        public State GetStateByID(int ID)
        {
            foreach (State s in StatesList)
                if (s.GetStateID == ID)
                    return s;

            return null;
        }
    }
}
