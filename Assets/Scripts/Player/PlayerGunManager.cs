using BallShooter.Player;
using UnityEngine;

public class PlayerGunManager : MonoBehaviour
{
	private PlayerManager playerManager;
	private bool _firing = false;
	private float LastFireTime = -1;
	private Gun_New gun;

	public void Initiate(PlayerManager playerManager)
	{
		this.playerManager = playerManager;

		playerManager.InputManager.playerControls.Ground.Fire.performed += (UnityEngine.InputSystem.InputAction.CallbackContext context) =>
		{
			_firing = true;
		};

		playerManager.EquipmentManager.e_OnEquipmentSwitched.AddListener(delegate
		{
			var selectedEquipment = playerManager.EquipmentManager.SelectedEquipment;
			if (selectedEquipment != null && selectedEquipment is Gun_New)
			{
				gun = (Gun_New)selectedEquipment;
			}
		});

		playerManager.InputManager.playerControls.Ground.Fire.canceled += (UnityEngine.InputSystem.InputAction.CallbackContext context) =>
		{
			_firing = false;
		};
	}

	private void Update()
	{
		if (_firing && gun)
		{
			HandleFire();
		}
	}

	private void HandleFire()
	{
		if (Time.time > LastFireTime + 1 / gun.specifics.FireRate)
		{
			gun.Fire();

			playerManager.RecoilHandler.GenerateRecoil(gun.specifics.recoilInfo.HorizontalRecoil, gun.specifics.recoilInfo.VerticalRecoil);
			playerManager.CrosshairManager.HandleCrossHair(gun.specifics.recoilInfo.VerticalRecoil);
			LastFireTime = Time.time;
		}
	}
}
