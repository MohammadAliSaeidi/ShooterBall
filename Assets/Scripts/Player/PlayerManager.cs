using Unity.Netcode;
using UnityEngine;

namespace BallShooter.Player
{
	[RequireComponent(typeof(PlayerEquipmentManager), typeof(RecoilHandler), typeof(PlayerInputManager))]
	[RequireComponent(typeof(CrosshairManager), typeof(PlayerGunManager), typeof(CameraLookPointHandler))]
	public class PlayerManager : NetworkBehaviour
	{
		[SerializeField]
		private PlayerConfig _playerConfig;
		public PlayerConfig playerConfig { get { return _playerConfig; } }

		#region Components

		public PlayerEquipmentManager EquipmentManager { get; private set; }

		public CrosshairManager CrosshairManager { get; private set; }

		public RecoilHandler RecoilHandler { get; private set; }

		public PlayerInputManager InputManager { get; private set; }

		public PlayerGunManager GunManager { get; private set; }

		public CameraLookPointHandler CameraLookPointHandler { get; private set; }

		public PlayerMovement PlayerMovement { get; private set; }

		#endregion

		[SerializeField] private GameObject[] _objectsToDestroy;
		public GameObject Orb;

		private void Awake()
		{
			EquipmentManager = GetComponent<PlayerEquipmentManager>();
			CrosshairManager = GetComponent<CrosshairManager>();
			InputManager = GetComponent<PlayerInputManager>();
			RecoilHandler = GetComponent<RecoilHandler>();
			GunManager = GetComponent<PlayerGunManager>();
			CameraLookPointHandler = GetComponent<CameraLookPointHandler>();
			PlayerMovement = GetComponent<PlayerMovement>();
		}

		private void Start()
		{ 

			//if (playerConfig.IsMultiplayer && !IsOwner) return;

			Initiate();
		}

		private void Update()
		{
			if (playerConfig.IsMultiplayer && !IsOwner)
			{
				Orb.tag = "Untagged";
				Orb.layer = 0;
				//EquipmentManager.enabled = false;
				CrosshairManager = GetComponent<CrosshairManager>();
				InputManager.enabled = false;
				RecoilHandler.enabled = false;
				//GunManager.enabled = false;
				CameraLookPointHandler.enabled = false;
				PlayerMovement.enabled = false;
				DestroyUnnecessaryParts();
				this.enabled = false;
			}
		}

		private void Initiate()
		{
			EquipmentManager.Initiate(this);
			GunManager.Initiate(this);
		}

		private void DestroyUnnecessaryParts()
		{
			foreach (var obj in _objectsToDestroy)
			{
				if (obj != null)
				{
					Destroy(obj);
				}
			}
		}
	}
}