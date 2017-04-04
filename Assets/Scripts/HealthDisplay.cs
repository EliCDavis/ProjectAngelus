using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour {

    public float health = 500f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = new Vector3(health / 500f, 1);
        transform.localPosition = new Vector3((health / 2f) - 250f, 0, 0);
	}
}
