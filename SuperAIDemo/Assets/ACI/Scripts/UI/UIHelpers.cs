using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class UIHelpers : MonoBehaviour {

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
}
