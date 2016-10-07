using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CraftingRecipe
{
    public GameObject m_createdObjectPrefab;
    public List<GameObject> m_craftingComponents;
    // optional recipe components
    public float m_time = 0.0f;
    public GameObject[] m_required_tools;
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
            GameObject component = recipe.m_craftingComponents[i];
            if (materialSource.CountOf(component.GetComponent<CraftingComponent>()) == 0)
                return null; // material missing, could not craft
        }
        // TODO make this less super slow
        for (int i = 0; i < recipe.m_craftingComponents.Count; ++i)
        {
            GameObject component = recipe.m_craftingComponents[i];
            materialSource.RemoveFromInventory(component.GetComponent<CraftingComponent>());
        }


        return Instantiate(recipe.m_createdObjectPrefab);
    }

    public CraftingRecipe GetRecipe(GameObject recipeTarget)
    {
        CraftingComponent target = recipeTarget.GetComponent<CraftingComponent>();
        foreach (CraftingRecipe rec in m_recipes)
        {
            if (target.name == rec.m_createdObjectPrefab.GetComponent<CraftingComponent>().name)
            {
                return rec;
            }
        }

        return null;
    }

        // Use this for initialization
        void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
