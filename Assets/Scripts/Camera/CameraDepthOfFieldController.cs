using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraDepthOfFieldController : MonoBehaviour
{
	[SerializeField] private CameraLookPointHandler cameraLookPointHandler;
	private List<Volume> volList = new List<Volume>();
	private Camera _camera;

	private void OnValidate()
	{
		_camera = GetComponent<Camera>();
		if (_camera == null)
		{
			Debug.LogError($"{typeof(CameraDepthOfFieldController).Name} must be attached to a camera");
		}
	}

	private void Start()
	{
		volList = FindObjectsOfType<Volume>().ToList();
	}

	private void Update()
	{
		if (cameraLookPointHandler)
		{
			foreach (var vol in volList)
			{
				if (vol.profile.TryGet(out DepthOfField depthOfField))
				{
					depthOfField.focusDistance.value = Vector3.Distance(cameraLookPointHandler.PointOfCameraLooking, _camera.transform.position);
					depthOfField.focalLength.value = _camera.focalLength;
				}
			}
		}
	}
}
