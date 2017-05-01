using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ISO.Player
{

	public class PlayerCameraHitWall : MonoBehaviour
	{

		/// <summary>
		/// The player we aim to always be in sight  
		/// </summary>
		[SerializeField]
		private GameObject playerToKeepInSight;

		/// <summary>
		/// How close the camera will get to the camera
		/// </summary>
		private float maxPlayerProximity = 1f;

		private float initialDistance;

	    [SerializeField]
	    private float m_SphereCastSize = 0.1f;

	    void Start()
	    {
			if (playerToKeepInSight == null) {
				Destroy (this);
				return;
			}
			this.initialDistance = Vector3.Distance(transform.position, playerToKeepInSight.transform.position);
	    }

		void OnCollisionEnter(){
			print ("Camera collision");
		}

	    void Update()
	    {
	        RaycastHit hit;

			Vector3 cameraToPlayer = playerToKeepInSight.transform.position - transform.position + Vector3.up;

			if (Physics.SphereCast (transform.position, m_SphereCastSize, -transform.forward, out hit, 1)) {

				// Move camera appropriatly
				transform.position += cameraToPlayer.normalized * hit.distance;

				// Recalculate vector
				cameraToPlayer = playerToKeepInSight.transform.position - transform.position + Vector3.up;

				// Ensure we're not too close
				if (cameraToPlayer.magnitude < maxPlayerProximity) {
					print ("Way too close");
					transform.position -= cameraToPlayer.normalized * (maxPlayerProximity - cameraToPlayer.magnitude);
				}

			} else if(Physics.SphereCast (transform.position, m_SphereCastSize, -transform.forward, out hit, 2f)) {
				// do nothing
			}else if (cameraToPlayer.magnitude > initialDistance + .5f) {
				transform.position += cameraToPlayer.normalized*(initialDistance - cameraToPlayer.magnitude);
			} else if(cameraToPlayer.magnitude < initialDistance - .5f) {
				transform.position -= cameraToPlayer.normalized*(initialDistance - cameraToPlayer.magnitude);
			}
	    }
	}

}