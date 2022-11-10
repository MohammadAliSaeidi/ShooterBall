using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BallShooter.Shared;

namespace BallShooter
{
	public class PlayerMovement : MonoBehaviour
	{

		#region Variables

		[SerializeField]
		private float movePower = 1;
		[SerializeField]
		private float jumpPower = 5;
		[SerializeField]
		private float _maxAngularVelocity = 10;
		private float groundCheck;
		public Rigidbody playerRB;

		bool CanJump = true;

		#endregion

		#region Events

		public UnityEvent EventOnJumped;
		public UnityEvent EventOnHitEvent;
		public UnityEvent EventOnSpawnEvent;

		#endregion

		#region Native Methods

		private void Start()
		{
			if(playerRB)
			{
				playerRB.maxAngularVelocity = _maxAngularVelocity;
				groundCheck = (playerRB.transform.localScale.x / 2) + 0.05f;
			}
		}

		private void LateUpdate()
		{
			if(Input.mouseScrollDelta.y < 0 &&
					Physics.Raycast(origin: playerRB.transform.position, direction: -Vector3.up, maxDistance: groundCheck))
			{
				PlayerJump();
			}
		}

		private void FixedUpdate()
		{
			if(playerRB)
			{ //&& Shared.playerHandler.allowMove) {
				MovePlayer();
			}
		}
		#endregion

		#region Methods

		private void MovePlayer()
		{
			Vector3 cameraDir = Vector3.Scale(a: Camera.main.transform.forward, b: new Vector3(1, 0, 1)).normalized;
			float v = Input.GetAxis("Vertical");
			float h = Input.GetAxis("Horizontal");

			Vector3 moveDirection = ((v * cameraDir) + (h * Camera.main.transform.right)).normalized; // create a normalized direction of the camera relative to the horizon

			playerRB.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x) * movePower);
			playerRB.AddForce(moveDirection * movePower);
		}

		private void PlayerJump()
		{
			if(playerRB && CanJump)
			{// && Shared.playerHandler.allowJump) {
				playerRB.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
				CanJump = false;
				StartCoroutine(Co_ResetJump());
				if(EventOnJumped != null)
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