﻿using UnityEngine;

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
                m_HitEffectPrefab = Resources.Load<GameObject>("Prefabs/HitEffect");
            }

            return m_HitEffectPrefab;
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

			GameObject soundFX = new GameObject ("shoot SFX");
			soundFX.transform.position = start;
			soundFX.AddComponent<AudioSource> ().clip = getShoootEffect ();
			soundFX.GetComponent<AudioSource> ().volume = .25f;
			soundFX.GetComponent<AudioSource> ().Play ();
			GameObject.Destroy (soundFX, getShoootEffect().length);

		}

        public static void CreatHitEffect(Transform _hitPoint)
        {
            GameObject hitParticles = new GameObject("Hit Particles");

            ParticleSystem particleSys = hitParticles.AddComponent<ParticleSystem>();

            particleSys.Stop(); 

            var main = particleSys.main;
            var emmision = particleSys.emission;

            main.duration = 1f;
            main.startLifetime = 1f;
            main.startSize = 0.05f;
            main.startColor = new Color(0f, 0f, 0f, 0f);
            emmision.rateOverTime = 0f;
            //emmision

            

        }

	}

}