using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CraftingUIManager : MonoBehaviour {

    public Dropdown m_recipeSelector;
    public Dropdown m_AIObjectiveSelector;
    public PlaningAI m_AIBot;
    public Inventory m_playersInventory;
    public Text m_errorReadout;

    private CraftingSystemTerminal m_craftingSystem;
    public void TryToCraft()
    {
        int recipeIndex = m_recipeSelector.value;

        GameObject crafted = m_craftingSystem.TryToCraft(m_playersInventory, recipeIndex, true);
        if (crafted)
        {
            m_playersInventory.AddToInventory(crafted);
        }
        else
        {
            m_errorReadout.text = m_craftingSystem.m_missingComponentErrorMessage;
        }
    }

    public void SetAIGoal()
    {
        int recipeIndex = m_AIObjectiveSelector.value;
        CraftingRecipe recp = m_craftingSystem.m_recipes[recipeIndex];
        m_AIBot.M_objectivePrefab = recp.m_createdObjectPrefab;
    }

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystemTerminal>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
