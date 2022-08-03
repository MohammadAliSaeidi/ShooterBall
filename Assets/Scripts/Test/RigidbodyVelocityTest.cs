using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyVelocityTest : MonoBehaviour
{
	private void FixedUpdate()
	{
		Debug.Log(gameObject.name +
			" rigidbody with mass" +
			GetComponent<Rigidbody>().mass +
			" velocity is : " + GetComponent<Rigidbody>().velocity);

		Debug.DrawRay(transform.position, GetComponent<Rigidbody>().velocity);
	}
}
