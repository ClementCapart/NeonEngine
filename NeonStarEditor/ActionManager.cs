using NeonEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonStarEditor
{
    static public class ActionManager
    {
        static public Queue<EditorAction> ActionsQueue = new Queue<EditorAction>();

        static public void SaveAction(ActionType actionType, object parameters)
        {
            ActionsQueue.Enqueue(new EditorAction(actionType, parameters));
            if (ActionsQueue.Count == 100)
                ActionsQueue.Dequeue();
        }

        static public void Undo()
        {
            if (ActionsQueue.Count > 0)
            {
                EditorAction LastAction = ActionsQueue.Dequeue();

                switch (LastAction.GetActionType)
                {
                    case ActionType.AddComponent:
                        Console.WriteLine("Undo 'Add Component'.");
                        break;

                    case ActionType.AddEntity:
                        (LastAction.GetParameters as Entity).Destroy();
                        Console.WriteLine("Undo 'Add Entity'.");
                        break;

                    case ActionType.ChangeEntityParameters:
                        Console.WriteLine("Undo 'Change Entity Parameters'.");
                        break;

                    case ActionType.DeleteComponent:
                        Console.WriteLine("Undo 'Delete Component'.");
                        break;

                    case ActionType.DeleteEntity:
                        Console.WriteLine("Undo 'Delete Entity'.");
                        break;
                }
            }
            else
                Console.WriteLine("Nothing to Undo !");
        }
    }
}
