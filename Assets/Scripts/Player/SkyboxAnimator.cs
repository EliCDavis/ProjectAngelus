using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ISO.Player {

	/// <summary>
	/// Spins and 'pulses' the skybox for a cute animation
	/// </summary>
	public class SkyboxAnimator : MonoBehaviour {

		/// <summary>
		/// The skybox we're going to animate...
		/// </summary>
		[SerializeField]
		Material skybox;
		
		void Update () {

			if (skybox == null) {
				return;
			}

			skybox.SetFloat ("_Exposure", 3f + (2f*Mathf.Sin(Time.time*2)));
			skybox.SetFloat ("_Rotation", (Time.time*2) %360);

		}

	}

}