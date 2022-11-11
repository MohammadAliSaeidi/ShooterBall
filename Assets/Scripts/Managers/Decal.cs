using UnityEngine;

public class Decal : MonoBehaviour
{
	private void Start()
	{
		DecalManager.Instance.AddDecalToQueue(this);
	}
}
