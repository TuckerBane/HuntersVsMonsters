using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class FollowFFPath : MonoBehaviour {
    public FFPath someoneElsesPath;
    public float speed = 1.0f;
    public float distance = 0.0f;
    private Transform myTransform;

	// Use this for initialization
	void Start () {
        myTransform = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        distance += speed * Time.deltaTime;
        myTransform.position = someoneElsesPath.PointAlongPath(distance);
	}
}
