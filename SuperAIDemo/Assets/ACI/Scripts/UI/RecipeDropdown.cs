using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecipeDropdown : MonoBehaviour {

    public bool m_removeDuplicates = false;
    CraftingSystemTerminal m_myCraftingSystem;
    Dropdown m_myDropdown;
    // Use this for initialization
    void Start () {
        m_myDropdown = GetComponent<Dropdown>();
        m_myDropdown.ClearOptions();
        m_myCraftingSystem = FindObjectOfType<CraftingSystemTerminal>();
        List<string> recipeNames = new List<string>();
        foreach(CraftingRecipe recipe in m_myCraftingSystem.m_recipes)
        {
            string recipeName = recipe.GetName();
            if ( !(m_removeDuplicates && recipeNames.Contains(recipeName)) )
                recipeNames.Add(recipeName);
        }
        //TODO add the option to remove repeat recipes
        m_myDropdown.AddOptions(recipeNames);
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
