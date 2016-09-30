using UnityEngine;
using System.Collections;

public class DestroyOnUse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void PlayerUseObject(GameObject player)
    {
        Destroy(gameObject);
    }
}
