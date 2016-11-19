using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingSystemTerminal : MonoBehaviour {

    private CraftingSystem m_craftingSystemPrefab;
    public float m_effectSpawnHeight = 7.0f;
    public GameObject[] m_baseMaterials
    {
        get { return m_craftingSystemPrefab.m_baseMaterials; }
    }
    public CraftingRecipe[] m_recipes
    {
        get { return m_craftingSystemPrefab.m_recipes; }
    }
    public int m_recipeToMake
    {
        get { return m_craftingSystemPrefab.m_recipeToMake; }
    }
    public string m_missingComponentErrorMessage
    {
        get { return m_craftingSystemPrefab.m_missingComponentErrorMessage; }
    }

    // Use this for initialization
    void Start () {
        GameObject craftingSystem = UIHelpers.GetAllPrefabsWithComponent<CraftingSystem>()[0];
        m_craftingSystemPrefab = craftingSystem.GetComponent<CraftingSystem>();
    }

    void PlayerUseObject(GameObject player)
    {
        m_craftingSystemPrefab.PlayerUseObject(player);
    }

    public void OnCraftingSucess(GameObject crafted)
    {
        GameObject effectIJustMade = (GameObject)Instantiate(m_craftingSystemPrefab.m_successEffect, transform.position, transform.rotation);
        effectIJustMade.transform.position += Vector3.up * m_effectSpawnHeight;

        effectIJustMade = (GameObject)Instantiate(crafted.GetComponent<CraftingComponent>().GetIconPrefab(), transform.position, transform.rotation);
        effectIJustMade.transform.position += Vector3.up * m_effectSpawnHeight;
    }
    public void OnCraftingFailure()
    {
        GameObject effectIJustMade = (GameObject)Instantiate(m_craftingSystemPrefab.m_failureEffect, transform.position, transform.rotation);
        effectIJustMade.transform.localPosition += Vector3.up * m_effectSpawnHeight;
    }

    public GameObject TryToCraft(Inventory materialSource, int recipeIndex, bool missingComponentErrorMessage = false)
    {
        GameObject crafted = m_craftingSystemPrefab.TryToCraft(materialSource, recipeIndex, missingComponentErrorMessage);
        if (crafted)
            OnCraftingSucess(crafted);
        else
            OnCraftingFailure();
        return crafted;
    }

    public GameObject TryToCraft(Inventory materialSource, CraftingRecipe recipe,
        bool missingComponentErrorMessage = false)
    {
        GameObject crafted = m_craftingSystemPrefab.TryToCraft(materialSource, recipe, missingComponentErrorMessage);
        if (crafted)
            OnCraftingSucess(crafted);
        else
            OnCraftingFailure();
        return crafted;
    }

    public CraftingRecipe GetBestRecipe(GameObject recipeTarget)
    {
        return m_craftingSystemPrefab.GetBestRecipe(recipeTarget);
    }

    public List<CraftingRecipe> GetRecipes(GameObject recipeTarget)
    {
        return m_craftingSystemPrefab.GetRecipes(recipeTarget);
    }

    public void AddRecipe(CraftingRecipe recipe)
    {
        m_craftingSystemPrefab.AddRecipe(recipe);
    }

    public int RemoveRecipe(int index)
    {
        return m_craftingSystemPrefab.RemoveRecipe(index);
    }
    public int RemoveRecipe(CraftingRecipe recipe)
    {
        return m_craftingSystemPrefab.RemoveRecipe(recipe);
    }
}
