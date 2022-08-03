using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public DropedGun dropedGun;

	[Header("Ammo")]
	public int Ammo = 30;
	public int MagazineAmmo = 30;

	[Header("Firing Options")]
	public Bullet bullet;
	public ParticleSystem BulletShellParticle;
	public float FireRate;
	public Transform BulletSpawnPoint;

	[Header("MuzzleFlash")]
	public GameObject MuzzleFlashContainer;
	public GameObject CoolDownContainer;
	ParticleSystem[] muzzleFlashParticles;
	ParticleSystem[] coolDownParticles;

	[Header("Heat")]
	public float MaxHeatParticles = 10;
	float heatAmount = 0;

	[Header("Recoil Attributes")]
	public float VerticalRecoil;
	public float HorizontalRecoil;
	public float ConeSpreadSize;

	[Header("CrossHair")]
	public CrossHairMaster crossHair;

	[Space(5)]
	public Transform PlayerGunHolder;

	bool gunIsOnRightPos = false;

	public void Start()
	{
		muzzleFlashParticles = MuzzleFlashContainer.GetComponentsInChildren<ParticleSystem>();
		coolDownParticles = CoolDownContainer.GetComponentsInChildren<ParticleSystem>();

		StopCoolDownEffect();
	}

	private void OnEnable()
	{
		
	}

	private void Update()
	{
		if(PlayerGunHolder && !gunIsOnRightPos)
		{
			float dist = Vector3.Distance(transform.position, PlayerGunHolder.transform.position);
			float angle = Quaternion.Angle(transform.rotation, PlayerGunHolder.transform.rotation);
			if(dist > 0.1f || angle > 0.1f)
			{
				float speed = 10f;

				transform.position = Vector3.Lerp(transform.position, PlayerGunHolder.transform.position, Time.deltaTime * speed / dist);
				transform.rotation = Quaternion.Lerp(transform.rotation, PlayerGunHolder.transform.rotation, Time.deltaTime * speed);
			}
			else
			{
				transform.position = PlayerGunHolder.position;
				transform.rotation = PlayerGunHolder.rotation;

				transform.parent = PlayerGunHolder.transform;

				gunIsOnRightPos = true;
			}
		}

		if(heatAmount > 0)
		{
			heatAmount -= MaxHeatParticles / 5 * Time.deltaTime;
			HandleCoolDown();
		}
		else if(heatAmount < 0)
		{
			heatAmount = 0;
		}
		else
		{
			StopCoolDownEffect();
		}
	}

	public void EmitMuzzleFlash()
	{
		foreach(var m in muzzleFlashParticles)
		{
			ParticleSystem.EmissionModule emit = m.emission;
			emit.enabled = true;
			float probability = emit.GetBurst(0).probability;
			float rand = (UnityEngine.Random.Range(0, 10)) / 10;
			if(rand <= probability)
			{
				m.Emit(UnityEngine.Random.Range(emit.GetBurst(0).minCount, emit.GetBurst(0).maxCount));
			}
		}

		if(heatAmount < MaxHeatParticles)
		{
			heatAmount += MaxHeatParticles / 10;
		}
	}

	public void StopMuzzleFlash()
	{
		foreach(var m in muzzleFlashParticles)
		{
			ParticleSystem.EmissionModule emit = m.emission;
			m.Clear();
			emit.enabled = false;
		}
	}

	public void HandleCoolDown()
	{
		foreach(var m in coolDownParticles)
		{
			ParticleSystem.EmissionModule emit = m.emission;
			ParticleSystem.MinMaxCurve rate = emit.rateOverTime;
			emit.enabled = true;
			rate.constant = heatAmount;
		}
	}

	private void StopCoolDownEffect()
	{
		foreach(var i in muzzleFlashParticles)
		{
			ParticleSystem.EmissionModule emit = i.emission;
			emit.enabled = false;
		}
		foreach(var i in coolDownParticles)
		{
			ParticleSystem.EmissionModule emit = i.emission;
			ParticleSystem.MinMaxCurve rate = emit.rateOverTime;
			rate.constant = 0;
			emit.enabled = false;
		}
	}
}
