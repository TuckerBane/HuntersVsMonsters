using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CraftingUIManager : MonoBehaviour {

    public Dropdown m_recipeSelector;
    public Inventory m_playersInventory;
    public Text m_errorReadout;

    private CraftingSystem m_craftingSystem;
    public void TryToCraft()
    {
        int recipeIndex = m_recipeSelector.value;
        string missingComponents =  " ";
        CraftingRecipe recp = m_craftingSystem.m_recipes[recipeIndex];
        GameObject crafted = m_craftingSystem.TryToCraft(m_playersInventory, recp, missingComponents);
        if (crafted)
        {
            m_playersInventory.AddToInventory(crafted);
        }
        else
        {
            m_errorReadout.text = missingComponents;
        }
    }

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystem>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
