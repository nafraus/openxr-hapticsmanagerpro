/// <summary>
/// Example File of how to setup HapticImpulse/HapticData for new types
/// </summary>

public class ExampleHapticImpulse : HapticImpulse
{
    public ExampleHapticImpulse(HapticData<ExampleType> scriptableObject, VRHapticsManager manager) //<< change ExampleHapticImpulse to be YOURTYPEHapticImpulse and ExampleType to your Type
    {
        data = scriptableObject;
        removeAction += manager.RemoveHaptic;
        ImpulseType = scriptableObject.Type;
        manager.AddTimeStepAction += AddTime;
    }
    public HapticData<ExampleType> data { get; set; } //<< Replace ExampleType with your Type

    public override float MaxTime { get => data.MaxDuration; }

    public override float Value
    {
        get
        {
            return data.Value.sample; //<< change the return to be your scriptable object's Value evaluation. See CurveHapticImpulse for an example
        }
    }
}