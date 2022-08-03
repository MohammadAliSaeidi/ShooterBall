using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MolotovFireHandler : MonoBehaviour
{
	public LayerMask groundLayer = -1;

	public GameObject FireInstance;
	public Transform FireCenterPoint;
	[Range(0.1f, 1f)]
	public float GroundCheckHeight = 0.1f;

	[Range(0, 3)]
	public float FireRadius = 1.0f;
	[Range(0.05f, 1)]
	public float FireExpandSpeed = 0.1f;
	[Range(0.05f, 1.0f)]
	public float LinearStepDistance = 0.1f;
	[Range(0.1f, 45)]
	public float CircularStepDistance = 3f;
	public float FireAge = 5;


	List<ParticleSystem> particles = new List<ParticleSystem>();
	public bool ExpandFire = false;
	bool MoveToGround = true;

	private void Start()
	{
		StartCoroutine(Co_Destroy());
	}

	private void Update()
	{
		if(MoveToGround)
		{
			RaycastHit hit = new RaycastHit();
			Ray ray = new Ray(transform.position, Vector3.down);
			if(Physics.Raycast(ray, out hit, Mathf.Infinity))
			{
				transform.position = Vector3.MoveTowards(transform.position, hit.point, 10 * Time.deltaTime);
			}
			else
			{
				transform.position = Vector3.MoveTowards(transform.position, Vector3.down, Time.deltaTime * 10);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		MoveToGround = false;
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			ExpandFire = true;
			StartCoroutine(Co_ExpandFire());
		}
	}

	private void OnTriggerStay(Collider other)
	{
		ExpandFire = true;
	}

	private void OnTriggerExit(Collider other)
	{
		MoveToGround = true;
		if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			ExpandFire = false;
		}
	}

	public IEnumerator Co_ExpandFire()
	{
		for(float l = LinearStepDistance; l < FireRadius; l += LinearStepDistance)
		{
			for(float c = 0; c <= 90; c += CircularStepDistance)
			{
				if(!ExpandFire)
				{
					yield return null;
				}
				// create Position on the ground then check if position is not behind any object (not any object is between center point and the new fire pos)
				RaycastHit hit_groundCheck;
				Ray ray_groundCheck = new Ray(transform.position + (FireCenterPoint.forward * Random.Range(l + l / 2, l - l / 2)) + (FireCenterPoint.up * GroundCheckHeight), Vector3.down);

				Debug.DrawRay(
					transform.position + (FireCenterPoint.forward * Random.Range(l + l / 2, l - l / 2)) + (FireCenterPoint.up * GroundCheckHeight),
					Vector3.down,
					Color.red
					);

				if(Physics.Raycast(ray_groundCheck, out hit_groundCheck, GroundCheckHeight * 2, groundLayer))
				{
					Debug.DrawLine(FireCenterPoint.position, hit_groundCheck.point, Color.green);
					Debug.DrawRay(FireCenterPoint.position, hit_groundCheck.point - FireCenterPoint.position, Color.yellow);

					//RaycastHit hit_wallCheck;
					Ray ray_wallCheck = new Ray(FireCenterPoint.position, hit_groundCheck.point - FireCenterPoint.position);
					if(!Physics.Raycast(ray_wallCheck, Vector3.Distance(FireCenterPoint.position, hit_groundCheck.point)/*, ~(1 << LayerMask.NameToLayer("IgnoreRaycast"))*/))
					{
						Vector3 newPos = hit_groundCheck.point;
						GameObject fire = Instantiate(FireInstance, newPos, new Quaternion());
						particles.AddRange(fire.GetComponentsInChildren<ParticleSystem>());
						yield return new WaitForSeconds(0.05f);
					}
				}

				FireCenterPoint.Rotate(0, Random.Range(c + c / 2, c - c / 2), 0);
			}
			if(CircularStepDistance > 2f)
				CircularStepDistance *= 0.6f;
			yield return new WaitForSeconds(FireExpandSpeed);
		}

		StartCoroutine(Co_TurnOfTheFire());
	}

	public IEnumerator Co_TurnOfTheFire()
	{
		yield return new WaitForSeconds(FireAge);
		particles.AddRange(GetComponentsInChildren<ParticleSystem>());
		foreach(var p in particles)
		{
			var emission = p.emission;
			emission.enabled = false;
			ObjectAge.AddObjectAge(p.transform.parent.gameObject, 0.5f);
			yield return new WaitForSeconds(Random.Range(0.0f, 0.2f));
		}
	}

	public IEnumerator Co_Destroy()
	{

		yield return new WaitForSeconds(FireAge * 2);
		foreach(var i in GetComponentsInChildren<ParticleSystem>())
		{
			var emission = i.emission;
			emission.enabled = false;
		}
		yield return new WaitForSeconds(2);
		if(!particles.Any())
		{
			Destroy(gameObject);
		}
	}
}
