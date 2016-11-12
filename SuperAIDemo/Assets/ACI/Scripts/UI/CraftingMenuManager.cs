using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class CraftingMenuManager : MonoBehaviour
{

    public GameObject m_craftingMenu;

    private bool m_menuOpen = false;

    // Use this for initialization
    void Start()
    {
        //m_craftingMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_menuOpen = !m_menuOpen;
            m_craftingMenu.SetActive(m_menuOpen);
            GetComponent<CharacterController>().enabled = !m_menuOpen;
            GetComponent<FirstPersonController>().enabled = !m_menuOpen;
            Cursor.visible = m_menuOpen;
        }
    }
}
