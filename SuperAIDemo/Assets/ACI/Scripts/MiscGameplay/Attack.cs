using UnityEngine;
using System.Collections;
using MessageTypes;

public class Attack : MonoBehaviour {

    public GameObject m_target;
    public float m_attackRate = 3.0f;
    public float m_attackRange = 5.0f;
    public float m_damageAmount = 1.0f;
    public DamageType m_damageType = DamageType.Bludgeoning;
    private float m_timeOfLastAttack = -1337.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!m_target)
            return;

	if ((transform.position - m_target.transform.position).magnitude <= m_attackRange &&  m_timeOfLastAttack + m_attackRate <= Time.fixedTime)
        {
            m_timeOfLastAttack = Time.fixedTime;
            m_target.SendMessage("TakeDamage", new DamageMessage(){type = m_damageType, amount = m_damageAmount} );
        }

	}
}
