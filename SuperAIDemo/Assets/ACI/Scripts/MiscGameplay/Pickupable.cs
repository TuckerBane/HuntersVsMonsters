using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayerUseObject(GameObject player)
    {
        HasIcon icon = gameObject.GetComponent<HasIcon>();
        if (icon != null)
        {
            GameObject iconInstance = (GameObject) Instantiate(icon.m_iconPrefab, transform.position, transform.rotation);
            iconInstance.AddComponent<Lifespan>();
            iconInstance.GetComponent<Lifespan>().m_time = 3.0f;
        }

        player.SendMessage("AddToInventory", gameObject);
    }

}
