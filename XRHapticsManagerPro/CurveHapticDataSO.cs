using UnityEngine;
//Look into NativeCurve
[CreateAssetMenu(menuName = "ScriptableObject/HapticData/Curve", fileName = "CurveHapticData", order = 1)]
public class CurveHapticDataSO : HapticData<AnimationCurve>
{
    public override HapticImpulse GenerateImpulse(VRHapticsManager manager)
    {
        return new CurveHapticImpulse(this, manager);
    }
}
