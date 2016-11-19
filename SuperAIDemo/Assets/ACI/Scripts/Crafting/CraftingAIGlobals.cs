using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingAIGlobals : MonoBehaviour {
    public static Dictionary<string, List<CraftingComponent>> m_craftingComponents = new Dictionary<string, List<CraftingComponent>>();
    public static Dictionary<string, List<Enemy>> m_craftingCompNameToEnemy = new Dictionary<string, List<Enemy>>();
    // HACK is this something I want?
    //public static Dictionary<string, string> m_materialToEnemy;
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

    public static void RegisterEnemy(Enemy enemy)
    {
        //CraftingComponent
        // note what crafting component it provides, maybe?
        if (enemy.m_deathDropMaterialPrefab == null)
        {
            //HACK is enemies not dropping anything a problem?
            Debug.Log("Enemy does not have death drop");
            return;
        }

        if (!m_craftingCompNameToEnemy.ContainsKey(enemy.m_deathDropMaterialPrefab.m_craftingName))
            m_craftingCompNameToEnemy.Add(enemy.m_deathDropMaterialPrefab.m_craftingName, new List<Enemy>());
        m_craftingCompNameToEnemy[enemy.m_deathDropMaterialPrefab.m_craftingName].Add(enemy);
    }
    public static void DeregisterEnemy(Enemy enemy)
    {
        if (enemy.m_deathDropMaterialPrefab == null)
            return;


            m_craftingCompNameToEnemy[enemy.m_deathDropMaterialPrefab.m_craftingName].Remove(enemy);
    }
    
    public static GameObject GetClosest(CraftingComponent comp, GameObject you)
    {
        List<CraftingComponent> comps = m_craftingComponents[comp.m_craftingName];

        if (m_craftingComponents[comp.m_craftingName].Count == 0)
            return null;

        Transform thingToBeCloseTo = you.transform;
        GameObject closestObject = comps[0].gameObject;
        float shortestDistanceSquared = (thingToBeCloseTo.position - closestObject.gameObject.transform.position).sqrMagnitude;

        for(int i = 1; i < comps.Count; ++i )
        {
            float newDistanceSquared = (thingToBeCloseTo.position - comps[i].gameObject.transform.position).sqrMagnitude;
            if (newDistanceSquared < shortestDistanceSquared)
            {
                shortestDistanceSquared = newDistanceSquared;
                closestObject = comps[i].gameObject;
            }

        }
       
        return closestObject;
    }
    public static GameObject GetClosestEnemy(CraftingComponent comp, GameObject you)
    {
        //HACK Copy pasted code. Fixing it is really irritating though.
        List<Enemy> comps = m_craftingCompNameToEnemy[comp.m_craftingName];

        if (m_craftingCompNameToEnemy[comp.m_craftingName].Count == 0)
            return null;

        Transform thingToBeCloseTo = you.transform;
        GameObject closestObject = comps[0].gameObject;
        float shortestDistanceSquared = (thingToBeCloseTo.position - closestObject.gameObject.transform.position).sqrMagnitude;

        for (int i = 1; i < comps.Count; ++i)
        {
            float newDistanceSquared = (thingToBeCloseTo.position - comps[i].gameObject.transform.position).sqrMagnitude;
            if (newDistanceSquared < shortestDistanceSquared)
            {
                shortestDistanceSquared = newDistanceSquared;
                closestObject = comps[i].gameObject;
            }
        }
        return closestObject;
    }

}
