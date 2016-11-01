using UnityEngine;
using System.Collections;
using UnityStandardAssets.Utility;

public class GraphDrawer : MonoBehaviour {

    public GameObject m_testSymbol;
    public GameObject m_arrowPrefab;

	// Use this for initialization
	void Start () {
//         GameObject obj1 = PutSymbolAtPos(m_testSymbol, new Vector3(7, 10, 0));
//         GameObject obj2 = PutSymbolAtPos(m_testSymbol, new Vector3(0, 30, 0));
//         DrawArrowBetweenThings(obj1, obj2);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void ScaleVector(ref Vector3 vec)
    {
        vec.Scale(new Vector3(1.0f / transform.lossyScale.x, 1.0f / transform.lossyScale.y, 1.0f / transform.lossyScale.z));
    }

    public GameObject PutSymbolAtPos(GameObject symbolPrefab, Vector3 pos)
    {
        if (symbolPrefab == null)
        {
            int i = 0;
        }

        GameObject newSymbol = Instantiate(symbolPrefab, transform) as GameObject;
        if (newSymbol.GetComponent<AutoMoveAndRotate>())
            Destroy(newSymbol.GetComponent<AutoMoveAndRotate>());
        //let pos act in world space
        ScaleVector(ref pos);
        newSymbol.transform.localPosition = pos;
        return newSymbol;
    }

    public void DrawArrowBetweenThings(GameObject start, GameObject end)
    {
        GameObject newArrow = Instantiate(m_arrowPrefab, start.transform) as GameObject;
        newArrow.transform.localPosition = Vector3.zero;
        newArrow.transform.LookAt(end.transform);
        // rotate the arrow into the diagram plane
        newArrow.transform.Rotate(0, -90, 0);

        Vector3 startToEnd = end.transform.position - start.transform.position;
        newArrow.transform.position += startToEnd / 2.0f;
        
    }
}
