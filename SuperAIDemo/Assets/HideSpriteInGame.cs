using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class HideSpriteInGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SpriteRenderer temp = GetComponent<SpriteRenderer>();
        temp.enabled = false;
	}
}
