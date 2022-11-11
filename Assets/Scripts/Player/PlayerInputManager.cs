using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
	#region Components

	public PlayerControls playerControls { get; private set; }

	#endregion

	private void Awake()
	{
		playerControls = new PlayerControls();
	}

	private void OnEnable()
	{
		playerControls.Enable();
	}

	private void OnDisable()
	{
		playerControls.Disable();
	}
}
