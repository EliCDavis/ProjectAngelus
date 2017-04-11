using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ISO.Scenes.EliTesting {

	public class MechAnimate : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			transform.Translate (Vector3.up*Mathf.Sin(Time.time*2)*Mathf.PerlinNoise(0, Time.time)*.5f*Time.deltaTime);
		}
	}

}