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
    public float m_goalDistance = 10.0f;
    public override bool DoAction(GameObject actor)
    {
        Vector3 toDestination = (m_destination - actor.transform.position);
        if (toDestination.magnitude <= m_goalDistance)
            return true;
        toDestination.Normalize();
        actor.GetComponent<Rigidbody>().velocity = toDestination * m_speed;
        return false;
    }
}

public class Stop : Action
{
    public override bool DoAction(GameObject actor)
    {
        actor.GetComponent<Rigidbody>().velocity = Vector3.zero;
        return true;
    }
}

public class Drop : Action
{
     public override bool DoAction(GameObject actor)
    {
        actor.GetComponent<Inventory>().PlaceObject();
        return true;
    }
}

public class PickUpSomething : Action
{
    public GameObject m_targetObject;
    public override bool DoAction(GameObject actor)
    {
        m_targetObject.SendMessage("PlayerUseObject", actor);
        return true;
    }
}

public class CraftSomething : Action
{
    public CraftingRecipe m_recipe;
    public CraftingSystem m_craftingSystem;
    public override bool DoAction(GameObject actor)
    {
        GameObject newObj = m_craftingSystem.TryToCraft(actor.GetComponent<Inventory>(), m_recipe);
        // HACK don't instantly pick it up, looks bad
        newObj.SendMessage("PlayerUseObject", actor);
        return true;
    }
}

public class ActionList : MonoBehaviour {

    public List<Action> m_list = new List<Action>();
	// Use this for initialization
	void Start () {
        //m_list.Add(new GoSomewhere());
	}
	
	// Update is called once per frame
	void Update () {
        // does action and checks for completion. Also allows for multiple 0 time actions in a single frame
            while (m_list.Count > 0 && m_list[0].DoAction(gameObject))
                m_list.RemoveAt(0);
	}
}


