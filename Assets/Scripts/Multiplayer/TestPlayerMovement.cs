using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class TestPlayerMovement : NetworkBehaviour
{
	[SerializeField] private float moveForce = 5;

	private Rigidbody _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody>();
	}

	void Update()
    {
		if (!IsOwner)
			return;

		Vector2 moveDir = new Vector2();

		if (Input.GetKey(KeyCode.W))
		{
			moveDir.y = 1;
		}

		if (Input.GetKey(KeyCode.A))
		{
			moveDir.x = -1;
		}

		if (Input.GetKey(KeyCode.S))
		{
			moveDir.y = -1;
		}

		if (Input.GetKey(KeyCode.D))
		{
			moveDir.x = 1;
		}

		_rb.AddForce(new Vector3(moveDir.x, 0, moveDir.y) * moveForce * Time.deltaTime);
	}
}
