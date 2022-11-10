using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
	public float explosionRadius;
	public float explosionForce;

	public GameObject DefaultExplosion;

	[Header("Explosion Impacts")]
	public GameObject Default;
	public GameObject Metal;
	public GameObject Soil;
	public GameObject Water;
	public GameObject Grass;
	public GameObject Wood;
	public GameObject Stone;

	float groundCheckDist = 0.05f;

	private void Start()
	{
		if(explosionForce > 0 && explosionRadius > 0)
		{
			Collider[] objects = Physics.OverlapSphere(transform.position, explosionRadius);
			foreach(Collider h in objects)
			{
				Rigidbody r = h.GetComponent<Rigidbody>();
				if(r != null)
				{
					r.AddExplosionForce(explosionForce, transform.position, explosionRadius);
				}
			}
		}

		RaycastHit hit = new RaycastHit();
		Ray ray = new Ray(transform.position, -Vector3.up);

		if(Physics.Raycast(ray, out hit, groundCheckDist))
		{
			ExplosionIdentifier explosionIdentifier = hit.transform.gameObject.GetComponent<ExplosionIdentifier>();
			if(explosionIdentifier)
			{
				Vector3 surfaceNormal = hit.normal;
				Vector3 hitPoint = hit.point;
				Instantiate(GetExplosion(explosionIdentifier.explosionTag), hitPoint, Quaternion.LookRotation(surfaceNormal));
				Destroy(gameObject);
			}
			else
			{
				Instantiate(DefaultExplosion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
	}

	private GameObject GetExplosion(ExplosionTag explosionTag)
	{
		switch(explosionTag)
		{
			case ExplosionTag.Default:
			if(Default)
			{
				return Default;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Metal:
			if(Metal)
			{
				return Metal;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Soil:
			if(Soil)
			{
				return Soil;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Water:
			if(Water)
			{
				return Water;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Grass:
			if(Grass)
			{
				return Grass;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Wood:
			if(Wood)
			{
				return Wood;
			}
			else
			{
				return null;
			}

			case ExplosionTag.Stone:
			if(Stone)
			{
				return Stone;
			}
			else
			{
				return null;
			}

			default:
			if(Default)
			{
				return Default;
			}
			else
			{
				return null;
			}
		}
	}
}
