using UnityEngine;
using UnityEngine.UI;

public class CrossHairMaster : MonoBehaviour
{
	[HideInInspector]
	public Slider[] sliders;
	private float currentVel;

	private void Awake()
	{
		sliders = transform.GetComponentsInChildren<Slider>();
	}

	private void Update()
	{
		foreach (Slider s in sliders)
		{
			if (s.value > 0)
			{
				s.value = Mathf.SmoothDamp(s.value, 0, ref currentVel, Time.deltaTime * 5);
			}
		}
	}
}
