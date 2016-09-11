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

    // Update is called once per frame
    void Update()
    {

        // return object from inventory
        if (Input.GetKeyDown(KeyCode.P))
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
                placedObject.GetComponent<Transform>().position = GetComponent<Transform>().position + (GetComponent<Transform>().rotation * Vector3.forward * m_objectPlaceDistance);
                m_objectStore.RemoveAt(m_objectStore.Count - 1);
            }
        }

    }

    void AddToInventory(GameObject obj)
    {
        m_objectStore.Add(obj);
        obj.SetActive(false);
    }
}
