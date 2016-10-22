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
    private CraftingAIGlobals m_globals;
    private List<RecipeNode> m_recipesForToolsForCurrentPlan = new List<RecipeNode>();
    

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystem>();
        m_myActions = GetComponent<ActionList>();
        
        MakePlan();
	}

    void MakePlan()
    {
        m_recipesForToolsForCurrentPlan.Clear();
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
        //TODO check for items in inventory
        foreach(ComponentAndCount componentCount in recipe.m_recipe.m_craftingComponents)
        { 
            CraftingRecipe newRecipe = m_craftingSystem.GetRecipe(componentCount.component.gameObject);

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
                if(componentCount.type == MaterialType.EnemyDrop)
                {
                    for (int i = 0; i < componentCount.count; ++i)
                    {
                        m_myActions.m_list.Add(new KillEnemy(componentCount.component));
                    }
                    return;
                }

                // HACK Ideally, use vision or something, but probably not.
                string craftingName = componentCount.component.m_craftingName;
                GameObject goalObj = CraftingAIGlobals.GetClosest(componentCount.component, gameObject);
                if (goalObj == null)
                {
                    Debug.Log("Crafting component not found");
                    return; // no plan for getting this because it doesn't exist :(
                }

                // TODO choose the closest object instead
                for (int i = 0; i < componentCount.count; ++i)
                {
                    GetSomething get = new GetSomething(componentCount.component);
                    m_myActions.m_list.Add(get);
                }
//                 GoSomewhere go = new GoSomewhere();
//                 go.m_destination = goalObject.transform.position;
//                 m_myActions.m_list.Add(go);
// 
//                 PickUpSomething pickup = new PickUpSomething();
//                 pickup.m_targetObject = goalObject;
//                 m_myActions.m_list.Add(pickup);
            }
        }

    }

	// Update is called once per frame
	void Update () {
	
	}
}
