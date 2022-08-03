using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHairMaster : MonoBehaviour
{
	[HideInInspector]
	public Slider[] sliders;

	private void Awake()
	{
		sliders = transform.GetComponentsInChildren<Slider>();
	}

	private void Update()
	{
		foreach(Slider s in sliders)
		{
			if(s.value > 0)
			{
				s.value = Mathf.Lerp(s.value, 0, Time.deltaTime * 5 / s.value);
			}
		}
	}
}
