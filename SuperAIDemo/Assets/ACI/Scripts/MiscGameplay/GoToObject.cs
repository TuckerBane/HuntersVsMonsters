using UnityEngine;
using System.Collections;

public class GoToObject : MonoBehaviour {

    public GameObject m_target = null;
    public float m_speed = 30.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_target)
            return;
        Vector3 toTarget = m_target.transform.position - transform.position;
        if (toTarget.magnitude < m_speed / 30.0f)
            return;
        toTarget.Normalize();
        transform.position += toTarget * m_speed * Time.deltaTime;

	}

}
