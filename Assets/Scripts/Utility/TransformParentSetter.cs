using UnityEngine;

public class TransformParentSetter : MonoBehaviour
{
	[SerializeField] private bool SetParentAtStart = false;
	[SerializeField] private Transform Parent;

	private void Start()
	{
		if (SetParentAtStart)
			SetTransformParent(Parent);
	}

	public void SetTransformParent(Transform parent)
	{
		transform.parent = parent;
	}
}
