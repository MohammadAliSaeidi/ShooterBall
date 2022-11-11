using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallShooter.Player
{
	public class PlayerEquipmentManager_new : MonoBehaviour
	{
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
		private Transform PointOfGunView;

		[SerializeField]
		private LayerMask _cameraPointOfLookingLayerMask;

		[SerializeField]
		private Transform GunHolderPivot;

		[SerializeField]
		private Transform GunHolder;

		public IEquipment SelectedEquipment { get; private set; }

		private GameObject _player;
		private Rigidbody _playerRB;
		private IEquipment _equipmentOnGround;
		private List<Gun_New> _guns = new List<Gun_New>();
		private List<Throwable> _throwables = new List<Throwable>();
		private Coroutine _moveEquipmentToPlayer = null;
		private Vector3 equipmentCurVel;

		public void Initiate(PlayerManager playerManager)
		{
			this.playerManager = playerManager;

			playerManager.InputManager.playerControls.Ground.Intract.performed +=
				delegate
				{
					if (_equipmentOnGround != null)
					{
						TryPickupEquipment(_equipmentOnGround);
					}
				};
		}

		private void Start()
		{
			_player = GameObject.FindWithTag("Player");
			if (_player)
			{
				_playerRB = _player.GetComponent<Rigidbody>();
			}
		}

		private void Update()
		{
			if (_camera)
			{
				var hit = new RaycastHit();
				var cameraRay = new Ray(_camera.transform.position, _camera.transform.forward);

				if (Physics.Raycast(cameraRay, out hit, EquipmentDetectionDist, layerMask: EquipmentLayerMask))
				{
					if (hit.collider.TryGetComponent(out IEquipment equipment) && _equipmentOnGround != equipment)
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


			if (GunHolderPivot && _player)
			{
				GunHolderPivot.transform.position = _player.transform.position;
			}


		}

		private void TryPickupEquipment(IEquipment equipment)
		{
			equipment.Equip();

			if (equipment is Gun_New)
			{
				var gun = (Gun_New)equipment;
				PutGunToSlots(gun);
				gun.Initiate(GunHolder.transform);
			}

			SwitchEquipment(equipment);

			if (_moveEquipmentToPlayer != null)
				StopCoroutine(_moveEquipmentToPlayer);
			_moveEquipmentToPlayer = StartCoroutine(Co_MoveEquipmentToPlayer(equipment));
		}

		private void PutGunToSlots(Gun_New gun)
		{
			if (_guns.Count < playerManager.playerConfig.MaxGunSlots)
			{
				_guns.Add(gun);
			}

			else if (SelectedEquipment is Gun_New selectedGun)
			{
				DropedGun(selectedGun);
			}

			else
			{
				DropedGun(_guns[0]);
			}
		}

		private void DropedGun(Gun_New gun)
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

		private void SwitchEquipment(IEquipment equipment)
		{
			HolsterSelectedEquipment();
			SelectedEquipment = equipment;
			(equipment as MonoBehaviour).gameObject.SetActive(true);

			e_OnEquipmentSwitched?.Invoke(equipment);
		}

		private IEnumerator Co_MoveEquipmentToPlayer(IEquipment equipment)
		{
			bool gunIsOnRightPos = false;
			Transform equipmentTr = null;
			equipmentTr = (equipment as MonoBehaviour).transform;

			while (!gunIsOnRightPos)
			{
				float dist = Vector3.Distance(equipmentTr.position, GunHolder.position);
				float angle = Quaternion.Angle(equipmentTr.rotation, GunHolder.rotation);

				Debug.Log(dist);
				Debug.Log(angle);


				if (dist <= 0.1f && angle <= 0.1f)
				{
					equipmentTr.SetPositionAndRotation(GunHolder.position, GunHolder.rotation);
					equipmentTr.SetParent(GunHolder);
					gunIsOnRightPos = true;
				}
				else
				{
					equipmentTr.SetPositionAndRotation(
						Vector3.SmoothDamp(equipmentTr.position, GunHolder.position, ref equipmentCurVel, Time.deltaTime * 10),
						Quaternion.Lerp(equipmentTr.rotation, GunHolder.rotation, Time.deltaTime * 10));
				}
				yield return new WaitForEndOfFrame();
			}

			yield return null;
		}
	}
}
