using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecipeDropdown : MonoBehaviour {

    CraftingSystem myCraftingSystem;
    Dropdown myDropdown;
    // Use this for initialization
    void Start () {
        myDropdown = GetComponent<Dropdown>();
        myDropdown.ClearOptions();
        myCraftingSystem = FindObjectOfType<CraftingSystem>();
        List<string> recipeNames = new List<string>();
        foreach(CraftingRecipe recipe in myCraftingSystem.m_recipes)
        {
            string recipeNameAndInfo = recipe.GetName();
//             recipeNameAndInfo += " (";
//             foreach(ComponentAndCount compCount in recipe.m_craftingComponents)
//             {
//                 recipeNameAndInfo += compCount.m_component.m_craftingName + "[" + compCount.m_count + "], ";
//             }
//             recipeNameAndInfo += " )";
            
            recipeNames.Add(recipeNameAndInfo);
        }
        myDropdown.AddOptions(recipeNames);
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
