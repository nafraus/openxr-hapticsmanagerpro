using UnityEngine;

public interface IHapticData<T>
{
    public abstract T Value { get; }
    public XRHapticsApplyType Type { get; }
    
    public float MaxDuration { get; }
    //public int OperationLayer { get; }
    //public int PriorityLayer { get; }
}

public enum XRHapticsApplyType
{
    Override = 1 <<0,
    Additive = 1<< 1,
    Multiplier = 1 << 2,
    OverrideAndCancelAll = 1 << 3,
}