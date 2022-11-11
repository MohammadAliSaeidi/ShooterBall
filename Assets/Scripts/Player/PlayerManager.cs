using UnityEngine;

[RequireComponent(typeof(PlayerEquipmentManager), typeof(RecoilHandler), typeof(PlayerInputManager))]
[RequireComponent(typeof(CrosshairManager), typeof(PlayerGunManager))]
public class PlayerManager : MonoBehaviour
{
	#region Components

	public PlayerEquipmentManager equipmentManager { get; private set; }
	public CrosshairManager crosshairManager { get; private set; }
	public RecoilHandler recoilHandler { get; private set; }
	public PlayerInputManager inputManager { get; private set; }
	public PlayerGunManager GunManager { get; private set; }

	#endregion

	private void Awake()
	{
		equipmentManager = GetComponent<PlayerEquipmentManager>();
		crosshairManager = GetComponent<CrosshairManager>();
		inputManager = GetComponent<PlayerInputManager>();
		recoilHandler = GetComponent<RecoilHandler>();
		GunManager = GetComponent<PlayerGunManager>();

		equipmentManager.Initiate(this);
		GunManager.Initiate(this);
	}
}
