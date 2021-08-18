using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public UnityEvent<Vector3> FireEvent = new UnityEvent<Vector3>();


    public void InvokeFireEvent(Vector3 target)
    {
        FireEvent.Invoke(target);
    }
}
