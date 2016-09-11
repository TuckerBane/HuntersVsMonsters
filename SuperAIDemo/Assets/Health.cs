using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public enum DamageType { Fire = 0, Ice, Bludgeoning, Piercing };


public class Health : MonoBehaviour {

    public int m_health = 10;

    public class DamageModifiers
    {
        public int m_fire;
        public int m_ice;
        public int m_bludgeoning;
        public int m_piercing;
    };

    public DamageModifiers m_damageModifers;

    public int GetDamageModifier(DamageType type)
    {
        switch(type)
        {
            case DamageType.Fire:
                return m_damageModifers.m_fire;
            case DamageType.Ice:
                return m_damageModifers.m_ice;
            case DamageType.Bludgeoning:
                return m_damageModifers.m_bludgeoning;
            case DamageType.Piercing:
                return m_damageModifers.m_piercing;
        }
        return 0;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void TakeDamage(DamageType type, int amount)
    {
        if ((int) type > 4 || type < 0)
        {
            Debug.Log("Invalid damage type");
        }
        amount += GetDamageModifier(type);
        if (amount < 0)
            return;
        m_health -= amount;
        if (m_health <= 0)
            Destroy(gameObject);
    }
}
