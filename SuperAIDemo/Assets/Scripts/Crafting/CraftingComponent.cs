using UnityEngine;
using System.Collections;

public class CraftingComponent : MonoBehaviour {

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
}

