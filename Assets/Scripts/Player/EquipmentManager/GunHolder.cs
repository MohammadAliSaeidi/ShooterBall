using UnityEngine;
using Unity.Netcode;
using UnityStandardAssets.Cameras;

[RequireComponent(typeof(LookatTarget))]
public class GunHolder : NetworkBehaviour
{
	private LookatTarget lookatTarget;

	private void Awake()
	{
		lookatTarget = GetComponent<LookatTarget>();
	}

	private void Start()
	{
		Debug.Log(NetworkObject);
	}

	public void SetLookingTarget(Transform target) => lookatTarget.SetTarget(target);
}
