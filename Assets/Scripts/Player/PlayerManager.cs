using UnityEngine;

namespace BallShooter.Player
{
	[RequireComponent(typeof(PlayerEquipmentManager_new), typeof(RecoilHandler), typeof(PlayerInputManager))]
	[RequireComponent(typeof(CrosshairManager), typeof(PlayerGunManager), typeof(CameraLookPointHandler))]
	public class PlayerManager : MonoBehaviour
	{
		[SerializeField]
		private PlayerConfig _playerConfig;
		public PlayerConfig playerConfig { get { return _playerConfig; } }

		#region Components

		public PlayerEquipmentManager_new EquipmentManager { get; private set; }

		public CrosshairManager CrosshairManager { get; private set; }

		public RecoilHandler RecoilHandler { get; private set; }

		public PlayerInputManager InputManager { get; private set; }

		public PlayerGunManager GunManager { get; private set; }

		public CameraLookPointHandler CameraLookPointHandler { get; private set; }

		#endregion

		private void Awake()
		{
			EquipmentManager = GetComponent<PlayerEquipmentManager_new>();
			CrosshairManager = GetComponent<CrosshairManager>();
			InputManager = GetComponent<PlayerInputManager>();
			RecoilHandler = GetComponent<RecoilHandler>();
			GunManager = GetComponent<PlayerGunManager>();
			CameraLookPointHandler = GetComponent<CameraLookPointHandler>();
		}

		private void Start()
		{
			Initiate();
		}

		private void Initiate()
		{
			EquipmentManager.Initiate(this);
			GunManager.Initiate(this);
		}
	}
}