using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class RecipeNode
{
    public CraftingRecipe m_recipe;
    // lack of pointers/references makes me sad
    public List<int> m_requiredRecipeIndexes = new List<int>();
}

public class PlaningAI : MonoBehaviour {

    public List<RecipeNode> m_recipeNodes = new List<RecipeNode>();
    public List<int> m_reusableIndexes = new List<int>();
    public RecipeNode GetNewRecipeNode(ref int index)
    {
        if (m_reusableIndexes.Count == 0)
        {
            m_recipeNodes.Add(new RecipeNode());
            index = m_recipeNodes.Count - 1;
            return m_recipeNodes[m_recipeNodes.Count - 1];
        }
        else
        {
            int i = m_reusableIndexes.Count - 1;
            RecipeNode node = m_recipeNodes[m_reusableIndexes[i]];
            index = m_reusableIndexes[i];
            m_reusableIndexes.RemoveAt(i);
            return node;
        }
    }


    public GameObject m_objectivePrefab;
    public RecipeNode m_goalRootNode;
    private CraftingSystem m_craftingSystem;
    private ActionList m_myActions;
    

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystem>();
        m_myActions = GetComponent<ActionList>();
        MakePlan();
	}

    void MakePlan()
    {
        RecipeNode m_goalRecipe = new RecipeNode();
        m_goalRecipe.m_recipe = m_craftingSystem.GetRecipe(m_objectivePrefab);
        ExtendRecipeChain(m_goalRecipe);
        m_goalRootNode = m_goalRecipe;

        // HACK copy paste
        GoSomewhere go = new GoSomewhere();
        go.m_destination = m_craftingSystem.gameObject.transform.position;
        m_myActions.m_list.Add(go); // go to crafting table

        CraftSomething craft = new CraftSomething();
        craft.m_recipe = m_goalRecipe.m_recipe;
        craft.m_craftingSystem = m_craftingSystem;
        m_myActions.m_list.Add(craft); // craft the thing

        // Stop
        m_myActions.m_list.Add(new Stop()); // stop moving

        // Drop
        m_myActions.m_list.Add(new Drop()); // drop what you made
    }
	
    void ExtendRecipeChain(RecipeNode recipe)
    {
        foreach(GameObject component in recipe.m_recipe.m_craftingComponents)
        { 
            CraftingRecipe newRecipe = m_craftingSystem.GetRecipe(component);

            if (newRecipe != null)
            {
                int recipeIndex = 0;
                RecipeNode newNode = GetNewRecipeNode(ref recipeIndex);
                newNode.m_recipe = newRecipe;
                recipe.m_requiredRecipeIndexes.Add(recipeIndex);

                ExtendRecipeChain( newNode );

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
