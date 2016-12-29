using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public enum Side {Left, Right}
public class ControllerReadout : MonoBehaviour {

    public Side LeftRight;
    public HandsTracker Tracker;
    public HeadTracker HeadTracker;

    private Text _AngleText;
    private Text _ReachText;
    private Text _DistanceText;

    private float _WaveTimer = 0f;

	// Use this for initialization
	void Start () {
	    foreach (var text in GetComponentsInChildren<Text>()) {
            if (text.gameObject.name == "Angle") {
                _AngleText = text;
            }
            else if (text.gameObject.name == "Reach") {
                _ReachText = text;
            }
            else if (text.gameObject.name == "Head distance")
            {
                _DistanceText = text;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        var angle = 0f;
        var reach = 0f;
        var handPosition = Vector3.zero;
	    if (LeftRight == Side.Left) {
            angle = Tracker.LeftHandAngle;
            reach = Tracker.LeftHandRing;
            if (Tracker.LeftHandWaving) {
                _WaveTimer = 2f;
            }
            handPosition = Tracker.LeftHand.localPosition;
        }
        else {
            angle = Tracker.RightHandAngle;
            reach = Tracker.RightHandRing;
            if (Tracker.RightHandWaving) {
                _WaveTimer = 2f;
            }
            handPosition = Tracker.RightHand.localPosition;
        }

        var headPosition = HeadTracker.HeadTransform.localPosition;
        var distance = Vector3.Distance(handPosition, headPosition);

        _AngleText.text = "Angle: " + Mathf.Round(angle);
        _ReachText.text = "Reach: " + reach;
        _DistanceText.text = "Distance: " + distance.ToString("F2");

        if (_WaveTimer > 0f) {
            _WaveTimer -= Time.deltaTime;
            _AngleText.color = Color.green;
            _ReachText.color = Color.green;
        }
        else {
            _AngleText.color = Color.white;
            _ReachText.color = Color.white;
        }
	}
}
