using UnityEngine;
using System.Collections;

public class CraftingComponent : MonoBehaviour {

    public string m_craftingName = "Default name to object name stripped of copy identifier";

    public void Reset()
    {
        if (m_craftingName == "Default name to object name stripped of copy identifier")
        {
            m_craftingName = gameObject.name;
            int copyIdentifierStartIndex = m_craftingName.IndexOf('(');
            if (copyIdentifierStartIndex != -1)
            {
                m_craftingName = m_craftingName.Remove(copyIdentifierStartIndex, m_craftingName.Length - copyIdentifierStartIndex);
                m_craftingName = m_craftingName.TrimEnd(' ');
            }
        }
    }

}
