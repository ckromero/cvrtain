public interface IGestureManager {
	CompletedGestureStruct LastGesture { get; }

	bool CompareGestureNames(string[] names);
}