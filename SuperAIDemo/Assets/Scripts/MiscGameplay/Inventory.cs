using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{

    public List<GameObject> m_objectStore;
    public float m_objectPlaceDistance;
    // Use this for initialization
    void Start()
    {

    }

    public int CountOf(CraftingComponent comp)
    {
        int count = 0;
        foreach(GameObject invObj in m_objectStore)
        {
            CraftingComponent compycomp = invObj.GetComponent<CraftingComponent>();
            if (compycomp == null)
                continue;
            if (compycomp.m_craftingName == comp.m_craftingName)
                ++count;
        }
        return count;
    }

    public void PlaceObject(GameObject obj)
    {
        obj.GetComponent<Transform>().position = GetComponent<Transform>().position + (GetComponent<Transform>().rotation * Vector3.forward * m_objectPlaceDistance);
    }

    public void PlaceObject()
    {
        if (m_objectStore.Count != 0)
        {
            GameObject placedObject = m_objectStore[m_objectStore.Count - 1];
            while (m_objectStore.Count != 0 && !placedObject)
            {
                m_objectStore.RemoveAt(m_objectStore.Count - 1);
                if (m_objectStore.Count != 0)
                    placedObject = m_objectStore[m_objectStore.Count - 1];
            }
            // TODO: put this in a function
            if (!placedObject)
                return;

            placedObject.SetActive(true);
            PlaceObject(placedObject);
            m_objectStore.RemoveAt(m_objectStore.Count - 1);
        }
    }

    // Update is called once per frame
    void Update()
    {

        // return object from inventory
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlaceObject();
        }

    }

    public void AddToInventory(GameObject obj)
    {
        m_objectStore.Add(obj);
        obj.SetActive(false);
    }

    public void RemoveFromInventory(CraftingComponent comp)
    {
        foreach (GameObject invObj in m_objectStore)
        {
            CraftingComponent compycomp = invObj.GetComponent<CraftingComponent>();
            if (compycomp == null)
                continue;
            if (compycomp.m_craftingName == comp.m_craftingName)
            {
                m_objectStore.Remove(invObj);
                return;
            }
        }
    }
}
