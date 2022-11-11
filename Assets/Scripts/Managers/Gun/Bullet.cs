using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	public float BulletForce = 10;
	public float Age = 5;
	public TrailRenderer trail;

	[Header("Bullet Impacts")]
	public GameObject Default;
	public GameObject Metal;
	public GameObject Soil;
	public GameObject Water;
	public GameObject Grass;
	public GameObject Wood;
	public GameObject Stone;

	Vector3 prevPos;
	Vector3 prevDir;

	Rigidbody rb;
	private void Start()
	{
		prevPos = transform.position;
		prevDir = transform.forward;

		trail.transform.parent = null;

		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * BulletForce, ForceMode.Impulse);
		StartCoroutine(Co_Age(Age));
	}

	//private void FixedUpdate()
	//{
	//	// Previous Position
	//	trail.transform.position = prevPos;

	//	float dist = Vector3.Distance(transform.position, prevPos);

	//	RaycastHit hit = new RaycastHit();
	//	Ray bulletRay = new Ray(prevPos, transform.forward);

	//	// Current Position
	//	prevPos = transform.position;
	//	prevDir = transform.forward;

	//	if(Physics.Raycast(bulletRay, out hit, dist))
	//	{
	//		BulletImpactIdentifier bulletImpactIdentifier = hit.transform.gameObject.GetComponent<BulletImpactIdentifier>();
	//		if(bulletImpactIdentifier)
	//		{
	//			Vector3 surfaceNormal = hit.normal;
	//			Vector3 hitPoint = hit.point;
	//			var bulletImpact = Instantiate(GetBulletImpact(bulletImpactIdentifier.bulletImpactTags), hitPoint, Quaternion.LookRotation(surfaceNormal));
	//			trail.transform.position = hitPoint;
	//			trail.autodestruct = true;
	//		}
	//		Destroy(gameObject);
	//	}
	//}

	private void Update()
	{
		trail.transform.position = prevPos;

		if (Physics.Linecast(prevPos, transform.position, out RaycastHit hit))
		{
			BulletImpactIdentifier bulletImpactIdentifier = hit.transform.gameObject.GetComponent<BulletImpactIdentifier>();
			if (bulletImpactIdentifier)
			{
				Vector3 surfaceNormal = hit.normal;
				Vector3 hitPoint = hit.point;
				var bulletImpact = Instantiate(GetBulletImpact(bulletImpactIdentifier.bulletImpactTags), hitPoint, Quaternion.LookRotation(surfaceNormal));
				trail.transform.position = hitPoint;
				trail.autodestruct = true;
			}
			Destroy(gameObject);
		}

		prevPos = transform.position;
	}

	private GameObject GetBulletImpact(BulletImpactTags bulletImpactTags)
	{
		switch(bulletImpactTags)
		{
			case BulletImpactTags.Default:
			if(Default)
			{
				return Default;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Metal:
			if(Metal)
			{
				return Metal;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Soil:
			if(Soil)
			{
				return Soil;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Water:
			if(Water)
			{
				return Water;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Grass:
			if(Grass)
			{
				return Grass;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Wood:
			if(Wood)
			{
				return Wood;
			}
			else
			{
				return null;
			}

			case BulletImpactTags.Stone:
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

	private void OnDestroy()
	{
		transform.DetachChildren();
	}

	public IEnumerator Co_Age(float age)
	{
		yield return new WaitForSeconds(age);
		Destroy(gameObject);
	}
}
