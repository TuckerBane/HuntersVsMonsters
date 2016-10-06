using UnityEngine;
using System.Collections;

public class PlayerUseObject : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            col.gameObject.transform.root.SendMessage("PlayerUseObject", gameObject ,SendMessageOptions.DontRequireReceiver);
        }
    }
}
