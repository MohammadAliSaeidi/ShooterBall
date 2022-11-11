using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
	private BulletSpeicifcs _bulletSpecifics;

	public float Age = 5;
	public TrailRenderer trail;
	private Vector3 prevPos;
	private Rigidbody rb;

	public void InitiateAndShoot(BulletSpeicifcs bulletSpecifics)
	{
		_bulletSpecifics = bulletSpecifics;
		prevPos = transform.position;

		trail.transform.parent = null;

		rb = GetComponent<Rigidbody>();
		rb.AddForce(transform.forward * _bulletSpecifics.BulletForce, ForceMode.Impulse);
		StartCoroutine(Co_Age(Age));
	}

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
				var bImpact = _bulletSpecifics.bulletImpacts.Where(b => b.bulletImpactTag == bulletImpactIdentifier.bulletImpactTag).FirstOrDefault();
				if (bImpact == null)
				{
					bImpact = _bulletSpecifics.bulletImpacts.Where(b => b.bulletImpactTag == BulletImpactTag.Default).FirstOrDefault();
				}
				Instantiate(bImpact, hitPoint, Quaternion.LookRotation(surfaceNormal));
				trail.transform.position = hitPoint;
				trail.autodestruct = true;
			}
			Destroy(gameObject);
		}

		prevPos = transform.position;
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
