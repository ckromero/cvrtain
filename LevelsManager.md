### Levels Manager

The Levels Manager contains a collection of Levels which defines the different Gestures that the audience will respond to at different levels of excitement.

Every time GestureManager.LastGesture changes, the LevelsManager  passes the name into Level.EvaluateGesture() of the current Level.

By checking the Completed or Failed properties of the current Level then LevelsManager determines whether or not to advance to the next Level or fallback to the last.

The LevelManager also checks if the player has been inactive for a long period via **DelayBeforeDecayStarts**, **DecayGap** and **InactionTimeout**. Whenever the player is not performing a gesture, InactionTimeout increments. If it exceeds DelayBeforeDecayStarts, then LevelManager begins calling Level.Decrement() of the current Level which will eventually lead to its Failed property being true.

## Level

Contains three lists of gesture names: Positive, Neutral, and Negative. Positive gestures are gestures that will increment towards the next level. Neutral gestures will provoke and audience response, but not advance/retreat the level. Negative gestures (**not currently used**) will decrement towards the previous level.

Other variables:
* PromotionRequirement: number of positive gestures that must be completed to advance
* DemotionRequirement: number of negative gestures that must be completed to fallback to the previous level

Properties:
* Complete: have enough positive gestures been completed to exceed  PromotionRequirement
* Failed: have enough negative gestures completed to exceed DemotionRequirement