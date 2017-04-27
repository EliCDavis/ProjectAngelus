using UnityEngine;

namespace ISO.FX {

	public static class ShootingFactory {

		private static Material laserInstance = null;
		private static Material GetLaserMaterial(){
			if(laserInstance == null){
				laserInstance = Resources.Load<Material>("Materials/LaserEffect");
			}
			return laserInstance;
		}

		/// <summary>
		/// Creates an effect that looks like a laser. Cleans itself up.
		/// </summary>
		/// <param name="start">Start.</param>
		/// <param name="end">End.</param>
		public static void CreateShootEffect(Vector3 start, Vector3 end) {

			GameObject laser = new GameObject("Laser Effect");

			LineRenderer line = laser.AddComponent<LineRenderer>();

			line.positionCount = 2;

			line.SetPositions (new Vector3[]{
				start, end
			});

			line.startWidth = 0.05f;
			line.endWidth = 0.04f;

			line.material = GetLaserMaterial ();

			GameObject.Destroy (laser, .1f);

		}

	}

}