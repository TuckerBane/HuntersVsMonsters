using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CraftingUIManager : MonoBehaviour {

    public Dropdown m_recipeSelector;
    public Dropdown m_AIObjectiveSelector;
    public Dropdown m_objectSpawnSelector;
    public PlaningAI m_AIBot;
    public Inventory m_playersInventory;
    public Text m_errorReadout;
    public Text m_inventoryReadout;
    public GameObject m_spawnEffect;

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

    public void SpawnSelected()
    {
        GameObject[] allOptions = UIHelpers.GetAllPrefabsWithComponent<CraftingComponent>();
        GameObject toSpawn = allOptions[m_objectSpawnSelector.value];
        GameObject spawned = Instantiate(toSpawn);
        FindObjectOfType<CharacterController>().gameObject.GetComponent<Inventory>().PlaceObject(spawned);
        if (m_spawnEffect)
        {
            GameObject effect = Instantiate(m_spawnEffect);
            effect.transform.position = spawned.transform.position;
            //FindObjectOfType<CharacterController>().gameObject.GetComponent<Inventory>().PlaceObject(m_spawnEffect);
        }
    }

	// Use this for initialization
	void Start () {
        m_craftingSystem = FindObjectOfType<CraftingSystemTerminal>();
	}
	
	// Update is called once per frame
	void Update () {
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        if (m_playersInventory == null || m_playersInventory.m_objectStore == null || m_playersInventory.m_objectStore.Count == 0)
            return;

        foreach(GameObject obj in m_playersInventory.m_objectStore)
        {
            string craftingName;
            if (obj.GetComponent<CraftingComponent>() == null)
            {
                Debug.Log(obj.name + " is not a crafting object, but is in an inventory");
                craftingName = obj.name;
            }
            else
                craftingName = obj.GetComponent<CraftingComponent>().m_craftingName;

            if (dictionary.ContainsKey(craftingName))
                ++dictionary[craftingName];
            else
                dictionary.Add(craftingName, 1);
        }

        string inventoryStuff = "";
        foreach ( KeyValuePair<string,int> pair in dictionary)
        {
            inventoryStuff += pair.Key +  "[" + pair.Value + "]\n";
        }

        m_inventoryReadout.text = inventoryStuff;
	}
}
