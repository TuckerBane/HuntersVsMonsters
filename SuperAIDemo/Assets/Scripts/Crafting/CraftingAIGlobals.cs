using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingAIGlobals : MonoBehaviour {
    public static Dictionary<string, List<CraftingComponent>> m_craftingComponents = new Dictionary<string, List<CraftingComponent>>();
    public GameObject m_defualtIconPrefab;
    public static void RegisterCraftingComponent( CraftingComponent comp )
    {
        
        if (!m_craftingComponents.ContainsKey(comp.m_craftingName))
            m_craftingComponents.Add(comp.m_craftingName, new List<CraftingComponent>() );
        m_craftingComponents[comp.m_craftingName].Add(comp);
    }

    public static void DeregisterCraftingComponent( CraftingComponent comp)
    {
        m_craftingComponents[comp.m_craftingName].Remove(comp);
    }

    public static GameObject GetClosest(CraftingComponent comp, GameObject you)
    {
        //ToDo get the closest
        return m_craftingComponents[comp.m_craftingName][0].gameObject;
    }

}
