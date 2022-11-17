using Unity.Netcode;
using UnityEngine;

public class GunHolderPivot : NetworkBehaviour
{
	[SerializeField] private Transform _player;

	private void Update()
	{
		if (_player)
		{
			transform.position = _player.transform.position;
		}
	}
}
