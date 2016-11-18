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
            recipeNames.Add(recipeNameAndInfo);
        }
        //TODO add the option to remove repeat recipes
        myDropdown.AddOptions(recipeNames);
        
    }
	
	// Update is called once per frame
	void Update () {
	    
	}
}
