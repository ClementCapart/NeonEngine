using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    public enum ActionType
    {
        DeleteEntity,
        AddEntity,
        ChangeEntityParameters,
        DeleteComponent,
        AddComponent
    }

    public class EditorAction
    {
        private ActionType actionType;
        public ActionType GetActionType 
        {
            get { return actionType; }
        }
        
        private object[] parameters;
        public object[] GetParameters
        {
            get { return parameters; }
        }

        public EditorAction(ActionType actionType, object[] parameters)
        {
            this.actionType = actionType;
            this.parameters = parameters;
        }

        public EditorAction(ActionType actionType, object parameters)
        {
            this.actionType = actionType;
            this.parameters = new object[1];
            this.parameters[0] = parameters;
        }

    }
}
