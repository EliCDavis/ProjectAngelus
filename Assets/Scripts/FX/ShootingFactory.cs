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

		private static AudioClip shootEffect = null;
		private static AudioClip getShoootEffect(){
			if (shootEffect == null)
			{
				shootEffect = Resources.Load<AudioClip>("SFX/single");
			}

			return shootEffect;
		}

        private static GameObject m_HitEffectPrefab;
        private static GameObject GetHitEffectPrefab()
        {
            if (m_HitEffectPrefab == null)
            {
                m_HitEffectPrefab = Resources.Load<GameObject>("EFXPrefabs/HitEffect");
            }
            return m_HitEffectPrefab;
        }

        private static GameObject m_DeathEffectPrefab;
        private static GameObject GetDeathEffectPrefab()
        {
            if (m_DeathEffectPrefab == null)
            {
                m_DeathEffectPrefab = Resources.Load<GameObject>("EFXPrefabs/DeathEffect");
            }
            else
            {
                Debug.Log("Failed to find resource: \"EFXPrefabs/DeathEffect\"");
            }

            return m_DeathEffectPrefab;
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

            //CreatHitEffect(end);

			GameObject.Destroy (laser, .1f);

			GameObject soundFX = new GameObject ("shoot SFX");
			soundFX.transform.position = start;
			soundFX.AddComponent<AudioSource> ().clip = getShoootEffect ();
			soundFX.GetComponent<AudioSource> ().volume = .25f;
			soundFX.GetComponent<AudioSource> ().Play ();
			GameObject.Destroy (soundFX, getShoootEffect().length);

		}

        public static void CreatHitEffect(Vector3 _hitPoint)
        {
			GameObject _hitEffect = GameObject.Instantiate(GetHitEffectPrefab(), _hitPoint, Quaternion.identity);

            _hitEffect.transform.position = _hitPoint;
            _hitEffect.name = "HitEffect";

            GameObject.Destroy(_hitEffect, 1f);
        }

        public static void CreateDeathEffect(Vector3 _deathPoint)
        {
            GameObject _deathEffect = GameObject.Instantiate(GetDeathEffectPrefab(), _deathPoint, Quaternion.identity);

            _deathEffect.transform.position = _deathPoint;
            _deathEffect.name = "DeathEffect";

            GameObject.Destroy(_deathEffect, 1f);
        }

	}

}