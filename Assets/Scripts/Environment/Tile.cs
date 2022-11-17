using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Tile : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	private void Update()
	{
		if(_animator)
		{

		}
	}
}
