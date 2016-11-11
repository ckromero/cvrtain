using UnityEngine;
using System.Collections;

public class HxDummyLight : MonoBehaviour {

    public LightType type = LightType.Point;
    public float range = 10;
    [Range(0, 179)]
    public float spotAngle = 40;
    public Color color = Color.white;
    [Range(0, 8)]
    public float intensity = 1;  


    public Texture cookie = null;

    //need this for the editor..
#if UNITY_EDITOR
    public void Update()
    {

    }
#endif
}
