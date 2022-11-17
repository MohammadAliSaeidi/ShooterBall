using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace BallShooter.Player
{
	public class PlayerEquipmentManager : NetworkBehaviour
	{
		#region Variables

		#region Components

		private PlayerManager playerManager;

		#endregion

		#region Events

		public UnityEvent_Equipment e_OnEquipmentSwitched = new UnityEvent_Equipment();

		#endregion

		[SerializeField]
		private LayerMask EquipmentLayerMask;

		[SerializeField]
		private Camera _camera;

		[SerializeField]
		private Transform cameraLookingPoint;

		[SerializeField]
		private float EquipmentDetectionDist = 2;

		[SerializeField]
		private LayerMask _cameraPointOfLookingLayerMask;

		[SerializeField]
		private GunHolderPivot GunHolderPivot;

		public Equipment SelectedEquipment { get; private set; }

		public GunHolder gunHolder;

		private GameObject _player;
		private Rigidbody _playerRB;
		private DroppedEquipment _equipmentOnGround;
		private List<Gun> _guns = new List<Gun>();
		private List<Throwable> _throwables = new List<Throwable>();
		private Vector3 equipmentCurVel;

		#endregion

		public void Initiate(PlayerManager playerManager)
		{
			this.playerManager = playerManager;

			if (!IsOwner) return;

			playerManager.InputManager.playerControls.Ground.Intract.performed +=
				delegate
				{
					if (_equipmentOnGround != null)
					{
						TryPickupEquipment(_equipmentOnGround);
					}
				};

			_player = playerManager.Orb;
			if (_player)
			{
				_playerRB = _player.GetComponent<Rigidbody>();
			}
		}

		private void Update()
		{
			if (!IsOwner) return;

			if (_camera)
			{
				var hit = new RaycastHit();
				var cameraRay = new Ray(_camera.transform.position, _camera.transform.forward);

				if (Physics.Raycast(cameraRay, out hit, EquipmentDetectionDist, layerMask: EquipmentLayerMask))
				{
					if (hit.collider.TryGetComponent(out DroppedEquipment equipment) && _equipmentOnGround != equipment)
					{
						_equipmentOnGround = equipment;
						Debug.Log(_equipmentOnGround);
					}
				}
				else
				{
					_equipmentOnGround = null;
				}
			}
			else
			{
				Debug.LogError($"Camera field is empty on {this.name}");
			}
		}

		private void TryPickupEquipment(DroppedEquipment equipment)
		{
			InstantiateEquipmentServerRpc(equipment.networkObject, NetworkObject);
			equipment.EquipServerRpc();
		}

		[ServerRpc]
		private void InstantiateEquipmentServerRpc(NetworkObjectReference droppedEquipmentRef, NetworkObjectReference senderNetworkObjectRef)
		{
			InstantiateEquipmentClientRpc(droppedEquipmentRef, senderNetworkObjectRef);
		}

		[ClientRpc]
		private void InstantiateEquipmentClientRpc(NetworkObjectReference droppedEquipmentRef, NetworkObjectReference senderNetworkObjectRef)
		{
			if (droppedEquipmentRef.TryGet(out NetworkObject droppedEquipment) &&
				senderNetworkObjectRef.TryGet(out NetworkObject senderNetworkObject))
			{
				var equipmentPrefab = droppedEquipment.GetComponent<DroppedEquipment>().EquipmentPrefab;
				var gunHolder = senderNetworkObject.GetComponent<PlayerEquipmentManager>().gunHolder;
				var playerEquipmentManager = senderNetworkObject.GetComponent<PlayerEquipmentManager>();
				var equipment = Instantiate(equipmentPrefab, gunHolder.transform.position, gunHolder.transform.rotation, gunHolder.transform);
				PutEquipmentToSlot(equipment, playerEquipmentManager);
				SwitchEquipment(equipment);
			}
		}

		private void PutEquipmentToSlot(Equipment equipment, PlayerEquipmentManager playerEquipmentManager)
		{
			if (equipment is Gun gun)
			{
				playerEquipmentManager.PutGunToSlots(gun, playerEquipmentManager);
			}
		}

		private void PutGunToSlots(Gun gun, PlayerEquipmentManager playerEquipmentManager)
		{
			if (playerEquipmentManager._guns.Count < playerEquipmentManager.playerManager.playerConfig.MaxGunSlots)
			{
				playerEquipmentManager._guns.Add(gun);
			}

			else if (SelectedEquipment is Gun selectedGun)
			{
				DropGun(selectedGun);
			}

			else
			{
				DropGun(_guns[0]);
			}
		}

		private void DropGun(Gun gun)
		{
			if (_guns.Contains(gun))
			{
				_guns.Remove(gun);
			}
		}

		private void HolsterSelectedEquipment()
		{
			if (SelectedEquipment != null)
			{
				(SelectedEquipment as MonoBehaviour).gameObject.SetActive(false);
				SelectedEquipment = null;
			}
		}

		private void SwitchEquipment(Equipment equipment)
		{
			HolsterSelectedEquipment();
			SelectedEquipment = equipment;
			equipment.gameObject.SetActive(true);

			e_OnEquipmentSwitched?.Invoke(equipment);
		}
	}
}