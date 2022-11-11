using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallShooter.Player
{
	public class Grenade : Throwable
	{
		public Explosion explosion;
		public float explosionForce = 50;
		public float explosionRadius = 3;

		[Space(5)]
		public float FuseTime = 3;

		private void Start()
		{
			if (explosion)
			{
				StartCoroutine(CO_Explosion());
			}
		}

		public IEnumerator CO_Explosion()
		{
			yield return new WaitForSeconds(FuseTime);
			Explosion();
		}

		private void Explosion()
		{
			GameObject inst = Instantiate(explosion.gameObject, transform.position, transform.rotation);
			Explosion exp = inst.GetComponent<Explosion>();
			exp.explosionForce = explosionForce;
			exp.explosionRadius = explosionRadius;
			Destroy(gameObject);
		}
	}
}