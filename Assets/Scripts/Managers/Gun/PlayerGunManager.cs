using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerGunManager : MonoBehaviour
{
	public GameObject Player;
	Rigidbody playerRB;

	[Header("Gun Holder elements")]
	public Transform GunHolderPivot;
	public Transform GunHolder;

	[Header("Camera elements")]
	public Camera cam;
	public Animator CameraAnimator_01;
	public Transform PointOfCameraView;

	[Header("Gun Attributes")]
	public float GunMinimumDetectionDistance = 2;
	public Gun GunSlot1;
	public Gun GunSlot2;
	public Transform PointOfGunView;
	Gun CurrentGun;
	DropedGun CurrentDropedGun; // current gun that is on the ground and the player is looking at it
	List<Gun> guns = new List<Gun>();

	[Header("Grenades")]
	public float GrenadeThrowPower = 10;
	DropedGrenade CurrentDropedGrenade;
	DropedGrenade CurrentGrenade;
	List<DropedGrenade> grenades = new List<DropedGrenade>();

	[Header("Spread Attributes")]
	public float MaxSpreadConeSize = 1.5f;
	public float SpreadIncrementSpeed = 0.15f;
	public float SpreadDecrementSpeed = 1;

	[Header("Recoil Attributes")]
	public Transform RecoilTransform;
	public float RecoilBackupSpeed = 2;
	Vector3 RecoilOriginalPos;

	private float LastFireTime = -1;

	[Header("CrossHair")]
	public CrossHairMaster crossHair;

	private void Start()
	{
		Player = GameObject.FindWithTag("Player");
		if(Player)
		{
			playerRB = Player.GetComponent<Rigidbody>();
		}

		// Recoil Init
		RecoilOriginalPos = RecoilTransform.localPosition;
	}

	private void Update()
	{
		if(cam)
		{
			RaycastHit hit = new RaycastHit();
			Ray cameraRay = new Ray(cam.transform.position, cam.transform.forward);

			if(Physics.Raycast(cameraRay, out hit, 500, ~(1 << LayerMask.NameToLayer("Player")))) // if the hit gameObject layer is not Player
			{
				PointOfCameraView.position = hit.point;
			}
			if(CurrentGun)
			{
				Ray gunRay = new Ray(CurrentGun.BulletSpawnPoint.position, CurrentGun.BulletSpawnPoint.forward);
				if(Physics.Raycast(gunRay, out hit, 500, ~(1 << LayerMask.NameToLayer("Player")))) // if the hit gameObject layer is not Player
				{
					PointOfGunView.position = hit.point;
				}
			}
			if(Physics.Raycast(cameraRay, out hit, GunMinimumDetectionDistance))
			{
				if(hit.collider.CompareTag("Gun"))
				{
					CurrentDropedGun = hit.collider.GetComponent<DropedGun>();
				}
				else if(!hit.collider.CompareTag("Gun") || hit.collider.gameObject == null)
				{
					CurrentDropedGun = null;
				}

				if(hit.collider.CompareTag("Grenade"))
				{
					CurrentDropedGrenade = hit.collider.GetComponent<DropedGrenade>();
				}
				else if(!hit.collider.CompareTag("Grenade") || hit.collider.gameObject == null)
				{
					CurrentDropedGrenade = null;
				}
			}
		}

		if(GunHolderPivot && Player)
		{
			GunHolderPivot.transform.position = Player.transform.position;
		}

		if(CurrentDropedGun) // the current droped gun on the ground that we are looking at
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				PickupTheGun();
			}
		}
		if(CurrentDropedGrenade && CurrentDropedGrenade.pickable)
		{
			if(Input.GetKeyDown(KeyCode.E))
			{
				PickupTheGrenade();
			}
		}
	}

	private void LateUpdate()
	{

		if(Input.GetKey(KeyCode.Mouse0))
		{
			if(CurrentGun)
			{
				HandleFire();
			}
		}
		else if(CurrentGun)
		{
			ResetSpread(CurrentGun);
		}

		if(Input.GetKeyUp(KeyCode.Mouse0))
		{
			if(CurrentGrenade)
			{
				ThrowGrenade();
			}
		}

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchWeapon(GunSlot1);
		}

		if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			SwitchWeapon(GunSlot2);
		}

		if(Input.GetKeyDown(KeyCode.Alpha3))
		{
			if(grenades.Any())
			{
				if(!CurrentGrenade)
				{
					SwitchToGrenade(grenades[0]);
				}

				else if(CurrentGrenade && grenades.Count > 1)
				{
					SwitchToNextGrenade();
				}
			}
		}

		ResetRecoil();
	}

	private void SwitchToGrenade(DropedGrenade grenade)
	{
		HolsterCurrentGun();
		HolsterCurrentGrenade();

		CurrentGrenade = grenade;
		CurrentGrenade.gameObject.SetActive(true);
	}

	private void SwitchToNextGrenade()
	{
		int nextIndex = grenades.IndexOf(CurrentGrenade);

		// if current grenade is the last grenade in the grenade list go at the first of the list
		if(nextIndex == grenades.Count - 1)
		{
			nextIndex = 0;
		}
		// else switch to next grenade
		else
		{
			nextIndex++;
		}

		SwitchToGrenade(grenades[nextIndex]);
	}

	private void ResetRecoil()
	{
		if(RecoilTransform.position != RecoilOriginalPos)
		{
			float dist = Vector3.Distance(RecoilTransform.localPosition, RecoilOriginalPos);
			RecoilTransform.localPosition = Vector3.Lerp(RecoilTransform.localPosition, RecoilOriginalPos, Time.deltaTime * RecoilBackupSpeed / dist);
		}
	}

	private void HandleFire()
	{
		if(Time.time > LastFireTime + 1 / CurrentGun.FireRate)
		{
			ShootBullet();
			CurrentGun.EmitMuzzleFlash();
			HandleRecoil();
			DropShell();
			CameraAnimation();
			IncreaseSpread(CurrentGun);
			LastFireTime = Time.time;
		}
	}

	private void ThrowGrenade()
	{
		GameObject grenade = Instantiate(CurrentGrenade.grenade, CurrentGrenade.transform.position, CurrentGrenade.transform.rotation).gameObject;
		Rigidbody grenadeRB = grenade.GetComponent<Rigidbody>();
		Vector3 playerVelocity = Vector3.zero;
		if(playerRB)
		{
			playerVelocity = playerRB.velocity;
		}
		grenadeRB.AddForce(GunHolder.forward * GrenadeThrowPower);
		grenadeRB.velocity += playerVelocity;
		grenades.Remove(CurrentGrenade);
		Destroy(CurrentGrenade.gameObject);
		if(grenades.Any())
		{
			SwitchToNextGrenade();
		}
		else
		{
			SwitchWeapon(GunSlot1);
		}
	}

	private void IncreaseSpread(Gun gun)
	{
		if(gun.ConeSpreadSize < MaxSpreadConeSize)
		{
			gun.ConeSpreadSize += SpreadIncrementSpeed;
		}
		else
		{
			gun.ConeSpreadSize = MaxSpreadConeSize;
		}
	}

	private void ResetSpread(Gun gun)
	{
		if(CurrentGun)
		{
			if(gun.ConeSpreadSize != 0)
			{
				gun.ConeSpreadSize = Mathf.Lerp(gun.ConeSpreadSize, 0,
														Time.deltaTime * SpreadDecrementSpeed / gun.ConeSpreadSize);
			}
		}
	}

	private void HandleCrossHair(float additiveValue)
	{
		foreach(var s in crossHair.sliders)
		{
			s.value += additiveValue;
		}
	}

	private void InitCrosshair()
	{
		if(CurrentGun)
		{
			crossHair.gameObject.SetActive(true);
			foreach(var s in crossHair.sliders)
			{
				s.maxValue = CurrentGun.VerticalRecoil * 10;
			}
		}
		else
		{
			crossHair.gameObject.SetActive(false);
		}
	}

	private void PickupTheGun()
	{
		if(GunHolder)
		{
			GameObject gunInstance = Instantiate(CurrentDropedGun.gun.gameObject, CurrentDropedGun.transform.position, CurrentDropedGun.transform.rotation);
			Gun gun = gunInstance.GetComponent<Gun>();

			guns.Add(gun);

			PutGunAtSlot(gun);

			HolsterCurrentGrenade();

			HolsterCurrentGun();

			InitPickedGun(gun);

			SwitchWeapon(gun);

			InitCrosshair();

			Destroy(CurrentDropedGun.gameObject);
		}
	}

	private void PickupTheGrenade()
	{
		if(GunHolder)
		{
			StartCoroutine(Co_PickupGrenade());
		}
	}

	private IEnumerator Co_PickupGrenade()
	{
		HolsterCurrentGrenade();

		DropedGrenade pickedGrenade = CurrentDropedGrenade;
		pickedGrenade.pickable = false;
		pickedGrenade.GetComponent<Rigidbody>().isKinematic = true;

		bool grenadeIsOnRightPos = false;
		while(!grenadeIsOnRightPos)
		{
			float dist = Vector3.Distance(pickedGrenade.transform.position, GunHolder.transform.position);
			float angle = Quaternion.Angle(pickedGrenade.transform.rotation, GunHolder.transform.rotation);
			if(dist > 0.5f || angle > 1f)
			{
				float speed = 10f;

				pickedGrenade.transform.position = Vector3.MoveTowards(pickedGrenade.transform.position, GunHolder.transform.position, Time.deltaTime * speed);
				pickedGrenade.transform.rotation = Quaternion.RotateTowards(pickedGrenade.transform.rotation, GunHolder.transform.rotation, Time.deltaTime * speed * 100);
			}
			else
			{
				pickedGrenade.transform.position = GunHolder.position;
				pickedGrenade.transform.rotation = GunHolder.rotation;

				transform.parent = GunHolder.transform;

				grenadeIsOnRightPos = true;
			}
			yield return null;
		}
		pickedGrenade.transform.parent = GunHolder;

		pickedGrenade.GetComponent<Collider>().enabled = false;

		grenades.Add(pickedGrenade);
		if(CurrentGun)
		{
			pickedGrenade.gameObject.SetActive(false);
		}
		else
		{
			SwitchToGrenade(grenades.Where(x => x == pickedGrenade).FirstOrDefault());
		}
	}

	private void HolsterCurrentGrenade()
	{
		if(CurrentGrenade)
		{
			CurrentGrenade.gameObject.SetActive(false);
			CurrentGrenade = null;
		}
	}

	private void HolsterCurrentGun()
	{
		if(CurrentGun)
		{
			CurrentGun.gameObject.SetActive(false);
			CurrentGun = null;
		}
	}

	private void InitPickedGun(Gun gun)
	{
		gun.PlayerGunHolder = GunHolder.transform;

		gun.Ammo = CurrentDropedGun.Ammo;
		gun.MagazineAmmo = CurrentDropedGun.MagazineAmmo;
	}

	private void PutGunAtSlot(Gun gun)
	{
		if(!GunSlot1 && !GunSlot2)
		{
			GunSlot1 = gun;
		}

		else if(GunSlot1 && !GunSlot2)
		{
			GunSlot2 = gun;
		}

		else if(GunSlot1 && GunSlot2)
		{
			if(CurrentGun == GunSlot1 || CurrentGun == null)
			{
				DropGun(GunSlot1);
				GunSlot1 = gun;
			}
			else if(CurrentGun == GunSlot2)
			{
				DropGun(GunSlot2);
				GunSlot2 = gun;
			}
		}
	}

	private void SwitchWeapon(Gun gun)
	{
		if(CurrentGun == gun)
		{
			return;
		}

		if(!guns.Contains(gun))
		{
			return;
		}

		HolsterCurrentGun();
		HolsterCurrentGrenade();

		gun.gameObject.SetActive(true);
		CurrentGun = gun;
	}

	private void DropGun(Gun gunSlot)
	{
		var dropedGun = Instantiate(gunSlot.dropedGun, gunSlot.transform.position, gunSlot.transform.rotation);
		dropedGun.Ammo = gunSlot.Ammo;
		dropedGun.MagazineAmmo = gunSlot.MagazineAmmo;
		Destroy(gunSlot.gameObject);
		gunSlot = null;
	}

	private void CameraAnimation()
	{
		CameraAnimator_01.SetTrigger("Shoot");
	}

	private void ShootBullet()
	{
		float xSpread = Random.Range(-1, 1);
		float ySpread = Random.Range(-1, 1);
		Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * CurrentGun.ConeSpreadSize;
		Quaternion rotation = Quaternion.Euler(spread) * CurrentGun.BulletSpawnPoint.rotation;
		Instantiate(CurrentGun.bullet, CurrentGun.BulletSpawnPoint.position, rotation);
	}

	private void DropShell()
	{
		CurrentGun.BulletShellParticle.Emit(1);
	}

	private void HandleRecoil()
	{
		float hRecoil = CurrentGun.HorizontalRecoil;
		float vRecoil = CurrentGun.VerticalRecoil;

		float Rnd_HRecoil = Random.Range(-hRecoil, hRecoil);
		float Rnd_VRecoil = Random.Range(vRecoil - (vRecoil / 5), vRecoil + (vRecoil / 5));

		RecoilTransform.localPosition += new Vector3(Rnd_HRecoil, Rnd_VRecoil, 0);

		HandleCrossHair(Rnd_VRecoil * 2);
	}
}

