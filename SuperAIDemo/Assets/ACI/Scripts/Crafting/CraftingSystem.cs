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
    public bool IsValid()
    {
        return m_createdObjectPrefab != null && m_craftingComponents.Count != 0;
    }
    public string GetName()
    {
        if (!IsValid())
            return "An invalid recipe";
        return m_createdObjectPrefab.GetComponent<CraftingComponent>().m_craftingName;
    }
}


public class CraftingSystem : MonoBehaviour
{
    public GameObject m_successEffect;
    public GameObject m_failureEffect;
    public GameObject[] m_baseMaterials;
    public CraftingRecipe[] m_recipes;
    public int m_recipeToMake;
    public string m_missingComponentErrorMessage;
    public void PlayerUseObject(GameObject player)
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
    public GameObject TryToCraft(Inventory materialSource, int recipeIndex, bool missingComponentErrorMessage = false)
    {
        return TryToCraft(materialSource, m_recipes[recipeIndex], missingComponentErrorMessage);
    }

    public GameObject TryToCraft(Inventory materialSource, CraftingRecipe recipe,
        bool missingComponentErrorMessage = false)
    {
        if (materialSource == null)
        {
            Debug.Log("Crafting system given a null inventory");
            return null;
        }

        if (missingComponentErrorMessage)
            m_missingComponentErrorMessage = "";

        List<int> indexesofMaterialsToExpend = new List<int>();
        bool lateExit = false;
        for (int i = 0; i < recipe.m_craftingComponents.Count; ++i)
        {
            GameObject component = recipe.m_craftingComponents[i].m_component.gameObject;
            if (materialSource.CountOf(component.GetComponent<CraftingComponent>()) < recipe.m_craftingComponents[i].m_count)
            {
                if (!missingComponentErrorMessage)
                    return null; // material missing, could not craft
                lateExit = true;
                int amountNeeded = recipe.m_craftingComponents[i].m_count - materialSource.CountOf(component.GetComponent<CraftingComponent>());
                m_missingComponentErrorMessage += recipe.m_craftingComponents[i].m_component.m_craftingName + "[" + amountNeeded + "]\n";
            }
        }

        if (lateExit)
            return null;

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
        foreach (CraftingRecipe recipe in recipes) // heuristic test
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

    public void AddRecipe(CraftingRecipe recipe)
    {
        CraftingRecipe[] temp = m_recipes;
        m_recipes = new CraftingRecipe[temp.Length + 1];
        temp.CopyTo(m_recipes, 0);
        m_recipes[m_recipes.Length - 1] = recipe;
    }

    public int RemoveRecipe(int index)
    {
        return RemoveRecipe(m_recipes[index]);
    }
    public int RemoveRecipe(CraftingRecipe recipe)
    {
        int indexToRemove = 0;
        for (int i = 0; i < m_recipes.Length; ++i)
        {
            if (recipe == m_recipes[i])
                indexToRemove = i;
        }

        CraftingRecipe[] temp = m_recipes;
        m_recipes = new CraftingRecipe[temp.Length - 1];
        if (m_recipes.Length == 0)
            return -1;
        int recipesIndex = 0;
        for (int tempIndex = 0; tempIndex < temp.Length; ++tempIndex)
        {
            if (tempIndex != indexToRemove)
            {
                if (recipesIndex == m_recipes.Length)
                {
                    m_recipes = temp;
                    Debug.Log("Recipe to remove not found");
                    break;
                }
                m_recipes[recipesIndex] = temp[tempIndex];
                ++recipesIndex;
            }

        }
        return indexToRemove;
    }
}
