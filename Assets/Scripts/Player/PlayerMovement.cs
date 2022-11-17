using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace BallShooter
{
	public class PlayerMovement : MonoBehaviour
	{

		private PlayerInputManager playerInputManager;

		#region Variables

		[SerializeField] private float MovePower = 1;
		[SerializeField] private float JumpPower = 5;
		[SerializeField] private float MaxAngularVelocity = 10;
		private float groundCheck;
		public Rigidbody playerRB;

		private Vector2 movementInputValue;
		private bool CanJump = true;

		#endregion

		#region Events

		[HideInInspector] public UnityEvent EventOnJumped;
		[HideInInspector] public UnityEvent EventOnHitEvent;
		[HideInInspector] public UnityEvent EventOnSpawnEvent;

		#endregion

		#region Unity Methods

		private void Awake()
		{
			playerInputManager = GetComponent<PlayerInputManager>();
		}

		private void Start()
		{
			if (playerRB)
			{
				playerRB.maxAngularVelocity = MaxAngularVelocity;
				groundCheck = (playerRB.transform.localScale.x / 2) + 0.05f;
			}

			playerInputManager.playerControls.Ground.Jump.performed += PlayerJump;
		}

		private void LateUpdate()
		{
			if (playerInputManager)
			{
				movementInputValue = playerInputManager.playerControls.Ground.Movement.ReadValue<Vector2>();
			}
		}

		private void FixedUpdate()
		{
			if (playerRB)
			{
				MovePlayer();
			}
		}
		#endregion

		#region Methods

		private void MovePlayer()
		{
			Vector3 cameraDir = Vector3.Scale(a: Camera.main.transform.forward, b: new Vector3(1, 0, 1)).normalized;
			float h = movementInputValue.x;
			float v = movementInputValue.y;

			Vector3 moveDirection = ((v * cameraDir) + (h * Camera.main.transform.right)).normalized; // create a normalized direction of the camera relative to the horizon

			playerRB.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * MovePower);
			playerRB.AddForce(moveDirection * MovePower);
		}

		private void PlayerJump(UnityEngine.InputSystem.InputAction.CallbackContext context)
		{
			if (playerRB && CanJump && Physics.Raycast(origin: playerRB.transform.position, direction: -Vector3.up, maxDistance: groundCheck))
			{
				playerRB.AddForce(Vector3.up * JumpPower, ForceMode.VelocityChange);
				CanJump = false;
				StartCoroutine(Co_ResetJump());
				if (EventOnJumped != null)
					EventOnJumped.Invoke();
			}
		}

		public IEnumerator Co_ResetJump()
		{
			yield return new WaitForSeconds(0.1f);
			CanJump = true;
		}

		#endregion
	}
}