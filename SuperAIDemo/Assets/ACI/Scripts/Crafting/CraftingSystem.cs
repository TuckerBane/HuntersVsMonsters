using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MaterialType { Normal = 0, Tool, EnemyDrop };
[System.Serializable]
public class ComponentAndCount
{
    public CraftingComponent m_component;
    public int m_count = 1;
    public MaterialType m_type = MaterialType.Normal;
}

[System.Serializable]
public class CraftingRecipe
{
    public GameObject m_createdObjectPrefab;
    public List<ComponentAndCount> m_craftingComponents = new List<ComponentAndCount>();
    // optional recipe components
    public float m_time = 0.0f;
}


public class CraftingSystem : MonoBehaviour {

    public GameObject[] m_baseMaterials;
    public CraftingRecipe[] m_recipes;
    public int m_recipeToMake;

    void PlayerUseObject(GameObject player)
    {
        
        Inventory playersInventory = player.GetComponent<Inventory>();
        GameObject craftingResult = TryToCraft(playersInventory, m_recipeToMake);
        if (craftingResult == null) // crafting failed
        {
            Debug.Log("Crafting attempt failed");
            ++m_recipeToMake;
            m_recipeToMake = m_recipeToMake % m_recipes.Length; // loop through recipes
            return;
        }
        playersInventory.PlaceObject(craftingResult);
    }
    GameObject TryToCraft(Inventory materialSource, int recipeIndex)
    {
        return TryToCraft(materialSource, m_recipes[recipeIndex]);
    }

    public GameObject TryToCraft(Inventory materialSource, CraftingRecipe recipe)
    {
        List<int> indexesofMaterialsToExpend = new List<int>();

        for (int i = 0; i < recipe.m_craftingComponents.Count; ++i)
        {
            GameObject component = recipe.m_craftingComponents[i].m_component.gameObject;
            if (materialSource.CountOf(component.GetComponent<CraftingComponent>()) < recipe.m_craftingComponents[i].m_count)
                return null; // material missing, could not craft
        }
        // HACK make this less super slow
        for (int i = 0; i < recipe.m_craftingComponents.Count; ++i)
        {
            if (recipe.m_craftingComponents[i].m_type == MaterialType.Tool)
                continue; // don't delete tools

            for (int j = 0; j < recipe.m_craftingComponents[i].m_count; ++j)
            {
                GameObject component = recipe.m_craftingComponents[i].m_component.gameObject;
                materialSource.DeleteFromInventory(component.GetComponent<CraftingComponent>());
            }
        }


        return Instantiate(recipe.m_createdObjectPrefab);
    }

    public CraftingRecipe GetBestRecipe(GameObject recipeTarget)
    {
        List<CraftingRecipe> recipes = GetRecipes(recipeTarget);
        if (recipes.Count == 0)
            return null;
        CraftingRecipe bestRecipe = recipes[0];
        foreach(CraftingRecipe recipe in recipes) // heuristic test
        {
            if (recipe.m_craftingComponents.Count < bestRecipe.m_craftingComponents.Count ||
                (
                (recipe.m_craftingComponents.Count == bestRecipe.m_craftingComponents.Count) && recipe.m_time < bestRecipe.m_time)
                )
            {
                bestRecipe = recipe;
            }
        }
        return bestRecipe;
    }

    public List<CraftingRecipe> GetRecipes(GameObject recipeTarget)
    {
        CraftingComponent target = recipeTarget.GetComponent<CraftingComponent>();
        List<CraftingRecipe> recipes = new List<CraftingRecipe>();
        foreach (CraftingRecipe rec in m_recipes)
        {
            if (target.name == rec.m_createdObjectPrefab.GetComponent<CraftingComponent>().name)
            {
                recipes.Add(rec);
            }
        }

        return recipes;
    }

        // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
