# Haptics Manager Pro for Unity OpenXR
System to expand upon the base functionality of sending haptic impulses in OpenXR for Unity. Allows the user to apply multiple haptic impulses to controllers, supports curves and custom types, and has different application types.

## Installation
Download and install the .unitypackage from here: [link]

## Usage
1. Create a new GameObject in your scene named 'HapticManager', and add the 'XRHapticsManagerPro" component.
2. Make a new folder in your project folder to store the haptic impulses you create, called 'HapticData'.
3. Create your first Haptic data by right clicking on your new folder, Create > ScriptableObject > HapticData > and pick one. See [HapticData](#hapticdata) for more
4. Create a new script or in an existing script add a field for a GenericHapticData. See SampleHapticManagerImplementation or the Impulse Tester prefab found in XRHapticsManagerPro > Samples. 
        
```c#
    public GenericHapticData Impulse;
```
5. Implement the XRHapticsManagerPro function call in your script `XRHapticsManagerPro.Instance.SendHapticImpulse()`, passing in Impulse and the controller you want to send the impulse. For example: 

``` 
public bool ActivateImpulse;

   private void Update()
    {
        if (ActivateImpulse)
        {
            ActivateImpulse = false;
        
            //Sends a haptic impulse to both conrtollers
            XRHapticsManagerPro.Instance.SendHapticImpulse(Data, XRHapticControllerSpecifier.both);
        }
   }
```

6.

## HapticData
HapticData is a typed scriptable object that the user creates to store information about the haptic impulse the user wants to send. Users will use the GenericHapticData class when creating fields in the inspector, and should not need to alter the HapticData class. XRHapticsManagerPro comes with FloatHapticData and CurveHapticData built in. See [Custom Types](#custom-types) to learn how to support other types. HapticData uses GenerateImpulse to generate a [HapticImpulse](#hapticimpulse) with writable data that is used by then XRHapticsManagerPro class.

The calculated impulse sent to the controllers is clamped from 0-1. Make sure to account for this when creating HapticData.

- Intensity: Dynamic type that will determine the strength of the haptic impulse at any given time.
- Type: Modifier type that changes how this haptic data will interact with other haptic data.
  - Additive: Adds itself to all other additives
  - Multiplier: Multiplies itself to all other multipliers, then multiplies with additives.
  - Override: Overrides all other active HapticData, and only using the applying the most recent override haptic data for each DirectInteractor.
- Duration: How long the haptic impulse will be active for.

HapticImpulse GenerateImpulse(): Generates an instance of HapticImpulse using the data set in HapticData. 

## HapticImpulse

## XRHapticsManagerPro

## Custom Types
XRHapticsManagerPro was designed to use generic types for HapticData and Impulses, and allows the user to use types other than float and curve with minimal effort.

### To Create a Custom Type:
1. Find and duplicate 'ExampleHapticDataSO' and 'ExampleHapticImpulse' in XRHapticsManagerPro > Samples.
2. Rename both duplicated files and replace Example with the name of the new type you will be using.
3. Open both of the new files and do this stuff:

