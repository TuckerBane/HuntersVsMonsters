

//Assets/Editor/SearchForComponents.cs
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecipeMaker : EditorWindow
{
    [MenuItem("CRAFTING/Recipe Maker")]
    static void Init()
    {
        RecipeMaker window = (RecipeMaker)EditorWindow.GetWindow(typeof(RecipeMaker));
        window.Show();
        window.position = new Rect(20, 80, 400, 300);
    }
    // allows scrolling
    Vector2 scroll;

    GameObject objectToBeCreated;
    GameObject craftingComponent1;
    // parallel arrays
    string[] prefabsWithCraftingComponentsPaths;
    GameObject[] prefabsWithCraftingComponents;
    int selectedCraftingPrefabIndex = 0;
    int recipeTargetIndex;

    // recipe stuff
    CraftingRecipe recipeInProgress = new CraftingRecipe();
    int numberOfMaterialNeeded = 1;
    MaterialType matType;
    GameObject craftingTablePrefab;

    // UI
    string feedback;

    // shouldn't need to be here
    List<string> prefabsWithCraftingComponentsPathsList = new List<string>();
    List<GameObject> prefabsWithCraftingComponentsList = new List<GameObject>();
    string[] allPrefabs;

    void CraftingSelectionButton(CraftingComponent comp)
    {
        if (GUILayout.Button(comp.m_craftingName))
        {
            Selection.activeGameObject = comp.gameObject;
        }
    }

    void OnGUI()
    {

        // Get all objects with crafting components
        prefabsWithCraftingComponentsPathsList = new List<string>();
        prefabsWithCraftingComponentsList = new List<GameObject>();
        allPrefabs = UIHelpers.GetAllPrefabs();

        foreach (string prefab in allPrefabs)
        {
            GameObject obj = (GameObject)AssetDatabase.LoadMainAssetAtPath(prefab);
            if (obj.GetComponent<CraftingComponent>())
            {
                prefabsWithCraftingComponentsPathsList.Add(UIHelpers.RemovePathAndExtention(prefab));
                prefabsWithCraftingComponentsList.Add(obj);
            }
            else if (obj.GetComponent<CraftingSystem>())
            {
                craftingTablePrefab = obj;
            }
       
        }
        prefabsWithCraftingComponentsPaths = prefabsWithCraftingComponentsPathsList.ToArray();
        prefabsWithCraftingComponents = prefabsWithCraftingComponentsList.ToArray();

        selectedCraftingPrefabIndex = EditorGUILayout.Popup("Selected crafting item", selectedCraftingPrefabIndex, prefabsWithCraftingComponentsPaths);
        numberOfMaterialNeeded = EditorGUILayout.IntField("Number needed", numberOfMaterialNeeded);
        if (numberOfMaterialNeeded <= 0)
        {
            feedback = "The number of a material that is needed must be at lest 1";
            numberOfMaterialNeeded = 1;
        }

        matType = (MaterialType)EditorGUILayout.EnumPopup("Material Type", matType);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add to recipe requirements", GUILayout.Width(position.width / 2 - 10)))
        {
            ComponentAndCount newRequirement = new ComponentAndCount();
            newRequirement.m_component = prefabsWithCraftingComponents[selectedCraftingPrefabIndex].GetComponent<CraftingComponent>();
            newRequirement.m_count = numberOfMaterialNeeded;
            newRequirement.m_type = matType;
            recipeInProgress.m_craftingComponents.Add(newRequirement);
            feedback = "Recipe requirement added";

            if (prefabsWithCraftingComponents[recipeTargetIndex].GetComponent<CraftingComponent>()
                == newRequirement.m_component)
                feedback += "\nAre you sure you want an object to be made out of itself?";
        }

        if (GUILayout.Button("Export recipe", GUILayout.Width(position.width / 2 - 10)))
        {
            recipeInProgress.m_createdObjectPrefab = prefabsWithCraftingComponents[recipeTargetIndex];
            if (recipeInProgress.IsValid())
            {
                craftingTablePrefab.GetComponent<CraftingSystem>().AddRecipe(recipeInProgress);
                recipeInProgress = new CraftingRecipe();
                feedback = "Recipe successfully exported";
            }
            else
            {
                Debug.Log("recipes must have a target object and at lest one required material");
                feedback = "Recipes must have at lest one required material";
            }
        }

        GUILayout.EndHorizontal();

        recipeTargetIndex = EditorGUILayout.Popup("Current recipe target", recipeTargetIndex, prefabsWithCraftingComponentsPaths);

        if (recipeInProgress.m_createdObjectPrefab)
        {
            if (GUILayout.Button(recipeInProgress.m_createdObjectPrefab.GetComponent<CraftingComponent>().m_craftingName))
            {
                Selection.activeGameObject = recipeInProgress.m_createdObjectPrefab;
            }
        }

        scroll = GUILayout.BeginScrollView(scroll);
        List<ComponentAndCount> toRemove = new List<ComponentAndCount>();
        GUILayout.Label("Recipe required materials");
        foreach (ComponentAndCount compCount in recipeInProgress.m_craftingComponents)
        {
            GUILayout.BeginHorizontal();
            CraftingSelectionButton(compCount.m_component);
            compCount.m_count = EditorGUILayout.IntField(compCount.m_count);
            compCount.m_type = (MaterialType)EditorGUILayout.EnumPopup("", compCount.m_type);
           // GUILayout.Label(compCount.m_type.ToString());
            if (GUILayout.Button("Remove from recipe", GUILayout.Width(position.width / 2 - 10)))
            {
                toRemove.Add(compCount);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();

        foreach (ComponentAndCount compCount in toRemove)
        {
            recipeInProgress.m_craftingComponents.Remove(compCount);
        }
        GUILayout.Label("");
        GUILayout.Label("");
        GUILayout.Label(feedback);


    }


}

