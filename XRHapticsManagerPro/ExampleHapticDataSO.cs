using UnityEngine;

//[CreateAssetMenu(menuName = "ScriptableObject/HapticData/Example", fileName = "ExampleHapticData", order = 1)]
//Uncomment and change the names in the line above
public class ExampleHapticDataSO : HapticData<ExampleType> //Replace ExampleType with the type you are using
{
    public override HapticImpulse GenerateImpulse(XRHapticsManagerPro manager)
    {
        return new ExampleHapticImpulse(this, manager); //Replace ExampleHapticImpulse with the haptic impulse you create
    }
}

public struct ExampleType { public float sample; } // << This is just an example type so that the script compiles. Delete this when making a new type of HapticData
