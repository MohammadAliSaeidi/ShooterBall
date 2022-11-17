using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace BallShooter.Player
{
	[RequireComponent(typeof(NetworkObject), typeof(NetworkTransform))]
	public abstract class DroppedEquipment : NetworkBehaviour
	{
		public Equipment EquipmentPrefab;
		public NetworkObject networkObject { get; private set; }

		private void Awake()
		{
			networkObject = GetComponent<NetworkObject>();
		}

		[ServerRpc(RequireOwnership = false)]
		public void EquipServerRpc()
		{
			networkObject.Despawn(true);
		}
	}
}