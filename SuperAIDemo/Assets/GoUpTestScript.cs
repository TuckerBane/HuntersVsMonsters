using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class GoUpTestScript : MonoBehaviour {

    private Transform m_Transform;

	// Use this for initialization
	void Start () {
        m_Transform = GetComponent<Transform>();
    }
	
	// Update is called once per frame
	void Update () {

        m_Transform.position = m_Transform.position + new Vector3(0,0.01f,0);
       // GetComponent<Rigidbody>().AddForce(new Vector3(0,1,0) );
        //transform.position. = 5;
	}
}
