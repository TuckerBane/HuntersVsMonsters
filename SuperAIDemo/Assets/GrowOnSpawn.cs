using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]

public class GrowOnSpawn : MonoBehaviour
{

    public float m_growTime = 2.0f;
    public Vector3 m_startSize = new Vector3(1, 1, 1);
    public bool m_growing = true;
    // grows to whatever size the prefab is

    private Vector3 endSize;
    private float currentTime = 0.0f;
    private Transform myTransform;


    IEnumerator Grow()
    {
        while (currentTime <= m_growTime)
        {
            currentTime += Time.deltaTime;
            myTransform.localScale = (m_startSize * (m_growTime - currentTime) + endSize * currentTime) / m_growTime;
            yield return null;
        }
        m_growing = false;
    }

    // Use this for initialization
    void Start()
    {
        myTransform = GetComponent<Transform>();
        endSize = myTransform.localScale;
        StartCoroutine("Grow");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
