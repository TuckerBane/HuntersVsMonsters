using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Action
{
    // returns true if the action is complete
    public virtual bool DoAction(GameObject actor){ return true; }
}

public class GoSomewhere : Action
{
    public Vector3 m_destination = new Vector3(0,0,0);
    public float m_speed = 10.0f;
    public float m_goalDistance = 20.0f;
    public override bool DoAction(GameObject actor)
    {
        Vector3 toDestination = (actor.transform.position - m_destination);
        if (toDestination.magnitude <= m_goalDistance)
            return true;
        toDestination.Normalize();
        actor.GetComponent<Rigidbody>().velocity = toDestination * m_speed;
        return false;
    }
}

public class ActionList : MonoBehaviour {

    List<Action> m_list = new List<Action>();
	// Use this for initialization
	void Start () {
        m_list.Add(new GoSomewhere());
	}
	
	// Update is called once per frame
	void Update () {
        // does action and checks for completion. Also allows for multiple 0 time actions in a single frame
            while (m_list.Count > 0 && m_list[0].DoAction(gameObject))
                m_list.RemoveAt(0);
	}
}
