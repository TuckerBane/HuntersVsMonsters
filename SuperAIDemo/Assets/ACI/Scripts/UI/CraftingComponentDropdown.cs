using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class CraftingComponentDropdown : MonoBehaviour {
    Dropdown myDropdown;
	// Use this for initialization
	void Start () {
        myDropdown = GetComponent<Dropdown>();
        myDropdown.ClearOptions();
        GameObject[] allOptions = UIHelpers.GetAllPrefabsWithComponent<CraftingComponent>();
        List<string> craftingOptions = new List<string>();
        foreach(GameObject option in allOptions)
        {
            craftingOptions.Add(option.GetComponent<CraftingComponent>().m_craftingName);
        }
        myDropdown.AddOptions(craftingOptions);
        //UIHelpers
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
