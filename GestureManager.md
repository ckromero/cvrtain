### Gesture Manager

The Gesture Manager maintains a list of all Gestures that the player can perform and updates them with information about the current state of the Vive hands and headset.

The basic job of the Gesture Manager is to, each frame, pass information to each Gesture. The information is collected from the **HandsTracker** and **HeadTracker**.

If any Gesture completes during the frame, it then exposes that information in a **CompletedGestureStruct** (which contains the name of the gesture and the time it was completed) via the **LastGesture** property.

The GestureManager also handles checking for the Weird Dance gesture. It tracks every position the hands have occupied within the time defined by **MovementTrackingWindow**. If the total amount of distance moved in that time exceeds **WeirdDanceRequirement** then the GestureManager will register a Weird Dance gesture as completing rather than a Gesture in the Gestures array.

In the event that the same Gesture has been detected 3 times in a row, rather than setting that Gesture to **LastGesture**, instead, **LastGesture** will be set to “Laughable”.

Also, any Gesture that is performed while _HeadTracker.Facing is equal to HeadFacing.Back, will result in the alternative event “First Backwards” the first time this occurs, and then “Laughable” every time there after.

## Gesture

Gestures are essentially just a wrapper for GestureRules. GestureRules are large bundles of variables that define different states that the head and hands trackers can be in.

The rules are put together into the array **Rules**. Each frame, the Gesture is passed the HeadTracker and HandsTracker from the GestureManager and checks the current state of the head and hands against it’s current rule. If the state matches the state defined in the rule, then the Gesture advances to the next rule. If there are no rules left, then the gesture has been completed.

If the **EvaluationDelay** of the rule is set to greater than 0, then the Gesture will not report itself as complete until that amount of time has elapsed post the completion of its final rule.