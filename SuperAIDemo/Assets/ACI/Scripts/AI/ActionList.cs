using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TODO add enter and exit events
public class Action
{
    // returns true if the action is complete
    public virtual bool DoAction(GameObject actor) { return true; }
}

#region BasicActions

public class GoSomewhere : Action
{
    public GoSomewhere() { }
    public GoSomewhere(Vector3 destination)
    {
        m_destination = destination;
    }

    public Vector3 m_destination = new Vector3(0,0,0);
    public float m_speed = 20.0f;
    public float m_goalDistance = 10.0f;
    public override bool DoAction(GameObject actor)
    {
        Vector3 toDestination = (m_destination - actor.transform.position);
        if (toDestination.magnitude <= m_goalDistance)
        {
            actor.GetComponent<Rigidbody>().velocity = Vector3.zero;
            return true;
        }
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
    public PickUpSomething(){}
    public PickUpSomething(GameObject obj)
    {
        m_targetObject = obj;
    }
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
        if (newObj == null)
        {
            Debug.Log("Crafting attempt failed");
            return true; // stop trying
        }

        newObj.SendMessage("PlayerUseObject", actor);
        return true;
    }
}
#endregion

#region AdvancedActions
class GetSomething : Action
{
    public GetSomething(CraftingComponent thingToGet) { m_thingToGet = thingToGet;}
    public CraftingComponent m_thingToGet;
    public override bool DoAction(GameObject actor) {
        ActionList list = actor.GetComponent<ActionList>();
        GameObject goalObj = CraftingAIGlobals.GetClosest(m_thingToGet, actor);
        list.m_list.Insert(1, new GoSomewhere(goalObj.transform.position));
        list.m_list.Insert(2, new PickUpSomething(goalObj));
        return true;
    }
}

class KillEnemy : Action
{
    CraftingComponent m_itemDropPrefab;
    private GameObject m_targetEnemy;
    private bool start = true;
    public KillEnemy(CraftingComponent itemDropPrefab) { m_itemDropPrefab = itemDropPrefab; }
    public override bool DoAction(GameObject actor)
    {
        if (start)
        {
            start = false;
            Start(actor);
        }

        if (m_targetEnemy == null) // if the enemy is dead
        {
            actor.GetComponent<ActionList>().m_list.Insert(1, new GetSomething(m_itemDropPrefab) );
            return true; // we're done here
        }

        Vector3 toDestination = (m_targetEnemy.transform.position - actor.transform.position);
        if (toDestination.magnitude > actor.GetComponent<Attack>().m_attackRange)
        {
            GoSomewhere go = new GoSomewhere(m_targetEnemy.transform.position);
            go.m_goalDistance = actor.GetComponent<Attack>().m_attackRange - 1.0f;
            actor.GetComponent<ActionList>().m_list.Insert(0, go);
            return false; // go to the enemy, then try this again
        }


        return false; // wait for the  enemy to die
    }
    private void Start(GameObject actor)
    {
        m_targetEnemy = CraftingAIGlobals.GetClosestEnemy(m_itemDropPrefab, actor);
        //TODO make enemy stuff generic, maybe using messages. That way it's more toolish.
        actor.GetComponent<Attack>().m_target = m_targetEnemy;
    }

}

#endregion

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


