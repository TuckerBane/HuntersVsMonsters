using UnityEngine;
using System.Collections;


public class Enemy : MonoBehaviour {
    public CraftingComponent m_deathDropMaterialPrefab;

    public void OnEnable()
    {
        CraftingAIGlobals.RegisterEnemy(this);
    }

    public void OnDisable()
    {
        CraftingAIGlobals.DeregisterEnemy(this);
    }

    public void DeathMessage()
    {
        Instantiate(m_deathDropMaterialPrefab);
        Destroy(gameObject);
    }

}
