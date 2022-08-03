using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMouseLook : MonoBehaviour {

	GameObject player;
	private bool allowMouseLook = true;

	public float mouseSpeed = 1.2f;
	public float minTilt = 45f;
	public float maxTilt = 75f;

	public float mouseSmoothing = 15f;
	public float followSmooth = 5f;

	float Horizontal;
	float Vertical;
	float smoothH;
	float smoothV;

	private void Start () {
		Horizontal = -transform.eulerAngles.x;
		Vertical = transform.eulerAngles.y;

		Cursor.lockState = CursorLockMode.Locked;
	}
	private void LateUpdate () {
		
	}
	private void Update () {
		playerFollower();
		mouseLookHandler();
	}
	private void playerFollower () {
		if(player) {
			transform.position = Vector3.Lerp(transform.position, player.transform.position, Time.deltaTime * followSmooth);
		}
		else {
			player = GameObject.FindWithTag("Player");
		}
	}
	private void mouseLookHandler () {
		Horizontal += Input.GetAxis("Mouse Y") * mouseSpeed;
		Vertical += Input.GetAxis("Mouse X") * mouseSpeed;

		Horizontal = Mathf.Clamp(Horizontal, -minTilt, maxTilt);
		mouseSmoothing = Mathf.Clamp(mouseSmoothing, 0f, 10f);

		if(mouseSmoothing > 0) {
			smoothH = Mathf.Lerp(smoothH, Horizontal, mouseSmoothing * Time.deltaTime);
			smoothV = Mathf.Lerp(smoothV, Vertical, mouseSmoothing * Time.deltaTime);

			transform.localRotation = Quaternion.Euler(-smoothH, smoothV, 0);
		}
		else {
			transform.localRotation = Quaternion.Euler(-Horizontal, Vertical, 0);
		}
	}
}
