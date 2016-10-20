using UnityEngine;
using System.Collections;

public class CraftingComponent : MonoBehaviour {


    #region CraftingName
    public string m_craftingName = "Name defaulted to object name stripped of copy identifier";
    void SetCraftingName()
    {
        m_craftingName = gameObject.name;
        int copyIdentifierStartIndex = m_craftingName.IndexOf('(');
        if (copyIdentifierStartIndex != -1)
        {
            m_craftingName = m_craftingName.Remove(copyIdentifierStartIndex, m_craftingName.Length - copyIdentifierStartIndex);
            m_craftingName = m_craftingName.TrimEnd(' ');
        }
    }
    public void Awake()
    {
        SetCraftingName();
    }
    public void Reset()
    {
        SetCraftingName();
    }
    public void OnDrawGizmosSelected()
    {
        SetCraftingName();
    }
    #endregion

    #region Registration
    public void OnEnable()
    {
        CraftingAIGlobals.RegisterCraftingComponent(this);
    }

    public void OnDisable()
    {
        CraftingAIGlobals.DeregisterCraftingComponent(this);
    }
    #endregion

    #region Pickupable
    void PlayerUseObject(GameObject player)
    {
        GameObject iconInstance = null;
        if (m_iconPrefab != null)
            iconInstance = (GameObject)Instantiate(m_iconPrefab, transform.position, transform.rotation);
        else
            iconInstance = (GameObject)Instantiate(FindObjectOfType<CraftingAIGlobals>().m_defualtIconPrefab, transform.position, transform.rotation);
        iconInstance.AddComponent<Lifespan>();
        iconInstance.GetComponent<Lifespan>().m_time = 3.0f;
        player.SendMessage("AddToInventory", gameObject);
    }
    #endregion

    #region HasIcon
    public GameObject m_iconPrefab;
    
    #endregion
}

