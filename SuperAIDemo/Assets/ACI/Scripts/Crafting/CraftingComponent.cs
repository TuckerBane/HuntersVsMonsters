using UnityEngine;
using System.Collections;

public class CraftingComponent : MonoBehaviour
{

  public bool Equals(CraftingComponent otherComp)
    {
        if (otherComp == null)
            return false;
        return m_craftingName == otherComp.m_craftingName;
    }

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
        if (m_iconPrefab == null)
            m_iconPrefab = FindObjectOfType<CraftingAIGlobals>().m_defualtIconPrefab;
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
        iconInstance = (GameObject)Instantiate(GetIconPrefab(), transform.position, transform.rotation);

        iconInstance.AddComponent<Lifespan>();
        iconInstance.GetComponent<Lifespan>().m_time = 3.0f;
        player.SendMessage("AddToInventory", gameObject);
    }
    #endregion

    #region HasIcon
    public GameObject m_iconPrefab;
    public GameObject GetIconPrefab()
    {
        if (m_iconPrefab == null)
            m_iconPrefab = FindObjectOfType<CraftingAIGlobals>().m_defualtIconPrefab;
        return m_iconPrefab;
    }
    #endregion
}

