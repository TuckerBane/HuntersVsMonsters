using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RecipeNode
{
    public CraftingRecipe m_recipe;
    // lack of pointers/references makes me sad
    // public List<RecipeNode > m_requiredRecipes;
}

public class PlaningAI : MonoBehaviour {

    public GameObject m_objectivePrefab;
    private CraftingSystem m_craftingSystem;
    private CraftingRecipe m_goalRecipe;
    private ActionList m_myActions;
    

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystem>();
        m_myActions = GetComponent<ActionList>();
        MakePlan();
	}

    void MakePlan()
    {
        m_goalRecipe = m_craftingSystem.GetRecipe(m_objectivePrefab);
        ExtendRecipeChain(m_goalRecipe);

        // HACK copy paste
        GoSomewhere go = new GoSomewhere();
        go.m_destination = m_craftingSystem.gameObject.transform.position;
        m_myActions.m_list.Add(go); // go to crafting table

        CraftSomething craft = new CraftSomething();
        craft.m_recipe = m_goalRecipe;
        craft.m_craftingSystem = m_craftingSystem;
        m_myActions.m_list.Add(craft); // craft the thing

        // Stop
        m_myActions.m_list.Add(new Stop()); // stop moving

        // Drop
        m_myActions.m_list.Add(new Drop()); // drop what you made
    }
	
    void ExtendRecipeChain(CraftingRecipe recipe)
    {
        foreach(GameObject component in recipe.m_craftingComponents)
        {
            CraftingRecipe newRecipe = m_craftingSystem.GetRecipe(component);
            if (newRecipe != null)
            {
                ExtendRecipeChain(newRecipe);

                // add use recipe to action list
                GoSomewhere go = new GoSomewhere();
                go.m_destination = m_craftingSystem.gameObject.transform.position;
                m_myActions.m_list.Add(go); // go to crafting table

                CraftSomething craft = new CraftSomething();
                craft.m_recipe = newRecipe;
                craft.m_craftingSystem = m_craftingSystem;
                m_myActions.m_list.Add(craft); // craft the thing

                // HACK go pick it up

                // make tree here
            }
            else
            {
                // add go get base component to action list
                // HACK find by crafting name instead. Ideally, use vision or something, but probably not.
                GameObject goalObject = GameObject.Find(component.name);
                GoSomewhere go = new GoSomewhere();
                go.m_destination = goalObject.transform.position;
                m_myActions.m_list.Add(go);

                PickUpSomething pickup = new PickUpSomething();
                pickup.m_targetObject = goalObject;
                m_myActions.m_list.Add(pickup);
            }
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
