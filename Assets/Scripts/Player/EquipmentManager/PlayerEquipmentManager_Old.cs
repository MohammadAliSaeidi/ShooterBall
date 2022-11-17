using UnityEngine;

public class PlayerEquipmentManager_Old : MonoBehaviour
{
	/*
	#region Components

	private PlayerManager playerManager;

	#endregion

	public IEquipment CurrentEquipment { get; private set; }
	private List<IEquipment> _equipments = new List<IEquipment>();






	public GameObject Player;
	private Rigidbody playerRB;

	[SerializeField] private LayerMask _cameraPointOfLookingLayerMask;
	private CameraLookPointHandler cameraLookPointHandler;

	[Header("Gun Holder elements")]
	public Transform GunHolderPivot;
	public Transform GunHolder;

	[Header("Camera elements")]
	public Camera cam;
	[SerializeField] private Transform cameraLookingPoint;

	[Header("Gun Attributes")]
	public float ItemDetectionDist = 2;
	public Gun GunSlot1;
	public Gun GunSlot2;
	public Transform PointOfGunView;
	public Gun CurrentGun { get; private set; }
	private DropedGun CurrentDropedGun; // current gun that is on the ground and the player is looking at it
	private List<Gun> guns = new List<Gun>();

	[Header("Grenades")]
	public float GrenadeThrowPower = 10;
	private DropedGrenade CurrentDropedGrenade;
	private DropedGrenade CurrentGrenade;
	private List<DropedGrenade> grenades = new List<DropedGrenade>();

	[Header("Spread Attributes")]
	public float MaxSpreadConeSize = 1.5f;
	public float SpreadIncrementSpeed = 0.15f;
	public float SpreadDecrementSpeed = 1;

	private void Awake()
	{
		cameraLookPointHandler = GetComponent<CameraLookPointHandler>();
	}

	private void Start()
	{
		Player = GameObject.FindWithTag("Player");
		if (Player)
		{
			playerRB = Player.GetComponent<Rigidbody>();
		}
	}

	private void Update()
	{
		if (cam)
		{
			RaycastHit hit = new RaycastHit();
			Ray cameraRay = new Ray(cam.transform.position, cam.transform.forward);

			cameraLookingPoint.position = cameraLookPointHandler.PointOfCameraLooking;

			if (CurrentGun)
			{
				Ray gunRay = new Ray(CurrentGun.BulletSpawnPoint.position, CurrentGun.BulletSpawnPoint.forward);
				if (Physics.Raycast(gunRay, out hit, 500, _cameraPointOfLookingLayerMask)) // if the hit gameObject layer is not Player
				{
					PointOfGunView.position = hit.point;
				}
			}

			if (Physics.Raycast(cameraRay, out hit, ItemDetectionDist))
			{
				if (hit.collider.CompareTag("Gun"))
				{
					CurrentDropedGun = hit.collider.GetComponent<DropedGun>();
				}
				else if (!hit.collider.CompareTag("Gun") || hit.collider.gameObject == null)
				{
					CurrentDropedGun = null;
				}

				if (hit.collider.CompareTag("Grenade"))
				{
					CurrentDropedGrenade = hit.collider.GetComponent<DropedGrenade>();
				}
				else if (!hit.collider.CompareTag("Grenade") || hit.collider.gameObject == null)
				{
					CurrentDropedGrenade = null;
				}
			}
		}

		if (GunHolderPivot && Player)
		{
			GunHolderPivot.transform.position = Player.transform.position;
		}

		if (CurrentDropedGun) // the current droped gun on the ground that we are looking at
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				PickupTheGun();
			}
		}

		if (CurrentDropedGrenade && !CurrentDropedGrenade.hasBeenPicked)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				PickupTheGrenade();
			}
		}
	}

	private void LateUpdate()
	{

		if (Input.GetKey(KeyCode.Mouse0))
		{
			if (CurrentGun)
			{
				HandleFire();
			}
		}
		else if (CurrentGun)
		{
			ResetSpread(CurrentGun);
		}

		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			if (CurrentGrenade)
			{
				ThrowGrenade();
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SwitchWeapon(GunSlot1);
		}

		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SwitchWeapon(GunSlot2);
		}

		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SwitchToGrenade();
		}


	}

	public void Initiate(PlayerManager playerManager)
	{
		this.playerManager = playerManager;
	}

	private void SwitchToGrenade()
	{
		if (grenades.Any())
		{
			DropedGrenade grenade = null;

			if (CurrentGrenade == null)
			{

				grenade = grenades[0];
			}

			else if (grenades.Count > 1)
			{
				// Switch to next grenade
				int nextIndex = grenades.IndexOf(CurrentGrenade);
				if (nextIndex != grenades.Count - 1)
					nextIndex++;
				else
					nextIndex = 0;

				grenade = grenades[nextIndex];
			}

			SwitchToGrenade(grenade);
		}
	}

	private void SwitchToGrenade(DropedGrenade grenade)
	{
		if (grenade != null)
		{
			HolsterCurrentGun();
			HolsterCurrentGrenade();

			CurrentGrenade = grenade;
			CurrentGrenade.gameObject.SetActive(true);
		}
	}

	private void HandleFire()
	{
		//if (Time.time > LastFireTime + 1 / CurrentGun.FireRate)
		//{
		//	ShootBullet();
		//	CurrentGun.EmitMuzzleFlash();
		//	playerManager.recoilHandler.GenerateRecoil(CurrentGun.HorizontalRecoil, CurrentGun.VerticalRecoil);
		//	playerManager.crosshairManager.HandleCrossHair(CurrentGun.VerticalRecoil);
		//	CurrentGun.BulletShellParticle.Emit(1);
		//	IncreaseSpread(CurrentGun);
		//	LastFireTime = Time.time;
		//}
	}

	private void ThrowGrenade()
	{
		GameObject grenade = Instantiate(CurrentGrenade.grenade, CurrentGrenade.transform.position, CurrentGrenade.transform.rotation).gameObject;
		Rigidbody grenadeRB = grenade.GetComponent<Rigidbody>();
		Vector3 playerVelocity = Vector3.zero;
		if (playerRB)
		{
			playerVelocity = playerRB.velocity;
		}
		grenadeRB.AddForce(GunHolder.forward * GrenadeThrowPower);
		grenadeRB.velocity += playerVelocity;
		grenades.Remove(CurrentGrenade);
		Destroy(CurrentGrenade.gameObject);
		if (grenades.Any())
		{
			SwitchToGrenade();
		}
		else
		{
			SwitchWeapon(GunSlot1);
		}
	}

	private void IncreaseSpread(Gun gun)
	{
		if (gun.specifics.recoilInfo.MaxConeSpreadSize < MaxSpreadConeSize)
		{
			gun.specifics.recoilInfo.MaxConeSpreadSize += SpreadIncrementSpeed;
		}
		else
		{
			gun.specifics.recoilInfo.MaxConeSpreadSize = MaxSpreadConeSize;
		}
	}

	private void ResetSpread(Gun gun)
	{
		if (CurrentGun)
		{
			if (gun.specifics.recoilInfo.MaxConeSpreadSize != 0)
			{
				gun.specifics.recoilInfo.MaxConeSpreadSize = Mathf.Lerp(gun.specifics.recoilInfo.MaxConeSpreadSize, 0,
														Time.deltaTime * SpreadDecrementSpeed / gun.specifics.recoilInfo.MaxConeSpreadSize);
			}
		}
	}

	private void PickupTheGun()
	{
		if (GunHolder)
		{
			GameObject gunInstance = Instantiate(CurrentDropedGun.gun.gameObject, CurrentDropedGun.transform.position, CurrentDropedGun.transform.rotation);
			Gun gun = gunInstance.GetComponent<Gun>();

			guns.Add(gun);

			PutGunAtSlot(gun);

			HolsterCurrentGrenade();

			HolsterCurrentGun();

			InitPickedGun(gun);

			SwitchWeapon(gun);

			playerManager.CrosshairManager.InitCrosshair(gun);

			Destroy(CurrentDropedGun.gameObject);
		}
	}

	private void PickupTheGrenade()
	{
		if (GunHolder)
		{
			StartCoroutine(Co_PickupGrenade());
		}
	}

	private IEnumerator Co_PickupGrenade()
	{
		HolsterCurrentGrenade();

		DropedGrenade pickedGrenade = CurrentDropedGrenade;
		pickedGrenade.hasBeenPicked = true;
		pickedGrenade.GetComponent<Rigidbody>().isKinematic = true;

		bool grenadeIsOnRightPos = false;
		while (!grenadeIsOnRightPos)
		{
			float dist = Vector3.Distance(pickedGrenade.transform.position, GunHolder.transform.position);
			float angle = Quaternion.Angle(pickedGrenade.transform.rotation, GunHolder.transform.rotation);
			if (dist > 0.5f || angle > 1f)
			{
				float speed = 10f;

				pickedGrenade.transform.SetPositionAndRotation(
					position: Vector3.MoveTowards(pickedGrenade.transform.position, GunHolder.transform.position, Time.deltaTime * speed),
					rotation: Quaternion.RotateTowards(pickedGrenade.transform.rotation, GunHolder.transform.rotation, Time.deltaTime * speed * 100));
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
		if (CurrentGun)
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
		if (CurrentGrenade)
		{
			CurrentGrenade.gameObject.SetActive(false);
			CurrentGrenade = null;
		}
	}

	private void HolsterCurrentGun()
	{
		if (CurrentGun)
		{
			CurrentGun.gameObject.SetActive(false);
			CurrentGun = null;
		}
	}

	private void InitPickedGun(Gun gun)
	{
		gun.PlayerGunHolder = GunHolder.transform;

		gun.CurrentMagazineAmmoCount = CurrentDropedGun.Ammo;
		gun.ExtraAmmoCount = CurrentDropedGun.MagazineAmmo;
	}

	private void PutGunAtSlot(Gun gun)
	{
		if (!GunSlot1 && !GunSlot2)
		{
			GunSlot1 = gun;
		}

		else if (GunSlot1 && !GunSlot2)
		{
			GunSlot2 = gun;
		}

		else if (GunSlot1 && GunSlot2)
		{
			if (CurrentGun == GunSlot1 || CurrentGun == null)
			{
				DropGun(GunSlot1);
				GunSlot1 = gun;
			}
			else if (CurrentGun == GunSlot2)
			{
				DropGun(GunSlot2);
				GunSlot2 = gun;
			}
		}
	}

	private void SwitchWeapon(Gun gun)
	{
		if (CurrentGun == gun)
		{
			return;
		}

		if (!guns.Contains(gun))
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
		//dropedGun.Ammo = gunSlot.Ammo;
		//dropedGun.MagazineAmmo = gunSlot.MagazineAmmo;
		Destroy(gunSlot.gameObject);
		gunSlot = null;
	}

	private void ShootBullet()
	{
		float xSpread = Random.Range(-1, 1);
		float ySpread = Random.Range(-1, 1);
		//Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * CurrentGun.ConeSpreadSize;
		//Quaternion rotation = Quaternion.Euler(spread) * CurrentGun.BulletSpawnPoint.rotation;
		//Instantiate(CurrentGun.bullet, CurrentGun.BulletSpawnPoint.position, rotation);
	}*/
}

