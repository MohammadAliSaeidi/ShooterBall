using UnityEngine;

public class SmoothMouseLook : MonoBehaviour
{
	[SerializeField] private PlayerInputManager playerInputManager;

	[SerializeField] private GameObject player;
	private bool allowMouseLook = true;

	public float mouseSpeed = 1.2f;
	public float minTilt = 45f;
	public float maxTilt = 75f;

	public float mouseSmoothing = 15f;
	public float followSmooth = 5f;
	private float Vertical;
	private float Horizontal;
	private float smoothH;
	private float smoothV;
	private Vector3 currentVel;

	private void Start()
	{
		Vertical = -transform.eulerAngles.x;
		Horizontal = transform.eulerAngles.y;

		//Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		FollowPlayer();
		MouseLookHandler();
	}

	private void FollowPlayer()
	{
		if (player)
		{
			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * followSmooth);
		}
	}

	private void MouseLookHandler()
	{
		var mouseDelta = playerInputManager.playerControls.Ground.MouseLook.ReadValue<Vector2>();
		if (playerInputManager)
		{
			Vertical += mouseDelta.y * mouseSpeed;
			Horizontal += mouseDelta.x * mouseSpeed;
		}

		Vertical = Mathf.Clamp(Vertical, -minTilt, maxTilt);
		mouseSmoothing = Mathf.Clamp(mouseSmoothing, 0f, 10f);

		if (mouseSmoothing > 0)
		{
			smoothH = Mathf.Lerp(smoothH, Vertical, mouseSmoothing * Time.deltaTime);
			smoothV = Mathf.Lerp(smoothV, Horizontal, mouseSmoothing * Time.deltaTime);

			transform.localRotation = Quaternion.Euler(-smoothH, smoothV, 0);
		}
		else
		{
			transform.localRotation = Quaternion.Euler(-Vertical, Horizontal, 0);
		}
	}

	public void HandleLook(float vertical, float horizontal)
	{
		Vertical += vertical;
		Horizontal += horizontal;
	}
}
