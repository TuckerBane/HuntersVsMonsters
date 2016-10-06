using UnityEngine;
using System.Collections;

public class Lifespan : MonoBehaviour {


    public float m_time = 5.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        m_time -= Time.deltaTime;
        if (m_time <= 0)
            Destroy(gameObject);
	}
}
