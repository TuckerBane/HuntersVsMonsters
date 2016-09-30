using UnityEngine;
using System.Collections;
using System.Collections.Generic;

struct ObjectDestroyedEvent { }

[RequireComponent(typeof(Transform))]
public class Spawner : MonoBehaviour {

    public float m_interval = 5.0f;
    public float m_timeInInterval = 0.0f;
    public float m_nextSpawnTime = 0.0f;
    // Use this for initialization

    public GameObject m_objectToSpawn;
    public Vector3 m_positionOffset;

    public int m_maxObjectsToMake = 10;
    public bool m_respawnExistingObjects = true;

    private List<Object> m_objectsIMade;
    private Transform m_ownerTransform;
    IEnumerator ObjectsIMadeCleanup()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            foreach (var obj in m_objectsIMade)
            {
                if (!obj)
                {
                    m_objectsIMade.Remove(obj);
                    break;
                }
            }
        }
    }

    void Start () {
        m_nextSpawnTime = Time.time + m_timeInInterval;
        m_ownerTransform = GetComponent<Transform>();
        m_objectsIMade = new List<Object>();
        StartCoroutine("ObjectsIMadeCleanup");
	}
	
	// Update is called once per frame
	void Update () {
        if ( Time.time >= m_nextSpawnTime)
        {
            m_nextSpawnTime += m_interval;

            // remove deleted objects from m_objectsIMade. Try using message board on spawn
            if (m_objectsIMade.Count >= m_maxObjectsToMake && m_respawnExistingObjects)
            {
                Object temp = m_objectsIMade[0];
                m_objectsIMade.Remove(temp);
                Destroy(temp);
            }
            if (m_objectsIMade.Count < m_maxObjectsToMake)
            {
                m_objectsIMade.Add(
                    Instantiate(m_objectToSpawn, m_ownerTransform.position + m_positionOffset, Quaternion.identity)
                    );

                //Do something callback registeration like?
                //GameObject newObj = (GameObject) m_objectsIMade[m_objectsIMade.Count - 1];

            }
        }

	}
}
