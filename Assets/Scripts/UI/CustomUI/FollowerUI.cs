using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerUI : MonoBehaviour
{
	[Tooltip("Follower element")]
	public RectTransform Follower;
	[Tooltip("Element to be followed ")]
	public Transform Target;


	RectTransform rectTransform;
	Camera camera;

	private void Awake()
	{
		rectTransform = Follower.GetComponent<RectTransform>();

		if(!camera)
		{
			camera = Camera.main;
		}
	}

	void Update()
	{
		FollowTransfrom();
	}

	[ContextMenu("FollowTransfrom")]
	private void FollowTransfrom()
	{
		if(!rectTransform)
		{
			rectTransform = Follower.GetComponent<RectTransform>();
		}

		if(rectTransform && Target)
		{
			rectTransform.position = Camera.main.WorldToScreenPoint(Target.position);
		}
	}

}
