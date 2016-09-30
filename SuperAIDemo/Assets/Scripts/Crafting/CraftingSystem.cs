using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingRecipe
{
    public GameObject m_createdObjectPrefab;
    public List<GameObject> m_craftingComponents;
    // optional recipe components
    public float m_time = 0.0f;
    public GameObject[] m_required_tools;
}

public class CraftingSystem : MonoBehaviour {

    public GameObject[] m_baseMaterials;
    public CraftingRecipe[] m_recipes;
    public CraftingRecipe m_testing;
    public int i;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
