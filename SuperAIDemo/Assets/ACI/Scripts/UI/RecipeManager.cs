

//Assets/Editor/SearchForComponents.cs
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecipeManager : EditorWindow
{
    GameObject craftingTablePrefab;
    // shouldn't need to be here
    List<string> prefabsWithCraftingComponentsPathsList = new List<string>();
    List<GameObject> prefabsWithCraftingComponentsList = new List<GameObject>();
    string[] allPrefabs;
    static List<CraftingRecipe> m_undoList = new List<CraftingRecipe>();

    [MenuItem("CRAFTING/Recipe Manager")]
    static void Init()
    {
        RecipeManager window = (RecipeManager)EditorWindow.GetWindow(typeof(RecipeManager));
        window.Show();
        window.position = new Rect(20, 80, 400, 300);
        
    }

    void OnGUI()
    {
        allPrefabs = UIHelpers.GetAllPrefabs();
        foreach (string prefab in allPrefabs)
        {
            GameObject obj = (GameObject)AssetDatabase.LoadMainAssetAtPath(prefab);
            if (obj.GetComponent<CraftingSystem>())
            {
                craftingTablePrefab = obj;
            }
        }

        List<CraftingRecipe> toRemove = new List<CraftingRecipe>();
        CraftingSystem craftingSystemPrefab = craftingTablePrefab.GetComponent<CraftingSystem>();
        if (m_undoList.Count != 0)
        {
            int lastElement = m_undoList.Count - 1;
            if (GUILayout.Button("Undo deletion of " + m_undoList[lastElement].GetName() + " recipe") )
            {
                craftingSystemPrefab.AddRecipe(m_undoList[lastElement]);
                m_undoList.RemoveAt(lastElement);
            }
         }
        foreach (CraftingRecipe recipe in craftingSystemPrefab.m_recipes)
        {
            string recipeName = recipe.m_createdObjectPrefab.GetComponent<CraftingComponent>().m_craftingName;

            if (GUILayout.Button("Delete " + recipeName + " recipe") )
            {
                toRemove.Add(recipe);
            }
        }

        foreach(CraftingRecipe recipe in toRemove)
        {
            m_undoList.Add(recipe);
            int removedIndex = craftingSystemPrefab.RemoveRecipe(recipe);
        }

    }


}

