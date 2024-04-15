using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo Genericize This so that it can be passed into inspector easier

public abstract class HapticData<T> : ScriptableObject, IHapticData<T>
{
    //Look into serialized properties
    [SerializeField] protected T intensity;
    [SerializeField] protected XRHapticsApplyType type;
    [SerializeField] protected float duration;
    [SerializeField] protected int operationLayer;
    [SerializeField] protected int priorityLayer;

    #region IHapticData Implementation
    public T Value { get => intensity; }
    public XRHapticsApplyType Type { get => type; }
    public float MaxDuration { get => duration; }
    //public int OperationLayer { get => operationLayer; }
    //public int PriorityLayer { get => priorityLayer; }
    #endregion

    //Rename
    public abstract HapticImpulse GenerateImpulse(VRHapticsManager manager);
}