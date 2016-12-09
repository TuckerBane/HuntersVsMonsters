using UnityEngine;
using System.Collections;

public class FacePlayer : MonoBehaviour {

    GameObject m_player;

	// Use this for initialization
	void Start () {
        m_player = FindObjectOfType<CharacterController>().gameObject;
	}
	
	// Update is called once per frame
	void Update () {
        if (m_player)
        {
            transform.rotation = m_player.transform.rotation;
            //transform.LookAt(m_player.transform);
        }
	}
}
