using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
	private PlayerManager playerManager;
	private bool _firing = false;
	private float LastFireTime = -1;

	public void Initiate(PlayerManager playerManager)
	{
		this.playerManager = playerManager;

		playerManager.inputManager.playerControls.Ground.Fire.performed += (UnityEngine.InputSystem.InputAction.CallbackContext context) =>
		{
			_firing = true;
		};

		playerManager.inputManager.playerControls.Ground.Fire.canceled += (UnityEngine.InputSystem.InputAction.CallbackContext context) =>
		{
			_firing = false;
		};
	}

	private void Update()
	{
		if (_firing)
		{
			HandleFire();
		}
	}

	private void HandleFire()
	{
		if (Time.time > LastFireTime + 1 / playerManager.equipmentManager.CurrentGun.FireRate)
		{
			ShootBullet();
			//playerManager.equipmentManager.CurrentGun CurrentGun.EmitMuzzleFlash();
			//playerManager.recoilHandler.GenerateRecoil(CurrentGun.HorizontalRecoil, CurrentGun.VerticalRecoil);
			//playerManager.crosshairManager.HandleCrossHair(CurrentGun.VerticalRecoil);
			//CurrentGun.BulletShellParticle.Emit(1);
			//CameraAnimator_01.SetTrigger("Shoot");
			//IncreaseSpread(CurrentGun);
			//LastFireTime = Time.time;
		}
	}

	private void ShootBullet()
	{
		float xSpread = Random.Range(-1, 1);
		float ySpread = Random.Range(-1, 1);
		//Vector3 spread = new Vector3(xSpread, ySpread, 0.0f).normalized * CurrentGun.ConeSpreadSize;
		//Quaternion rotation = Quaternion.Euler(spread) * CurrentGun.BulletSpawnPoint.rotation;
		//Instantiate(CurrentGun.bullet, CurrentGun.BulletSpawnPoint.position, rotation);
	}
}
