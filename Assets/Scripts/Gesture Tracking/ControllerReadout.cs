using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Side {Left, Right}
public class ControllerReadout : MonoBehaviour {

    public Side LeftRight;
    public HandsTracker Tracker;

    private Text _AngleText;
    private Text _ReachText;

	// Use this for initialization
	void Start () {
	    foreach (var text in GetComponentsInChildren<Text>()) {
            if (text.gameObject.name == "Angle") {
                _AngleText = text;
            }
            else if (text.gameObject.name == "Reach") {
                _ReachText = text;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        var angle = 0f;
        var reach = 0f;
	    if (LeftRight == Side.Left) {
            angle = Tracker.LeftHandAngle;
            reach = Tracker.LeftHandRing;
        }
        else {
            angle = Tracker.RightHandAngle;
            reach = Tracker.RightHandRing;
        }

        _AngleText.text = "Angle: " + Mathf.Round(angle);
        _ReachText.text = "Reach: " + reach;
	}
}
