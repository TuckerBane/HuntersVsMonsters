using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingAIGlobals : MonoBehaviour {
    public static Dictionary<string, List<CraftingComponent>> m_craftingComponents = new Dictionary<string, List<CraftingComponent>>();
    public static Dictionary<string, List<Enemy>> m_craftingCompNameToEnemy = new Dictionary<string, List<Enemy>>();
    // TODO is this something I want?
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
            //TODO is enemies not dropping anything a problem?
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
        //TODO get the closest
        return m_craftingComponents[comp.m_craftingName][0].gameObject;
    }
    public static GameObject GetClosestEnemy(CraftingComponent comp, GameObject you)
    {
        //TODO get the closest
        return m_craftingCompNameToEnemy[comp.m_craftingName][0].gameObject;
    }

}
