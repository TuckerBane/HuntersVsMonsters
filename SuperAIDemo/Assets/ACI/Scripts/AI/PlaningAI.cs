using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class RecipeNode
{
    public CraftingRecipe m_recipe;
    // lack of pointers/references makes me sad
    public List<int> m_requiredRecipeIndexes = new List<int>();
    public List<CraftingComponent> m_requiredBasicMaterials = new List<CraftingComponent>();
}

public class InvetoryItem
{
    public string m_craftingName;
    public int m_count;
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
    private GraphDrawer m_graphDrawer;
    private Inventory m_myInventory;
    // Easy way to track inventory usage during plan making
    private Inventory m_imaginaryInventory;
    
	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystem>();
        m_graphDrawer = GetComponent<GraphDrawer>();
        m_myActions = GetComponent<ActionList>();
        m_myInventory = GetComponent<Inventory>();
        
        MakePlan();
        MakePlanGraph(m_goalRootNode);
	}


    // TODO don't get things we already have
    void MakePlan()
    {

        m_imaginaryInventory = m_myInventory.DeepCopy();

        m_recipesForToolsForCurrentPlan.Clear();
        RecipeNode m_goalRecipe = new RecipeNode();
        CraftingRecipe bestRecipe = GetBestRecipe(m_objectivePrefab.GetComponent<CraftingComponent>());
        m_goalRecipe.m_recipe = GetBestRecipe(m_objectivePrefab.GetComponent<CraftingComponent>());
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
	
    CraftingRecipe GetBestRecipe(CraftingComponent component)
    {
        int smallestNumberOfRequirments = int.MaxValue;
        int smallestNumberOfItemsUsed = int.MaxValue;
        CraftingRecipe bestRecipe = null;

        foreach(CraftingRecipe rec in m_craftingSystem.m_recipes)
        {
            // possible recipe
            if (rec.m_createdObjectPrefab.GetComponent<CraftingComponent>().Equals(component) )
            {
                int itemsNeeded = 0;
                int itemsUsed = 0;
                foreach(ComponentAndCount compCount in rec.m_craftingComponents)
                {
                    int countInInventory = m_imaginaryInventory.CountOf(compCount.m_component);
                    if (compCount.m_count > countInInventory) // we still need more of this thing
                    {
                        itemsNeeded += compCount.m_count - countInInventory;
                        itemsUsed += countInInventory;
                    }
                    else
                    {
                        itemsUsed += compCount.m_count;
                    }
                }

                if (itemsNeeded < smallestNumberOfRequirments || itemsNeeded == smallestNumberOfRequirments && itemsUsed < smallestNumberOfItemsUsed) // new best recipe
                {
                    smallestNumberOfRequirments = itemsNeeded;
                    smallestNumberOfItemsUsed = itemsUsed;
                    bestRecipe = rec;
                }

            }
        }

        return bestRecipe;
    }

    void ComsumeResources(ComponentAndCount componentCount)
    {
        if (componentCount.m_type == MaterialType.Tool) // don't consume tools
            return;
        int countInInventory = m_imaginaryInventory.CountOf(componentCount.m_component);
        int numberToConsume = System.Math.Min(countInInventory, componentCount.m_count);
        for (int i = 0; i < numberToConsume; ++i)
        {
            m_imaginaryInventory.DeleteFromInventory(componentCount.m_component);
        }

    }
    void ExtendRecipeChain(RecipeNode recipe)
    {
        //TODO check for items in inventory
        foreach(ComponentAndCount componentCount in recipe.m_recipe.m_craftingComponents)
        {
            int howManyMoreNeeded = componentCount.m_count - m_imaginaryInventory.CountOf(componentCount.m_component);
            howManyMoreNeeded = System.Math.Max(0, howManyMoreNeeded);
            CraftingRecipe newRecipe = GetBestRecipe(componentCount.m_component);

            if (newRecipe != null) // if there's a recipe
            {
                    int countInInventory = m_imaginaryInventory.CountOf(componentCount.m_component);
                    // get more if we need them
                    for (int i = 0; i < howManyMoreNeeded; ++i)
                    {
                        int recipeIndex = 0;
                        RecipeNode newNode = GetNewRecipeNode(ref recipeIndex);
                        newNode.m_recipe = newRecipe;
                        recipe.m_requiredRecipeIndexes.Add(recipeIndex);

                        ExtendRecipeChain(newNode);
                        // add use recipe to action list
                        GoSomewhere go = new GoSomewhere();
                        go.m_destination = m_craftingSystem.gameObject.transform.position;
                        m_myActions.m_list.Add(go); // go to crafting table

                        CraftSomething craft = new CraftSomething();
                        craft.m_recipe = newRecipe;
                        craft.m_craftingSystem = m_craftingSystem;
                        m_myActions.m_list.Add(craft); // craft the thing 
                    }
                    ComsumeResources(componentCount);
            }
            else // if there isn't a recipe
            {
                // HACK do something special for enemy kill missions
                recipe.m_requiredBasicMaterials.Add(componentCount.m_component);
                if(componentCount.m_type == MaterialType.EnemyDrop)
                {
                    for (int i = 0; i < howManyMoreNeeded; ++i)
                    {
                        m_myActions.m_list.Add(new KillEnemy(componentCount.m_component));
                    }
                    continue;
                }

                // HACK Ideally, use vision or something, but probably not.
                string craftingName = componentCount.m_component.m_craftingName;
                GameObject goalObj = CraftingAIGlobals.GetClosest(componentCount.m_component, gameObject);
                if (goalObj == null)
                {
                    Debug.Log("Crafting component not found");
                    return; // no plan for getting this because it doesn't exist :(
                }

                for (int i = 0; i < howManyMoreNeeded; ++i)
                {
                    GetSomething get = new GetSomething(componentCount.m_component);
                    m_myActions.m_list.Add(get);
                }
                //HACK duplicated from the above case
                ComsumeResources(componentCount);
            }
        }

    }

	// Update is called once per frame
	void Update () {
	
	}

    GameObject GetNodeIcon(RecipeNode node)
    {
        return node.m_recipe.m_createdObjectPrefab.GetComponent<CraftingComponent>().GetIconPrefab();
    }

    void MakePlanGraph(RecipeNode goalRootNode)
    {
        Vector3 currentNodePosition = new Vector3(0, 10, 0);
        GameObject startingIcon = m_graphDrawer.PutSymbolAtPos(GetNodeIcon(goalRootNode), currentNodePosition);
        RecursiveGraphDraw(startingIcon, currentNodePosition, goalRootNode);
    }

    void RecursiveGraphDraw(GameObject prevIcon, Vector3 prevIconPosition, RecipeNode prevNode, float nodeSeperation = 10)
    {
        float numNodes = prevNode.m_requiredRecipeIndexes.Count + prevNode.m_requiredBasicMaterials.Count;
        Vector3 newPosition = prevIconPosition + new Vector3( -(numNodes - 0.5f) * nodeSeperation / 2.0f, 10, 0);


        foreach(int nodeIndex in prevNode.m_requiredRecipeIndexes)
        {
            // HACK a little copy-paste like
            RecipeNode currentNode = m_recipeNodes[nodeIndex];
            GameObject newIconPrefab = GetNodeIcon(currentNode);
            GameObject newIcon = m_graphDrawer.PutSymbolAtPos(newIconPrefab, newPosition);
            m_graphDrawer.DrawArrowBetweenThings(newIcon, prevIcon);
            RecursiveGraphDraw(newIcon, newPosition, currentNode, nodeSeperation / 2);
            newPosition += new Vector3(nodeSeperation, 0, 0);
        }

        foreach(CraftingComponent comp in prevNode.m_requiredBasicMaterials)
        {
            GameObject newIconPrefab = comp.GetIconPrefab();
            GameObject newIcon = m_graphDrawer.PutSymbolAtPos(newIconPrefab, newPosition);
            m_graphDrawer.DrawArrowBetweenThings(newIcon, prevIcon);
            //RecursiveGraphDraw(newIcon, newPosition, currentNode, nodeSeperation / 2);
            newPosition += new Vector3(nodeSeperation, 0, 0);
        }
       

    }

}
