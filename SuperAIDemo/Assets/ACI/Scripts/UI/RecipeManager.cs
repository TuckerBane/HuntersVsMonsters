

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

        for (int i = 0; i < craftingSystemPrefab.m_recipes.Length; ++i)
        {
            CraftingRecipe recipe = craftingSystemPrefab.m_recipes[i];
            string recipeName = recipe.m_createdObjectPrefab.GetComponent<CraftingComponent>().m_craftingName;
            GUILayout.BeginHorizontal();

            GUILayout.Label(recipeName);

            if (GUILayout.Button("Move Up"))
            {
                if (i == 0)
                    continue;
                CraftingRecipe swappy = recipe;
                craftingSystemPrefab.m_recipes[i] = craftingSystemPrefab.m_recipes[i - 1];
                craftingSystemPrefab.m_recipes[i - 1] = swappy;
            }

            if (GUILayout.Button("Move Down"))
            {
                if (i == craftingSystemPrefab.m_recipes.Length - 1)
                    continue;
                CraftingRecipe swappy = recipe;
                craftingSystemPrefab.m_recipes[i] = craftingSystemPrefab.m_recipes[i + 1];
                craftingSystemPrefab.m_recipes[i + 1] = swappy;
            }


            if (GUILayout.Button("Delete " + recipeName + " recipe") )
            {
                toRemove.Add(recipe);
            }
            GUILayout.EndHorizontal();
        }

        foreach(CraftingRecipe recipe in toRemove)
        {
            m_undoList.Add(recipe);
            int removedIndex = craftingSystemPrefab.RemoveRecipe(recipe);
        }

    }


}

