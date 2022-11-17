using BallShooter.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGunManager : NetworkBehaviour
{
	private PlayerManager playerManager;
	private bool _firing = false;
	private float LastFireTime = -1;
	private Gun gun;

	public GameObject _testBulletPrefab;
	public Transform _testSpawnPoint;

	public void Initiate(PlayerManager playerManager)
	{
		this.playerManager = playerManager;

		if (!IsOwner) return;

		playerManager.EquipmentManager.e_OnEquipmentSwitched.AddListener(delegate
		{
			var selectedEquipment = playerManager.EquipmentManager.SelectedEquipment;
			if (selectedEquipment != null && selectedEquipment is Gun)
			{
				gun = (Gun)selectedEquipment;
			}
		});


		playerManager.InputManager.playerControls.Ground.Fire.performed +=
			(InputAction.CallbackContext context) => { FiringServerRpc(); };

		playerManager.InputManager.playerControls.Ground.Fire.canceled +=
			(InputAction.CallbackContext context) => { StopFiringServerRpc(); };
	}

	[ServerRpc]
	private void FiringServerRpc()
	{
		FiringClientRpc();
	}

	[ClientRpc]
	private void FiringClientRpc()
	{
		_firing = true;
	}

	[ServerRpc]
	private void StopFiringServerRpc()
	{
		StopFiringClientRpc();
	}

	[ClientRpc]
	private void StopFiringClientRpc()
	{
		_firing = false;
	}

	private void Update()
	{
		if (!IsServer) return;

		if (_firing && gun != null)
		{
			//HandleFire();
			HandleFireClientRpc();
		}
	}

	private void HandleFire()
	{
		if (Time.time > LastFireTime + 1 / gun.Specifics.FireRate)
		{
			gun.Fire(IsServer);

			playerManager.RecoilHandler.GenerateRecoil(gun.Specifics.recoilInfo.HorizontalRecoil, gun.Specifics.recoilInfo.VerticalRecoil);
			playerManager.CrosshairManager.HandleCrossHair(gun.Specifics.recoilInfo.VerticalRecoil);
			LastFireTime = Time.time;
		}
		HandleFireServerRpc();
	}

	[ServerRpc]
	private void HandleFireServerRpc()
	{
		HandleFireClientRpc();
	}

	[ClientRpc]
	private void HandleFireClientRpc()
	{
		if (Time.time > LastFireTime + 1 / gun.Specifics.FireRate)
		{
			Instantiate(_testBulletPrefab, _testSpawnPoint.position, _testSpawnPoint.rotation);

			LastFireTime = Time.time;
		}
	}
}
