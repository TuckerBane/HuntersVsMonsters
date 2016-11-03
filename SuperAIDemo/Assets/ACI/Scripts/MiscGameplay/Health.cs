using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using MessageTypes;

public enum DamageType { Fire = 0, Ice, Bludgeoning, Piercing };


public class Health : MonoBehaviour {

    public float m_health = 10;

    public class DamageModifiers
    {
        public int m_fire = 0;
        public int m_ice = 0;
        public int m_bludgeoning = 0;
        public int m_piercing = 0;
    };

    public DamageModifiers m_damageModifers = new DamageModifiers();

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
        // probably don't need this
// 	    if (m_health <= 0.001)
//         {
//             gameObject.SendMessage("DeathMessage");
//         }

	}

    void TakeDamage(DamageMessage message)
    {
        if ((int) message.type > 4 || message.type < 0)
        {
            Debug.Log("Invalid damage type");
        }
        message.amount += GetDamageModifier(message.type);
        if (message.amount < 0)
            return;
        m_health -= message.amount;
        if (m_health <= 0)
            gameObject.SendMessage("DeathMessage");
    }
}
