using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Molotov : Throwable
{
    public MolotovFireHandler explosion;

    [Space(5)]
    public float FuseTime = 3;
	public float MinHitVelocity; // min velocity of collision to explosion


    void Start()
    {
        if(explosion)
        {
            StartCoroutine(CO_Explosion());
        }
    }

	private void OnCollisionEnter(Collision collision)
	{
		float hitVelocity = collision.relativeVelocity.magnitude;
		if(hitVelocity > MinHitVelocity)
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(transform.position, Vector3.down);
			if(Physics.Raycast(ray, out hit, 0.5f))
			{
				Vector3 hitPoint = hit.point;
				Explosion(hitPoint);
			}
			else
			{
				Explosion(transform.position);
			}
		}
	}

	public IEnumerator CO_Explosion()
	{
		yield return new WaitForSeconds(FuseTime);
		Explosion(transform.position);
	}

	private void Explosion(Vector3 hitPoint)
	{
		GameObject inst = Instantiate(explosion.gameObject, hitPoint, new Quaternion());
		Destroy(gameObject);
	}
}
