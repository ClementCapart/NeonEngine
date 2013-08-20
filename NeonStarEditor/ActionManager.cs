using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NeonStarEditor
{
    static public class ActionManager
    {
        static private List<EditorAction> ActionsList = new List<EditorAction>();

        static public void SaveAction(ActionType actionType, object[] parameters)
        {
            ActionsList.Add(new EditorAction(actionType, parameters));
            if (ActionsList.Count == 100)
                ActionsList.RemoveAt(0);
        }

        static public void SaveAction(ActionType actionType, object parameters)
        {
            ActionsList.Add(new EditorAction(actionType, parameters));
            if (ActionsList.Count == 100)
                ActionsList.RemoveAt(0);
        }

        static public void Undo()
        {
            if (ActionsList.Count > 0)
            {
                EditorAction LastAction = ActionsList.Last();
                ActionsList.Remove(ActionsList.Last());

                switch (LastAction.GetActionType)
                {
                    case ActionType.AddComponent:
                        (LastAction.GetParameters[0] as Component).Remove();
                        (LastAction.GetParameters[1] as InspectorControl).InstantiateProperties((LastAction.GetParameters[0] as Component).entity);
                        Console.WriteLine("Undo 'Add Component'.");
                        break;

                    case ActionType.AddEntity:
                        (LastAction.GetParameters[0] as Entity).Destroy();
                        Console.WriteLine("Undo 'Add Entity'.");
                        break;

                    case ActionType.ChangeEntityParameters:
                        DataManager.LoadComponentParameters(LastAction.GetParameters[1] as XElement, LastAction.GetParameters[0] as Component);
                        Console.WriteLine("Undo 'Change Entity Parameters'.");
                        break;

                    case ActionType.DeleteComponent:
                        Component c = (Component)Activator.CreateInstance(LastAction.GetParameters[0] as Type, LastAction.GetParameters[2] as Entity);
                        DataManager.LoadComponentParameters(LastAction.GetParameters[1] as XElement, c);
                        (LastAction.GetParameters[2] as Entity).AddComponent(c);
                        (LastAction.GetParameters[3] as InspectorControl).InstantiateProperties(LastAction.GetParameters[2] as Entity);
                        Console.WriteLine("Undo 'Delete Component'.");
                        break;

                    case ActionType.DeleteEntity:
                        DataManager.LoadPrefab(LastAction.GetParameters[0] as XElement, LastAction.GetParameters[1] as EditorScreen);
                        Console.WriteLine("Undo 'Delete Entity'.");
                        break;
                }
            }
            else
                Console.WriteLine("Nothing to Undo !");
        }
    }
}
