//using UnityEngine;
//using System.Collections.Generic;

//public class MainThreadDispatcher : MonoBehaviour
//{
//    private static readonly Queue<System.Action> _executionQueue = new Queue<System.Action>();

//    public static void Enqueue(System.Action action)
//    {
//        lock (_executionQueue)
//        {
//            _executionQueue.Enqueue(action);
//        }
//    }

//    private void Update()
//    {
//        // Execute all actions in the queue on the main thread
//        while (_executionQueue.Count > 0)
//        {
//            _executionQueue.Dequeue().Invoke();
//        }
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
    private static readonly Queue<System.Action> actions = new Queue<System.Action>();

    public static void Enqueue(System.Action action)
    {
        lock (actions)
        {
            actions.Enqueue(action);
        }
    }

    private void Update()
    {
        lock (actions)
        {
            while (actions.Count > 0)
            {
                actions.Dequeue().Invoke();
            }
        }
    }
}
