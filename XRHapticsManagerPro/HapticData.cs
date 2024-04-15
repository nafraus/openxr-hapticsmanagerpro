using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Todo Genericize This so that it can be passed into inspector easier

public abstract class HapticData<T> : GenericHapticData
{
    //Look into serialized properties
    [SerializeField] protected T intensity;
    [SerializeField] protected XRHapticsApplyType type;
    [SerializeField] protected float duration;
    [SerializeField] protected int operationLayer;
    [SerializeField] protected int priorityLayer;

    public T Value { get => intensity; }

    #region IHapticData Implementation
    public XRHapticsApplyType Type { get => type; }
    public float MaxDuration { get => duration; }
    //public int OperationLayer { get => operationLayer; }
    //public int PriorityLayer { get => priorityLayer; }
    #endregion

    //Rename
    public override abstract HapticImpulse GenerateImpulse(XRHapticsManagerPro manager);
}