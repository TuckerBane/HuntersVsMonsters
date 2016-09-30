using UnityEngine;
using System.Collections;


public class NevMeshTest : MonoBehaviour {
    public NavMeshAgent myNavMeshAgemt;
	// Use this for initialization
	void Start () {
        myNavMeshAgemt = GetComponent<NavMeshAgent>();  
	}
	
	// Update is called once per frame
	void Update () {
        myNavMeshAgemt.destination = new Vector3(47,0,270);
	}
}
