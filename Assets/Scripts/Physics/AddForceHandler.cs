using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AddForceHandler : MonoBehaviour
{
	[SerializeField] private Vector3 Force;
	[SerializeField] private Space ForceSpace = Space.Self;
	[SerializeField] private bool ForceAtStart = true;

	private Rigidbody _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		if (ForceAtStart)
		{
			AddForce(Force, ForceSpace);
		}
	}

	public void AddForce(Vector3 force, Space space)
	{
		if (space == Space.Self)
		{
			_rb.AddRelativeForce(force, ForceMode.Impulse);
		}
		else if (space == Space.World)
		{
			_rb.AddForce(force, ForceMode.Impulse);
		}
	}
}
