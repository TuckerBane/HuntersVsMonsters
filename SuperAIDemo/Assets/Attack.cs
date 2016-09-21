using UnityEngine;
using System.Collections;
using MessageTypes;

public class Attack : MonoBehaviour {

    public GameObject target;
    public float attackRate = 3.0f;
    public float attackRange = 5.0f;
    public float damageAmount = 1.0f;
    public DamageType damageType = DamageType.Bludgeoning;
    private float timeOfLastAttack = -1337.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!target)
            return;

	if ((transform.position - target.transform.position).magnitude <= attackRange &&  timeOfLastAttack + attackRate <= Time.fixedTime)
        {
            timeOfLastAttack = Time.fixedTime;
            target.SendMessage("TakeDamage", new DamageMessage(){type = damageType, amount = damageAmount} );
        }

	}
}
