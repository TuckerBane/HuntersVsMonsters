

//Assets/Editor/SearchForComponents.cs
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class RecipeMaker : EditorWindow
{
    [MenuItem("CRAFTING/RecipeMaker")]
    static void Init()
    {
        RecipeMaker window = (RecipeMaker)EditorWindow.GetWindow(typeof(RecipeMaker));
        window.Show();
        window.position = new Rect(20, 80, 400, 300);
    }


    string[] modes = new string[] { "Select item to be created", "Select materials it is made of" };

    List<string> listResult;
    int editorMode, editorModeOld;
    MonoScript targetComponent, lastChecked;

    string componentName = "";
    Vector2 scroll;

    GameObject objectToBeCreated;
    GameObject craftingComponent1;
    // parallel arrays
    string[] prefabsWithCraftingComponentsPaths;
    GameObject[] prefabsWithCraftingComponents;
    int selectedCraftingPrefabIndex = 0;

    // recipe stuff
    CraftingRecipe recipeInProgress = new CraftingRecipe();
    int numberOfMaterialNeeded = 1;
    MaterialType matType;


    // shouldn't need to be here
    List<string> prefabsWithCraftingComponentsPathsList = new List<string>();
    List<GameObject> prefabsWithCraftingComponentsList = new List<GameObject>();
    string[] allPrefabs;

    public static string[] GetAllPrefabs()
    {
        string[] temp = AssetDatabase.GetAllAssetPaths();
        List<string> result = new List<string>();
        foreach (string s in temp)
        {
            if (s.Contains(".prefab")) result.Add(s);
        }
        return result.ToArray();
    }

    public static string RemovePathAndExtention(string s)
    {
        int startIndex = s.LastIndexOf('/');
        int endIndex = s.IndexOf('.');
        return s.Substring(startIndex + 1, endIndex - startIndex - 1);
    }

    void OnGUI()
    {
        GUILayout.Space(3);
        int oldValue = GUI.skin.window.padding.bottom;
        GUI.skin.window.padding.bottom = -20;
           Rect windowRect = GUILayoutUtility.GetRect(1, 17);
        windowRect.x += 4;
        windowRect.width -= 7;
           editorMode = GUI.SelectionGrid(windowRect, editorMode, modes, 2, "Window");
        GUI.skin.window.padding.bottom = oldValue;

        if (editorModeOld != editorMode)
        {
            editorModeOld = editorMode;
            listResult = new List<string>();
            componentName = targetComponent == null ? "" : targetComponent.name;
            lastChecked = null;
        }

        switch (editorMode)
        {
            case 0: // Select item to be created

                // Get all objects with crafting components
                prefabsWithCraftingComponentsPathsList = new List<string>();
                prefabsWithCraftingComponentsList = new List<GameObject>();
                allPrefabs = GetAllPrefabs();
               
                foreach (string prefab in allPrefabs)
                {
                    GameObject obj = (GameObject) AssetDatabase.LoadMainAssetAtPath(prefab);
                    if (obj.GetComponent<CraftingComponent>())
                    {
                        prefabsWithCraftingComponentsPathsList.Add( RemovePathAndExtention(prefab) );
                        prefabsWithCraftingComponentsList.Add(obj);
                    }
                }
                prefabsWithCraftingComponentsPaths = prefabsWithCraftingComponentsPathsList.ToArray();
                prefabsWithCraftingComponents = prefabsWithCraftingComponentsList.ToArray();

                selectedCraftingPrefabIndex = EditorGUILayout.Popup("Selected crafting item", selectedCraftingPrefabIndex, prefabsWithCraftingComponentsPaths);
                numberOfMaterialNeeded = EditorGUILayout.IntField("number needed" , numberOfMaterialNeeded);
                if (numberOfMaterialNeeded <= 0)
                    numberOfMaterialNeeded = 1;

                matType = (MaterialType) EditorGUILayout.EnumPopup("Material Type", matType);
                if (GUILayout.Button("Add selected item to recipe requirements", GUILayout.Width(position.width / 2 - 10)))
                {
                    ComponentAndCount newRequirement = new ComponentAndCount();
                    //TODO add in time and type. Shouldn't be hard.
                    newRequirement.m_component = prefabsWithCraftingComponents[selectedCraftingPrefabIndex].GetComponent<CraftingComponent>();
                    recipeInProgress.m_craftingComponents.Add(newRequirement);
                }
                if (GUILayout.Button("Make selected item recipe target", GUILayout.Width(position.width / 2 - 10)))
                {
                    recipeInProgress.m_createdObjectPrefab = prefabsWithCraftingComponents[selectedCraftingPrefabIndex];
                }

                if (GUILayout.Button("Export recipe", GUILayout.Width(position.width / 2 - 10)))
                {
                    FindObjectOfType<CraftingSystem>().AddRecipe(recipeInProgress);
                    recipeInProgress = new CraftingRecipe();
                }


                scroll = GUILayout.BeginScrollView(scroll);
                foreach (string s in prefabsWithCraftingComponentsPathsList)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(s, GUILayout.Width(position.width / 2));
                    if (GUILayout.Button("Add to recipe", GUILayout.Width(position.width / 2 - 10)))
                    {
                        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(s);
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();

//                 if (targetComponent != lastChecked)
//                 {
//                     lastChecked = targetComponent;
//                     componentName = targetComponent.name;
//                     AssetDatabase.SaveAssets();
//                     string targetPath = AssetDatabase.GetAssetPath(targetComponent);
//                     listResult = new List<string>();
//                     foreach (string prefab in allPrefabs)
//                     {
//                         string[] single = new string[] { prefab };
//                         string[] dependencies = AssetDatabase.GetDependencies(single);
//                         foreach (string dependedAsset in dependencies)
//                         {
//                             if (dependedAsset == targetPath)
//                             {
//                                 listResult.Add(prefab);
//                             }
//                         }
//                     }
//                 }
                break;
            case 1:
//                 if (GUILayout.Button("Search!"))
//                 {
//                     string[] allPrefabs = GetAllPrefabs();
//                     listResult = new List<string>();
//                     foreach (string prefab in allPrefabs)
//                     {
//                         UnityEngine.Object o = AssetDatabase.LoadMainAssetAtPath(prefab);
//                         GameObject go;
//                         try
//                         {
//                             go = (GameObject)o;
//                             Component[] components = go.GetComponentsInChildren<Component>(true);
//                             foreach (Component c in components)
//                             {
//                                 if (c == null)
//                                 {
//                                     listResult.Add(prefab);
//                                 }
//                             }
//                         }
//                         catch
//                         {
//                             Debug.Log("For some reason, prefab " + prefab + " won't cast to GameObject");
// 
//                         }
//                     }
//                 }
                break;
        }

//         if (listResult != null)
//         {
//             if (listResult.Count == 0)
//             {
//                 GUILayout.Label(editorMode == 0 ? (componentName == "" ? "Choose a component" : "No prefabs use component " + componentName) : ("No prefabs have missing components!\nClick Search to check again"));
//             }
//             else
//             {
//                 GUILayout.Label(editorMode == 0 ? ("The following prefabs use component " + componentName + ":") : ("The following prefabs have missing components:"));
//                 scroll = GUILayout.BeginScrollView(scroll);
//                 foreach (string s in listResult)
//                 {
//                     GUILayout.BeginHorizontal();
//                     GUILayout.Label(s, GUILayout.Width(position.width / 2));
//                     if (GUILayout.Button("Select", GUILayout.Width(position.width / 2 - 10)))
//                     {
//                         Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(s);
//                     }
//                     GUILayout.EndHorizontal();
//                 }
//                 GUILayout.EndScrollView();
//             }
//         }
    }


}

