using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
	protected Transform _playerGunHolder;
	protected ulong _ownerNetworkId;
	protected Coroutine _moveEquipmentToPlayer = null;

	protected IEnumerator Co_MoveToPlayerEquipmentHolder(ulong ownerClientId)
	{
		bool gunIsOnRightPos = false;

		while (!gunIsOnRightPos)
		{
			float dist = Vector3.Distance(transform.position, _playerGunHolder.transform.position);
			float angle = Quaternion.Angle(transform.rotation, _playerGunHolder.transform.rotation);

			if (dist <= 0.1f && angle <= 0.1f)
			{
				transform.SetParent(_playerGunHolder.transform);


				transform.localPosition = Vector3.zero;
				transform.localRotation = Quaternion.identity;

				gunIsOnRightPos = true;
			}
			else
			{
				transform.SetPositionAndRotation(
					Vector3.Lerp(transform.position, _playerGunHolder.transform.position, Time.deltaTime * 10),
					Quaternion.Lerp(transform.rotation, _playerGunHolder.transform.rotation, Time.deltaTime * 10));
			}
			yield return new WaitForEndOfFrame();
		}

		yield return null;
	}

	//[ServerRpc]
	//private void ChangeOwnershipServerRpc(ulong senderClientId, NetworkObjectReference networkObjectReference)
	//{
	//	if (networkObjectReference.TryGet(out NetworkObject networkObject))
	//	{
	//		networkObject.ChangeOwnership(senderClientId);
	//	}
	//}

	//[ServerRpc]
	//private void SetParentServerRpc(ulong senderClientId, NetworkObjectReference objRef, NetworkObjectReference newParentRef)
	//{
	//	if (objRef.TryGet(out NetworkObject obj) && newParentRef.TryGet(out NetworkObject newParent))
	//	{
	//		obj.transform.SetParent(newParent.transform);
	//	}
	//}
}

